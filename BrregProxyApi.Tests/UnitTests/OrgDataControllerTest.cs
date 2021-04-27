using System;
using System.Threading.Tasks;
using BrregProxyApi.Controllers;
using BrregProxyApi.Models;
using BrregProxyApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace BrregProxyApi.Tests.UnitTests
{
    public class OrgDataControllerTest
    {
        [Theory]
        [InlineData("919300388")]
        [InlineData("988986453")]
        [InlineData("984851006")]
        public async Task Get_WithValidId_Within_CacheEntryExpirationDate_ShouldOnlyCallService_Once(string validId)
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var testData = new OrgData(validId, "testname", "test");

            var mockedService = new Mock<IOrgDataService>();
            mockedService
                .Setup(x => x.GetOrgDataById(validId))
                .Returns(Task.FromResult<OrgData?>(testData));

            var controller = new OrgDataController(mockedService.Object, cache);

            await controller.Get(validId);
            await controller.Get(validId);

            cache.Count.Should().Be(1);
            mockedService.Verify(x => x.GetOrgDataById(validId), Times.Once);
        }

        [Theory]
        [InlineData("123123123")]
        [InlineData("321321321")]
        public async Task Get_WithBadId_Within_CacheEntryExpirationDate_ShouldOnlyCallService_Once(string badId)
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var mockedService = new Mock<IOrgDataService>();
            mockedService
                .Setup(x => x.GetOrgDataById(badId))
                .Returns(Task.FromResult<OrgData?>(null));

            var controller = new OrgDataController(mockedService.Object, cache);

            await controller.Get(badId);
            await controller.Get(badId);

            cache.Count.Should().Be(1);
            mockedService.Verify(x => x.GetOrgDataById(badId), Times.Once);
        }


        [Fact(Skip = "Takes 30 seconds")]
        public async Task Get_WithInterval_Over_CacheEntryExpirationDate_ShouldCallServiceTwice()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            const string validId = "123123123";

            var testData = new OrgData(validId, "testname", "test");

            var mockedService = new Mock<IOrgDataService>();
            mockedService
                .Setup(x => x.GetOrgDataById(validId))
                .Returns(Task.FromResult<OrgData?>(testData));

            var controller = new OrgDataController(mockedService.Object, cache);

            await controller.Get(validId);

            await Task.Delay(TimeSpan.FromSeconds(31));
            
            await controller.Get(validId);

            cache.Count.Should().Be(1);
            mockedService.Verify(x => x.GetOrgDataById(validId), Times.Exactly(2));
        }
    }
}