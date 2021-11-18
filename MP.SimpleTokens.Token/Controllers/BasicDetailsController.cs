using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MP.DocumentDB;
using MP.DocumentDB.Interfaces;
using MP.SimpleTokens.Common.Models.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.SimpleTokens.Token.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasicDetailsController : ControllerBase
    {
        private readonly ILogger<BasicDetailsController> _logger;

        public BasicDetailsController(ILogger<BasicDetailsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSummary([FromServices] IDocumentCollection<TokenInfo> collection, string id)
        {
            var token = await collection.GetFirstOrDefault(x => x.Id == id);

            if (token == null)
            {
                _logger.LogWarning($"Token requested that does not exist: {id ?? "null"}");
                return NotFound();
            }



            return Ok(token);
        }
    }
}
