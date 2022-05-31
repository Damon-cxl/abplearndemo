using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.Books
{
    public class BookOnly : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
    }
}
