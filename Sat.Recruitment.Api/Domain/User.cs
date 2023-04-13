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

        public void ApplyGif()
        {

            var percentage = 0m;
            switch (this.UserType)
            {
                case UserType.Normal:
                    {
                        if (this.Money > 100)
                        {
                            //If new user is normal and has more than USD100
                            percentage = 0.12m;
                        }
                        if (this.Money <= 100) //TODO verify equals to 100 condition
                        {
                            if (this.Money > 10)
                            {
                                percentage = 0.8m;
                            }
                        }
                        break;
                    }
                case UserType.SuperUser:
                    {
                        if (this.Money > 100)
                        {
                            percentage = 0.20m;
                        }
                        break;
                    }
                case UserType.Premium:
                    {
                        if (this.Money > 100)
                        {
                            percentage = 1;
                        }
                        break;
                    }
            }
            this.Money *= (1 + percentage);
        }
    }
}
