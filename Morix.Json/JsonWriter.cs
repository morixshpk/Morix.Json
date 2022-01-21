﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace Morix.Json
{
	using ErrorType = JsonSerializationException.ErrorType;

	/// <summary>
	/// Represents a TextWriter adapter that can write string representations of JsonValues.
	/// </summary>
	internal sealed class JsonWriter
	{
		private int indent;
		private bool isNewLine;

		/// <summary>
		/// Gets or sets the string representing a indent in the output.
		/// </summary>
		public string IndentString { get; set; }

		/// <summary>
		/// Gets or sets the string representing a space in the output.
		/// </summary>
		public string SpacingString { get; set; }

		/// <summary>
		/// Gets or sets the string representing a new line on the output.
		/// </summary>
		public string NewLineString { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether JsonObject properties should be written in a deterministic order.
		/// </summary>
		public bool SortObjects { get; set; }

		/// <summary>
		/// Gets or sets the TextWriter to which this JsonWriter writes.
		/// </summary>
		public TextWriter InnerWriter { get; set; }

		/// <summary>
		/// Initializes a new instance of JsonWriter.
		/// </summary>
		/// <param name="innerWriter">The TextWriter used to write JsonValues.</param>
		public JsonWriter(TextWriter innerWriter)
		{
			if (JsonConvert.Beautify)
			{
				this.IndentString = "\t";
				this.SpacingString = " ";
				this.NewLineString = "\n";
			}

			InnerWriter = innerWriter;
		}

		private void Write(string text)
		{
			if (this.isNewLine)
			{
				this.isNewLine = false;
				WriteIndentation();
			}

			InnerWriter.Write(text);
		}

		private void WriteEncodedJsonValue(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
					Write("null");
					break;

				case JsonValueType.Boolean:
					Write(value.Value);
					break;

				case JsonValueType.Number:

					Write(value.Value);
					break;

				case JsonValueType.String:
					WriteEncodedString(value.Value);
					break;

				case JsonValueType.Object:
					Write(string.Format("JsonObject[{0}]", value.Count));
					break;

				case JsonValueType.Array:
					Write(string.Format("JsonArray[{0}]", value.Count));
					break;

				default:
					throw new InvalidOperationException("Invalid value type.");
			}
		}

		private void WriteEncodedString(string text)
		{
			Write("\"");

			for (int i = 0; i < text.Length; i += 1)
			{
				var currentChar = text[i];

				// Encoding special characters.
				switch (currentChar)
				{
					case '\\':
						InnerWriter.Write("\\\\");
						break;

					case '\"':
						InnerWriter.Write("\\\"");
						break;

					case '/':
						InnerWriter.Write("\\/");
						break;

					case '\b':
						InnerWriter.Write("\\b");
						break;

					case '\f':
						InnerWriter.Write("\\f");
						break;

					case '\n':
						InnerWriter.Write("\\n");
						break;

					case '\r':
						InnerWriter.Write("\\r");
						break;

					case '\t':
						InnerWriter.Write("\\t");
						break;

					default:
						InnerWriter.Write(currentChar);
						break;
				}
			}

			InnerWriter.Write("\"");
		}

		private void WriteIndentation()
		{
			for (var i = 0; i < this.indent; i += 1)
			{
				Write(this.IndentString);
			}
		}

		private void WriteSpacing()
		{
			Write(this.SpacingString);
		}

		private void WriteLine()
		{
			Write(this.NewLineString);
			this.isNewLine = true;
		}

		private void WriteLine(string line)
		{
			Write(line);
			WriteLine();
		}

		private void Render(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
				case JsonValueType.Boolean:
				case JsonValueType.Number:
				case JsonValueType.String:
					WriteEncodedJsonValue(value);
					break;

				case JsonValueType.Object:
					Render((JsonObject)value);
					break;

				case JsonValueType.Array:
					Render((JsonArray)value);
					break;

				default:
					throw new JsonSerializationException(ErrorType.InvalidValueType);
			}
		}

		private void Render(JsonArray value)
		{
			WriteLine("[");

			indent += 1;

			using (var enumerator = value.GetEnumerator())
			{
				var hasNext = enumerator.MoveNext();

				while (hasNext)
				{
					Render(enumerator.Current);

					hasNext = enumerator.MoveNext();

					if (hasNext)
					{
						WriteLine(",");
					}
					else
					{
						WriteLine();
					}
				}
			}

			indent -= 1;

			Write("]");
		}

		private void Render(JsonObject value)
		{
			WriteLine("{");

			indent += 1;

			using(var enumerator = GetJsonObjectEnumerator(value))
			{
				var hasNext = enumerator.MoveNext();

				while (hasNext)
				{
					WriteEncodedString(enumerator.Current.Key);
					Write(":");
					WriteSpacing();
					Render(enumerator.Current.Value);

					hasNext = enumerator.MoveNext();

					if (hasNext)
					{
						WriteLine(",");
					}
					else
					{
						WriteLine();
					}
				}
			}

			indent -= 1;

			Write("}");
		}

		/// <summary>
		/// Gets an JsonObject enumarator based on the configuration of this JsonWriter.
		/// If JsonWriter.SortObjects is set to true, then a ordered enumerator is returned.
		/// Otherwise, a faster non-deterministic enumerator is returned.
		/// </summary>
		/// <param name="jsonObject">The JsonObject for which to get an enumerator.</param>
		private IEnumerator<KeyValuePair<string, JsonValue>> GetJsonObjectEnumerator(JsonObject jsonObject)
		{
			if (this.SortObjects)
			{
				var sortedDictionary = new SortedDictionary<string, JsonValue>(StringComparer.Ordinal);

				foreach (var item in jsonObject)
				{
					sortedDictionary.Add(item.Key, item.Value);
				}

				return sortedDictionary.GetEnumerator();
			}
			else
			{
				return jsonObject.GetEnumerator();
			}
		}

		/// <summary>
		/// Writes the given value to the InnerWriter.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to write.</param>
		public void Write(JsonValue jsonValue)
		{
			this.indent = 0;
			this.isNewLine = true;

			Render(jsonValue);
		}

		/// <summary>
		/// Generates a string representation of the given value.
		/// </summary>
		/// <param name="value">The value to serialize.</param>
		public static string Serialize(JsonValue value)
		{
			using (var stringWriter = new StringWriter())
			{
				var jsonWriter = new JsonWriter(stringWriter);

				jsonWriter.Write(value);

				return stringWriter.ToString();
			}
		}
	}
}
