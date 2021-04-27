using System.Text.Json.Serialization;

namespace BrregProxyApi.Models
{
    public class OrgDataDto
    {
        [JsonPropertyName(name:"organisasjonsnummer")]
        public string OrganizationNumber{ get; set; }
    
        [JsonPropertyName(name:"navn")]
        public string Name { get; set; }
        
        [JsonPropertyName(name:"organisasjonsform")]
        public OrganizationTypeDto OrganizationForm { get; set; }
    }

    public class OrganizationTypeDto
    {
        [JsonPropertyName(name:"kode")]
        public string Code { get; set; }
    }
}