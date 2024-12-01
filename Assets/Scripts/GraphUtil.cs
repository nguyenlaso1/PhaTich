// @sonhg: class: GraphUtil
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphUtil : ScriptableObject
{
	public static string GetPictureQuery(string facebookID, int? width = null, int? height = null, string type = null, bool onlyURL = false)
	{
		string text = string.Format("/{0}/picture", facebookID);
		string text2 = (width == null) ? string.Empty : ("&width=" + width.ToString());
		text2 += ((height == null) ? string.Empty : ("&height=" + height.ToString()));
		text2 += ((type == null) ? string.Empty : ("&type=" + type));
		if (onlyURL)
		{
			text2 += "&redirect=false";
		}
		if (text2 != string.Empty)
		{
			text = text + "?g" + text2;
		}
		return text;
	}

	public static void LoadImgFromURL(string imgURL, Action<Texture> callback)
	{
		Coroutiner.StartCoroutine(GraphUtil.LoadImgEnumerator(imgURL, callback));
	}

	public static IEnumerator LoadImgEnumerator(string imgURL, Action<Texture> callback)
	{
		WWW www = new WWW(imgURL);
		yield return www;
		if (www.error != null)
		{
			UnityEngine.Debug.LogError(www.error);
			yield break;
		}
		callback(www.texture);
		yield break;
	}

	public static string DeserializePictureURL(object userObject)
	{
		Dictionary<string, object> dictionary = userObject as Dictionary<string, object>;
		object obj;
		if (dictionary.TryGetValue("picture", out obj))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)((Dictionary<string, object>)obj)["data"];
			return (string)dictionary2["url"];
		}
		return null;
	}

	public static int GetScoreFromEntry(object obj)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
		return Convert.ToInt32(dictionary["score"]);
	}
}
