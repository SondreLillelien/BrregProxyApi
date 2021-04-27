using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BrregProxyApi.Model;

namespace BrregProxyApi.Services
{
    public interface IOrgDataService
    {
        Task<OrgData?> GetOrgDataById(string orgId);
    }


    public class OrgDataService : IOrgDataService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public OrgDataService(HttpClient client, string baseUrl)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        }


        public async Task<OrgData?> GetOrgDataById(string orgId)
        {
            const string regexString = @"[0-9]{9}$";
            Regex regex = new Regex(regexString);
            if (!regex.IsMatch(orgId))
            {
                throw new ArgumentException($"Argument: {orgId} did not match regex: {regexString}");
            }
            
            var url = $"{_baseUrl}/{orgId}";

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw new HttpRequestException(
                    $"Request to {url} was not successful, status code: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<OrgDataDto>(responseContent);

            return new OrgData(dto.OrganizationNumber, dto.Name, dto.OrganizationForm.Code);
        }
    }
}