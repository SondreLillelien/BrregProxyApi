using System;
using System.Threading.Tasks;
using BrregProxyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BrregProxyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrgDataController : ControllerBase
    {
        private readonly IOrgDataService _service;
        private readonly MemoryCache _cache;

        public OrgDataController(IOrgDataService service, MemoryCache memoryCache)
        {
            _service = service;
            _cache = memoryCache;
        }

        [HttpGet]
        [Route("{orgId}")]
        //[ResponseCache(Duration = 30)]
        public async Task<IActionResult> GetOrgDataById(string orgId)
        {
            
            
            if (!int.TryParse(orgId, out var idAsInt) || orgId.Length != 9 || orgId.Trim().Length != 9)
            {
                return BadRequest($"{orgId} is not a valid input. Id must be an integer with 9 digits.");
            }

            if (!_cache.TryGetValue(orgId, out var cacheEntry))
            {
                var orgData = await _service.GetOrgDataById(orgId);
                
                _cache.Set(orgId, orgData, TimeSpan.FromSeconds(30));
                
                if (orgData == null)
                {
                    return NotFound($"Organization with id: {orgId} not found");
                }
                
                return Ok(orgData);
            }
            
            if (cacheEntry == null)
            {
                return NotFound($"Organization with id: {orgId} not found");
            }

            return Ok(cacheEntry);
        }
    }
}