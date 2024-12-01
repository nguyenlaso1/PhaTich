// @sonhg: class: SubAvatarController
using System;
using System.Collections;
using System.IO;
using Sfs2X.Entities;
using UnityEngine;
using UnityEngine.UI;

public class SubAvatarController : MonoBehaviour
{
	public void LoadAvatar(User user)
	{
		Context.currentMono.StartCoroutine(this.LoadAva(JokerUserUtils.GetAvatar(user)));
	}

	public void GrayCanvas()
	{
		this.canvas.DoAlpha(0.5f, 0.5f);
	}

	public void HideCanvas()
	{
		this.canvas.alpha = 0f;
	}

	public void ResetCanvas()
	{
		this.canvas.alpha = 1f;
	}

	private IEnumerator LoadAva(string _url)
	{
		string fileName = Path.GetFileName(_url);
		string localName = Application.persistentDataPath + "/" + fileName;
		UnityEngine.Debug.Log("Local name: " + localName);
		if (File.Exists(localName) && !_url.Contains("http"))
		{
			UnityEngine.Debug.Log("Exists " + localName);
			WWW ccc = new WWW("file:///" + localName);
			yield return ccc;
			if (ccc.error == null)
			{
				this.ingameAvatar.texture = ccc.texture;
			}
		}
		else
		{
			UnityEngine.Debug.Log("++++++++++++Exists " + localName);
			string urlPhoto = (!_url.Contains("http")) ? (Joker2XConfigUtils.URL_PHOTO + "/" + _url) : _url;
			WWW www = new WWW(urlPhoto);
			yield return www;
			if (www.error == null)
			{
				this.ingameAvatar.texture = www.texture;
				File.WriteAllBytes(localName, www.bytes);
			}
		}
		yield break;
	}

	public Image ingameFlag;

	public Text ingameName;

	public RawImage ingameAvatar;

	public CanvasGroup canvas;
}
