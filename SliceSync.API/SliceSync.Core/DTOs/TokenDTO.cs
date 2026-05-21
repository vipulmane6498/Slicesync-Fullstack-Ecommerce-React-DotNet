using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Core.DTOs
{
    public class TokenDTO
    {
        //Below 2 properites are received from client to server
        public string? Token { get; set; }

        public string? RefreshToken { get; set; }
    }
}
