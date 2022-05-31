using System;
using System.Collections.Generic;
using System.Text;
using Acme.BookStore.Localization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Service
{
    /* Inherit your application services from this class.
     */
    public class TestService: IRemoteService, ITransientDependency, IAuditingEnabled
    {
        public TestService()
        {

        }

        public string GetBookId(string name)
        {
            return "Test";
        }
    }
}
