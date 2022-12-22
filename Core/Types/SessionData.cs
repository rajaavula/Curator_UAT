using System;
using System.Collections.Generic;
using System.Linq;

namespace LeadingEdge.Curator.Core
{
	[Serializable]
	public class SessionData
	{
		public SessionData()
		{
			Sources = new Dictionary<string, object>();
		}

		private Dictionary<string, object> Sources { get; set; }

		public void Set(string name, object data)
		{
			if (Sources.ContainsKey(name))
			{
				Sources[name] = data;
			}
			else
			{
				Sources.Add(name, data);
			}
		}

		public object Get(string name)
		{
			if (Sources.ContainsKey(name))
			{
				return Sources[name];
			}

			return null;
		}

		public T Get<T>(string name) where T : class
		{
			if (Sources.ContainsKey(name))
			{
				return (T)Convert.ChangeType(Sources[name], typeof(T));
			}

			return default(T);
		}

		public void ClearAll()
		{
			Sources = new Dictionary<string, object>();
		}

		public bool Contains(string name)
		{
			return Sources.ContainsKey(name) == false;
		}

		public bool ContainsLike(string name)
		{
			return Sources.Any(source => source.Key.Contains(name));
		}

		public void Remove(string name)
		{
			Sources.Remove(name);
		}
	}
}
