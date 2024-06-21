using System.Text.Json.Serialization;

namespace PracticalTaskSelenium.Business.Models;

public class Address
{
    [JsonPropertyName("street")]
    public string Street { get; set; }
    [JsonPropertyName("suite")]
    public string Suite { get; set; }
    [JsonPropertyName("city")]
    public string City { get; set; }
    [JsonPropertyName("zipcode")]
    public string Zipcode { get; set; }
    [JsonPropertyName("geo")]
    public Geo Geo { get; set; }
}