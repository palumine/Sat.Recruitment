using System;

namespace Sat.Recruitment.Api.Repository
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException() : base("User is duplicated")
        {
        }
    }
}
