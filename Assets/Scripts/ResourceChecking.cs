// @sonhg: class: ResourceChecking
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResourceChecking : MonoBehaviour
{
	private void Start()
	{
	}

	protected string GetFileName(string _url)
	{
		return Path.GetFileName(_url);
	}

	public static string BaseIp()
	{
		return BombConfigUtils.BASE_RESOURCE_PATH;
	}

	public void CaculateTotalFileNeedToLoad(string _url)
	{
		if (!string.IsNullOrEmpty(_url) && (_url.Contains(".png") || _url.Contains(".jpg")))
		{
			string imgId = _url.Substring(0, _url.Length - 4);
			if (this.CheckSpriteInResource(imgId) == null)
			{
				string text = Application.persistentDataPath + "/" + _url;
				this.totalFile += 1f;
			}
		}
	}

	public IEnumerator FindSprite(string _url, bool _isLoadLocal)
	{
		if (!string.IsNullOrEmpty(_url))
		{
			Sprite sprite = this.CheckSpriteInResource(_url);
			if ((_url.Contains(".png") || _url.Contains(".jpg")) && sprite == null)
			{
				yield return base.StartCoroutine(this.DownloadResource(_url, _isLoadLocal));
				this.percentLoaded += 1f;
				if (this.percentImg != null)
				{
					this.percentImg.fillAmount = this.percentLoaded / this.totalFile;
				}
			}
		}
		yield break;
	}

	public IEnumerator CheckThenFindSprite(string _url)
	{
		yield return base.StartCoroutine(this.LoadInLocalPath(_url));
		yield return base.StartCoroutine(this.FindSprite(_url, true));
		yield break;
	}

	public Sprite CheckSpriteInResource(string _imgId)
	{
		if (_imgId[0] == '/')
		{
			_imgId = _imgId.Remove(0, 1);
		}
		if (_imgId.Contains(".png") || _imgId.Contains(".jpg"))
		{
			_imgId = _imgId.Replace(".png", string.Empty);
			_imgId = _imgId.Replace(".jpg", string.Empty);
		}
		string text = "Textures/" + _imgId;
		string[] array = text.Split(new char[]
		{
			'/'
		});
		string atlasName = array[array.Length - 2];
		string name = array[array.Length - 1].Split(new char[]
		{
			'.'
		})[0];
		SpritePool atlasByName = SpritePool.GetAtlasByName(atlasName);
		if (atlasByName == null)
		{
			return Resources.Load<Sprite>(text);
		}
		return atlasByName.FindByName(name);
	}

	private IEnumerator DownloadResource(string _imgId, bool _isLoadLocal)
	{
		if (!ResourcesManager.SpriteList.ContainsKey(_imgId))
		{
			string[] _directoryArr = _imgId.Split(new char[]
			{
				'/'
			});
			string directoryPath = "/";
			for (int i = 0; i < _directoryArr.Length - 1; i++)
			{
				directoryPath = directoryPath + _directoryArr[i] + "/";
			}
			if (!Directory.Exists(Application.persistentDataPath + directoryPath))
			{
				Directory.CreateDirectory(Application.persistentDataPath + directoryPath);
			}
			string localName = Application.persistentDataPath + "/" + _imgId;
			if (!File.Exists(localName))
			{
				WWW www = new WWW(ResourceChecking.BaseIp() + _imgId);
				yield return www;
				if (www.error == null)
				{
					File.WriteAllBytes(localName, www.bytes);
					UnityEngine.Debug.LogFormat("Downloaded:<color=yellow>{0}</color>", new object[]
					{
						_imgId
					});
					ResourcesManager.SpriteList.Add(_imgId, TextureCutter.TextureToSprite(www.texture));
				}
				else
				{
					UnityEngine.Debug.LogError(string.Concat(new string[]
					{
						"Error: ",
						www.error,
						"--Link: ",
						ResourceChecking.BaseIp(),
						_imgId
					}));
				}
			}
			else if (_isLoadLocal)
			{
				yield return base.StartCoroutine(this.LoadInLocalPath(_imgId));
			}
		}
		yield break;
	}

	private IEnumerator LoadInLocalPath(string localPath)
	{
		if (localPath[0] == '/')
		{
			localPath = localPath.Remove(0, 1);
		}
		if (!ResourcesManager.SpriteList.ContainsKey(localPath))
		{
			Sprite sprite = this.CheckSpriteInResource(localPath);
			if (sprite != null)
			{
				ResourcesManager.SpriteList.Add(localPath, sprite);
				yield break;
			}
			string path = Application.persistentDataPath;
			string localName = path + "/" + localPath;
			WWW ccc = new WWW("file:///" + localName);
			yield return ccc;
			if (ccc.error == null)
			{
				if (!ResourcesManager.SpriteList.ContainsKey(localPath))
				{
					ResourcesManager.SpriteList.Add(localPath, TextureCutter.TextureToSprite(ccc.texture));
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Can't load:" + localPath + " Error:" + ccc.error);
			}
		}
		yield break;
	}

	public void LoadSpriteInAtlas(List<string> urlList)
	{
		Dictionary<string, SpritePool> dictionary = new Dictionary<string, SpritePool>();
		foreach (string text in urlList)
		{
			if (!ResourcesManager.SpriteList.ContainsKey(text))
			{
				string[] array = text.Split(new char[]
				{
					'/'
				});
				string text2 = array[array.Length - 2];
				string name = array[array.Length - 1].Split(new char[]
				{
					'.'
				})[0];
				SpritePool spritePool;
				if (!dictionary.ContainsKey(text2))
				{
					spritePool = SpritePool.GetAtlasByName(text2);
					dictionary.Add(text2, spritePool);
				}
				else
				{
					spritePool = dictionary[text2];
				}
				Sprite value = spritePool.FindByName(name);
				ResourcesManager.SpriteList.Add(text, value);
			}
		}
		dictionary.Clear();
	}

	[SerializeField]
	private Image percentImg;

	private float totalFile;

	private float percentLoaded;
}
