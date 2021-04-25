using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BrregProxyApi.Options;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace BrregProxyApi.Tests.UnitTests
{
    public class AppSettingsTest
    {
        public class AppSettingsDTO
        {
            [JsonPropertyName("OrgDataSettings")]
            public OrgDataSettings OrgDataSettings { get; set; }
        }

        public class OrgDataSettingsDTO
        {
            [JsonPropertyName("BaseUrl")]
            public string BaseUrl { get; set; }
        }

        //[Fact]
        //public async Task OrgDataServiceBaseUrl_ShouldBe_EqualTo_AppSettings_JsonBaseUrl()
        //{
            //var jsonString = File.ReadAllText($"{Directory.GetCurrentDirectory()}/BrregProxyApi/appsettings.json");
            //TextReader textReader =
            //    new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory.ToString()}/BrregProxyApi/appsettings.json").;
            //JsonReader jsonReader = new JsonTextReader(textReader);
            
            
            //var jsonBaseUrl = JsonSerializer.Deserialize<AppSettingsDTO>(jsonString);

        //} 
    }
}