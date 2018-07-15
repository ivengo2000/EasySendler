using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BuisenessLogicLayer;

namespace EasySendler.Extensions
{
    public class UniqueEmailInMailSettingsValidationAttribute : ValidationAttribute
    {
        private MySmtpEntities _db;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _db = new MySmtpEntities();
            var email = Convert.ToString(value);
            if (_db.MailSettings.Any(x => x.Email.Equals(email)))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }
    }
}