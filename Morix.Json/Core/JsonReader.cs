using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace Morix.Json
{
	using ErrorType = JsonParseException.ErrorType;

	/// <summary>
	/// Represents a reader that can read JsonValues.
	/// </summary>
	internal sealed class JsonReader
	{
		private readonly TextScanner _scanner;

		private JsonReader(TextReader reader)
		{
			this._scanner = new TextScanner(reader);
		}

		private string ReadJsonKey()
		{
			return ReadString();
		}

		private JsonValue ReadJsonValue()
		{
			this._scanner.SkipWhitespace();

			var next = this._scanner.Peek();

			if (char.IsNumber(next))
			{
				return ReadNumber();
			}

			switch (next)
			{
				case '{':
					return ReadObject();

				case '[':
					return ReadArray();

				case '"':
					return new JsonValue(ReadString());

				case '-':
					return ReadNumber();

				case 't':
				case 'f':
					return ReadBoolean();

				case 'n':
					return ReadNull();

				default:
					throw new JsonParseException(
						ErrorType.InvalidOrUnexpectedCharacter,
						this._scanner.Position
					);
			}
		}

		private JsonValue ReadNull()
		{
			this._scanner.Assert("null");
			return JsonValue.Null;
		}

		private JsonValue ReadBoolean()
		{
			switch (this._scanner.Peek())
			{
				case 't':
					this._scanner.Assert("true");
					return new JsonValue(true);

				case 'f':
					this._scanner.Assert("false");
					return new JsonValue(false);

				default:
					throw new JsonParseException(
						ErrorType.InvalidOrUnexpectedCharacter,
						this._scanner.Position
					);
			}
		}

		private void ReadDigits(StringBuilder builder)
		{
			while (this._scanner.CanRead && char.IsDigit(this._scanner.Peek()))
			{
				builder.Append(this._scanner.Read());
			}
		}

		private JsonValue ReadNumber()
		{
			var builder = new StringBuilder();

			if (this._scanner.Peek() == '-')
			{
				builder.Append(this._scanner.Read());
			}

			if (this._scanner.Peek() == '0')
			{
				builder.Append(this._scanner.Read());
			}
			else
			{
				ReadDigits(builder);
			}

			if (this._scanner.CanRead && this._scanner.Peek() == '.')
			{
				builder.Append(this._scanner.Read());
				ReadDigits(builder);
			}

			if (this._scanner.CanRead && char.ToLowerInvariant(this._scanner.Peek()) == 'e')
			{
				builder.Append(this._scanner.Read());

				var next = this._scanner.Peek();

				switch (next)
				{
					case '+':
					case '-':
						builder.Append(this._scanner.Read());
						break;
				}

				ReadDigits(builder);
			}

			return new JsonValue(JsonType.Number, builder.ToString());
		}

		private string ReadString()
		{
			var builder = new StringBuilder();

			this._scanner.Assert('"');

			while (true)
			{
				var c = this._scanner.Read();

				if (c == '\\')
				{
					c = this._scanner.Read();

					switch (char.ToLower(c))
					{
						case '"':  // "
						case '\\': // \
						case '/':  // /
							builder.Append(c);
							break;
						case 'b':
							builder.Append('\b');
							break;
						case 'f':
							builder.Append('\f');
							break;
						case 'n':
							builder.Append('\n');
							break;
						case 'r':
							builder.Append('\r');
							break;
						case 't':
							builder.Append('\t');
							break;
						case 'u':
							builder.Append(ReadUnicodeLiteral());
							break;
						default:
							throw new JsonParseException(
								ErrorType.InvalidOrUnexpectedCharacter,
								this._scanner.Position
							);
					}
				}
				else if (c == '"')
				{
					break;
				}
				else
				{
                    /*
                     * According to the spec:
                     * 
                     * unescaped = %x20-21 / %x23-5B / %x5D-10FFFF
                     * 
                     * i.e. c cannot be < 0x20, be 0x22 (a double quote) or a 
                     * backslash (0x5C).
                     * 
                     * c cannot be a back slash or double quote as the above 
                     * would have hit. So just check for < 0x20.
                     * 
                     * > 0x10FFFF is unnecessary *I think* because it's obviously
                     * out of the range of a character but we might need to look ahead
                     * to get the whole utf-16 codepoint
                     */
                    if (c < '\u0020')
                    {
                        throw new JsonParseException(
                            ErrorType.InvalidOrUnexpectedCharacter,
                            this._scanner.Position
                        );
                    }
                    else
					{
						builder.Append(c);
					}
				}
			}

			return builder.ToString();
		}

		private int ReadHexDigit()
		{
			switch (char.ToUpper(this._scanner.Read()))
			{
				case '0':
					return 0;

				case '1':
					return 1;

				case '2':
					return 2;

				case '3':
					return 3;

				case '4':
					return 4;

				case '5':
					return 5;

				case '6':
					return 6;

				case '7':
					return 7;

				case '8':
					return 8;

				case '9':
					return 9;

				case 'A':
					return 10;

				case 'B':
					return 11;

				case 'C':
					return 12;

				case 'D':
					return 13;

				case 'E':
					return 14;

				case 'F':
					return 15;

				default:
					throw new JsonParseException(
						ErrorType.InvalidOrUnexpectedCharacter,
						this._scanner.Position
					);
			}
		}

		private char ReadUnicodeLiteral()
		{
			int value = 0;

			value += ReadHexDigit() * 4096; // 16^3
			value += ReadHexDigit() * 256;  // 16^2
			value += ReadHexDigit() * 16;   // 16^1
			value += ReadHexDigit();        // 16^0

			return (char)value;
		}

		private JsonObject ReadObject()
		{
			return ReadObject(new JsonObject());
		}

		private JsonObject ReadObject(JsonObject jsonObject)
		{
			this._scanner.Assert('{');

			this._scanner.SkipWhitespace();

			if (this._scanner.Peek() == '}')
			{
				this._scanner.Read();
			}
			else
			{
				while (true)
				{
					this._scanner.SkipWhitespace();

					var key = ReadJsonKey();

					if (jsonObject.ContainsKey(key))
					{
						throw new JsonParseException(
							ErrorType.DuplicateObjectKeys,
							this._scanner.Position
						);
					}

					this._scanner.SkipWhitespace();

					this._scanner.Assert(':');

					this._scanner.SkipWhitespace();

					var value = ReadJsonValue();

					jsonObject.Add(key, value);

					this._scanner.SkipWhitespace();

					var next = this._scanner.Read();

					if (next == '}')
					{
						break;
					}
					else if (next == ',')
					{
						continue;
					}
					else
					{
						throw new JsonParseException(
							ErrorType.InvalidOrUnexpectedCharacter,
							this._scanner.Position
						);
					}
				}
			}

			return jsonObject;
		}

		private JsonArray ReadArray()
		{
			return ReadArray(new JsonArray());
		}

		private JsonArray ReadArray(JsonArray jsonArray)
		{
			this._scanner.Assert('[');

			this._scanner.SkipWhitespace();

			if (this._scanner.Peek() == ']')
			{
				this._scanner.Read();
			}
			else
			{
				while (true)
				{
					this._scanner.SkipWhitespace();

					var value = ReadJsonValue();

					jsonArray.Add(value);

					this._scanner.SkipWhitespace();

					var next = this._scanner.Read();

					if (next == ']')
					{
						break;
					}
					else if (next == ',')
					{
						continue;
					}
					else
					{
						throw new JsonParseException(
							ErrorType.InvalidOrUnexpectedCharacter,
							this._scanner.Position
						);
					}
				}
			}

			return jsonArray;
		}

		private JsonValue Parse()
		{
			this._scanner.SkipWhitespace();
			return ReadJsonValue();
		}

		/// <summary>
		/// Creates a JsonValue by using the given TextReader.
		/// </summary>
		/// <param name="reader">The TextReader used to read a JSON message.</param>
		public static JsonValue Parse(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}

			return new JsonReader(reader).Parse();
		}

		/// <summary>
		/// Creates a JsonValue by reader the JSON message in the given string.
		/// </summary>
		/// <param name="source">The string containing the JSON message.</param>
		public static JsonValue Parse(string source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			using (var reader = new StringReader(source))
			{
				return new JsonReader(reader).Parse();
			}
		}

		/// <summary>
		/// Creates a JsonValue by reading the given file.
		/// </summary>
		/// <param name="path">The file path to be read.</param>
		public static JsonValue ParseFile(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			using (var reader = new StreamReader(path))
			{
				return new JsonReader(reader).Parse();
			}
		}
	}
}
