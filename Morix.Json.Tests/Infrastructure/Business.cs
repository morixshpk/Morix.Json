using System.Collections.Generic;


namespace Morix.Json.Tests
{
	public class Business
	{
		[JsonProperty("n")]
		public string Name { get; set; }

		[JsonProperty("f")]
		public int Founded { get; set; }

		[JsonIgnore]
		public string PhoneNumber;

		[JsonProperty("l")]
		public List<double> Location { get; set; }
	}
}