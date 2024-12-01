// @sonhg: class: Bomb.BomberMainMenu
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
//using Facebook.Unity;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Invitation;
using UnityEngine;
using UnityEngine.UI;

namespace Bomb
{
	public class BomberMainMenu : BaseScene
	{
		private void Awake()
		{
			if (!SmartFoxConnection.IsConnected)
			{
				Application.LoadLevelAsync("Loading");
				return;
			}
			Context.currentMono = this;
			this.smartFox = SmartFoxConnection.Connection;
			this.AddEventListener();
			if (BombConfigUtils.OFFLINE_ONLY)
			{
				foreach (GameObject gameObject in this.disableButton)
				{
					gameObject.SetActive(false);
				}
				this.ArenaButton.interactable = false;
			}
			else
			{
				foreach (GameObject gameObject2 in this.disableButton)
				{
					gameObject2.SetActive(true);
				}
				this.ArenaButton.interactable = true;
			}
		}

		private void Start()
		{
			this.checker = base.GetComponent<ResourceChecking>();
			this.InitScene();
			this.PlayMusic();
			//PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().Build();
			//PlayGamesPlatform.InitializeInstance(configuration);
			//PlayGamesPlatform.DebugLogEnabled = true;
			//PlayGamesPlatform.Activate();
			Social.localUser.Authenticate(delegate(bool state)
			{
				UnityEngine.Debug.Log("Social.localUser.Authenticate: " + state);
			});
		}

		public override void AddEventListener()
		{
			base.AddEventListener();
		}

		protected override void OnLoginSuccess(User user, ISFSObject data)
		{
			UnityEngine.Debug.Log("Loading: OnLoginSuccess");
			Context.Waiting.HideWaiting();
			this.RepaintInfo(user);
		}

		public override void OnExtensionResponse(BaseEvent evt)
		{
			SFSObject sfsobject = (SFSObject)evt.Params["params"];
			string text = (string)evt.Params["cmd"];
			string text2 = text;
			switch (text2)
			{
			case "lobby-message":
			{
				string utfString = sfsobject.GetUtfString("data");
				string utfString2 = sfsobject.GetUtfString("displayname");
				this.CheckScrollView();
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.chatLogPrefabs);
				gameObject.transform.SetParent(this.gridChat);
				gameObject.transform.localScale = Vector3.one;
				this.chatNotifyObject.SetActive(true);
				int num2 = utfString2.Length + utfString.Length;
				if (num2 > 16)
				{
					this.chatNotifyObject.transform.GetChild(0).GetComponent<Text>().text = string.Concat(new string[]
					{
						"<color=yellow>",
						utfString2.ToLower(),
						": </color>",
						utfString.Substring(0, 16 - utfString2.Length),
						"..."
					});
				}
				else
				{
					this.chatNotifyObject.transform.GetChild(0).GetComponent<Text>().text = "<color=yellow>" + utfString2.ToLower() + ": </color>" + utfString;
				}
				gameObject.transform.Find("Msg").GetComponent<Text>().text = "<color=yellow>" + utfString2.ToLower() + ": </color>" + utfString;
				this.chatLogObject.transform.Find("Scrollbar").GetComponent<Scrollbar>().value = 0f;
				return;
			}
			case "b-item":
				ItemResponse.RunMessage(evt, this);
				return;
			case "hot-event":
				HotEventResponse.RunMessage(evt, this);
				return;
			}
			base.OnExtensionResponse(evt);
		}

		public void AddMsgToChatLog(Text _contentLbl)
		{
			string text = _contentLbl.text;
			if (this.CheckEmptyString(text))
			{
				this.CheckScrollView();
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.chatLogPrefabs);
				gameObject.transform.SetParent(this.gridChat);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.Find("Msg").GetComponent<Text>().text = "<color=yellow>" + JokerUserUtils.GetFormatDisplayName(this.smartFox.MySelf, 0).ToLower() + ": </color>" + text;
				this.chatLogObject.transform.Find("Scrollbar").GetComponent<Scrollbar>().value = 0f;
			}
		}

		public void CheckScrollView()
		{
			if (this.gridChat.childCount > 20)
			{
				int childCount = this.gridChat.childCount;
				for (int i = childCount - 1; i > 0; i--)
				{
					UnityEngine.Object.Destroy(this.gridChat.GetChild(i).gameObject);
				}
			}
		}

		public override void InitScene()
		{
			OnlineAdmob.HideBannerAds();
			Context.currentGameId = "BOMB_BATTLE";
			ChooseGameRequest.SendMessage("BOMB_BATTLE");
			Context.clientGameState = JokerEnum.ClientGameState.GS_ON_MAINMENU_NORMAL;
			this._inventoryController.avatarController.SetUser(SmartFoxConnection.Connection.MySelf);
			Joker2XUtils.LoadImage(this.avatarTexture, JokerUserUtils.GetAvatar(SmartFoxConnection.Connection.MySelf), false);
			this.GetInventory();
			List<Map> list = ResourcesManager.MapDict.Values.ToList<Map>();
			foreach (Map map in list)
			{
				base.StartCoroutine(this.checker.FindSprite(map.Thumb, true));
			}
			List<Tile> list2 = ResourcesManager.TilesDict.Values.ToList<Tile>();
			foreach (Tile tile in list2)
			{
				base.StartCoroutine(this.checker.FindSprite(tile.Path, true));
			}
		}

		public override void OnUserVariablesUpdate(BaseEvent evt)
		{
			SFSUser sfsuser = (SFSUser)evt.Params["user"];
			if (sfsuser.IsItMe)
			{
				this.RepaintInfo(sfsuser);
			}
		}

		private void RepaintInfo(User user)
		{
			this.TextInfoArr[0].text = JokerUserUtils.GetFormatDisplayName(user, 17);
			this.TextInfoArr[1].text = JokerUserUtils.GetFormatChip(user);
			this.TextInfoArr[2].text = JokerUserUtils.GetFormatGold(user);
			this.TextInfoArr[3].text = "Level:  " + JokerUserUtils.GetLevel(user);
			this.TextInfoArr[4].text = "ID: " + JokerUserUtils.GetUserId(user).ToString();
		}

		public void OnClickInappButton()
		{
			this._inventoryController.Init();
			this.shopPanel.GetComponent<Animator>().Play("ShopUIAnim_back");
			this.inventoryPanel.GetComponent<Animator>().Play("ShopUIAnim_back");
			int num = 0;
			if (Context.GameInfo.DeviceLanguageId == 0)
			{
				num = 1;
			}
			if ((Joker2XConfigUtils.TURN_ON_CARD || Joker2XConfigUtils.TURN_ON_SMS) && num == 0)
			{
				CardSMSRequest.SendMessage(null);
				InappRequest.SendMessage(null);
				return;
			}
			if (Joker2XConfigUtils.TURN_ON_INAPP)
			{
				InappRequest.SendMessage(null);
				CardSMSRequest.SendMessage(null);
				return;
			}
			if (Joker2XConfigUtils.TURN_ON_CARD || Joker2XConfigUtils.TURN_ON_SMS)
			{
				CardSMSRequest.SendMessage(null);
				InappRequest.SendMessage(null);
				return;
			}
		}

		protected override void JokerUpdate()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				this.OnClickCloseShop();
				this.OnClickCloseInventory();
				this.OnClickCloseArenaButton();
				this.chatLogObject.SetBool("isOpen", false);
				if (base.transform.Find("TopUpBoxUI(Clone)") != null)
				{
					base.transform.Find("TopUpBoxUI(Clone)").GetComponent<TopupBox>().OnClickCloseButton();
				}
				if (base.transform.Find("GameSettingBox(Clone)") != null)
				{
					base.transform.Find("GameSettingBox(Clone)").GetComponent<MainMenuSettingBox>().OnClickCloseButton();
				}
				if (base.transform.Find("TopUpBoxUI(Clone)") == null && base.transform.Find("GameSettingBox(Clone)") == null && this.chatLogObject.transform.localPosition.y > 400f && this.selectModePanel.transform.localScale.x == 0f && this.inventoryPanel.GetComponent<RectTransform>().localPosition.x < -500f && this.shopPanel.GetComponent<RectTransform>().localPosition.x < -500f)
				{
					Application.Quit();
				}
			}
		}

		public void OnClickChangeMode()
		{
			if (this._currentSelectModeIndex == 1)
			{
				this._currentSelectModeIndex = 0;
				this.selectModeLabel.text = "solo";
			}
			else
			{
				this._currentSelectModeIndex = 1;
				this.selectModeLabel.text = "team";
			}
		}

		public void OnClickShowChatLog()
		{
			this.chatLogObject.SetBool("isOpen", true);
		}

		public void OnClickToggleChatLog()
		{
			bool value = !this.chatLogObject.GetBool("isOpen");
			this.chatLogObject.SetBool("isOpen", value);
		}

		public void OnClickSettingButton()
		{
			this._inventoryController.detailPanel.SetActive(false);
			this.selectModePanel.transform.localScale = Vector3.zero;
			MainMenuSettingBox setting = Context.Setting;
		}

		public void OnClickInventory()
		{
			this._inventoryController.Init();
			this.selectModePanel.transform.localScale = Vector3.zero;
			this.inventoryPanel.SetActive(true);
			this.inventoryPanel.GetComponent<Animator>().Play("ShopUIAnim");
			this.OnClickCloseShop();
		}

		public void OnClickShop()
		{
			this.selectModePanel.transform.localScale = Vector3.zero;
			this.OnClickCloseInventory();
			this.shopPanel.SetActive(true);
			this.shopPanel.GetComponent<Animator>().Play("ShopUIAnim");
		}

		public void OnClickCloseShop()
		{
			this._inventoryController.Init();
			int childCount = this._shopController.gridTrans.childCount;
			for (int i = childCount - 1; i > 0; i--)
			{
				UnityEngine.Object.Destroy(this._shopController.gridTrans.GetChild(i).gameObject);
			}
			this.shopPanel.GetComponent<Animator>().Play("ShopUIAnim_back");
			this.shopPanel.transform.GetChild(0).Find("CategoryPanel").gameObject.SetActive(true);
			this.shopPanel.transform.GetChild(0).Find("ShopPanel").gameObject.SetActive(false);
		}

		public void OnClickCloseInventory()
		{
			this._inventoryController.Init();
			int childCount = this._inventoryController.gridTrans.childCount;
			for (int i = childCount - 1; i > 0; i--)
			{
				UnityEngine.Object.Destroy(this._inventoryController.gridTrans.GetChild(i).gameObject);
			}
			this.inventoryPanel.GetComponent<Animator>().Play("ShopUIAnim_back");
			this.inventoryPanel.transform.GetChild(0).Find("CategoryPanel").gameObject.SetActive(true);
			this.inventoryPanel.transform.GetChild(0).Find("ShopPanel").gameObject.SetActive(false);
		}

		public void OnClickBossButton()
		{
			Application.LoadLevelAsync("BomberMap");
		}

		public void OnClickArenaButton()
		{
			this._inventoryController.detailPanel.SetActive(false);
			if (this.selectModePanel.transform.localScale.x < 1f)
			{
				iTween.ScaleTo(this.selectModePanel, iTween.Hash(new object[]
				{
					"scale",
					Vector3.one,
					"time",
					0.2f,
					"easetype",
					iTween.EaseType.linear
				}));
			}
		}

		public void OnClickCloseArenaButton()
		{
			iTween.Stop(this.selectModePanel);
			iTween.ScaleTo(this.selectModePanel, iTween.Hash(new object[]
			{
				"scale",
				Vector3.zero,
				"time",
				0.2f,
				"easetype",
				iTween.EaseType.linear
			}));
		}

		public void OnClickQuickPlay()
		{
			base.StartCoroutine(this.WaitToLoadSprite(0));
		}

		public void OnClickCreateGame()
		{
			base.StartCoroutine(this.WaitToLoadSprite(1));
		}

		public void OnClickBattleTeamButton()
		{
			Context.currentGameId = "BATTLE_TEAM";
			ChooseGameRequest.SendMessage("BATTLE_TEAM");
			ChooseModeBox chooseMode = Context.ChooseMode;
			Context.OnDeletegateObject onVSOthers = delegate(object obj)
			{
				base.StartCoroutine(this.WaitToLoadSprite(1));
			};
			Context.OnDeletegateObject onVSFriends = delegate(object obj)
			{
				base.StartCoroutine(this.WaitToLoadSprite(0));
			};
			chooseMode.AddDelegate(onVSFriends, onVSOthers);
		}

		public void OnClickFriendButton()
		{
			//if (!FB.IsInitialized)
			//{
			//	FacebookAPI.Instance.CallFBInit(delegate
			//	{
			//		this.InviteFBFriends();
			//	});
			//}
			//else
			//{
			//	this.InviteFBFriends();
			//}
		}

		private void InviteFBFriends()
		{
			//FacebookAPI.Instance.InviteFriend(new FacebookDelegate<IAppRequestResult>(this.HandleResult));
		}

		//private void HandleResult(IAppRequestResult result)
		//{
		//	if (result == null)
		//	{
		//		return;
		//	}
		//	if (!string.IsNullOrEmpty(result.Error))
		//	{
		//		Context.Confirm.AddMessage(Language.INVITE_FB_FRIENDS_ERROR, string.Empty, string.Empty);
		//	}
		//	else if (!result.Cancelled)
		//	{
		//		if (!string.IsNullOrEmpty(result.RawResult))
		//		{
		//			ShareFacebookRequest.SendMessage(result.To.Count<string>());
		//		}
		//	}
		//}

		private IEnumerator WaitToLoadSprite(int _num)
		{
			if (_num == 0)
			{
				this.ShowLoading(true, "FINDING ROOM...");
			}
			else if (_num == 1)
			{
				this.ShowLoading(true, "CREATING ROOM...");
			}
			else if (_num == 2)
			{
				this.ShowLoading(true, "LOADING...");
			}
			else
			{
				this.ShowLoading(true, "LOADING ROOM...");
			}
			if (!BomberMainMenu.isLoadedInventory)
			{
				List<Map> mapList = ResourcesManager.MapDict.Values.ToList<Map>();
				foreach (Map _map in mapList)
				{
					yield return base.StartCoroutine(this.checker.FindSprite(_map.Thumb, true));
				}
				List<Tile> tileList = ResourcesManager.TilesDict.Values.ToList<Tile>();
				foreach (Tile _tile in tileList)
				{
					yield return base.StartCoroutine(this.checker.FindSprite(_tile.Path, true));
				}
				List<Item> itemList = ResourcesManager.ItemsDict.Values.ToList<Item>();
				foreach (Item _item in itemList)
				{
					yield return base.StartCoroutine(this.checker.FindSprite(_item.Path, true));
					yield return base.StartCoroutine(this.checker.FindSprite(_item.Icon, true));
				}
				BomberMainMenu.isLoadedInventory = true;
			}
			if (_num == 0)
			{
				if (this._currentSelectModeIndex == 0)
				{
					ChooseGameRequest.SendMessage("BOMB_BATTLE");
				}
				else
				{
					ChooseGameRequest.SendMessage("BATTLE_TEAM");
				}
				JoinRoomRequest.SendMessage(1, -1, 0, -1, false);
			}
			else if (_num == 1)
			{
				if (this._currentSelectModeIndex == 0)
				{
					ChooseGameRequest.SendMessage("BOMB_BATTLE");
				}
				else
				{
					ChooseGameRequest.SendMessage("BATTLE_TEAM");
				}
				CreateRoomRequest.SendMessage(1);
			}
			else if (_num == 2)
			{
				this._inventoryController.Init();
				this.inventoryPanel.SetActive(true);
				this.inventoryPanel.transform.DOLocalMove(new Vector3(3f, 3f, 0f), 10f, false);
				this.ShowLoading(false, null);
			}
			BomberMainMenu.isLoadedInventory = true;
			yield break;
		}

		public override void OnInvitation(BaseEvent evt)
		{
			Invitation invitation = (Invitation)evt.Params["invitation"];
			User inviter = invitation.Inviter;
			ISFSObject @params = invitation.Params;
			@params.PutUtfString("gameName", JokerUserUtils.GetGameName(inviter));
			int num = Convert.ToInt32(@params.GetUtfString("r-gname"));
			string str = string.Format(Language.INVITE_INVITE, JokerUserUtils.GetFormatDisplayName(inviter, 20), Language.GetGameName(JokerUserUtils.GetGameName(inviter).ToUpper()));
			ConfirmBox confirm = Context.Confirm;
			confirm.AddMessageYesNo(str, new Context.OnDeletegateObject(this.AcceptInvitation), @params, new Context.OnDeletegateObject(this.DenieInvitation), confirm, string.Empty, string.Empty, false);
		}

		public void OnClickExitButton()
		{
			base.ExitGame();
			StaticPrefab.GetPrefab("Prefabs/Joker2x/GameItem/RankingItem");
		}

		private void AcceptInvitation(object obj)
		{
			int @int = ((ISFSObject)obj).GetInt("r-id");
			int num = Convert.ToInt32(((ISFSObject)obj).GetUtfString("r-gname"));
			int int2 = ((ISFSObject)obj).GetInt("position");
			string utfString = ((ISFSObject)obj).GetUtfString("gameName");
			Context.currentGameId = utfString;
			ChooseGameRequest.SendMessage(utfString);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"vao phong id = ",
				@int,
				"--GroupId: ",
				num,
				" POSITION ",
				int2,
				" Game name ",
				utfString
			}));
			base.StartCoroutine(this.WaitToLoadRoomInvi(@int, num, int2));
		}

		private IEnumerator WaitToLoadRoomInvi(int roomId, int GroupId, int position)
		{
			yield return base.StartCoroutine(this.WaitToLoadSprite(3));
			this.LoadGame(GroupId, roomId, 3, position);
			yield break;
		}

		private void DenieInvitation(object obj)
		{
			if (obj is UnityEngine.Object)
			{
				UnityEngine.Object.Destroy((UnityEngine.Object)obj);
			}
		}

		public void LoadGame(int groupIndex, int roomId, int joinRoomType, int position)
		{
			UnityEngine.Debug.Log("Load game");
			JoinRoomRequest.SendMessage(groupIndex, roomId, joinRoomType, position, true);
		}

		public void ReJoin()
		{
			base.StartCoroutine(this.WaitToSendMsg());
		}

		public void ShowPopupConfirmJoinRoom()
		{
			this.count = 0;
			ConfirmBox confirm = Context.Confirm;
			this._currentObj = confirm.gameObject;
			confirm.AddMessageYesNo(Language.ALL_ROOM_ARE_BUSY, new Context.OnDeletegateObject(this.SendJoinMsg), null, new Context.OnDeletegateObject(this.SendCreateMsg), null, null, "CREATE", true);
		}

		private void SendJoinMsg(object obj)
		{
			UnityEngine.Object.Destroy(this._currentObj);
			JoinRoomRequest.SendMessage(1, -1, 0, -1, false);
			this.ShowLoading(true, "LOADING...");
		}

		private void SendCreateMsg(object onj)
		{
			CreateRoomRequest.SendMessage(1);
		}

		private IEnumerator WaitToSendMsg()
		{
			this.count++;
			yield return new WaitForSeconds(1f);
			JoinRoomRequest.SendMessage(1, -1, 0, -1, false);
			yield break;
		}

		public void ShowLoading(bool _isShow, string _msg = null)
		{
			this.findingObj.SetActive(_isShow);
			if (_msg != null && _isShow)
			{
				this.loadingText.text = _msg;
			}
		}

		public void ShowPopupProfile()
		{
			GameObject profile = Context.Profile;
			profile.transform.SetParent(base.transform, false);
		}

		public void ShowLeaderBoard()
		{
			Social.ShowLeaderboardUI();
		}

		private void PlayMusic()
		{
			MusicManager.instance.PlayMusic(this.menuMusic);
		}

		public void GetInventory()
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutInt("task", 0);
			ItemRequest.SendMessage(sfsobject);
		}

		public static void SaveDataToPlayerPref()
		{
			PlayerPrefs.SetInt("PlayerHead", MMOUserUtils.GetHead(SmartFoxConnection.Connection.MySelf));
			PlayerPrefs.SetInt("PlayerHair", MMOUserUtils.GetHair(SmartFoxConnection.Connection.MySelf));
			PlayerPrefs.SetInt("PlayerBody", MMOUserUtils.GetBody(SmartFoxConnection.Connection.MySelf));
			PlayerPrefs.SetInt("PlayerBomb", MMOUserUtils.GetBomb(SmartFoxConnection.Connection.MySelf));
		}

		private void FillExp()
		{
			int exp = JokerUserUtils.GetExp(SmartFoxConnection.Connection.MySelf);
			int expNextLevel = JokerUserUtils.GetExpNextLevel(SmartFoxConnection.Connection.MySelf);
			this.expBar.fillAmount = (float)exp / (float)expNextLevel;
		}

		private bool CheckEmptyString(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = text.Trim();
				return text2.Length > 0;
			}
			return false;
		}

		public override void OnJoinRoomError(BaseEvent evt)
		{
			JoinRoomErrorResponse.RunMessage(evt, this);
		}

		[SerializeField]
		private BaseAvatarContainer _avatarContainer;

		public GameObject topUpObj;

		public GameObject findingObj;

		public GameObject inventoryPanel;

		public GameObject selectModePanel;

		public GameObject shopPanel;

		public Text selectModeLabel;

		public GameObject chatNotifyObject;

		public GameObject avatarTexture;

		public Text loadingText;

		public InventoryController _inventoryController;

		public ShopController _shopController;

		public Text[] TextInfoArr;

		public AudioClip menuMusic;

		public Transform gridChat;

		public GameObject chatLogPrefabs;

		public Animator chatLogObject;

		public Image expBar;

		private ResourceChecking checker;

		private int _currentSelectModeIndex;

		public static bool isLoadedInventory;

		public GameObject[] disableButton;

		public Button ArenaButton;

		public int count;

		private GameObject _currentObj;
	}
}
