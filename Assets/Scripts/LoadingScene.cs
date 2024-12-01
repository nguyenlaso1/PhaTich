// @sonhg: class: LoadingScene
using System;
using System.Collections;
using System.Collections.Generic;
using Bomb;
//using Facebook.Unity;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ResourceChecking))]
public class LoadingScene : BaseScene
{
    static Dictionary<string, int> _003C_003Ef__switch_0024mapB;
	public override void AddEventListener()
	{
		base.AddEventListener();
	}

	private void Awake()
	{
		UnityEngine.Debug.Log("Awake Loading");
		Context.currentMono = this;
		if (Context.clientGameState == JokerEnum.ClientGameState.GS_INIT_GAME)
		{
			if (Context.GameInfo.LastAccounType == 2 && Context.fbKey.Equals(string.Empty))
			{
				//FacebookAPI.Instance.LoginFB(new FacebookDelegate<ILoginResult>(base.LoginFBCallBack), this.onLoginFBSuccess);
			}
			else
			{
				base.InitServer();
			}
		}
		else
		{
			Context.Waiting.HideWaiting();
		}
		if (Context.clientGameState == JokerEnum.ClientGameState.GS_LOGOUT_MERGE_FB)
		{
			this.OnClickFB();
		}
		Context.ShowGameStateDes();
		Context.currentGameId = string.Empty;
		MusicManager.instance.StopMusic();
		OneSignal.Init(Config.pushNotifiactionID, Config.googleProjectID, new OneSignal.NotificationReceived(LoadingScene.HandleNotification));
		Context.googleAnalytics = this.googleAnalytics;
	}

	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive)
	{
	}

	protected override void OnPressEscape()
	{
	}

	private void Start()
	{
		Context.Init();
		this.InitScene();
	}

	private void OnLevelWasLoaded(int level)
	{
		UnityEngine.Debug.Log("OnLevelWasLoaded");
	}

	public void GoToOfflineSelectMap()
	{
		Application.LoadLevelAsync("BomberMap");
	}

	protected override void OnLoginSuccess(User user, ISFSObject data)
	{
		UnityEngine.Debug.Log("Loading: OnLoginSuccess");
		this.bottomButton.SetActive(false);
		Context.Waiting.HideWaiting();
		Context.Waiting.ShowWaitingWithoutIcon();
		base.StartCoroutine(this.CheckThenLoad(data, user));
	}

	private IEnumerator CheckThenLoad(ISFSObject data, User user)
	{
		yield return base.StartCoroutine(this.LoadingReource(data));
		this.LoginProcess(data, user);
		yield break;
	}

	private IEnumerator LoadingReource(ISFSObject _data)
	{
		ResourceChecking checker = base.GetComponent<ResourceChecking>();
		ISFSArray maps = _data.GetSFSArray("maps");
		ISFSArray maptiles = _data.GetSFSArray("map_tileds");
		ISFSArray items = _data.GetSFSArray("items");
		foreach (object obj in maps)
		{
			SFSObject _map = (SFSObject)obj;
			Map newMap = new Map();
			newMap.Id = _map.GetInt("i-id");
			newMap.Description = _map.GetUtfString("i-des");
			newMap.Thumb = _map.GetUtfString("thumbnail");
			newMap.Name = _map.GetUtfString("i-name");
			newMap.Path = _map.GetUtfString("i-path");
			if (!ResourcesManager.MapDict.ContainsKey(newMap.Id.ToString()))
			{
				ResourcesManager.MapDict.Add(newMap.Id.ToString(), newMap);
			}
		}
		foreach (object obj2 in maptiles)
		{
			SFSObject _maptiled = (SFSObject)obj2;
			Tile newTile = new Tile();
			newTile.IRow = _maptiled.GetInt("i-row");
			newTile.IColumn = _maptiled.GetInt("i-column");
			newTile.Id = _maptiled.GetInt("i-id");
			newTile.Category = _maptiled.GetInt("category");
			newTile.Path = _maptiled.GetUtfString("i-path");
			if (!ResourcesManager.TilesDict.ContainsKey(newTile.Id.ToString()))
			{
				ResourcesManager.TilesDict.Add(newTile.Id.ToString(), newTile);
			}
		}
		foreach (object obj3 in items)
		{
			SFSObject _item = (SFSObject)obj3;
			if (!string.IsNullOrEmpty(_item.GetUtfString("i-path")))
			{
				string _imdIdNoExtension = _item.GetUtfString("i-path").Substring(0, _item.GetUtfString("i-path").Length - 4);
				if (!checker.CheckSpriteInResource(_imdIdNoExtension))
				{
					checker.CaculateTotalFileNeedToLoad(_item.GetUtfString("i-path"));
				}
			}
			if (!string.IsNullOrEmpty(_item.GetUtfString("i-icon")))
			{
				string _imdIdNoExtension2 = _item.GetUtfString("i-icon").Substring(0, _item.GetUtfString("i-icon").Length - 4);
				if (!checker.CheckSpriteInResource(_imdIdNoExtension2))
				{
					checker.CaculateTotalFileNeedToLoad(_item.GetUtfString("i-icon"));
				}
			}
		}
		foreach (object obj4 in items)
		{
			SFSObject _item2 = (SFSObject)obj4;
			Item newItem = new Item();
			newItem.IRow = _item2.GetInt("i-row");
			newItem.IColumn = _item2.GetInt("i-column");
			newItem.Name = _item2.GetUtfString("i-name");
			newItem.Id = _item2.GetInt("i-id");
			newItem.PType = _item2.GetInt("i-priceType");
			newItem.Category = _item2.GetInt("category");
			newItem.Description = _item2.GetUtfString("i-des");
			newItem.Price = _item2.GetInt("i-price");
			newItem.Expire = _item2.GetInt("i-exprice");
			newItem.Experience = _item2.GetInt("i-experience");
			newItem.Path = _item2.GetUtfString("i-path");
			newItem.Icon = _item2.GetUtfString("i-icon");
			newItem.Data = _item2.GetUtfString("i-data");
			if (!string.IsNullOrEmpty(newItem.Path))
			{
				string _imdIdNoExtension3 = newItem.Path.Substring(0, newItem.Path.Length - 4);
				if (!checker.CheckSpriteInResource(_imdIdNoExtension3))
				{
					yield return base.StartCoroutine(checker.FindSprite(newItem.Path, true));
				}
			}
			if (!string.IsNullOrEmpty(newItem.Icon))
			{
				string _imdIdNoExtensionIcon = newItem.Icon.Substring(0, newItem.Icon.Length - 4);
				if (!checker.CheckSpriteInResource(_imdIdNoExtensionIcon))
				{
					yield return base.StartCoroutine(checker.FindSprite(newItem.Icon, true));
				}
			}
			if (!ResourcesManager.ItemsDict.ContainsKey(newItem.Id.ToString()))
			{
				ResourcesManager.ItemsDict.Add(newItem.Id.ToString(), newItem);
			}
		}
		BomberMainMenu.isLoadedInventory = true;
		yield break;
	}

	private void LoginProcess(ISFSObject data, User user)
	{
		Context.dailyGiftChip = -1;
		if (data != null && data.ContainsKey("chipReward"))
		{
			Context.dailyGiftChip = data.GetInt("chipReward");
			Context.daysGiftChip = data.GetInt("noReward");
		}
		Context.loginMessage = string.Empty;
		if (data != null && data.ContainsKey("loginText"))
		{
			Context.loginMessage = data.GetUtfString("loginText");
		}
		string key = StoreKey.KEY_USER_ITEM + Config.versionCode;
		if (!PlayerPrefs.HasKey(key))
		{
			PlayerPrefs.DeleteKey(StoreKey.KEY_USER_ITEM);
			PlayerPrefs.SetInt(key, 1);
		}
		if (data.ContainsKey("p-data"))
		{
			ISFSArray sfsarray = data.GetSFSArray("p-data");
			Context.arrUserItemList = sfsarray;
		}
		if (data.ContainsKey("p-delete-all-key"))
		{
			PlayerPrefs.DeleteAll();
		}
		if (Joker2XConfigUtils.IsLoaded())
		{
			this.GoListGameScreen();
		}
	}

	public void OnLoadVPConfig(SFSObject dataObject)
	{
		UnityEngine.Debug.Log("OnLoadVPConfig");
		Joker2XConfigUtils.SetInfo(dataObject);
		BombConfigUtils.SetInfo(dataObject);
	}

	public void GoListGameScreen()
	{
		if (Config.defaultGame.Equals(string.Empty))
		{
			if (Joker2XConfigUtils.LIST_GAMES_ID.Count >= 2)
			{
				if (Joker2XConfigUtils.LIST_GAMES_ID.Contains("BOMB_BATTLE"))
				{
					Context.currentGameId = "BOMB_BATTLE";
				}
				else
				{
					Context.currentGameId = Joker2XConfigUtils.LIST_GAMES_ID[0];
				}
				Context.LoadLevelAsyn("BomberMainMenu", false);
			}
			else
			{
				if (Joker2XConfigUtils.LIST_GAMES_ID.Count == 1)
				{
					Context.currentGameId = Joker2XConfigUtils.LIST_GAMES_ID[0];
				}
				else
				{
					Context.currentGameId = "BOMB_BATTLE";
				}
				Context.LoadLevelAsyn("BomberMainMenu", false);
			}
		}
		else
		{
			Context.currentGameId = Config.defaultGame;
			Context.LoadLevelAsyn("BomberMainMenu", false);
		}
	}

	public void OnClickFB()
	{
		UnityEngine.Debug.Log("Click Facebook");
		//if (FB.IsLoggedIn)
		//{
		//	base.OnLoginFB();
		//}
		//else
		//{
		//	FacebookAPI.Instance.LoginFB(new FacebookDelegate<ILoginResult>(base.LoginFBCallBack), this.onLoginFBSuccess);
		//}
	}

	public void OnClickGuest()
	{
		base.OnLoginGuest();
	}

	public override void OnExtensionResponse(BaseEvent evt)
	{
		SFSObject sfsobject = (SFSObject)evt.Params["params"];
		string text = (string)evt.Params["cmd"];
		UnityEngine.Debug.Log("cmd: " + text + " OnExtensionResponse" + sfsobject.GetDump());
		string text2 = text;
		if (text2 != null)
		{
			if (LoadingScene._003C_003Ef__switch_0024mapB == null)
			{
				LoadingScene._003C_003Ef__switch_0024mapB = new Dictionary<string, int>(1)
				{
					{
						"s-loadconfig",
						0
					}
				};
			}
			int num;
			if (LoadingScene._003C_003Ef__switch_0024mapB.TryGetValue(text2, out num))
			{
				if (num == 0)
				{
					UnityEngine.Debug.Log("OnResponseCmds");
					this.OnLoadVPConfig(sfsobject);
					return;
				}
			}
		}
		base.OnExtensionResponse(evt);
	}

	public override void InitScene()
	{
		this.versionLabel.text = Config.versionName;
		this.bottomButton.SetActive(LoadingScene.firstLogin);
		if (!LoadingScene.firstLogin)
		{
			LoadingScene.firstLogin = true;
		}
	}

	public void ActiveBottomButton()
	{
		this.bottomButton.SetActive(true);
	}

	public override void OnJoinRoomError(BaseEvent evt)
	{
		throw new NotImplementedException();
	}

	public GameObject bottomButton;

	public Text versionLabel;

	public GoogleAnalyticsV3 googleAnalytics;

	private static bool firstLogin;
}
