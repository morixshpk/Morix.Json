# Morix.Json
Json Library for .NET inspired by:
- [LightJson](https://github.com/MarcosLopezC/LightJson), 
- [SimpleJSON](https://github.com/Bunny83/SimpleJSON/blob/master/SimpleJSON.cs)
- [TinyJSON](https://github.com/pbhogan/TinyJSON)

Include the namespace

using Morix.Json


Fluent API support for creating json objects

```
var json = new JsonObject
{
	["null"] = new JsonValue(),
	["bool"] = true,
	["number"] = 123,
	["string"] = "property value",
	["datetime"] = new DateTime(2022, 1, 15, 14, 59, 33, DateTimeKind.Utc),
	["list"] = new JsonArray(1, 2.1f, 3.3d)
};

var text = json.ToString();
Debug.Print(text);
//{"null":null,"bool":true,"number":123,"string":"property value","datetime":"2022-01-15T14:59:33.0000000Z","list":[1,2.1,3.3]}
```

Serialize/Deserialize Objects
You can serialize/Deserialize any class or struct with JsonConvert Class.


```
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

var business = new Business
{
	Name = "My business",
	Founded = 2010,
	PhoneNumber = "+355 69-40-00-111",
	Location = new List<double> { 41.321693420410156, 19.799325942993164 }
};


//serialize
var json = JsonConvert.Serialize(business);

//deserialize from text to object
var back = JsonConvert.Deserialize<Business>(json);
  ```
  
Using  JsonIgnore attribute to properties or fields you can igrnore serialization/deserialization of your data.
Also youc an set custom names to proeprty or fields with JsonProperty attribute.


