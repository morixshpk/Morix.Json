using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;


namespace Morix.Json.Tests
{
    [TestClass]
	public partial class Tests
	{
		[TestMethod]
		public void TestDebug()
		{
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
		}

		[TestMethod]
		public void TestBasicTypesDeserialize()
		{
			InnerTest("null");
			InnerTest("true");
			InnerTest("false");
			InnerTest("123");
			InnerTest("-123");
			InnerTest("123.45");
			InnerTest("-123.45");
			InnerTest("\"morix\"");
			InnerTest("[1,2,3]");
			InnerTest("{\"a\":3,\"b\":[1,2,3]}");
			InnerTest("{\"a\":true,\"b\":[1,2,3]}");
			InnerTest("{\"a\":false,\"b\":[1,2,3]}");
			InnerTest("{\"a\":\"prova\",\"b\":[1,2,3]}");
			InnerTest("{\"a\":\"null\",\"b\":[-1,2,1.12345]}");
		}

		private void InnerTest(string jsonText)
		{
			var jsonValue = JsonConvert.Parse(jsonText);
			Assert.IsNotNull(jsonValue);
			Assert.AreEqual(jsonText, jsonValue.ToString());
			//Debug.Print(jsonText + " " + jsonValue.Type.ToString());
		}

		[TestMethod]
		public void TestParseExampleMessage()
		{
			var message = @"
				{
					""menu"": [
						""home"",
						""projects"",
						""about""
					]
				}
			";

			var json = JsonConvert.Parse(message);

			Assert.IsTrue(json.IsObject);

			Assert.AreEqual(1, json.Count);
			Assert.IsTrue(json.ContainsKey("menu"));

			var items = json["menu"];

			Assert.IsNotNull(items);
			Assert.AreEqual(3, items.Count);
			Assert.IsTrue(items.Contains("home"));
			Assert.IsTrue(items.Contains("projects"));
			Assert.IsTrue(items.Contains("about"));
		}

		[TestMethod]
		public void TestSerializeExampleMessage()
		{
			var json = new JsonObject
			{
				["menu"] = new JsonArray
				{
					"home",
					"projects",
					"about",
				}
			};

			var expectedMessage = @"{""menu"":[""home"",""projects"",""about""]}";

			Assert.AreEqual(expectedMessage, json.ToString());
		}

		[TestMethod]
		public void TestNumbers()
		{
			double d = 1.0;
			var json = JsonConvert.Parse(d);
			Assert.AreEqual(d, json.ToDouble());

			d = 1.0E+2;
			json = JsonConvert.Parse(d.ToString());
			Assert.AreEqual(d, json.ToDouble());
		}

		[TestMethod]
		public void TestPrimitivesTypes()
		{
			var obj1 = Morix.Json.Tests.PrimitiveObject.Create();
			var json1 = JsonConvert.Serialize(obj1);

			var obj2 = JsonConvert.Deserialize<PrimitiveObject>(json1);
			var json2 = JsonConvert.Serialize(obj2);

			Assert.AreEqual(json1, json2);
			Assert.AreEqual(obj1.Bool, obj2.Bool);
			Assert.AreEqual(obj1.Byte, obj2.Byte);
			Assert.AreEqual(obj1.SByte, obj2.SByte);
			Assert.AreEqual(obj1.Short, obj2.Short);
			Assert.AreEqual(obj1.UShort, obj2.UShort);
			Assert.AreEqual(obj1.Int, obj2.Int);
			Assert.AreEqual(obj1.UInt, obj2.UInt);
			Assert.AreEqual(obj1.Long, obj2.Long);
			Assert.AreEqual(obj1.ULong, obj2.ULong);
			Assert.AreEqual(obj1.Single, obj2.Single);
			Assert.AreEqual(obj1.Double, obj2.Double);
			Assert.AreEqual(obj1.Decimal, obj2.Decimal);
			Assert.AreEqual(obj1.Time, obj2.Time);
			Assert.AreEqual(obj1.Char, obj2.Char);
			Assert.AreEqual(obj1.String, obj2.String);

			CollectionAssert.AreEqual(obj1.ListOfInt, obj2.ListOfInt);
			CollectionAssert.AreEqual(obj1.DictOfStrings, obj2.DictOfStrings);
		}

		static void Test<T>(T expected, string json)
		{
			T value = Morix.Json.JsonConvert.Deserialize<T>(json);

			Assert.AreEqual(expected, value);
		}

		[TestMethod]
		public void TestDifferentValues()
		{
			Test(12345, "12345");
			Test(12345L, "12345");
			Test(12345UL, "12345");
			Test(12.532f, "12.532");
			Test(12.532m, "12.532");
			Test(12.532d, "12.532");
			Test("hello", "\"hello\"");
			Test("hello there", "\"hello there\"");
			Test("hello\nthere", "\"hello\nthere\"");
			Test("hello\"there", "\"hello\\\"there\"");
			Test(true, "true");
			Test(false, "false");
			Test<object>(null, "sfdoijsdfoij");
			Test(Color.Green, "\"Green\"");
			Test(Color.Blue, "2");
			Test(Color.Blue, "\"2\"");
			Test(Color.Red, "\"sfdoijsdfoij\"");
			Test(Style.Bold | Style.Italic, "\"Bold, Italic\"");
			Test(Style.Bold | Style.Italic, "3");
			Test("\u94b1\u4e0d\u591f!", "\"\u94b1\u4e0d\u591f!\"");
			Test("\u94b1\u4e0d\u591f!", "\"\\u94b1\\u4e0d\\u591f!\"");
		}

		static void ArrayTest<T>(T[] expected, string json)
		{
			var value = JsonConvert.Deserialize<T[]>(json);
			CollectionAssert.AreEqual(expected, value);
		}

		[TestMethod]
		public void TestArrayOfValues()
		{
			ArrayTest(new string[] { "one", "two", "three" }, "[\"one\",\"two\",\"three\"]");
			ArrayTest(new int[] { 1, 2, 3 }, "[1,2,3]");
			ArrayTest(new bool[] { true, false, true }, "     [true    ,    false,true     ]   ");
			ArrayTest(new object[] { null, null }, "[null,null]");
			ArrayTest(new float[] { 0.24f, 1.2f }, "[0.24,1.2]");
			ArrayTest(new double[] { 0.15, 0.19 }, "[0.15, 0.19]");
			ArrayTest<object>(null, "[garbled");
		}

		static void ListTest<T>(List<T> expected, string json)
		{
			var value = JsonConvert.Deserialize<List<T>>(json);
			CollectionAssert.AreEqual(expected, value);
		}

		[TestMethod]
		public void TestListOfValues()
		{
			ListTest(new List<string> { "one", "two", "three" }, "[\"one\",\"two\",\"three\"]");
			ListTest(new List<int> { 1, 2, 3 }, "[1,2,3]");
			ListTest(new List<bool> { true, false, true }, "     [true    ,    false,true     ]   ");
			ListTest(new List<object> { null, null }, "[null,null]");
			ListTest(new List<float> { 0.24f, 1.2f }, "[0.24,1.2]");
			ListTest(new List<double> { 0.15, 0.19 }, "[0.15, 0.19]");
			ListTest<object>(null, "[garbled");
		}

		[TestMethod]
		public void TestRecursiveLists()
		{
			var expected = new List<List<int>> { new List<int> { 1, 2 }, new List<int> { 3, 4 } };
			var actual = JsonConvert.Deserialize<List<List<int>>>("[[1,2],[3,4]]");
			Assert.AreEqual(expected.Count, actual.Count);
			for (int i = 0; i < expected.Count; i++)
				CollectionAssert.AreEqual(expected[i], actual[i]);
		}

		[TestMethod]
		public void TestRecursiveArrays()
		{
			var expected = new int[][] { new int[] { 1, 2 }, new int[] { 3, 4 } };
			var actual = JsonConvert.Deserialize<int[][]>("[[1,2],[3,4]]");
			Assert.AreEqual(expected.Length, actual.Length);
			for (int i = 0; i < expected.Length; i++)
				CollectionAssert.AreEqual(expected[i], actual[i]);
		}

		static void DictTest<K, V>(Dictionary<K, V> expected, string json)
		{
			var value = JsonConvert.Deserialize<Dictionary<K, V>>(json);
			Assert.AreEqual(expected.Count, value.Count);
			foreach (var pair in expected)
			{
				Assert.IsTrue(value.ContainsKey(pair.Key));
				Assert.AreEqual(pair.Value, value[pair.Key]);
			}
		}

		[TestMethod]
		public void TestDictionary()
		{
			DictTest(new Dictionary<string, int> { { "foo", 5 }, { "bar", 10 }, { "baz", 128 } }, "{\"foo\":5,\"bar\":10,\"baz\":128}");
			DictTest(new Dictionary<string, float> { { "foo", 5f }, { "bar", 10f }, { "baz", 128f } }, "{\"foo\":5,\"bar\":10,\"baz\":128}");
			DictTest(new Dictionary<string, string> { { "foo", "\"" }, { "bar", "hello" }, { "baz", "," } }, "{\"foo\":\"\\\"\",\"bar\":\"hello\",\"baz\":\",\"}");
		}

		[TestMethod]
		public void TestRecursiveDictionary()
		{
			var result = JsonConvert.Deserialize<Dictionary<string, Dictionary<string, string>>>
				("{\"foo\":{ \"bar\":\"\\\"{,,:[]}\" }}");
			Assert.AreEqual("\"{,,:[]}", result["foo"]["bar"]);
		}

		[TestMethod]
		public void TestSimpleObject()
		{
			OrderObject value = JsonConvert.Deserialize<OrderObject>("{\"A\":123,\"b\":456,\"C\":\"789\",\"D\":[10,11,12]}");
			Assert.IsNotNull(value);
			Assert.AreEqual(123, value.A);
			Assert.AreEqual(456f, value.B);
			Assert.AreEqual("789", value.C);
			CollectionAssert.AreEqual(new List<int> { 10, 11, 12 }, value.D);

			value = JsonConvert.Deserialize<OrderObject>("dfpoksdafoijsdfij");
			Assert.IsNull(value);
		}

		[TestMethod]
		public void TestSimpleStruct()
		{
			AStructWithClassMember value = JsonConvert.Deserialize<AStructWithClassMember>("{\"obj\":{\"A\":12345}}");
			Assert.IsNotNull(value.Obj);
			Assert.AreEqual(value.Obj.A, 12345);
		}

		[TestMethod]
		public void StructTests()
		{
			var items = JsonConvert.Deserialize<List<PriceStruct>>("[{\"Value\":1},{\"Value\":2},{\"Value\":3}]");
			for (int i = 0; i < items.Count; i++)
			{
				Assert.AreEqual(i + 1, items[i].Value);
			}
		}

		[TestMethod]
		public void TestDeepObject()
		{
			var value = JsonConvert.Deserialize<CompositeObject>("{\"C\":{\"C\":{\"C\":{}}}}");
			Assert.IsNotNull(value);
			Assert.IsNotNull(value.C);
			Assert.IsNotNull(value.C.C);
			Assert.IsNotNull(value.C.C.C);

			value = JsonConvert.Deserialize<CompositeObject>("{\"D\":[{},null,{\"C\":{}}]}");
			Assert.IsNotNull(value);
			Assert.IsNotNull(value.D);
			Assert.IsNotNull(value.D[0]);
			Assert.IsNull(value.D[1]);
			Assert.IsNotNull(value.D[2].A);

			value = JsonConvert.Deserialize<CompositeObject>("{\"E\":{\"Obj\":{\"A\":5}}}");
			Assert.IsNotNull(value);
			Assert.IsNotNull(value.E.Obj);
			Assert.AreEqual(5, value.E.Obj.A);
		}

		[TestMethod]
		public void PerformanceTest()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("[");
			const int numTests = 100000;
			for (int i = 0; i < numTests; i++)
			{
				builder.Append("{\"C\":{\"B\":10}}");
				if (i < numTests - 1)
					builder.Append(",");
			}
			builder.Append("]");

			string json = builder.ToString();
			var result = JsonConvert.Deserialize<List<CompositeObject>>(json);
			for (int i = 0; i < result.Count; i++)
				Assert.AreEqual(10, result[i].C.B);
		}

		[TestMethod]
		public void CorruptionTest()
		{
			JsonConvert.Deserialize<object>("{{{{{{[[[]]][[,,,,]],],],]]][[nulldsfoijsfd[[]]]]]]]]]}}}}}{{{{{{{{{D{FD{FD{F{{{{{}}}XXJJJI%&:,,,,,");
			JsonConvert.Deserialize<List<List<int>>>("[[,[,,[,:::[[[[[[[");
			JsonConvert.Deserialize<Dictionary<string, object>>("{::,[][][],::::,}");
		}

		[TestMethod]
		public void DynamicParserTest()
		{
			var list = JsonConvert.Deserialize<List<object>>("[0,1,2,3]");
			Assert.IsTrue(list.Count == 4 && (int)list[3] == 3);
			var dict = JsonConvert.Deserialize<Dictionary<string, object>>("{\"Foo\":\"Bar\"}");
			Assert.IsTrue((string)dict["Foo"] == "Bar");

			string expected = "{\"A\":123,\"B\":456,\"C\":\"789\",\"D\":[10,11,12]}";
			var obj = JsonConvert.Deserialize<Dictionary<string, object>>(expected);
			var actual = JsonConvert.Serialize(obj);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestNastyStruct()
		{
			NastyStruct s = JsonConvert.Deserialize<NastyStruct>("{\"R\":234,\"G\":123,\"B\":11}");
			Assert.AreEqual(234, s.R);
			Assert.AreEqual(123, s.G);
			Assert.AreEqual(11, s.B);
		}

		[TestMethod]
		public void TestEscaping()
		{
			var orig = new Dictionary<string, string> { { "hello", "world\n \" \\ \b \r \\0\u263A" } };
			var parsed = JsonConvert.Deserialize<Dictionary<string, string>>("{\"hello\":\"world\\n \\\" \\\\ \\b \\r \\0\\u263a\"}");
			Assert.AreEqual(orig["hello"], parsed["hello"]);
		}

		[TestMethod]
		public void TestMultithread()
		{
			// Lots of threads
			for (int i = 0; i < 100; i++)
			{
				new Thread(() =>
				{
					// Each threads has enough work to potentially hit a race condition
					for (int j = 0; j < 1000; j++)
					{
						TestValues();
						TestArrayOfValues();
						TestListOfValues();
						TestRecursiveLists();
						TestRecursiveArrays();
						TestDictionary();
						TestRecursiveDictionary();
						TestSimpleObject();
						TestSimpleStruct();
						StructTests();
						TestDeepObject();
						CorruptionTest();
						DynamicParserTest();
						TestNastyStruct();
						TestEscaping();
					}
				}).Start();
			}
		}

		[TestMethod]
		public void TestIgnoreDataMember()
		{
			var value = JsonConvert.Deserialize<IgnoreDataMemberObject>("{\"A\":123,\"B\":456,\"Ignored\":10,\"C\":789,\"D\":14}");
			Assert.IsNotNull(value);
			Assert.AreEqual(123, value.A);
			Assert.AreEqual(0, value.B);
			Assert.AreEqual(789, value.C);
			Assert.AreEqual(0, value.D);
		}

		[TestMethod]
		public void TestDataMemberObject()
		{
			DataMemberObject value = JsonConvert.Deserialize<DataMemberObject>("{\"a\":123,\"B\":456,\"c\":789,\"D\":14}");
			Assert.IsNotNull(value);
			Assert.AreEqual(123, value.A);
			Assert.AreEqual(456, value.B);
			Assert.AreEqual(789, value.C);
			Assert.AreEqual(14, value.D);
		}

		[TestMethod]
		public void TestEnumMember()
		{
			EnumClass value = JsonConvert.Deserialize<EnumClass>("{\"Colors\":\"Green\",\"Style\":\"Bold, Underline\"}");
			Assert.IsNotNull(value);
			Assert.AreEqual(Color.Green, value.Colors);
			Assert.AreEqual(Style.Bold | Style.Underline, value.Style);

			value = JsonConvert.Deserialize<EnumClass>("{\"Colors\":3,\"Style\":10}");
			Assert.IsNotNull(value);
			Assert.AreEqual(Color.Yellow, value.Colors);
			Assert.AreEqual(Style.Italic | Style.Strikethrough, value.Style);

			value = JsonConvert.Deserialize<EnumClass>("{\"Colors\":\"3\",\"Style\":\"10\"}");
			Assert.IsNotNull(value);
			Assert.AreEqual(Color.Yellow, value.Colors);
			Assert.AreEqual(Style.Italic | Style.Strikethrough, value.Style);

			value = JsonConvert.Deserialize<EnumClass>("{\"Colors\":\"sfdoijsdfoij\",\"Style\":\"sfdoijsdfoij\"}");
			Assert.IsNotNull(value);
			Assert.AreEqual(Color.Red, value.Colors);
			Assert.AreEqual(Style.None, value.Style);
		}

		[TestMethod]
		public void TestDuplicateKeysInAnonymousObject()
		{
			var parsed = JsonConvert.Deserialize<object>(@"{""hello"": ""world"", ""goodbye"": ""heaven"", ""hello"": ""hell""}");
			var dictionary = (Dictionary<string, object>)parsed;

			Assert.IsTrue(dictionary.ContainsKey("hello"), "The dictionary is missing the duplicated key");

			Assert.IsTrue(dictionary.ContainsKey("goodbye"), "The dictionary is missing the non-duplicated key");
			Assert.AreEqual(dictionary["hello"], "hell", "The parser stored an incorrect value for the duplicated key");
		}

		[TestMethod]
		public void TestValues()
		{
			Assert.AreEqual("\"\u94b1\u4e0d\u591f!\"", JsonConvert.Serialize("\u94b1\u4e0d\u591f!"));
			Assert.AreEqual("123", JsonConvert.Serialize(123));
			Assert.AreEqual("true", JsonConvert.Serialize(true));
			Assert.AreEqual("false", JsonConvert.Serialize(false));
			Assert.AreEqual("[1,2,3]", JsonConvert.Serialize(new int[] { 1, 2, 3 }));
			Assert.AreEqual("[1,2,3]", JsonConvert.Serialize(new List<int> { 1, 2, 3 }));
			Assert.AreEqual("\"Green\"", JsonConvert.Serialize(Color.Green));
			Assert.AreEqual("\"Green\"", JsonConvert.Serialize((Color)1));
			Assert.AreEqual("\"10\"", JsonConvert.Serialize((Color)10));
			Assert.AreEqual("\"Bold\"", JsonConvert.Serialize(Style.Bold));
			Assert.AreEqual("\"Bold, Italic\"", JsonConvert.Serialize(Style.Bold | Style.Italic));
			Assert.AreEqual("\"19\"", JsonConvert.Serialize(Style.Bold | Style.Italic | (Style)16));
		}

		[TestMethod]
		public void TestObjects()
		{
			var obj1 = new OrderObject
			{
				A = 123,
				B = 1.12f,
				C = "Test",
				D = new List<int> { 1, 2, 3 }
			};

			var json = JsonConvert.Serialize(obj1);

			var obj2 = JsonConvert.Deserialize<OrderObject>(json);

			Assert.IsNotNull(obj2);
			Assert.AreEqual(json, JsonConvert.Serialize(obj2));

			var obj3 = new AStructWithClassMember { Obj = obj1 };

			Assert.AreEqual("{\"Obj\":{\"A\":123,\"B\":1.12,\"C\":\"Test\",\"D\":[1,2,3]}}",
				JsonConvert.Serialize(obj3));

			var obj4 = new InheritedClass
			{
				A = 123,
				B = 1.12f,
				C = "Test",
				D = new List<int> { 1, 2, 3 },
				X = 9
			};

			Assert.AreEqual("{\"X\":9,\"A\":123,\"B\":1.12,\"C\":\"Test\",\"D\":[1,2,3]}",
				JsonConvert.Serialize(obj4));
		}

		[TestMethod]
		public void TestIgnoreDataMemberObject()
		{
			Assert.AreEqual("{\"A\":10,\"C\":30}",
				JsonConvert.Serialize(
				new IgnoreDataMemberObject
				{
					A = 10,
					B = 20,
					C = 30,
					D = 40
				}));
		}

		public void TestOrderItem()
		{
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

		}
	}
}