using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.SimpleTokens.Token.Contracts
{
    public class PublicTokenIdentity
    {
        public bool IsVerified { get; set; }
        public string? Name { get; set; }
    }
}
