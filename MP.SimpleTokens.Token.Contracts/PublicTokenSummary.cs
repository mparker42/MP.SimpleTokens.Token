using MP.SimpleTokens.Common.Models.Tokens;

namespace MP.SimpleTokens.Token.Contracts
{
    public class PublicTokenSummary
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public string? Title { get; set; }

        public string? Type { get; set; }

        public TokenType? HostingType { get; set; }

        public IEnumerable<PublicTokenTransaction>? TokenTransactions { get; set; }
    }
}