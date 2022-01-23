using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morix.Json
{
	public class JsonArray : JsonValue, IEnumerable<JsonValue>
	{
		private readonly IList<JsonValue> _items;
		
		public override int Count { 
			get { return _items.Count; }
		}

		public override JsonValue this[int index]
		{
			get
			{
				if (index >= 0 && index < this._items.Count)
				{
					return this._items[index];
				}
				else
				{
					return JsonValue.Null;
				}
			}
			set
			{
				this._items[index] = value;
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
			this._items = new List<JsonValue>();
		}

		public JsonArray(params JsonValue[] values) : this()
		{
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}

			foreach (var value in values)
			{
				this._items.Add(value);
			}
		}

		public IEnumerator<JsonValue> GetEnumerator()
		{
			return this._items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public override JsonValue Add(JsonValue jsonValue)
		{
			this._items.Add(jsonValue);
			return this;
		}

		public override bool Contains(JsonValue value)
		{
			return this._items.Contains(value);
		}
		
		public override void Clear()
		{
			_items.Clear();
		}

		public override JsonValue Clone()
        {
			var clone = new JsonArray();
			foreach (var item in this._items)
				clone.Add(item.Clone());
			return clone;
		}

        public override int GetHashCode()
		{
			return this._items.GetHashCode();
		}
	}
}
