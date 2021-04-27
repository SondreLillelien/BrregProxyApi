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
        [Route("{id}")]
        public async Task<IActionResult> Get([RegularExpression(@"[0-9]{9}")] string id)
        {
            var orgData = await _cache.GetOrCreateAsync(id, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                cacheEntry.Size = 1;
                return _service.GetOrgDataById(id);
            });

            if (orgData is null)
            {
                return NotFound($"Organization with id: {id} not found");
            }

            return Ok(orgData);
        }
    }
}