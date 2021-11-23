using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MP.DocumentDB;
using MP.DocumentDB.Interfaces;
using MP.SimpleTokens.Common.Ethereum.Interfaces;
using MP.SimpleTokens.Common.Models.Tokens;
using MP.SimpleTokens.Common.Models.Tokens.ERC721Metadata;
using MP.SimpleTokens.Identity.Clients;
using MP.SimpleTokens.Identity.Contracts;
using MP.SimpleTokens.Token.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.SimpleTokens.Token.Controllers
{
    [ApiController]
    [Route("basicdetails")]
    public class BasicDetailsController : ControllerBase
    {
        private readonly ILogger<BasicDetailsController> _logger;
        private readonly IEthereumService _ethereumService;
        private readonly IHttpClientFactory _httpClientFactory;

        public BasicDetailsController(ILogger<BasicDetailsController> logger, IEthereumService ethereumService, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _ethereumService = ethereumService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PublicTokenSummary), 200)]
        public async Task<IActionResult> GetSummary([FromServices] IDocumentCollection<TokenInfo> collection, [FromServices] ITokenClient tokenClient, string id)
        {
            var token = await collection.GetFirstOrDefault(x => x.Id == id);

            if (token == null)
            {
                _logger.LogWarning($"Token requested that does not exist: {id ?? "null"}");
                return NotFound();
            }

            switch (token.Type)
            {
                case TokenType.Ethereum:
                    if (token.BlockchainInfo == null)
                    {
                        _logger.LogError($"Stored token {token.Id} is considered to be an {nameof(TokenType.Ethereum)} token but blockchain info is null");

                        throw new ArgumentNullException(nameof(token.BlockchainInfo));
                    }

                    var transactions = await _ethereumService.GetTokenTransactionHistory(token.BlockchainInfo);

                    if (transactions == null)
                    {
                        return Ok(token);
                    }

                    var tokenIdentities = await tokenClient.GetPublicUsersForBlockchainTokenTransactions(
                        transactions.Select(t => new TokenTransaction
                        {
                            To = t.Event.To,
                            From = t.Event.From
                        })
                    );

                    var tokenUri = await _ethereumService.GetTokenURI(token.BlockchainInfo);

                    var metadataClient = _httpClientFactory.CreateClient();
                    var tokenGenericMetadata = await metadataClient.GetFromJsonAsync<ERC1155Suggested>(tokenUri) ?? new ERC1155Suggested();

                    return Ok(
                        new PublicTokenSummary
                        {
                            Name = tokenGenericMetadata.Name,
                            Description = tokenGenericMetadata.Description,
                            Image = tokenGenericMetadata.Image,
                            Title = tokenGenericMetadata.Title,
                            Type = tokenGenericMetadata.Type,
                            HostingType = TokenType.Ethereum,
                            TokenTransactions = transactions.Select(t =>
                                new PublicTokenTransaction
                                {
                                    To = tokenIdentities[t.Event.To].Name,
                                    From = tokenIdentities[t.Event.From].Name
                                }
                            )
                        }
                    );
                case TokenType.Simple:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(TokenType));
            }

            return NoContent();
        }
    }
}
