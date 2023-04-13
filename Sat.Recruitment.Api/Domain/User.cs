using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sat.Recruitment.Api.Domain
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public UserType UserType { get; set; }

        public decimal Money { get; set; }


        private readonly GifCalculator _gifCalulator;
        public decimal MoneyPlusGif 
        { 
            get { return this._gifCalulator.Calculate(this.Money); } 
        }

        public User(string name, string email, string address, string phone, UserType userType, decimal money)
        {
            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
            UserType = userType;
            Money = money;

            this._gifCalulator = (new GifCalcuatorFactory()).GetCalculator(userType);
        }

        public void ValidateErrors(ref string errors)
        {
            var sbErrors = new StringBuilder();

            if (this.Name == null)
                //Validate if Name is null
                sbErrors.Append("The name is required");
            if (this.Email == null)
                //Validate if Email is null
                sbErrors.Append("The email is required");
            if (this.Address == null)
                //Validate if Address is null
                sbErrors.Append("The address is required");
            if (this.Phone == null)
                //Validate if Phone is null
                sbErrors.Append("The phone is required");

            errors = sbErrors.ToString();
        }

        public string NormalizedEmail
        {
            get
            {
                var aux = this.Email.Split('@', StringSplitOptions.RemoveEmptyEntries);

                var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

                aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

                return string.Join("@", new string[] { aux[0], aux[1] });
            }
        }
    }
}
