using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.SimpleTokens.Token.Contracts
{
    public class PublicTokenTransaction
    {
        public PublicTokenIdentity? To { get; set; }
        public PublicTokenIdentity? From { get; set; }
    }
}
