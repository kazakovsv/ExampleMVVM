using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExampleMVVM.DesktopClient.Models
{
    public class Customer : IDataErrorInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsCompany { get; set; }
        public decimal TotalSales { get; set; }

        #region Creation

        private Customer()
        {
        }

        public static Customer Create(
            string firstName,
            string lastName,
            string email,
            bool isCompany,
            decimal totalSales)
        {
            return new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                IsCompany = isCompany,
                TotalSales = totalSales
            };
        }

        public static Customer Empty = new Customer();

        #endregion // Creation

        #region IDataErrorInfo Members

        public string Error => null;

        public string this[string propertyName] => Validate(propertyName);

        #endregion // IDataErrorInfo Members

        #region Validation

        private static string[] _properties = new[]
        {
            nameof(FirstName),
            nameof(LastName),
            nameof(Email)
        };

        public bool IsValid => !_properties.Any(property => Validate(property) != null);

        private string Validate(string propertyName)
        {
            string error;

            switch (propertyName)
            {
                case nameof(Email):
                    error = ValidateEmail(); break;
                case nameof(FirstName):
                    error = ValidateFirstName(); break;
                case nameof(LastName):
                    error = ValidateLastName(); break;
                default:
                    throw new ArgumentException("Unexpected property", nameof(propertyName));
            }

            return error;
        }

        private string ValidateEmail()
        {
            if (IsStringMissing(Email))
            {
                return "E-mail address is missing";
            }
            
            if (!IsValidEmailAddress(Email))
            {
                return "E-mail address is invalid";
            }

            return null;
        }

        private string ValidateFirstName()
        {
            if (IsStringMissing(FirstName))
            {
                return "First name is missing";
            }

            return null;
        }

        private string ValidateLastName()
        {
            if (IsCompany)
            {
                if (!IsStringMissing(LastName))
                {
                    return "Companies have no last name";
                }
            }
            else
            {
                if (IsStringMissing(LastName))
                {
                    return "Last name is missing";
                }
            }

            return null;
        }

        private static bool IsStringMissing(string value)
        {
            return value == null || string.IsNullOrWhiteSpace(value.Trim());
        }

        private static bool IsValidEmailAddress(string email)
        {
            if (IsStringMissing(email))
            {
                return false;
            }

            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        #endregion // Validation
    }
}
