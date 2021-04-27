using System;
using System.Net.Http;
using System.Threading.Tasks;
using BrregProxyApi.Models;
using BrregProxyApi.Services;
using FluentAssertions;
using Xunit;

namespace BrregProxyApi.Tests.UnitTests
{
    public class OrgDataServiceTest
    {
        private readonly IOrgDataService _service;
        private const string BaseUrl = "https://data.brreg.no/enhetsregisteret/api/enheter/";

        public OrgDataServiceTest()
        {
            var _client = new HttpClient();
            _client.BaseAddress = new Uri(BaseUrl);
            _service = new OrgDataService(_client);
        }

        [Theory]
        [InlineData("988986453")]
        [InlineData("919300388")]
        [InlineData("984851006")]
        public async Task Get_OrgData_WithValid_RequestParameter_ShouldReturn_OrgDataObject(string validId)
        {
            var orgData = await _service.GetOrgDataById(validId);
            orgData.Should().BeOfType<OrgData>();
        }

        [Theory]
        [InlineData("123123123")]
        [InlineData("321321321")]
        [InlineData("123321123")]
        public async Task Get_OrgData_WithBad_RequestParameter_ShouldReturn_Null(string badId)
        {
            var orgData = await _service.GetOrgDataById(badId);
            orgData.Should().BeNull();
        }

        [Theory]
        [InlineData(" 0439 920 0")]
        [InlineData("fffffffff")]
        [InlineData("4932u90gw3u932")]
        [InlineData("")]
        [InlineData("             ")]
        [InlineData(null)]
        [InlineData(" e2fd2a2")]
        [InlineData("()¤#")]
        [InlineData("123123123/blabla")]
        public async Task Get_OrgData_WithInvalid_Parameter_ShouldThrow_ArgumentException(string id)
        {
            await _service.Invoking(x => x.GetOrgDataById(id)).Should().ThrowAsync<ArgumentException>();
        }
    }
}