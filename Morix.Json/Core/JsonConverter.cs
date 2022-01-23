using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Morix.Json
{
    internal class JsonConverter
    {
        private static readonly Dictionary<Type, Dictionary<string, FieldInfo>> _fieldInfoCached = new Dictionary<Type, Dictionary<string, FieldInfo>>();
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _propertyInfoCached = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        private static readonly object _sync = new object();
        public JsonConverter()
        {

        }

        /// <summary>
        /// Serialize any object to string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            var json = ParseObject(obj);
            return json.ToString();
        }

        /// <summary>
        /// Deserialize string to given T type
        /// </summary>
        /// <typeparam name="T">Type to be convert to</typeparam>
        /// <param name="json">Json string value to be parsed</param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            var jsonValue = JsonReader.Parse(json);

            //Parse the thing!
            return (T)ParseValue(typeof(T), jsonValue);
        }

        /// <summary>
        /// Parse anonymous object to JsonValue
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private JsonValue ParseObject(object obj)
        {
            if (obj == null)
                return JsonValue.Null;

            var type = obj.GetType();
            if (type == typeof(string) || type == typeof(char))
            {
                return new JsonValue(obj.ToString());
            }
            else if (type == typeof(bool))
            {
                return new JsonValue((bool)obj);
            }
            else if (type == typeof(byte))
            {
                return new JsonValue((byte)obj);
            }
            else if (type == typeof(sbyte))
            {
                return new JsonValue((sbyte)obj);
            }
            else if (type == typeof(short))
            {
                return new JsonValue((short)obj);
            }
            else if (type == typeof(ushort))
            {
                return new JsonValue((ushort)obj);
            }
            else if (type == typeof(int))
            {
                return new JsonValue((int)obj);
            }
            else if (type == typeof(uint))
            {
                return new JsonValue((uint)obj);
            }
            else if (type == typeof(long))
            {
                return new JsonValue((long)obj);
            }
            else if (type == typeof(ulong))
            {
                return new JsonValue((ulong)obj);
            }
            else if (type == typeof(float))
            {
                return new JsonValue((float)obj);
            }
            else if (type == typeof(double))
            {
                return new JsonValue((double)obj);
            }
            else if (type == typeof(decimal))
            {
                return new JsonValue((decimal)obj);
            }
            else if (type == typeof(DateTime))
            {
                return new JsonValue((DateTime)obj);
            }
            else if (type.IsEnum)
            {
                return new JsonValue(obj.ToString());
            }
            else if (obj is IList list)
            {
                var jarray = new JsonArray();
                for (int i = 0; i < list.Count; i++)
                {
                    jarray.Add(ParseObject(list[i]));
                }
                return jarray;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                IDictionary dict = obj as IDictionary;
                var jobject = new JsonObject();
                foreach (var key in dict.Keys)
                {
                    jobject.Add(key.ToString(), ParseObject(dict[key]));
                }
                return jobject;
            }
            else
            {
                var jobject = new JsonObject();

                if (JsonConvert.IncludeFields)
                {
                    var nameToFields = GetFields(type);

                    foreach (var member in nameToFields)
                    {
                        object value = member.Value.GetValue(obj);

                        if (value != null)
                        {
                            var jvalue = ParseObject(value);
                            jobject.Add(member.Key, jvalue);
                        }
                    }
                }
                if (JsonConvert.IncludeProperties)
                {
                    var nameToProperties = GetProperties(type);
                    foreach (var member in nameToProperties)
                    {
                        object value = member.Value.GetValue(obj, null);
                        if (value != null)
                        {
                            var jvalue = ParseObject(value);
                            jobject.Add(member.Key, jvalue);
                        }
                    }
                }
                return jobject;
            }
        }

        /// <summary>
        /// Parse value from given type and JsonValue
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private object ParseValue(JsonValue json)
        {
            if (json.IsNull)
                return null;

            if (json.IsObject)
            {
                var jsonObject = json as JsonObject;
                var dict = new Dictionary<string, object>(jsonObject.Count);
                foreach (var jv in jsonObject)
                {
                    dict[jv.Key] = ParseValue(jv.Value);
                }
                return dict;
            }
            if (json.IsArray)
            {
                var jsonArray = json as JsonArray;
                var finalList = new List<object>(jsonArray.Count);
                for (int i = 0; i < jsonArray.Count; i++)
                    finalList.Add(ParseValue(jsonArray[i]));
                return finalList;
            }
            if (json.IsString)
            {
                return json.Value;
            }
            if (json.IsNumber)
            {
                if (json.Value.Contains("."))
                {
                    double.TryParse(json.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double result);
                    return result;
                }
                else
                {
                    int.TryParse(json.Value, out int result);
                    return result;
                }
            }
            if (json.IsBoolean)
                return json.ToBolean();

            // handles json == "null" as well as invalid JSON
            return null;
        }

        /// <summary>
        /// Parse value from given JsonValue. In most cases this primitive data bave to be convered from JsonValue.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private object ParseValue(Type type, JsonValue json)
        {
            if (json.IsNull)
                return null;
            if (type == typeof(string))
            {
                if (json.IsString)
                    return json.Value;
                return null;
            }
            if (type == typeof(char))
            {
                if (json.IsString)
                {
                    if (json.Value.Length == 1)
                        return json.Value[0];
                    if (json.Value.Length > 1)
                        return json[1];
                }
                return null;
            }
            if (type == typeof(bool))
            {
                if (json.IsBoolean)
                    return json.ToBolean();
            }
            if (type.IsPrimitive)
            {
                if (json.IsNumber)
                {
                    var result = Convert.ChangeType(json.Value, type, System.Globalization.CultureInfo.InvariantCulture);
                    return result;
                }
            }
            if (type == typeof(decimal))
            {
                if (json.IsNumber)
                {
                    decimal.TryParse(json.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out decimal result);
                    return result;
                }
            }
            if (type == typeof(DateTime))
            {
                if (json.IsString)
                {
                    DateTime.TryParse(json.Value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result);
                    return result;
                }
            }
            if (type.IsEnum)
            {
                if (json.IsString || json.IsNumber)
                {
                    try
                    {
                        return Enum.Parse(type, json.Value, false);
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            if (type.IsArray)
            {
                Type arrayType = type.GetElementType();
                if (json.IsArray == false)
                    return null;

                var jsonArray = json as JsonArray;
                Array newArray = Array.CreateInstance(arrayType, jsonArray.Count);
                int i = 0;
                foreach (var jv in jsonArray)
                {
                    newArray.SetValue(ParseValue(arrayType, jv), i++);
                }
                return newArray;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type listType = type.GetGenericArguments()[0];
                if (json.IsArray == false)
                    return null;

                var jsonArray = json as JsonArray;
                var list = (IList)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { jsonArray.Count });
                foreach (var jv in jsonArray)
                    list.Add(ParseValue(listType, jv));
                return list;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type keyType, valueType;
                {
                    Type[] args = type.GetGenericArguments();
                    keyType = args[0];
                    valueType = args[1];
                }

                //Refuse to parse dictionary keys that aren't of type string
                if (keyType != typeof(string))
                    return null;

                //refuse to parse non JsonObjects
                if (json.IsObject == false)
                    return null;

                var jsonObject = json as JsonObject;
                var dictionary = (IDictionary)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { jsonObject.Count / 2 });

                foreach (var jv in jsonObject)
                {
                    object val = ParseValue(valueType, jv.Value);
                    dictionary[jv.Key] = val;
                }
                return dictionary;
            }
            if (json.IsObject)
            {
                return ParseObject(type, json);
            }
            if (type == typeof(object))
            {
                return ParseValue(json);
            }
            // signal that value has not been parsed to the specific type
            return null;
        }

        /// <summary>
        /// Parse object of specific type from JsonValue;
        /// </summary>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private object ParseObject(Type type, JsonValue json)
        {
            object instance = FormatterServices.GetUninitializedObject(type);

            if (json.IsObject == false)
                return instance;

            var nameToField = GetFields(type);
            var nameToProperty = GetProperties(type);
            var jsonObject = json as JsonObject;
            foreach (var jv in jsonObject)
            {
                if (JsonConvert.IncludeFields && nameToField.TryGetValue(jv.Key, out FieldInfo fieldInfo))
                    fieldInfo.SetValue(instance, ParseValue(fieldInfo.FieldType, jv.Value));
                else if (JsonConvert.IncludeProperties && nameToProperty.TryGetValue(jv.Key, out PropertyInfo propertyInfo))
                    propertyInfo.SetValue(instance, ParseValue(propertyInfo.PropertyType, jv.Value), null);
            }
            return instance;
        }

        /// <summary>
        /// Get members in a dictionary for a specific type
        /// </summary>
        /// <typeparam name="T">Type to get fiels/properties</typeparam>
        /// <param name="members">Members in array</param>
        /// <returns></returns>
        private Dictionary<string, T> CreateMemberNameDictionary<T>(T[] members) where T : MemberInfo
        {
            Dictionary<string, T> nameToMember = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < members.Length; i++)
            {
                T member = members[i];
                if (member.IsDefined(typeof(JsonIgnore), true))
                    continue;

                string name = member.Name;
                if (member.IsDefined(typeof(JsonProperty), true))
                {
                    JsonProperty jsonProperty = (JsonProperty)Attribute.GetCustomAttribute(member, typeof(JsonProperty), true);
                    if (!string.IsNullOrEmpty(jsonProperty.Name))
                        name = jsonProperty.Name;
                }
                nameToMember.Add(name, member);
            }
            return nameToMember;
        }

        /// <summary>
        /// Internal method to get fields of the object stored in cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Dictionary<string, FieldInfo> GetFields(Type type)
        {
            Dictionary<string, FieldInfo> nameToField;

            lock (_sync)
            {
                if (!_fieldInfoCached.TryGetValue(type, out nameToField))
                {
                    nameToField = CreateMemberNameDictionary(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                    _fieldInfoCached[type] = nameToField;
                }
            }
            return nameToField;
        }

        /// <summary>
        /// Internal method to get properties of the object stored in cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Dictionary<string, PropertyInfo> GetProperties(Type type)
        {
            Dictionary<string, PropertyInfo> nameToProperty;
            lock (_sync)
            {
                if (!_propertyInfoCached.TryGetValue(type, out nameToProperty))
                {
                    nameToProperty = CreateMemberNameDictionary(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                    _propertyInfoCached[type] = nameToProperty;
                }
            }
            return nameToProperty;
        }
    }
}