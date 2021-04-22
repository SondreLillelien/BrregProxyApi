using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BrregProxyApi.Model;
using Microsoft.Extensions.DependencyInjection;

namespace BrregProxyApi.Services
{
    public interface IOrgDataService
    {
        Task<OrgData?> GetOrgDataById(string orgId);
    }
    
    
    public class OrgDataService : IOrgDataService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "https://data.brreg.no/enhetsregisteret/api/enheter";

        public OrgDataService(HttpClient client)
        {
            _client = client;
        }


        public async Task<OrgData?> GetOrgDataById(string orgId)
        {
            var url = $"{BaseUrl}/{orgId}";

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw new HttpRequestException();
                
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<OrgDataDTO>(responseContent);
            var data = new OrgData
            {
                OrganizationNumber = dto.OrganizationNumber,
                Name = dto.Name,
                OrganizationType = dto.OrganizationForm.Code
            };

            return data;

        }
    }
}