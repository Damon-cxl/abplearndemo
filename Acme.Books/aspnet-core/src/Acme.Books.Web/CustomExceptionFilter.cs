using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace Acme.Books.Web
{
    public class CustomExceptionFilter : AbpExceptionFilter
    {
        private readonly IExceptionToErrorInfoConverter errorInfoConverter;
        private readonly IHttpExceptionStatusCodeFinder statusCodeFinder;
        private readonly IJsonSerializer jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExceptionFilter"/> class.
        /// </summary>
        /// <param name="errorInfoConverter">errorInfoConverter.</param>
        /// <param name="statusCodeFinder">statusCodeFinder.</param>
        /// <param name="jsonSerializer">jsonSerializer.</param>
        public CustomExceptionFilter(
            [NotNull] IExceptionToErrorInfoConverter errorInfoConverter,
            IHttpExceptionStatusCodeFinder statusCodeFinder,
            IJsonSerializer jsonSerializer)
            : base()
        {
            this.errorInfoConverter = errorInfoConverter;
            this.statusCodeFinder = statusCodeFinder;
            this.jsonSerializer = jsonSerializer;

            Logger = NullLogger<CustomExceptionFilter>.Instance;

            var defaultErrorInfoConverter = (DefaultExceptionToErrorInfoConverter)errorInfoConverter;
            if (defaultErrorInfoConverter != null)
            {
                //defaultErrorInfoConverter.SendAllExceptionsToClients = false;
            }
        }

        /// <summary>
        /// Gets or sets logger.
        /// </summary>
        /// <value>
        /// ILogger CustomExceptionFilter.
        /// </value>
        public new ILogger<CustomExceptionFilter> Logger { get; set; }

        /// <inheritdoc/>
        protected override bool ShouldHandleException(ExceptionContext context)
        {
            //TODO: Create DontWrap attribute to control wrapping..?
            var fff = context.ActionDescriptor is ControllerActionDescriptor;
            var tt = context.ActionDescriptor.GetMethodInfo().ReturnType;

            if (context.ActionDescriptor.IsControllerAction() &&
                context.ActionDescriptor.HasObjectResult())
            {
                return true;
            }

            if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
            {
                return true;
            }

            if (context.HttpContext.Request.IsAjax())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// HandleAndWrapException.
        /// </summary>
        /// <param name="context">context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override Task HandleAndWrapException(ExceptionContext context)
        {
            // do something here if you want, like send message to infoapi.
            context.HttpContext.Response.Headers.Add(AbpHttpConsts.AbpErrorFormat, "true");
            context.HttpContext.Response.StatusCode = (int)statusCodeFinder.GetStatusCode(context.HttpContext, context.Exception);
            context.HttpContext.Response.ContentType = "application/json";

            var remoteServiceErrorInfo = errorInfoConverter.Convert(context.Exception, true);

            context.Result = new ObjectResult(new RemoteServiceErrorResponse(remoteServiceErrorInfo));

            var logLevel = context.Exception.GetLogLevel();

            Logger.LogWithLevel(logLevel, nameof(RemoteServiceErrorInfo));
            Logger.LogWithLevel(logLevel, jsonSerializer.Serialize(remoteServiceErrorInfo, indented: true));
            Logger.LogException(context.Exception, logLevel);

            context.Exception = null; // Handled!

            return Task.CompletedTask;
        }
    }
}
