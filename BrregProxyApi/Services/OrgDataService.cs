using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BrregProxyApi.Models;

namespace BrregProxyApi.Services
{
    public interface IOrgDataService
    {
        Task<OrgData?> GetOrgDataById(string orgId);
    }


    public class OrgDataService : IOrgDataService
    {
        private readonly HttpClient _client;

        public OrgDataService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            if (_client.BaseAddress is null) throw new ArgumentException("Client needs a BaseAddress");
        }


        public async Task<OrgData?> GetOrgDataById(string orgId)
        {
            const string regexString = @"[0-9]{9}$";
            Regex regex = new Regex(regexString);
            if (!regex.IsMatch(orgId))
            {
                throw new ArgumentException($"Argument: {orgId} did not match regex: {regexString}");
            }
            

            var response = await _client.GetAsync(orgId);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw new HttpRequestException(
                    $"Request to {response.RequestMessage?.RequestUri} was not successful, status code: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<OrgDataDto>(responseContent);

            return new OrgData(dto.OrganizationNumber, dto.Name, dto.OrganizationForm.Code);
        }
    }
}