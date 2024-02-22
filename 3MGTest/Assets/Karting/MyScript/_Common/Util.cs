#define INTERNET_CHECK_USING_PING
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

[System.Serializable]
public class TimeInfo
{
	public string abbreviation;
	public string client_ip;
	public string datetime;
	public int day_of_week;
	public int day_of_year;
	public bool dst;
	public object dst_from;
	public int dst_offset;
	public object dst_until;
	public int raw_offset;
	public string timezone;
	public int unixtime;
	public string utc_datetime;
	public string utc_offset;
	public int week_number;

	// Additional property to represent the time in seconds
	public double TimeInSeconds
	{
		get { return unixtime; }
	}
}

public static class Util {

	private const string TimeApiUrl = "http://worldtimeapi.org/api/ip";
	public async static Task<TimeInfo> GetServerTime()
	{
		using (HttpClient client = new HttpClient())
		{
			try
			{
				HttpResponseMessage response = await client.GetAsync(TimeApiUrl);
				if (response.IsSuccessStatusCode)
				{
					string responseContent = await response.Content.ReadAsStringAsync();
					// Parse the JSON response to get the server time

					//Debug.LogError("responseContent : " + responseContent);

					// Parse JSON using JsonUtility
					TimeInfo timeInfo = JsonUtility.FromJson<TimeInfo>(responseContent);
					//Debug.LogError("timeInfo datetime : " + timeInfo.datetime);
					return timeInfo;
				}
				else
				{
					//Debug.LogError("Failed to get server time. Status Code: " + response.StatusCode);
				}
			}
			catch (Exception e)
			{
				Debug.LogError("[try-catch] Exception while getting server time: " + e.Message);
			}
		}

		// Return DateTime.MinValue in case of failure
		return null;
	}

	public readonly static Func<string, string> JSON_ENCODER_DECODER = Util.ROT47Str;

		//private static Job _IsCheckingForInternet;

		private static int _RequestNumber = 0;
		public static int GetRequestNumber() {
			return Interlocked.Increment(ref _RequestNumber);
		}

		public static string FileUpdateLocation {
			get {
				return Application.persistentDataPath + "/njfu";
			}
		}		

		/// <summary>
		/// Gets the current UTC timestamp.
		/// </summary>
		/// <value>
		/// The current UTC timestamp.
		/// </value>
		public static int CurrentUTCTimestamp {
				get {
						return (int)ToUnixTimestamp (DateTime.UtcNow);
				}
		}
	
		public static string DeviceId {
				get {
//			if(LocalStorage.Instance.HasKey("device_id")){
//				var deviceId = LocalStorage.Instance.GetString("device_id");
//				
//				if(string.IsNullOrEmpty(deviceId)){
//					deviceId = new System.Guid().ToString();
//					LocalStorage.Instance.SetString("device_id", deviceId);
//				}
//				
//				return deviceId;
//			}
//			else{
//				var deviceId = new System.Guid().ToString ();
//				LocalStorage.Instance.SetString("device_id", deviceId);
//				return deviceId;
//			}
			
						return SystemInfo.deviceUniqueIdentifier;
				}
		}
	
		public static string DeviceModel {
				get {
						return SystemInfo.deviceModel;
				}
		}

		private static bool _IsInternetReachable = true;
		/// <summary>
		/// Gets a value indicating is internet reachable.
		/// </summary>
		/// <value><c>true</c> if is internet reachable; otherwise, <c>false</c>.</value>
		public static bool IsInternetReachable {
				get {
						return _IsInternetReachable;
				}
				set {
						_IsInternetReachable = value;
				}
		}


		/// <summary>
		/// Reads the bytes from resource.
		/// </summary>
		/// <returns>
		/// The bytes from resource.
		/// </returns>
		/// <param name='resourceName'>
		/// Resource name.
		/// </param>
		public static byte[] ReadBytesFromResource(string resourceName) {
			TextAsset data = (TextAsset)Resources.Load (resourceName, typeof(TextAsset));
			return null != data ? data.bytes : null;		
		}

		/// <summary>
		/// Gets the time zone.
		/// </summary>
		/// <returns>The time zone.</returns>
		public static double GetTimeZone() {
			var date = DateTime.Now;
			return Math.Round((date - date.ToUniversalTime()).TotalHours, 1, MidpointRounding.AwayFromZero);
		}
	
		/// <summary>
		/// Serializes to XML.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		/// <param name="filePath">The filepath.</param>
		public static void SerializeToXML<T> (T obj, string filePath)
		{
				Log ("Starting XML Serialization");
				using (FileStream file = new FileStream(filePath, FileMode.Create)) {
						XmlSerializer serializer = new XmlSerializer (typeof(T));
						serializer.Serialize (file, obj);
				}
				Log ("XML Serialization Complete - " + filePath);
		}

		/// <summary>
		/// DeSerialize from file.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filePath">The file path.</param>
		/// <returns></returns>
		public static T DeSerializeFromFile<T> (string filePath)
		{
				T obj = default(T);
				if (File.Exists (filePath)) {
						using (FileStream file = new FileStream(filePath, FileMode.Open)) {
								XmlSerializer serializer = new XmlSerializer (typeof(T));
								obj = (T)serializer.Deserialize (file);
						}
				}
				return obj;
		}

		/// <summary>
		/// Serializes the json document to file.
		/// </summary>
		/// <returns><c>true</c>, if json document to file was serialized, <c>false</c> otherwise.</returns>
		/// <param name="filePath">File path.</param>
		/// <param name="jsonDoc">Json document.</param>
		public static bool SerializeJsonDocToFile(string filePath, IDictionary<string, object> jsonDoc) {
			return SerializeJsonDocToFile(filePath, jsonDoc, JSON_ENCODER_DECODER);
		}

		/// <summary>
		/// Serializes the json document to file.
		/// </summary>
		/// <returns><c>true</c>, if json document to file was serialized, <c>false</c> otherwise.</returns>
		/// <param name="filePath">File path.</param>
		/// <param name="encodeMethod">Encode method.</param>
		public static bool SerializeJsonDocToFile(string filePath, IDictionary<string, object> jsonDoc, Func<string, string> encodeMethod) {
			bool result = false;
			try {
				if(null != jsonDoc) {
					string resultStr = SimpleJson.SimpleJson.SerializeObject(jsonDoc);
					if(!string.IsNullOrEmpty(resultStr)) {
						if(null != encodeMethod) {
							resultStr = encodeMethod(resultStr);
						}

						if(!string.IsNullOrEmpty(resultStr)) {
							File.WriteAllText(filePath, resultStr);
							result = true;
						}
					}
				}
			}
			catch(Exception ex) {
				Debug.Log("[Util] Error in persisting file: " + ex.Message);
				result = false;
			}
			return result;
		}

		/// <summary>
		/// DeSerialize json document from file.
		/// </summary>
		/// <returns>The serialize json document from file.</returns>
		/// <param name="filePath">File path.</param>
		public static IDictionary<string, object> DeSerializeJsonDocFromFile(string filePath) {
			return DeSerializeJsonDocFromFile(filePath, JSON_ENCODER_DECODER);
		}

		/// <summary>
		/// DeSerialize json document from file.
		/// </summary>
		/// <returns>The serialize json document from file.</returns>
		/// <param name="filePath">File path.</param>
		public static IDictionary<string, object> DeSerializeJsonDocFromFile(string filePath, Func<string, string> decodeMethod) {
			IDictionary<string, object> result = null;
			if (File.Exists (filePath)) {
				string fileData = File.ReadAllText(filePath);
				if(null != decodeMethod && !string.IsNullOrEmpty(fileData)) {
					fileData = decodeMethod(fileData);
				}
				if(!string.IsNullOrEmpty(fileData)) {
					result = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(fileData);
				}
			}
			return result;
		}
	
		/// <summary>
		/// Reads the text from resource.
		/// </summary>
		/// <returns>
		/// The text from resource.
		/// </returns>
		/// <param name='resourceName'>
		/// Resource name.
		/// </param>
		public static string ReadTextFromResource (string resourceName)
		{
				TextAsset data = (TextAsset)Resources.Load (resourceName, typeof(TextAsset));
				return null != data ? data.text : null;
		}

		/// <summary>
		/// DeSerialize from resource.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resourceName">Name of the resource.</param>
		/// <returns></returns>
		public static T DeSerializeFromResource<T> (string resourceName)
		{
				T obj = default(T);
				string text = ReadTextFromResource (resourceName);
				if (null != text) {
						using (StringReader reader = new StringReader(text)) {
								XmlSerializer serializer = new XmlSerializer (typeof(T));
								obj = (T)serializer.Deserialize (reader);
						}
				}
				return obj;
		}
	
		/// <summary>
		/// DeSerializes the object from persistent data path if file exists,
		/// else falls back to DeSerializing file in Resources folder which was shipped with the application.
		/// </summary>
		/// <returns>The serialize game resource object.</returns>
		/// <param name="resourceName">Resource name.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T DeSerializeGameResourceObject<T> (string resourceName)
		{
				T obj = default(T);
				string filePath = Application.persistentDataPath + resourceName;
				if (File.Exists (filePath)) {
						obj = DeSerializeFromFile<T> (filePath);
				} else {
						obj = DeSerializeFromResource<T> (resourceName);
				}
				return obj;
		}

		/// <summary>
		/// Deserialize json document from resource.
		/// </summary>
		/// <returns>The serialize json document from resource.</returns>
		/// <param name="resourceName">Resource name.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IDictionary<string, object> DeSerializeJsonDocFromResource(string resourceName) {
			IDictionary<string, object> result = null;
			string text = ReadTextFromResource (resourceName);
			if(null != text) {
				object obj;
				if(SimpleJson.SimpleJson.TryDeserializeObject(text, out obj)) {
					result = (IDictionary<string, object>)obj;
				}
			}
			return result;
		}

		/// <summary>
		/// Deserialize json document from cache or resource.
		/// </summary>
		/// <returns>The deserialized json document from cache or resource.</returns>
		/// <param name="resourceName">Resource name.</param>
		public static IDictionary<string, object> DeSerializeJsonDocFromCacheOrResource(string resourceName) {
			IDictionary<string, object> result = null;
			string filePath = Path.Combine(Application.persistentDataPath, resourceName);
			if(File.Exists(filePath)) {
				Log("[DE-SERIALIZE] OPENING FILE :" + filePath);
				result = DeSerializeJsonDocFromFile(filePath, null);
			}
			if(null == result) {
				Log("[DE-SERIALIZE] OPENING RESOURCE :" + resourceName);
				result = DeSerializeJsonDocFromResource(resourceName);
			}
			return result;
		}

		/// <summary>
		/// Select the specified list and selector.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="selector">Selector.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <typeparam name="V">The 2nd type parameter.</typeparam>
		public static List<V> Select<T, V>(IEnumerable<T> list, Func<T, V> selector) {
			List<V> values = new List<V> ();

			foreach (T item in list) {
				values.Add(selector(item));
			}

			return values;
		}

		/// <summary>
		/// Filters the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="comparator">The comparator.</param>
		/// <returns></returns>
		public static List<T> FilterList<T> (IEnumerable<T> list, Func<T, bool> comparator)
		{
				List<T> filteredList = new List<T> ();
				if(null != list) {
					foreach (T item in list) {
						if (comparator (item)) {
								filteredList.Add (item);
						}
					}
				}
				return filteredList;
		}

		/// <summary>
		/// Flattens hierarchy
		/// </summary>
		/// <returns>
		/// The many.
		/// </returns>
		/// <param name='source'>
		/// Source.
		/// </param>
		/// <param name='selector'>
		/// Selector.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		/// <typeparam name='TResult'>
		/// The 2nd type parameter.
		/// </typeparam>
		public static List<TResult> SelectMany<T, TResult> (IEnumerable<T> source, Func<T, IEnumerable<TResult>> selector)
		{
				List<TResult> list = new List<TResult> ();
				foreach (T item in source) {
						list.AddRange (selector (item));
				}
				return list;
		}

		/// <summary>
		/// Firsts the or default.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="comparator">The comparator.</param>
		/// <returns></returns>
		public static T FirstOrDefault<T> (IEnumerable<T> list, Func<T, bool> comparator)
		{
				foreach (T item in list) {
						if (comparator (item)) {
								return item;
						}
				}
				return default(T);
		}

		/// <summary>
		/// Find if any element in the list matches the predicate specified
		/// </summary>
		/// <param name="items">Items.</param>
		/// <param name="comparator">Comparator.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool Any<T> (IEnumerable<T> items, Func<T, bool> comparator)
		{
				if (null == items)
						return false;

				foreach (T item in items) {
						if (comparator (item)) {
								return true;
						}
				}
				return false;
		}
	
		/// <summary>
		/// Uniques the list.
		/// </summary>
		/// <returns>
		/// The list.
		/// </returns>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static List<T> UniqueList<T> (List<T> list)
		{
				List<T> uniqueList = new List<T> ();
				foreach (T item in list) {
						if (!uniqueList.Contains (item))
								uniqueList.Add (item);
				}
				return uniqueList;
		}
	
		/// <summary>
		/// Contains the specified data and value.
		/// </summary>
		/// <param name='data'>
		/// If set to <c>true</c> data.
		/// </param>
		/// <param name='value'>
		/// If set to <c>true</c> value.
		/// </param>
		public static bool Contains (string[] data, string value)
		{
				if (null != data) {
						foreach (string s in data) {
								if (s == value)
										return true;
						}
				}
				return false;
		}
	
		/// <summary>
		/// Adds the element to array.
		/// </summary>
		/// <param name='array'>
		/// Array.
		/// </param>
		/// <param name='newItem'>
		/// New item.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static void AddElementToArray<T> (ref T[] array, T newItem)
		{
				Array.Resize (ref array, array.Length + 1);
				array [array.Length - 1] = newItem;
				//Log("AddElementToArray" + newItem);
		}
	
		/// <summary>
		/// Shuffle the specified list.
		/// </summary>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static List<T> Shuffle<T> (List<T> list)
		{
				System.Random rng = new System.Random (DateTime.Now.Millisecond);
				List<T> shuffleList = new List<T> (list);
				int n = list.Count;
				while (n > 1) {
						n--;
						int k = rng.Next (n + 1);
						T value = shuffleList [k];
						shuffleList [k] = shuffleList [n];
						shuffleList [n] = value;  
				}
				return shuffleList;
		}
	
		/// <summary>
		/// Shuffle the specified list.
		/// </summary>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static T[] Shuffle<T> (T[] list)
		{
				return Shuffle (new List<T> (list)).ToArray ();
		}
	
		/// <summary>
		/// Fors the each.
		/// </summary>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <param name='action'>
		/// Action.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static void ForEach<T> (IEnumerable<T> list, Action<T> action)
		{
				foreach (T item in list) {
						action (item);
				}
		}
	
		/// <summary>
		/// Count the specified items in the list using the comparator.
		/// </summary>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <param name='action'>
		/// Action.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static int Count<T> (IEnumerable<T> list, Func<T, bool> comparator)
		{
				return FilterList<T> (list, comparator).Count;
		}

		/// <summary>
		/// All the specified list and predicate.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="predicate">Predicate.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool All<T>(IEnumerable<T> list, Predicate<T> predicate) {
			if(null != list) {
				foreach(T item in list) {
					if(false == predicate(item)) {
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Gets the element having the Max value of the selected field.
		/// </summary>
		/// <returns>The by.</returns>
		/// <param name="source">Source.</param>
		/// <param name="selector">Selector.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		/// <typeparam name="TKey">The 2nd type parameter.</typeparam>
		public static TSource MaxBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector) {
			return MaxBy(source, selector, Comparer<TKey>.Default);
		}

		/// <summary>
		/// Gets the element having the Max value of the selected field.
		/// </summary>
		/// <returns>The by.</returns>
		/// <param name="source">Source.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		/// <typeparam name="TKey">The 2nd type parameter.</typeparam>
		public static TSource MaxBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer){
			if (source == null) throw new ArgumentNullException("source");
			if (selector == null) throw new ArgumentNullException("selector");
			if (comparer == null) throw new ArgumentNullException("comparer");
			using (var sourceIterator = source.GetEnumerator())
			{
				if (!sourceIterator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				var max = sourceIterator.Current;
				var maxKey = selector(max);
				while (sourceIterator.MoveNext()) {
					var candidate = sourceIterator.Current;
					var candidateProjected = selector(candidate);
					if (comparer.Compare(candidateProjected, maxKey) > 0) {
						max = candidate;
						maxKey = candidateProjected;
					}
				}
				return max;
			}
		}


		/// <summary>
		/// Contains the specified list and value.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool Contains<T> (IEnumerable<T> list, T value) {
				foreach (T item in list) {
						if (item.Equals (value)) {
								return true;
						}
				}
				return false;
		}
	
		/// <summary>
		/// Convert to unix timestamp.
		/// </summary>
		/// <returns>
		/// The unix timestamp.
		/// </returns>
		/// <param name='dt'>
		/// Date time object
		/// </param>
		public static long ToUnixTimestamp (System.DateTime dt)
		{
				DateTime unixRef = new DateTime (1970, 1, 1, 0, 0, 0);
				return (dt.Ticks - unixRef.Ticks) / 10000000;
		}
	
		/// <summary>
		/// Convert from the unix timestamp.
		/// </summary>
		/// <returns>
		/// The date time object.
		/// </returns>
		/// <param name='timestamp'>
		/// Timestamp.
		/// </param>
		public static DateTime FromUnixTimestamp (long timestamp)
		{
				DateTime unixRef = new DateTime (1970, 1, 1, 0, 0, 0);
				return unixRef.AddSeconds (timestamp);
		}
	
		/// <summary>
		/// DeSerializes the JSON string.
		/// </summary>
		/// <returns>
		/// Dictionary object.
		/// </returns>
		/// <param name='jsonStr'>
		/// Json string.
		/// </param>
		public static IDictionary<string, object> DeSerializeJSON (string jsonStr) {
				return (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject (jsonStr);
		}

		/// <summary>
		/// SHA1 encode.
		/// </summary>
		/// <param name="plainText">The plain text.</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static string SHA1Encode (string plainText)
		{
				var inputBytes = ASCIIEncoding.ASCII.GetBytes (plainText);        
				var hashBytes = new SHA1Managed ().ComputeHash (inputBytes);
				StringBuilder hashValue = new StringBuilder ();
				Array.ForEach<byte> (hashBytes, b => hashValue.Append (b.ToString ("x2")));
				return hashValue.ToString ();
		}

		/// <summary>
		/// Executes the post command.
		/// </summary>
		/// <returns>
		/// The post command.
		/// </returns>
		/// <param name='url'>
		/// URL.
		/// </param>
		/// <param name='data'>
		/// Data.
		/// </param>
		/// <param name='responseCode'>
		/// Response code.
		/// </param>
		public static IEnumerator ExecutePostCommand (string url, string data, Action<WWW> callback)
		{
				var form = new WWWForm ();
				form.headers ["Content-Type"] = "application/json";
				int requestNo = GetRequestNumber();
				Util.Log (string.Format("[HTTP POST] Request {0} {1} {2}", requestNo, url, data));
		
				using (WWW www = new WWW(url, Encoding.UTF8.GetBytes(data), form.headers)) {
//						if (!Util.IsInternetReachable) {
//								if (null != callback) {
//										callback (www);
//								}
//								yield break;
//						}

						yield return www;
						//Util.Log (string.Format("[HTTP POST] Response {0} {1}", requestNo, (!string.IsNullOrEmpty (www.error)) ? (" Error - " + www.error) : www.text));
						if (www.isDone && null != callback) {
								callback (www);
						}		
				}
		}
	

	public static IEnumerator ExecutePostCommandWithBasicAuth (string url,string username, string password, string data, Action<WWW> callback)
	{
		var form = new WWWForm ();
		var headers = form.headers;
		
		headers["Content-Type"] = "application/json";
		headers["Authorization"]="Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}",username,password)));
		
		//Util.Log("headers  "+headers.ToStringFull());
		int requestNo = GetRequestNumber();
		Util.Log (string.Format("[HTTP POST] Request {0} {1} {2}:{3} {4}", requestNo, url, username, password, data));

		using (WWW www = new WWW(url, Encoding.UTF8.GetBytes(data), headers)) {
			//						if (!Util.IsInternetReachable) {
			//								if (null != callback) {
			//										callback (www);
			//								}
			//								yield break;
			//						}
			
			yield return www;
			//Util.Log (string.Format("[HTTP POST] Response {0} {1}", requestNo, (!string.IsNullOrEmpty (www.error)) ? (" Error - " + www.error) : www.text));

			if (www.isDone && null != callback) {
				callback (www);
			}		
		}
	}
		/// <summary>
		/// Executes the get command.
		/// </summary>
		/// <returns>
		/// The get command.
		/// </returns>
		/// <param name='url'>
		/// URL.
		/// </param>
		/// <param name='callback'>
		/// Callback.
		/// </param>
		public static IEnumerator ExecuteGetCommand (string url, Action<WWW> callback)
		{
			int requestNo = GetRequestNumber();
			Util.Log (string.Format("[HTTP GET] Request {0} {1}", requestNo, url));
			
			if (!Util.IsInternetReachable) {
				if (null != callback) {
					callback (null);
				}
				yield break;
			}

			using (WWW www = new WWW(url)) {
	
				yield return www;
				//Util.Log (string.Format("[HTTP GET] Response {0} {1}", requestNo, (!string.IsNullOrEmpty (www.error)) ? (" Error - " + www.error) : www.text));
				if (www.isDone && null != callback) {
						callback (www);
				}
			}
		}

		/// <summary>
		/// Executes the ping command.
		/// </summary>
		/// <returns>The ping command.</returns>
		/// <param name="address">Address.</param>
		/// <param name="timeOut">Time out.</param>
		/// <param name="callback">Callback.</param>
		public static IEnumerator ExecutePingCommand (string address, float timeOut, Action<float> callback)
		{
				Ping ping = new Ping (address);
				float timeDiff = 0.1f;
				float time = 0.0f;
				while (!ping.isDone) {
						yield return new WaitForSeconds (timeDiff);
						time += timeDiff;
						if (time >= timeOut) {
								break;
						}
				}
				//Util.Log("PING:" + time.ToString("0.0") + " TIMEOUT:" + timeOut + " IsDone:" + ping.isDone + " Time:" + ping.time);
				if (null != callback) {
						callback (time);
				}
		}

		/// <summary>
		/// Log the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void Log (object message)
		{
			//--	Debug.Log(message);
		}

		/// <summary>
		/// Logs the error.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void LogError (object message)
		{
			//--	Debug.LogError(message);
		}

		/// <summary>
		/// Log the specified message and context.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="context">Context.</param>
		public static void Log (object message, UnityEngine.Object context)
		{
			//-- Debug.Log(message, context);
		}

		public static string ConvertToString (Dictionary<string, string> d)
		{
				// Build up each line one-by-one and then trim the end
				StringBuilder builder = new StringBuilder ();
				foreach (KeyValuePair<string, string> pair in d) {
						builder.Append (pair.Key).Append (":").Append (pair.Value).Append (',');
				}
				string result = builder.ToString ();
				// Remove the final delimiter
				result = result.TrimEnd (',');
				return result;
		}

		public static string ToTitleCase (string word)
		{
				if (word == null)
						return null;
		
				if (word.Length > 1)
						return char.ToUpper (word [0]) + word.Substring (1);
		
				return word.ToUpper ();
		}

		/// <summary>
		/// Updates the vector3.
		/// </summary>
		/// <returns>The vector3.</returns>
		/// <param name="v">V.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		public static Vector3 UpdateVector3 (Vector3 v, float x, float y, float z)
		{
				v.x = x;
				v.y = y;
				v.z = z;
				return v;
		}
	/*
		/// <summary>
		/// Starts the internet reachability check.
		/// </summary>
		public static void StartInternetReachabilityCheck ()
		{
				if (null == _IsCheckingForInternet || _IsCheckingForInternet.IsPaused) {
					if(null != _IsCheckingForInternet)
						_IsCheckingForInternet.Kill();
					_IsCheckingForInternet = Job.Create (TestInternetConnection ());
				}
		}

		/// <summary>
		/// Tests the internet connection.
		/// </summary>
		/// <returns>The internet connection.</returns>
		public static IEnumerator TestInternetConnection ()
		{
				long startTime = 0;
				long timeTaken = 0;
				float maxTime = 5.0F;
				float secondsToWait = 10F;

				#if INTERNET_CHECK_USING_PING
				while (true) {
					yield return Job.CreateAsCoroutine (
						ExecutePingCommand ("74.125.224.72", maxTime,
					    	time => {
								Log ("RECEIVED PING - " + time);
								if (time < maxTime) {
										IsInternetReachable = true;
										secondsToWait = 20F;
								} else {
										IsInternetReachable = false;
										secondsToWait = 5F;
								}
						}));
					yield return new WaitForSeconds (secondsToWait);
				}
				#else
				while (true) {
					startTime = DateTime.Now.Ticks;
					//Util.Log("START TICK - " + startTime);
					yield return Coroutiner.StartCoroutine(
						Util.ExecuteGetCommand("http://www.google.com",
							www => {
								timeTaken = DateTime.Now.Ticks - startTime;
								//Util.Log("END TICK - " + DateTime.Now.Ticks);
								//Util.Log("TIME TAKEN - " + timeTaken);
								if(!string.IsNullOrEmpty(www.error)) {
									//Util.Log("ERROR - " + www.error);
									IsInternetReachable = false;
									secondsToWait = 5;
								}
								else {
									IsInternetReachable = true;
									secondsToWait = 20;
								}
							}));
					yield return new WaitForSeconds(secondsToWait);
				}
				#endif
		}
	*/
		public static Color GetColorByCode (string colorCode)
		{
				if(string.IsNullOrEmpty(colorCode))
				   return Color.white;

				var r = int.Parse (colorCode.Substring (0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
				var g = int.Parse (colorCode.Substring (2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
				var b = int.Parse (colorCode.Substring (4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

				return new Color ((float)r / 255f, (float)g / 255f, (float)b / 255f, 1f);
		}		

	/// <summary>
	/// Gets the comparison ascending.
	/// </summary>
	/// <returns>The comparison ascending.</returns>
	/// <param name="fieldSelector">Field selector.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	/// <typeparam name="F">The 2nd type parameter.</typeparam>
	public static Comparison<T> GetComparisonAscending<T, F>(Func<T, F> fieldSelector) where F : IComparable {
		return (o1, o2) => {
			return fieldSelector(o1).CompareTo(fieldSelector(o2));
		};
	}

	/// <summary>
	/// Gets the comparison descending.
	/// </summary>
	/// <returns>The comparison descending.</returns>
	/// <param name="fieldSelector">Field selector.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	/// <typeparam name="F">The 2nd type parameter.</typeparam>
	public static Comparison<T> GetComparisonDescending<T, F>(Func<T, F> fieldSelector) where F : IComparable {
		return (o1, o2) => {
			return fieldSelector(o2).CompareTo(fieldSelector(o1));
		};
	}

	/// <summary>
	/// ROT47 the string
	/// </summary>
	/// <returns>The Rot47 string.</returns>
	/// <param name="value">Value.</param>
	public static string ROT47Str(string value) {
		if (!string.IsNullOrEmpty(value)) {
			char[] array = value.ToCharArray();
			for (int i = 0; i < array.Length; i++) {
				int number = (int)array[i];
				if (number >= 33 && number <= 126) {
					array[i] = (char)(33 + ((number + 14) % 94));
				}
				else {
					array[i] = (char)number;
				}
			}
			return new string(array);
		}
		return value;
	}

	public static IDictionary<string, object> ConvertToGenericDictionary(IDictionary obj) {
		IDictionary<string, object> result = new Dictionary<string, object>(); 
		foreach(var key in obj.Keys) {
			string keyStr = key.ToString();
			if(!obj.Contains(key)) {
				result.Add(keyStr, obj[key]);
			}
		}
		return result;
	}

    /// <summary>
    /// Medians the specified list.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <returns></returns>
    public static int Median(int[] list) {
        int result = 0;
		Array.Sort(list);
		var listSize = list.Length;

        int midIndex = listSize / 2;
        if (0 == listSize % 2) {    // Even number of elements            
            result = ((list[midIndex - 1] + list[midIndex]) / 2);
        }
        else {                      // Odd number of elements
            result = list[midIndex];
        }

        return result;
    }

	/// <summary>
	/// Determines if is number the specified obj.
	/// </summary>
	/// <returns><c>true</c> if is number the specified obj; otherwise, <c>false</c>.</returns>
	/// <param name="obj">Object.</param>
	public static bool IsNumber(object obj) {
		if(null == obj)
			return false;

		return (obj is Int64 || obj is int || obj is float || obj is double);
	}

	//------

	public static string  getPlayerName (string sFrdId)
	{
		//abcdefgh 123456 // 8 + 6 = 14

		string name = sFrdId.Substring (0, sFrdId.Length - 6);

		Util.LogError ("name : " + name);
		return name;
	}

	public static string  getPlayerID (string sFrdId)
	{
		Util.LogError ("sFrdId : " + sFrdId);

		string id = sFrdId.Substring (sFrdId.Length - 6, 6);
		Util.LogError ("id : " + id);

		return id;
	}
}