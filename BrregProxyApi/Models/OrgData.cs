namespace BrregProxyApi.Models
{
    public class OrgData
    {
        public OrgData(string organizationNumber, string name, string organizationType)
        {
            OrganizationNumber = organizationNumber;
            Name = name;
            OrganizationType = organizationType;
        }

        public string OrganizationNumber { get; }
        public string Name { get; }
        public string OrganizationType { get; }
    }
}