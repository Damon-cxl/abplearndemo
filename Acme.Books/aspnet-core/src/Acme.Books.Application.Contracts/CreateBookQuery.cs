using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Acme.Books
{
    public class CreateBookQuery : IValidatableObject
    {
        [StringLength(5)]
        [NotNull]
        public string Name { get; set; }

        public string Publisher { get; set; }

        public List<CreateAuthQuery> Auths { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (Name == "1")
            {
                yield return new ValidationResult(
                    "Name can not be the 1!",
                    new string[] { "Name" });
            }

            if (Name == Publisher)
            {
                yield return new ValidationResult(
                    "Name and Publisher can not be the same!",
                    new string[] { "Name", "Publisher" });
            }
        }
    }
}
