using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.DTOs
{
    public class AuthenticationResponseDTO
    {
        public string? PersonName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty ;

        public string? JwtToken { get; set; } = string.Empty;

        public DateTime? JwtTokenExpiration { get; set; }
    }
}
