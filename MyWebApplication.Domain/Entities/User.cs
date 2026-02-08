using MyWebApplication.Domain.Abstract;
using MyWebApplication.Domain.Enums;
using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities
{
    public class User : BaseUser, IUser
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public LanguageLevel Level { get; set; }
    }
}
