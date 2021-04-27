using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace BrregProxyApi.Tests.IntegrationTests
{
    public class OrgDataApiTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private const string BaseRoute = "OrgData";

        public OrgDataApiTest()
        {
            _server = new TestServer(new WebHostBuilder().UseConfiguration(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
            ).UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        
        [Theory]
        [InlineData("123123123")]
        [InlineData("321321321")]
        public async Task Get_OrgData_WithBad_OrgId_ShouldReturn_StatusCode_404(string badId)
        {
            var response = await _client.GetAsync($"{BaseRoute}/{badId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("988986453")]
        [InlineData("919300388")]
        [InlineData("984851006")]
        public async Task Get_OrgData_WithValid_OrgId_ShouldReturn_StatusCode_200(string validId)
        {
            var response = await _client.GetAsync($"{BaseRoute}/{validId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Theory]
        [InlineData("tulleid")]
        [InlineData("ikke9siffer")]
        [InlineData("12312312324654323131236532412315")]
        [InlineData("12345678 ")]
        [InlineData(" 23456789")]
        [InlineData(" 2345678 ")]
        [InlineData("1234 6789")]
        [InlineData("1234%6789")]
        [InlineData("1234(6789")]
        public async Task Get_OrgData_WithInvalid_RequestParameter_ShouldReturn_StatusCode_400(string invalidId)
        {
            var response = await _client.GetAsync($"{BaseRoute}/{invalidId}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}   