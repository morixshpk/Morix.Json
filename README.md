# Morix.Json
Json Library for .NET inspired by:
- [LightJson](https://github.com/MarcosLopezC/LightJson), 
- [SimpleJSON](https://github.com/Bunny83/SimpleJSON/blob/master/SimpleJSON.cs)
- [TinyJSON](https://github.com/pbhogan/TinyJSON)

Include the namespace

using Morix.Json


Flunet API support for creating json objects

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

```
public class Business {
  public string Name { get; set; }
  public int Founded { get; set; }
  public List<double> Location { get; set; }
}

var business = new Business
{
  Name = "Morix",
  Founded = 2015,
  Location = new List<double> { 41.321693420410156f, 19.799325942993164f }
};

//serialize
var json = JsonConvert.Serialize(business);

//deserialize from text to object
var back = JsonConvert.Deserialize<Business>(json);
  ```
  

