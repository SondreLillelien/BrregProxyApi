using System;
using System.ComponentModel.DataAnnotations;
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
        private readonly IMemoryCache _cache;

        public OrgDataController(IOrgDataService service, IMemoryCache memoryCache)
        {
            _service = service;
            _cache = memoryCache;
        }

        [HttpGet]
        [Route("{orgId}")]
        //[ResponseCache(Duration = 30)]
        public async Task<IActionResult> GetOrgDataById( [RegularExpression(@"[0-9]{9}")]string orgId)
        {
            var orgData = await _cache.GetOrCreateAsync(orgId, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                cacheEntry.Size = 1;
                return _service.GetOrgDataById(orgId);
            });

            if (orgData is null)
            {
                return NotFound($"Organization with id: {orgId} not found");
            }

            return Ok(orgData);
        }
    }
}