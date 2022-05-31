using System;
using System.Collections.Generic;
using System.Text;
using Acme.BookStore.Localization;
using Acme.BookStore.Service;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore
{
    /* Inherit your application services from this class.
     */
    public class TestTestService: IRemoteService, ITransientDependency, IAuditingEnabled
    {
        public readonly TestService testService;
        public TestTestService(TestService testService)
        {
            this.testService = testService;
        }

        public string GetTestBookId(string name)
        {
            return GetTestBookId2(name);
        }

        public string GetTestBookId2(string name)
        {
            return testService.GetBookId(name);
        }
    }
}
