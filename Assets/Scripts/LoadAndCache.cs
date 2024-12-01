// @sonhg: class: LoadAndCache
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadAndCache : MonoBehaviour
{
	private static Texture GetTexture(string key)
	{
		for (int i = 0; i < LoadAndCache._listKey.Count; i++)
		{
			if (LoadAndCache._listKey[i] == key)
			{
				return LoadAndCache._listTexture[i];
			}
		}
		return null;
	}

	private static void AddTexture(string key, Texture texture)
	{
		for (int i = 0; i < LoadAndCache._listKey.Count; i++)
		{
			if (LoadAndCache._listKey[i] == key)
			{
				return;
			}
		}
		LoadAndCache._listKey.Add(key);
		LoadAndCache._listTexture.Add(texture);
	}

	public bool isCached(string link)
	{
		this._key = MathUtils.getHashSha256(link);
		Texture texture = LoadAndCache.GetTexture(this._key);
		if (texture != null)
		{
			base.transform.GetComponent<UITexture>().mainTexture = texture;
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<LoadAndCache>());
			return true;
		}
		string @string = PlayerPrefs.GetString(this._key, string.Empty);
		if (@string != string.Empty)
		{
			Texture2D texture2D = new Texture2D(100, 100);
			LoadAndCache.AddTexture(this._key, texture2D);
			texture2D.LoadImage(Convert.FromBase64String(@string));
			base.transform.GetComponent<UITexture>().mainTexture = texture2D;
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<LoadAndCache>());
			return true;
		}
		return false;
	}

	public void Load(string link, bool isCache = false)
	{
		this._isCache = isCache;
		this._link = link;
		base.StartCoroutine(this.LoadImage(this._link));
	}

	private IEnumerator LoadImage(string link)
	{
		this._reloadCounter++;
		WWW www = new WWW(link);
		yield return www;
		try
		{
			string error = www.error + string.Empty;
			if (error.Length < 2)
			{
				this.isComplete = true;
				base.StopAllCoroutines();
				base.transform.GetComponent<RawImage>().texture = www.texture;
				if (this._isCache)
				{
					LoadAndCache.AddTexture(this._key, www.texture);
					if (PlayerPrefs.GetString(this._key, string.Empty) == string.Empty)
					{
						string bytes = Convert.ToBase64String(www.bytes);
						PlayerPrefs.SetString(this._key, bytes);
					}
				}
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<LoadAndCache>());
			}
			else if (this._reloadCounter < 5)
			{
				this.Load(link, this._isCache);
			}
		}
		catch (Exception ex)
		{
			Exception e = ex;
			UnityEngine.Debug.Log("load image error:" + e.Message + ": " + link);
			if (this._reloadCounter < 5 && !this.isComplete)
			{
				this.Load(link, this._isCache);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<LoadAndCache>());
			}
		}
		yield break;
	}

	private void OnDestroy()
	{
		ImageManager.Remove();
	}

	public LoadAndCache.OnLoadCompleted onCompleted;

	private string _key;

	private int _reloadCounter;

	public bool isComplete;

	private static List<string> _listKey = new List<string>();

	private static List<Texture> _listTexture = new List<Texture>();

	private string _link;

	private bool _isCache = true;

	public delegate void OnLoadCompleted(Texture2D texture);
}
