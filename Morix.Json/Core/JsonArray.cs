using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morix.Json
{
	public class JsonArray : JsonValue, IEnumerable<JsonValue>
	{
		private readonly IList<JsonValue> items;
		
		public override int Count { 
			get { return items.Count; }
		}

		public override JsonValue this[int index]
		{
			get
			{
				if (index >= 0 && index < this.items.Count)
				{
					return this.items[index];
				}
				else
				{
					return JsonValue.Null;
				}
			}
			set
			{
				this.items[index] = value;
			}
		}

		public override JsonValue this[string key]
		{
			get
			{
				throw new InvalidOperationException("This value does not represent a JObject.");
			}
			set
			{
				throw new InvalidOperationException("This value does not represent a JObject.");
			}
		}

		public JsonArray() : base(JsonType.Array)
		{
			this.items = new List<JsonValue>();
		}

		public JsonArray(params JsonValue[] values) : this()
		{
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}

			foreach (var value in values)
			{
				this.items.Add(value);
			}
		}

		public IEnumerator<JsonValue> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public override JsonValue Add(JsonValue jsonValue)
		{
			this.items.Add(jsonValue);
			return this;
		}

		public override bool Contains(JsonValue value)
		{
			return this.items.Contains(value);
		}
		
		public override void Clear()
		{
			items.Clear();
		}

		public override JsonValue Clone()
        {
			var clone = new JsonArray();
			foreach (var item in this.items)
				clone.Add(item.Clone());
			return clone;
		}

        public override int GetHashCode()
		{
			return this.items.GetHashCode();
		}
	}
}
