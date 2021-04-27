# BrregProxyApi
ASP.NET proxy Web API to get organization data from https://data.brreg.no/enhetsregisteret/api/enheter/

The API has a GET endpoint.
Input id must be an integer with 9 digits.

If organization with corresponding id is found, returns OK

Output format: 
{
  "OrganizationNumber": "exampleId",
  "Name": "exampleName",
  "OrganizationType": "AS"
}

