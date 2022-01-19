# Morix.Json
Json Library for .NET


Fluet API support

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
