using System;

namespace Sat.Recruitment.Repositories
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException() : base("User is duplicated")
        {
        }
    }
}
