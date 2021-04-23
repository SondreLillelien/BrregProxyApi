using System.Threading.Tasks;
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
        [Route("{orgId}")]
        public async Task<IActionResult> GetOrgDataById(string orgId)
        {
            if (!int.TryParse(orgId, out var idAsInt) || orgId.Length != 9)
            {
                return BadRequest($"{orgId} is not a valid input. Id must be an integer with 9 digits.");
            }

            var data = await _service.GetOrgDataById(orgId);
            if (data == null)
            {
                return NotFound($"Organization with id {orgId} not found");
            }

            return Ok(data);


            //Finn ut hvordan gjøre orgid til en del av ruta
        }
    }
}