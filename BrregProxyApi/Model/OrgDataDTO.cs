using System.Text.Json.Serialization;

namespace BrregProxyApi.Model
{
    public class OrgDataDTO
    {
        [JsonPropertyName(name:"organisasjonsnummer")]
        public string OrganizationNumber{ get; set; }
    
        [JsonPropertyName(name:"navn")]
        public string Name { get; set; }
        
        [JsonPropertyName(name:"organisasjonsform")]
        public OrganizationTypeDTO OrganizationForm { get; set; }
    }

    public class OrganizationTypeDTO
    {
        [JsonPropertyName(name:"kode")]
        public string Code { get; set; }
    }
}