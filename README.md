# Morix.Json
Json Library for .NET inspired by:
- [LightJson](https://github.com/MarcosLopezC/LightJson), 
- [SimpleJSON](https://github.com/Bunny83/SimpleJSON/blob/master/SimpleJSON.cs)
- [TinyJSON](https://github.com/pbhogan/TinyJSON)

Include the namespace

using Morix.Json


Flunet API support for creating json objects

```
var jobj = new JsonObject
{
  ["null"] = new JsonNull(),
  ["bool"] = new JsonBoolean(true),
  ["number"] = new JsonNumber(123),
  ["string"] = new JsonString("string"),
  ["datetime"] = new JsonString(new DateTime(2022, 1, 15, 14, 59, 33, DateTimeKind.Utc)),
  ["list"] = new JsonArray(
    new JsonNumber(1),
    new JsonNumber(2.1f),
    new JsonNumber(3.3d)),
};

var json = jobj.ToString();
Debug.Print(json);
//{"null":null,"bool":true,"number":123,"string":"string","datetime":"2022-01-15T14:59:33.0000000Z","list":[1,2.1,3.3]}
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
  

