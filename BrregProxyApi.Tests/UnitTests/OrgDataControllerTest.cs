using System;
using System.Threading.Tasks;
using BrregProxyApi.Controllers;
using BrregProxyApi.Model;
using BrregProxyApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace BrregProxyApi.Tests.UnitTests
{
    public class OrgDataControllerTest
    {   
        private const string BaseRoute = "OrgData";
        
        [Theory]
        [InlineData("919300388")]
        [InlineData("988986453")]
        [InlineData("984851006")]
        public async Task Get_WithValidId_Within_CacheEntryExpirationDate_ShouldOnlyCallService_Once(string validId)
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            
            var testdata = new OrgData(validId, "testname", "test");
            
            var mockedService = new Mock<IOrgDataService>();
            mockedService
                .Setup(x => x.GetOrgDataById(validId))
                .Returns(Task.FromResult(testdata));
            
            var controller = new OrgDataController(mockedService.Object, cache);

            var result1 = await controller.Get(validId);
            var result2 = await controller.Get(validId);

            cache.Count.Should().Be(1);
            mockedService.Verify(x => x.GetOrgDataById(validId),Times.Once);
  
        }
        [Theory]
        [InlineData("123123123")]
        [InlineData("321321321")]
        public async Task Get_WithBadId_Within_CacheEntryExpirationDate_ShouldOnlyCallService_Once(string badId)
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            
            var testdata = new OrgData(badId, "testname", "test");
            
            var mockedService = new Mock<IOrgDataService>();
            mockedService
                .Setup(x => x.GetOrgDataById(badId))
                .Returns(Task.FromResult<OrgData?>(null));
            
            var controller = new OrgDataController(mockedService.Object, cache);

            var result1 = await controller.Get(badId);
            var result2 = await controller.Get(badId);

            cache.Count.Should().Be(1);
            mockedService.Verify(x => x.GetOrgDataById(badId),Times.Once);
  
        }

        
        [Fact(Skip = "Takes 30 seconds")]
        public async Task Get_WithInterval_Over_CacheEntryExpirationDate_ShouldCallServiceTwice()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var validId = "123123123";
            
            var testdata = new OrgData(validId, "testname", "test");
            
            var mockedService = new Mock<IOrgDataService>();
            mockedService
                .Setup(x => x.GetOrgDataById(validId))
                .Returns(Task.FromResult(testdata));
            
            var controller = new OrgDataController(mockedService.Object, cache);

            var result1 = await controller.Get(validId);

            await Task.Delay(TimeSpan.FromSeconds(31));
            var result2 = await controller.Get(validId);

            cache.Count.Should().Be(1);
            mockedService.Verify(x => x.GetOrgDataById(validId),Times.Exactly(2));
  
        }
    }
}