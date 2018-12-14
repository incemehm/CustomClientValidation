using CustomClientValidation.Models.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomClientValidation.Models
{
    public class Person
    {
        [Required]
        [ExcludeChars("_^$*\\/")]
        public string Name { get; set; }

        [ValidateAge(18, 65)]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EqualsToProperty("Password", "Password not equal")]
        public string PasswordAgain { get; set; }

        [MustBeTrue]
        public bool ReadContract { get; set; }

        public DateTime StartDate { get; set; }

        [DateGreaterThan("StartDate", "EndDate must be greater than StartDate")]
        public DateTime EndDate { get; set; }
    }
}