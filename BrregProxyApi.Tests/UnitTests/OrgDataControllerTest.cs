using System;
using System.Threading.Tasks;
using BrregProxyApi.Controllers;
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
        [InlineData("123123123")]
        [InlineData("321321321")]
        public async Task Get_OrgData_ShouldReturn_CachedResponse_IfAvailable(string validId)
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var service = new Mock<IOrgDataService>();
            
            cache.Count.Should().Be(0);
            var firstOrgData = await cache.GetOrCreateAsync(validId, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
                return service.Object.GetOrgDataById(validId);
            });
        
            cache.Count.Should().Be(1);
        
            var secondOrgData = await cache.GetOrCreateAsync(validId, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
                return service.Object.GetOrgDataById(validId);
            });
        
            cache.Count.Should().Be(1);
            firstOrgData.Should().BeSameAs(secondOrgData);
        }

        [Theory]
        [InlineData("919300388")]
        [InlineData("123123123")]
        public async Task Get_OrgData_CacheEntry_ShouldExpire_AfterExpirationDate(string orgId)
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var service = new Mock<IOrgDataService>();
            
            await cache.GetOrCreateAsync(orgId, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
                return service.Object.GetOrgDataById(orgId);
            });

            cache.TryGetValue(orgId, out var cacheEntry1).Should().BeTrue();

            await Task.Delay(TimeSpan.FromMilliseconds(1200));

            cache.TryGetValue(orgId, out var cacheEntry2).Should().BeFalse();
        }
    }
}