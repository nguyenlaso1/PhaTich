// @sonhg: class: BombOffline.Offline_TapToPlayScreen
using System;
using System.Collections;
using System.Collections.Generic;
//using Facebook.Unity;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Sfs2X.Entities.Data;
using TapjoyUnity;
using UnityEngine;

namespace BombOffline
{
	public class Offline_TapToPlayScreen : MonoBehaviour
	{
		private void Start()
		{
			PlayerPrefs.SetFloat("MASTER_KEY", 0f);
			//PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().Build();
			//PlayGamesPlatform.InitializeInstance(configuration);
			//PlayGamesPlatform.DebugLogEnabled = true;
			//PlayGamesPlatform.Activate();
			Social.localUser.Authenticate(delegate(bool state)
			{
				UnityEngine.Debug.Log("Social.localUser.Authenticate: " + state);
			});
			//OneSignal.Init(Config.pushNotifiactionID, Config.googleProjectID, delegate(string message, Dictionary<string, object> additionalData, bool isActive)
			//{
			//	UnityEngine.Debug.Log("Init push notification");
			//});
			base.StartCoroutine(this.OfflineChecker());
			base.StartCoroutine(this.OfflineLoadInApp());
			//GoogleAnalyticsV3 googleAnalyticsV = UnityEngine.Object.Instantiate<GoogleAnalyticsV3>(this.googleAnalytics);
			//Context.googleAnalytics = googleAnalyticsV;
			//if (FB.IsInitialized)
			//{
			//	FB.ActivateApp();
			//}
			//else
			//{
			//	FB.Init(delegate()
			//	{
			//		FB.ActivateApp();
			//	}, null, null);
			//}
			BomberAds.RequestAds();
			//Context.GameInfo.InitAppsFlyer();
			if (!Tapjoy.IsConnected)
			{
				Tapjoy.Connect();
			}
			Tapjoy.OnConnectSuccess += this.HandleConnectSuccess;
			Tapjoy.OnConnectFailure += this.HandleConnectFailure;
		}

		public void HandleConnectSuccess()
		{
			UnityEngine.Debug.Log("Tapjoy Connect Success");
		}

		public void HandleConnectFailure()
		{
			UnityEngine.Debug.Log("Tapjoy Connect Failure");
		}

		private IEnumerator OfflineChecker()
		{
			string filePath = "Levels/resource";
			TextAsset txt = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
			string text = txt.text;
			ISFSObject mapObject = SFSObject.NewFromJsonData(text);
			ISFSArray tiledList = mapObject.GetSFSArray("tiled");
			for (int i = 0; i < tiledList.Count; i++)
			{
				ISFSObject tiled = tiledList.GetSFSObject(i);
				if (!ResourcesManager.TilesDict.ContainsKey(tiled.GetInt("i-id").ToString()))
				{
					Tile tile = new Tile();
					tile.Id = tiled.GetInt("i-id");
					tile.Path = tiled.GetUtfString("i-path");
					tile.Category = tiled.GetInt("category");
					ResourcesManager.TilesDict.Add(tile.Id.ToString(), tile);
				}
			}
			ISFSArray itemList = mapObject.GetSFSArray("item");
			for (int j = 0; j < itemList.Count; j++)
			{
				ISFSObject itemObj = itemList.GetSFSObject(j);
				if (!ResourcesManager.ItemsDict.ContainsKey(itemObj.GetInt("i-id").ToString()))
				{
					Item item = new Item();
					item.Id = itemObj.GetInt("i-id");
					item.Path = itemObj.GetUtfString("i-path");
					item.Icon = itemObj.GetUtfString("i-icon");
					item.Data = itemObj.GetUtfString("i-data");
					item.Price = itemObj.GetInt("i-price");
					item.Category = itemObj.GetInt("category");
					ResourcesManager.ItemsDict.Add(item.Id.ToString(), item);
				}
			}
			ISFSArray monsters = mapObject.GetSFSArray("monster");
			foreach (object obj in monsters)
			{
				SFSObject monster = (SFSObject)obj;
				Monster newMonster = new Monster();
				newMonster.Id = monster.GetInt("i-id");
				newMonster.Icon = monster.GetUtfString("i-icon");
				newMonster.Path = monster.GetUtfString("i-path");
				newMonster.Type = monster.GetInt("type");
				newMonster.Point = monster.GetInt("point");
				newMonster.Drop = monster.GetSFSObject("drop");
				if (!ResourcesManager.MonsterDict.ContainsKey(newMonster.Id.ToString()))
				{
					ResourcesManager.MonsterDict.Add(newMonster.Id.ToString(), newMonster);
				}
			}
			int[] _idArr = new int[]
			{
				PlayerPrefs.GetInt("PlayerHead", 53),
				PlayerPrefs.GetInt("PlayerBody", 57),
				PlayerPrefs.GetInt("PlayerHair", 49)
			};
			this.SetSkinForPlayer(_idArr, this.player.transform, TextureCutter.Parse("FFFFFF"));
			yield break;
		}

		public void SetSkinForPlayer(int[] _id, Transform _char, Color _hairColor)
		{
			Texture2D[] textureArr = new Texture2D[]
			{
				TextureCutter.GetSkinTexture(_id[0]),
				TextureCutter.GetSkinTexture(_id[1]),
				TextureCutter.GetSkinTexture(_id[2])
			};
			TextureCutter.CutAll(textureArr, _char, _hairColor);
		}

		public void OnClickScreen()
		{
			if (!this.isClick)
			{
				Offline_Context.Waitting.ShowWaiting();
				LoadingScreenManager.LoadScene("BomberMap", true);
			}
		}

		private IEnumerator OfflineLoadInApp()
		{
			TextAsset txt = (TextAsset)Resources.Load("inapp", typeof(TextAsset));
			string text = txt.text;
			ISFSArray inapp = SFSArray.NewFromJsonData(text);
			ResourcesManager.inappList.Clear();
			foreach (object obj in inapp)
			{
				SFSObject data = (SFSObject)obj;
				ResourcesManager.inappList.Add(new InappItem(data.GetUtfString("id"), data.GetDouble("pay").ToString(), data.GetInt("chip"), data.GetInt("gold")));
			}
			yield break;
		}

		public GoogleAnalyticsV3 googleAnalytics;

		public GameObject player;

		private bool isClick;
	}
}
