// @sonhg: class: BombOffline.OfflineMapChooser
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
//using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	public class OfflineMapChooser : MonoBehaviour
	{
		public static string GetNextLevel()
		{
			int num = OfflineMapChooser.mapList.FindIndex((string level) => level.Equals(OfflineMapChooser.CurrentLevel));
			if (num + 1 >= OfflineMapChooser.mapList.Count)
			{
				return null;
			}
			return OfflineMapChooser.mapList[num + 1];
		}

		public static bool CanUnlockNextZone()
		{
			int num = OfflineMapChooser.mapList.FindIndex((string level) => level.Equals(OfflineMapChooser.CurrentLevel));
			float num2 = (float)num / (float)OfflineMapChooser.mapList.Count;
			return num2 >= 0f;
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && this.backOnlineButton.activeSelf)
			{
				this.BacktoLoading();
			}
		}

		private void Awake()
		{
			this.ReadGameProgress();
			base.StartCoroutine(this.WaitForScroll());
		}

		private IEnumerator WaitForScroll()
		{
			this.canvasGroup.blocksRaycasts = false;
			yield return new WaitForSeconds(0.5f);
			this.canvasGroup.blocksRaycasts = true;
			yield break;
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus)
			{
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
			}
		}

		private void Start()
		{
			MusicManager.instance.PlayMusic(this.music);
		}

		public void LoadZone(string zoneName, string zoneID)
		{
			this.isSwitchScene = true;
			OfflineMapChooser.CurrentZone = zoneName;
			OfflineMapChooser.CurrentZoneProgress = BombSaveGame.LoadZoneProgress(OfflineMapChooser.CurrentZone);
			OfflineMapChooser.Index = OfflineMapChooser.zoneList.FindIndex((string zone) => zone.CompareTo(zoneName) == 0);
			string path = "Levels/Bomber/" + zoneName;
			List<TextAsset> list = new List<TextAsset>(Resources.LoadAll<TextAsset>(path));
			OfflineMapChooser.mapList = new List<string>();
			list.Sort((TextAsset x, TextAsset y) => this.Eval(x.name) - this.Eval(y.name));
			if (OfflineMapChooser.CurrentZoneProgress.CurrentLevel == null)
			{
				OfflineMapChooser.CurrentZoneProgress.CurrentLevel = list[0].name;
			}
			OfflineMapChooser.CurrentLevel = OfflineMapChooser.CurrentZoneProgress.CurrentLevel;
			OfflineMapChooser.CurrentZoneID = zoneID;
			foreach (TextAsset textAsset in list)
			{
				OfflineMapChooser.mapList.Add(textAsset.name);
			}
			Offline_Context.Waitting.ShowWaiting();
			LoadingScreenManager.LoadScene("OfflineMainScene", true);
		}

		public void ShowLeaderBoard()
		{
			if (Social.localUser.authenticated)
			{
				Social.ShowLeaderboardUI();
			}
			else
			{
				Social.localUser.Authenticate(delegate(bool success)
				{
					if (success)
					{
						Social.ShowLeaderboardUI();
					}
				});
			}
		}

		public void BacktoLoading()
		{
			if (!SmartFoxConnection.IsConnected)
			{
				Application.LoadLevelAsync("Loading");
				return;
			}
			Application.LoadLevelAsync("BomberMainMenu");
		}

		private int Eval(string level)
		{
			string[] array = level.Replace("lvl", string.Empty).Split(new char[]
			{
				'-'
			});
			int num = int.Parse(array[0]) * 1000;
			int num2 = int.Parse(array[1]);
			return num + num2;
		}

		private void ReadGameProgress()
		{
			OfflineMapChooser.zoneList = new List<string>();
			foreach (ZoneItem zoneItem in this.zoneItemList)
			{
				OfflineMapChooser.zoneList.Add(zoneItem.zoneName);
			}
			OfflineMapChooser.CurrentGameProgress = GameProgress.LoadGameProgress();
            int num = OfflineMapChooser.zoneList.Count - 1;
			GameProgress.SaveGameProgress();
			for (int i = 0; i <= num; i++)
			{
				this.zoneItemList[i].Interactable = true;
			}
			this.ResetScrollPosition(PlayerPrefs.GetInt("ScrollIndex", 0));
		}

		private void ResetScrollPosition(int index)
		{
			int num = this.scroll.content.childCount - 1;
			if (index < 0)
			{
				index = 0;
			}
			if (index > num)
			{
				index = num;
			}
			DOTween.To(() => this.scroll.horizontalNormalizedPosition, delegate(float x)
			{
				this.scroll.horizontalNormalizedPosition = x;
			}, (float)index / (float)num, 1f).SetDelay(0.5f);
		}

		private bool CheckZone(string zoneName)
		{
			BombSaveGame bombSaveGame = BombSaveGame.LoadZoneProgress(zoneName);
			if (bombSaveGame.IsPassed)
			{
				return true;
			}
			int mapIndex = this.GetMapIndex(zoneName);
			return mapIndex == OfflineMapChooser.mapList.Count - 1;
		}

		private bool CheckFirstMap()
		{
			return this.GetMapIndex("forest") > 0;
		}

		private int GetMapIndex(string zoneName)
		{
			BombSaveGame zoneProgress = BombSaveGame.LoadZoneProgress(zoneName);
			string path = "Levels/Bomber/" + zoneName;
			List<TextAsset> list = new List<TextAsset>(Resources.LoadAll<TextAsset>(path));
			OfflineMapChooser.mapList = new List<string>();
			list.Sort((TextAsset x, TextAsset y) => this.Eval(x.name) - this.Eval(y.name));
			foreach (TextAsset textAsset in list)
			{
				OfflineMapChooser.mapList.Add(textAsset.name);
			}
			return OfflineMapChooser.mapList.FindIndex((string map) => map.CompareTo(zoneProgress.CurrentLevel) == 0);
		}

		public static string CurrentLevel = null;

		public static BombSaveGame CurrentZoneProgress;

		public static GameProgress CurrentGameProgress;

		public static bool isNextZone = false;

		public static int MaxLevel;

		public static int Index;

		public static List<string> mapList;

		public static string CurrentZone;

		public static string CurrentZoneID;

		public static string TempZone;

		public static List<string> zoneList = new List<string>();

		[SerializeField]
		protected GameObject backOnlineButton;

		[SerializeField]
		protected AudioClip music;

		[SerializeField]
		protected ScrollRect scroll;

		[SerializeField]
		protected CanvasGroup canvasGroup;

		private bool isSwitchScene;

		public ConfirmBox confirmBox;

		public Offline_MapSelectItem mapSelectItem;

		[SerializeField]
		private List<ZoneItem> zoneItemList;
	}
}
