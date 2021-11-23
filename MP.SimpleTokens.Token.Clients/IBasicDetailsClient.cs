using MP.SimpleTokens.Token.Contracts;
using Refit;

namespace MP.SimpleTokens.Token.Clients
{
    public interface IBasicDetailsClient
    {
        [Get("/basicdetails/{id}")]
        Task<PublicTokenSummary> GetSummary(string id);
    }
}