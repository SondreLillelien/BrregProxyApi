using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using BrregProxyApi.Model;
using BrregProxyApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrregProxyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrgDataController : ControllerBase
    {
        private readonly IOrgDataService _service;

        public OrgDataController(IOrgDataService service)
        {
            _service = service;
        }
        
        [HttpGet]
        [Route("org/{orgId}")]
        public async Task<IActionResult> GetOrgDataById(string orgId)
        {
            if (!Int32.TryParse(orgId, out int idAsInt) || orgId.Length != 9)
            {
                return BadRequest($"{orgId} is not a valid input. Id must be an integer with 9 digits.");
            }

            try
            {
                var data = await _service.GetOrgDataById(orgId);
                return Ok(data);
            }
            catch (HttpRequestException e)
            {
                return NotFound(e.Message);
            }

            
            //Finn ut hvordan gjøre orgid til en del av ruta
            
            
        }
    }
    

}