using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MVC03.DAL.Models
{
    public class AppUser : IdentityUser
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public bool IsAgree { get; set; }


    }
}
