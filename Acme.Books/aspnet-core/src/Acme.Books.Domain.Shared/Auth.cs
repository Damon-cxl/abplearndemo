using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.Books
{
    public class Auth : AuditedEntity<Guid>
    {
        public string Name { get; set; }
    }
}
