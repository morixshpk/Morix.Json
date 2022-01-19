﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Morix.Json
{
    public static class JsonConvert
    {
        [ThreadStatic] static Stack<List<string>> arrayPool;
        [ThreadStatic] static StringBuilder builder;

        [ThreadStatic] static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoCache;
        [ThreadStatic] static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyInfoCache;

        static JsonConvert()
        {
            propertyInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
            fieldInfoCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();
        }

        public static string Serialize(object obj)
        {
            var json = Parse(obj);
            return json.ToString();
        }

        public static JsonValue Parse(string json)
        {
            return JsonReader.Parse(json);
        }

        public static JsonValue Parse(object obj)
        {
            if (obj == null)
                return JsonNull.Null;

            var type = obj.GetType();
            if (type == typeof(string))
            {
                return new JsonString(obj.ToString());
            }
            else if (type == typeof(char))
            {
                return new JsonString(obj.ToString());
            }
            else if (type == typeof(bool))
            {
                return new JsonBoolean((bool)obj);
            }
            else if (type == typeof(byte))
            {
                return new JsonNumber((byte)obj);
            }
            else if (type == typeof(sbyte))
            {
                return new JsonNumber((sbyte)obj);
            }
            else if (type == typeof(short))
            {
                return new JsonNumber((short)obj);
            }
            else if (type == typeof(ushort))
            {
                return new JsonNumber((ushort)obj);
            }
            else if (type == typeof(int))
            {
                return new JsonNumber((int)obj);
            }
            else if (type == typeof(uint))
            {
                return new JsonNumber((uint)obj);
            }
            else if (type == typeof(long))
            {
                return new JsonNumber((long)obj);
            }
            else if (type == typeof(ulong))
            {
                return new JsonNumber((ulong)obj);
            }
            else if (type == typeof(float))
            {
                return new JsonNumber((float)obj);
            }
            else if (type == typeof(double))
            {
                return new JsonNumber((double)obj);
            }
            else if (type == typeof(decimal))
            {
                return new JsonNumber((decimal)obj);
            }
            else if (type == typeof(DateTime))
            {
                return new JsonString((DateTime)obj);
            }
            else if (type.IsEnum)
            {
                return new JsonString(obj.ToString());
            }
            else if (obj is IList list)
            {
                var jarray = new JsonArray();
                for (int i = 0; i < list.Count; i++)
                {
                    jarray.Add(Parse(list[i]));
                }
                return jarray;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                IDictionary dict = obj as IDictionary;
                var jobject = new JsonObject();
                foreach (var key in dict.Keys)
                {
                    jobject.Add(key.ToString(), Parse(dict[key]));
                }
                return jobject;
            }
            else
            {
                var jobject = new JsonObject();
                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    if (fieldInfos[i].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                        continue;

                    object value = fieldInfos[i].GetValue(obj);

                    if (value != null)
                    {
                        var name = GetMemberName(fieldInfos[i]);
                        var jvalue = Parse(value);
                        jobject.Add(name, jvalue);
                    }
                }
                PropertyInfo[] propertyInfo = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    if (!propertyInfo[i].CanRead || propertyInfo[i].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                        continue;

                    object value = propertyInfo[i].GetValue(obj, null);
                    if (value != null)
                    {
                        var name = GetMemberName(propertyInfo[i]);
                        var jvalue = Parse(value);
                        jobject.Add(name, jvalue);
                    }
                }

                return jobject;
            }
        }

        static string GetMemberName(MemberInfo member)
        {
            if (member.IsDefined(typeof(DataMemberAttribute), true))
            {
                DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(member, typeof(DataMemberAttribute), true);
                if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
                    return dataMemberAttribute.Name;
            }

            return member.Name;
        }

        public static T Deserialize<T>(this string json)
        {
            // Initialize, if needed, the ThreadStatic variables
            if (propertyInfoCache == null) propertyInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
            if (fieldInfoCache == null) fieldInfoCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();
            if (builder == null) builder = new StringBuilder();
            if (arrayPool == null) arrayPool = new Stack<List<string>>();

            //Remove all whitespace not within strings to make parsing simpler
            builder.Length = 0;
            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '"')
                {
                    i = AppendUntilStringEnd(true, i, json);
                    continue;
                }
                if (char.IsWhiteSpace(c))
                    continue;

                builder.Append(c);
            }

            //Parse the thing!
            return (T)ParseValue(typeof(T), builder.ToString());
        }

        static int AppendUntilStringEnd(bool appendEscapeCharacter, int startIdx, string json)
        {
            builder.Append(json[startIdx]);
            for (int i = startIdx + 1; i < json.Length; i++)
            {
                if (json[i] == '\\')
                {
                    if (appendEscapeCharacter)
                        builder.Append(json[i]);
                    builder.Append(json[i + 1]);
                    i++;//Skip next character as it is escaped
                }
                else if (json[i] == '"')
                {
                    builder.Append(json[i]);
                    return i;
                }
                else
                    builder.Append(json[i]);
            }
            return json.Length - 1;
        }

        //Splits { <value>:<value>, <value>:<value> } and [ <value>, <value> ] into a list of <value> strings
        static List<string> Split(string json)
        {
            List<string> splitArray = arrayPool.Count > 0 ? arrayPool.Pop() : new List<string>();
            splitArray.Clear();
            if (json.Length == 2)
                return splitArray;
            int parseDepth = 0;
            builder.Length = 0;
            for (int i = 1; i < json.Length - 1; i++)
            {
                switch (json[i])
                {
                    case '[':
                    case '{':
                        parseDepth++;
                        break;
                    case ']':
                    case '}':
                        parseDepth--;
                        break;
                    case '"':
                        i = AppendUntilStringEnd(true, i, json);
                        continue;
                    case ',':
                    case ':':
                        if (parseDepth == 0)
                        {
                            splitArray.Add(builder.ToString());
                            builder.Length = 0;
                            continue;
                        }
                        break;
                }

                builder.Append(json[i]);
            }

            splitArray.Add(builder.ToString());

            return splitArray;
        }

        static object ParseValue(Type type, string json)
        {
            if (type == typeof(string))
            {
                if (json.Length <= 2)
                    return string.Empty;
                StringBuilder parseStringBuilder = new StringBuilder(json.Length);
                for (int i = 1; i < json.Length - 1; ++i)
                {
                    if (json[i] == '\\' && i + 1 < json.Length - 1)
                    {
                        int j = "\"\\nrtbf/".IndexOf(json[i + 1]);
                        if (j >= 0)
                        {
                            parseStringBuilder.Append("\"\\\n\r\t\b\f/"[j]);
                            ++i;
                            continue;
                        }
                        if (json[i + 1] == 'u' && i + 5 < json.Length - 1)
                        {
                            if (UInt32.TryParse(json.Substring(i + 2, 4), System.Globalization.NumberStyles.AllowHexSpecifier, null, out uint c))
                            {
                                parseStringBuilder.Append((char)c);
                                i += 5;
                                continue;
                            }
                        }
                    }
                    parseStringBuilder.Append(json[i]);
                }
                return parseStringBuilder.ToString();
            }
            if (type == typeof(char))
            {
                if (json.Length == 1)
                    return json[0];
                if (json.Length == 3)
                    return json[1];
                return null;
            }
            if (type.IsPrimitive)
            {
                var result = Convert.ChangeType(json, type, System.Globalization.CultureInfo.InvariantCulture);
                return result;
            }
            if (type == typeof(decimal))
            {
                decimal.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out decimal result);
                return result;
            }
            if (type == typeof(DateTime))
            {
                DateTime.TryParse(json.Replace("\"", ""), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result);
                return result;
            }
            if (json == "null")
            {
                return null;
            }
            if (type.IsEnum)
            {
                if (json[0] == '"')
                    json = json.Substring(1, json.Length - 2);
                try
                {
                    return Enum.Parse(type, json, false);
                }
                catch
                {
                    return 0;
                }
            }
            if (type.IsArray)
            {
                Type arrayType = type.GetElementType();
                if (json[0] != '[' || json[json.Length - 1] != ']')
                    return null;

                List<string> elems = Split(json);
                Array newArray = Array.CreateInstance(arrayType, elems.Count);
                for (int i = 0; i < elems.Count; i++)
                    newArray.SetValue(ParseValue(arrayType, elems[i]), i);
                arrayPool.Push(elems);
                return newArray;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type listType = type.GetGenericArguments()[0];
                if (json[0] != '[' || json[json.Length - 1] != ']')
                    return null;

                List<string> elems = Split(json);
                var list = (IList)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { elems.Count });
                for (int i = 0; i < elems.Count; i++)
                    list.Add(ParseValue(listType, elems[i]));
                arrayPool.Push(elems);
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
                //Must be a valid dictionary element
                if (json[0] != '{' || json[json.Length - 1] != '}')
                    return null;
                //The list is split into key/value pairs only, this means the split must be divisible by 2 to be valid JSON
                List<string> elems = Split(json);
                if (elems.Count % 2 != 0)
                    return null;

                var dictionary = (IDictionary)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { elems.Count / 2 });
                for (int i = 0; i < elems.Count; i += 2)
                {
                    if (elems[i].Length <= 2)
                        continue;
                    string keyValue = elems[i].Substring(1, elems[i].Length - 2);
                    object val = ParseValue(valueType, elems[i + 1]);
                    dictionary[keyValue] = val;
                }
                return dictionary;
            }
            if (type == typeof(object))
            {
                return ParseAnonymousValue(json);
            }
            if (json[0] == '{' && json[json.Length - 1] == '}')
            {
                return ParseObject(type, json);
            }

            return null;
        }

        static object ParseAnonymousValue(string json)
        {
            if (json.Length == 0)
                return null;
            if (json[0] == '{' && json[json.Length - 1] == '}')
            {
                List<string> elems = Split(json);
                if (elems.Count % 2 != 0)
                    return null;
                var dict = new Dictionary<string, object>(elems.Count / 2);
                for (int i = 0; i < elems.Count; i += 2)
                    dict[elems[i].Substring(1, elems[i].Length - 2)] = ParseAnonymousValue(elems[i + 1]);
                return dict;
            }
            if (json[0] == '[' && json[json.Length - 1] == ']')
            {
                List<string> items = Split(json);
                var finalList = new List<object>(items.Count);
                for (int i = 0; i < items.Count; i++)
                    finalList.Add(ParseAnonymousValue(items[i]));
                return finalList;
            }
            if (json[0] == '"' && json[json.Length - 1] == '"')
            {
                string str = json.Substring(1, json.Length - 2);
                return str.Replace("\\", string.Empty);
            }
            if (char.IsDigit(json[0]) || json[0] == '-')
            {
                if (json.Contains("."))
                {
                    double.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double result);
                    return result;
                }
                else
                {
                    int.TryParse(json, out int result);
                    return result;
                }
            }
            if (json == "true")
                return true;
            if (json == "false")
                return false;
            // handles json == "null" as well as invalid JSON
            return null;
        } 
        
        static object ParseObject(Type type, string json)
        {
            object instance = FormatterServices.GetUninitializedObject(type);

            //The list is split into key/value pairs only, this means the split must be divisible by 2 to be valid JSON
            List<string> elems = Split(json);
            if (elems.Count % 2 != 0)
                return instance;

            if (!fieldInfoCache.TryGetValue(type, out Dictionary<string, FieldInfo> nameToField))
            {
                nameToField = CreateMemberNameDictionary(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                fieldInfoCache.Add(type, nameToField);
            }
            if (!propertyInfoCache.TryGetValue(type, out Dictionary<string, PropertyInfo> nameToProperty))
            {
                nameToProperty = CreateMemberNameDictionary(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                propertyInfoCache.Add(type, nameToProperty);
            }

            for (int i = 0; i < elems.Count; i += 2)
            {
                if (elems[i].Length <= 2)
                    continue;
                string key = elems[i].Substring(1, elems[i].Length - 2);
                string value = elems[i + 1];

                if (nameToField.TryGetValue(key, out FieldInfo fieldInfo))
                    fieldInfo.SetValue(instance, ParseValue(fieldInfo.FieldType, value));
                else if (nameToProperty.TryGetValue(key, out PropertyInfo propertyInfo))
                    propertyInfo.SetValue(instance, ParseValue(propertyInfo.PropertyType, value), null);
            }

            return instance;
        }

        static Dictionary<string, T> CreateMemberNameDictionary<T>(T[] members) where T : MemberInfo
        {
            Dictionary<string, T> nameToMember = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < members.Length; i++)
            {
                T member = members[i];
                if (member.IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    continue;

                string name = member.Name;
                if (member.IsDefined(typeof(DataMemberAttribute), true))
                {
                    DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(member, typeof(DataMemberAttribute), true);
                    if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
                        name = dataMemberAttribute.Name;
                }

                nameToMember.Add(name, member);
            }

            return nameToMember;
        }
    }
}