using System;
using System.Collections;
using System.Collections.Generic;

public class BaseModel {

	protected IDictionary<string, object> _Record;

	/// <summary>
	/// Gets or sets the <see cref="BaseModel"/> with the specified key.
	/// </summary>
	/// <param name="key">Key.</param>
	public object this[string key] {
		get {
			return _Record[key];
		}
		set {
			_Record[key] = value;
		}
	}
	

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseModel"/> class.
	/// </summary>
	/// <param name="doc">The document.</param>
	public BaseModel(IDictionary<string, object> doc) {
		UpdateDoc(doc);
	}

	/// <summary>
	/// Updates the document.
	/// </summary>
	/// <param name="updatedDoc">Updated document.</param>
	public virtual void UpdateDoc(IDictionary<string, object> updatedDoc) {
		this._Record = updatedDoc;
	}

	/// <summary>
	/// Gets the int.
	/// </summary>
	/// <returns>The int.</returns>
	/// <param name="key">Key.</param>
	public int GetInt(string key) {
		int result = 0;
		if(_Record.ContainsKey(key)) {
			result = int.Parse(_Record[key].ToString());
		}
		return result;
	}

	/// <summary>
	/// Gets the long.
	/// </summary>
	/// <returns>The long.</returns>
	/// <param name="key">Key.</param>
	public long GetLong(string key) {
		long result = 0;
		if(_Record.ContainsKey(key)) {
			result = long.Parse(_Record[key].ToString());
		}
		return result;
	}

	/// <summary>
	/// Gets the float.
	/// </summary>
	/// <returns>The float.</returns>
	/// <param name="key">Key.</param>
	public float GetFloat(string key) {
		float result = 0.0f;
		if(_Record.ContainsKey(key)) {
			result = float.Parse(_Record[key].ToString());
		}
		return result;
	}

	/// <summary>
	/// Gets the bool.
	/// </summary>
	/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
	/// <param name="key">Key.</param>
	public bool GetBool(string key) {
		bool result = false;
		if(_Record.ContainsKey(key)) {
			result = (bool)_Record[key];
		}
		return result;
	}

	/// <summary>
	/// Gets the string.
	/// </summary>
	/// <returns>The string.</returns>
	/// <param name="key">Key.</param>
	public string GetString(string key) {
		return Get<string>(key);
	}

	/// <summary>
	/// Gets as string.
	/// </summary>
	/// <returns>The as string.</returns>
	/// <param name="key">Key.</param>
	public string GetAsString(string key) {
		string value = string.Empty;
		if(_Record.ContainsKey(key)) {
			value = _Record[key].ToString();
		}
		return value;
	}

	/// <summary>
	/// Gets the string list.
	/// </summary>
	/// <returns>The string list.</returns>
	/// <param name="key">Key.</param>
	public List<string> GetStringList(string key) {
		List<object> objects = Get<List<object>> (key);
		List<string> strings = new List<string>();

		if(null != objects) {
			foreach(object o in objects) {
				if(null != o && o is String) {
					strings.Add((string)o);
				}
			}
		}

		return strings;
	}

	/// <summary>
	/// Get the specified key.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public T Get<T>(string key) {
		T result = default(T);
		if(_Record.ContainsKey(key) && _Record[key] is T) {
			result = (T)_Record[key];
		}
		return result;
	}

	/// <summary>
	/// Gets the enum.
	/// </summary>
	/// <returns>The enum.</returns>
	/// <param name="key">Key.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public T GetEnum<T>(string key) {
		return (T)Enum.Parse(typeof(T), GetString(key));
	}

	/// <summary>
	/// Gets a base model object.
	/// </summary>
	/// <returns>The model.</returns>
	/// <param name="key">Key.</param>
	/// <param name="converter">Converter.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public T GetModel<T>(string key, Func<IDictionary<string, object>, T> converter) {
		return converter(Get<IDictionary<string, object>>(key));
	}

	/// <summary>
	/// Gets the model list from array.
	/// </summary>
	/// <returns>The model list from array.</returns>
	/// <param name="key">Key.</param>
	/// <param name="converter">Converter.</param>
	/// <typeparam name="T">The final type parameter.</typeparam>
	/// <typeparam name="JType">The json type parameter.</typeparam>
	public List<T> GetModelListFromArray<T, JType>(string key, Func<JType, T> converter) where JType : class {
		List<T> values = null;
		var array = Get<SimpleJson.JsonArray>(key);
		if(null != array && null != converter) {
			values = new List<T>();
			foreach(var item in array) {
				if(null != item && item is JType) {
					values.Add(converter(item as JType));
				}
			}
		}
		return values;
	}

	/// <summary>
	/// Gets the model list from json array.
	/// </summary>
	/// <returns>The model list from array.</returns>
	/// <param name="key">Key.</param>
	/// <param name="converter">Converter.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public List<T> GetModelListFromArray<T>(string key, Func<IDictionary<string, object>, T> converter) {
		return GetModelListFromArray<T, IDictionary<string, object>>(key, converter);
	}

	/// <summary>
	/// Set the specified key and value.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public void Set(string key, object value) {
		if(null != _Record) {
			if(_Record.ContainsKey(key)) {
				_Record[key] = value;
			}
			else {
				_Record.Add(key, value);
			}
		}
	}

	/// <summary>
	/// Gets the raw.
	/// </summary>
	/// <returns>The raw.</returns>
	public IDictionary<string, object> GetRaw() {
		return _Record;
	}
}

