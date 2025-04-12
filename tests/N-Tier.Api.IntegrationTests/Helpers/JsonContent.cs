using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace N_Tier.Api.IntegrationTests.Helpers;

public class JsonContent(object obj) : StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
