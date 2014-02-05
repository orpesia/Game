using UnityEngine;

using System.Collections.Generic;

namespace Unnamed
{



	public class SerializableDictionary<Key,Value>
	{
		[System.Serializable]
		public class KeyValue<KType,VType>
		{
			public KType Key;
			public VType Value;
			
			public KeyValue(KType key, VType value)
			{
				this.Key = key;
				this.Value = value;
			}
		};

		[SerializeField]
		public List<KeyValue<Key,Value>> Data = new List<KeyValue<Key, Value>>();

		public Dictionary<Key,Value> Tree = new Dictionary<Key, Value>();

		public void SearchMode()
		{
			foreach(KeyValue<Key,Value> v in Data)
			{
				Tree.Add(v.Key, v.Value);
			}

			Data.Clear ();
		}

		public void DataMode()
		{
			foreach(KeyValuePair<Key,Value> v in Tree)
			{
				Data.Add(new KeyValue<Key,Value>(v.Key,v.Value));
			}
			
			Tree.Clear ();
		}

	}

}