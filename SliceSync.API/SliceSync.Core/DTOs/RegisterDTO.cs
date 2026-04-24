using Microsoft.Extensions.Options;
using SliceSync.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.DTOs
{
    public class RegisterDTO
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        
        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }


        //[Required(ErrorMessage = "Select onyl from Admin, Customer, DeliveryGuy")]
        [EnumDataType(typeof(UserTypeOptions), ErrorMessage = "Invalid role")]
        public UserTypeOptions? userTypeOptions { get; set; }
    }
}
