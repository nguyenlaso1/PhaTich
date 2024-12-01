// @sonhg: class: MainMenuScene
using System;
using System.Collections.Generic;
using DG.Tweening;
//using Facebook.Unity;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Invitation;
using UnityEngine;

public class MainMenuScene : BaseScene
{
    static Dictionary<string, int> _003C_003Ef__switch_0024mapC;
	public override void AddEventListener()
	{
		base.AddEventListener();
	}

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
	}

	private void Start()
	{
		this.InitScene();
	}

	protected override void JokerUpdate()
	{
	}

	protected override void OnPressEscape()
	{
		base.ExitGame();
	}

	protected override void OnPressMenu()
	{
	}

	private void OnLevelWasLoaded(int level)
	{
		if (Context.loginMessage != string.Empty)
		{
			Context.Confirm.AddMessageYes(Context.loginMessage, null, null, string.Empty);
			Context.loginMessage = string.Empty;
		}
		else if (Context.dailyGiftChip > 0)
		{
			string dailyChipDayText = Language.GetDailyChipDayText(Context.daysGiftChip);
			string text = Context.dailyGiftChip.ToString();
			StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/DailyRewardBox").GetComponent<DailyRewardBox>().SetInfo(Context.daysGiftChip, dailyChipDayText, "+$" + text, text);
			Context.dailyGiftChip = -1;
			Context.daysGiftChip = -1;
		}
		if (Context.arrUserItemList == null)
		{
			GetUserItemRequest.SendMessage(-1);
		}
		if (Context.clientGameState == JokerEnum.ClientGameState.GS_ON_MAINMENU_TOPUP)
		{
			this.OnClickShopButton();
		}
	}

	public void OnClickLogout()
	{
		base.Logout(JokerEnum.ClientGameState.GS_LOGOUT_NORMAL);
	}

	public void OnClickMergeFB()
	{
		if (Context.GameInfo.LastAccounType == 0)
		{
			Context.Waiting.ShowWaiting();
			//FacebookAPI.Instance.LoginFB(new FacebookDelegate<ILoginResult>(base.MergeFBCallBack), this.onLoginFBSuccess);
			UnityEngine.Debug.Log("Click merge");
		}
	}

	public void UpdateGameName()
	{
		this.gameLabel.GetComponent<UILabel>().text = Language.GetGameName(Context.currentGameId);
	}

	public override void InitScene()
	{
		this._avatarContainerScipt = this.avatarContainer.GetComponent<BaseAvatarContainer>();
		this._avatarContainerScipt.SetButtonParams(SmartFoxConnection.Connection.MySelf, delegate(object o)
		{
			StaticGameObject.GetGameObject("Prefabs/Bomber/Boxs/ProfileBox");
		}, null);
		this.soundButton.GetComponent<UIToggle>().value = (Context.GameInfo.Volume > 0f);
		this.scrollViewController.InitScrollView();
		this.UpdateGameName();
		this.versionLabel.GetComponent<UILabel>().text = Config.versionName;
		this.hotlineLabel.GetComponent<UILabel>().text = Joker2XConfigUtils.HOT_LINE;
		Context.clientGameState = JokerEnum.ClientGameState.GS_ON_MAINMENU_NORMAL;
	}

	public void OnClickShopButton()
	{
		int num = 0;
		if (Context.GameInfo.DeviceLanguageId == 0)
		{
			num = 1;
		}
		if ((Joker2XConfigUtils.TURN_ON_CARD || Joker2XConfigUtils.TURN_ON_SMS) && num == 0)
		{
			CardSMSRequest.SendMessage(null);
			return;
		}
		if (Joker2XConfigUtils.TURN_ON_INAPP)
		{
			InappRequest.SendMessage(null);
			return;
		}
		if (Joker2XConfigUtils.TURN_ON_CARD || Joker2XConfigUtils.TURN_ON_SMS)
		{
			CardSMSRequest.SendMessage(null);
			return;
		}
	}

	public void OnClickRankingButton()
	{
		SFSObject sfsobject = new SFSObject();
		sfsobject.PutInt("type-id", 0);
		sfsobject.PutInt("p-page", 0);
		sfsobject.PutInt("p-length", 30);
		UserListRequest.SendMessage(sfsobject);
	}

	public void OnClickSoundButton()
	{
		Context.GameInfo.Volume = ((Context.GameInfo.Volume <= 0f) ? 1f : 0f);
		NGUITools.soundVolume = Context.GameInfo.Volume;
	}

	public void OnClickExitButton()
	{
		base.ExitGame();
	}

	public void OnClickLanguage()
	{
		if (Context.GameInfo.DeviceLanguageId == 0)
		{
			Context.GameInfo.DeviceLanguageId = 1;
			this.languageButton.transform.Find("BackgroundVn").gameObject.SetActive(true);
			this.languageButton.transform.Find("BackgroundEN").gameObject.SetActive(false);
		}
		else
		{
			Context.GameInfo.DeviceLanguageId = 0;
			this.languageButton.transform.Find("BackgroundEN").gameObject.SetActive(true);
			this.languageButton.transform.Find("BackgroundVn").gameObject.SetActive(false);
		}
		Localization.language = Context.GameInfo.DeviceLanguageName;
		LanguageRequest.SendMessage(Context.GameInfo.DeviceLanguageId);
		Context.Tooltip.AddMessage(Language.SETTING_UPDATE, 5, string.Empty, string.Empty);
		this.UpdateClientLanguage();
		this.HideSetting();
	}

	public void OnClickPlayNowButton()
	{
		JoinRoomRequest.SendMessage(-1, -1, 0, -1, false);
		UnityEngine.SceneManagement.SceneManager.LoadScene("BOMB_BATTLE");
	}

	public void OnClickInviteFBButton()
	{
		if (Context.GameInfo.LastAccounType == 2)
		{
			//FacebookAPI.Instance.InviteFriend(delegate(ActionFacebook action, ActionState state)
			//{
			//	switch (state)
			//	{
			//	case ActionState.Success:
			//		Context.Tooltip.AddMessage(Language.INVITE_FB_FRIENDS_SUCCESS, 5, string.Empty, string.Empty);
			//		break;
			//	case ActionState.Error:
			//		Context.Tooltip.AddMessage(Language.INVITE_FB_FRIENDS_ERROR, 5, string.Empty, string.Empty);
			//		break;
			//	case ActionState.PermisionNotPermitted:
			//		Context.Tooltip.AddMessage(Language.INVITE_FB_FRIENDS_NOTPERMITTED, 5, string.Empty, string.Empty);
			//		break;
			//	}
			//});
		}
		else
		{
			Context.OnDeletegateObject onYesClick = delegate(object obj)
			{
				Context.Waiting.ShowWaiting();
				//FacebookAPI.Instance.LoginFB(new FacebookDelegate<ILoginResult>(Context.currentMono.MergeFBCallBack), Context.currentMono.onLoginFBSuccess);
			};
			Context.OnDeletegateObject onDeletegateObject = delegate(object obj)
			{
			};
			Context.Confirm.AddMessageYesNo(Language.MERGE_FB_REQUEST, onYesClick, null, null, null, Language.MERGE_FB, null, false);
		}
	}

	public void UpdateClientLanguage()
	{
		this.UpdateGameName();
	}

	public void OnClickSetting()
	{
		if (this.soundButton.transform.localScale.x == 0f)
		{
			this.ShowSetting();
		}
		else
		{
			this.HideSetting();
		}
	}

	public void OnClickHideSetting()
	{
		this.HideSetting();
	}

	private void ShowSetting()
	{
		if (Context.GameInfo.DeviceLanguageId == 0)
		{
			this.languageButton.transform.Find("BackgroundEN").gameObject.SetActive(true);
			this.languageButton.transform.Find("BackgroundVn").gameObject.SetActive(false);
		}
		else
		{
			this.languageButton.transform.Find("BackgroundVn").gameObject.SetActive(true);
			this.languageButton.transform.Find("BackgroundEN").gameObject.SetActive(false);
		}
		DOTween.CompleteAll(false);
		DOTween.Sequence().Append(this.soundButton.transform.DOScale(Vector3.one, 0.2f)).OnComplete(delegate
		{
			DOTween.Sequence().Append(this.languageButton.transform.DOScale(Vector3.one, 0.2f));
		});
		base.Invoke("HideSetting", 5f);
	}

	private void HideSetting()
	{
		base.CancelInvoke("HideSetting");
		DOTween.CompleteAll(false);
		DOTween.Sequence().Append(this.languageButton.transform.DOScale(Vector3.zero, 0.2f)).OnComplete(delegate
		{
			DOTween.Sequence().Append(this.soundButton.transform.DOScale(Vector3.zero, 0.2f));
		});
	}

	public override void OnExtensionResponse(BaseEvent evt)
	{
		SFSObject sfsobject = (SFSObject)evt.Params["params"];
		string text = (string)evt.Params["cmd"];
		UnityEngine.Debug.Log("cmd: " + text + " OnExtensionResponse" + sfsobject.GetDump());
		string text2 = text;
		if (text2 != null)
		{
			if (MainMenuScene._003C_003Ef__switch_0024mapC == null)
			{
				MainMenuScene._003C_003Ef__switch_0024mapC = new Dictionary<string, int>(0);
			}
			int num;
			if (MainMenuScene._003C_003Ef__switch_0024mapC.TryGetValue(text2, out num))
			{
			}
		}
		base.OnExtensionResponse(evt);
	}

	public override void OnInvitation(BaseEvent evt)
	{
		Invitation invitation = (Invitation)evt.Params["invitation"];
		User inviter = invitation.Inviter;
		if (JokerUserUtils.GetGameName(inviter).ToLower() != Context.currentGameId.ToLower())
		{
			return;
		}
		ISFSObject @params = invitation.Params;
		int num = Convert.ToInt32(@params.GetUtfString("r-gname"));
		int chip = 1000;
		string str = string.Format(Language.INVITE_INVITE, JokerUserUtils.GetFormatDisplayName(inviter, 0), Joker2XUtils.FormatChip(chip));
		Context.Tooltip.AddMessageYes(str, 10, new Context.OnDeletegateObject(this.AcceptInvitation), @params, string.Empty);
	}

	private void AcceptInvitation(object obj)
	{
		int @int = ((ISFSObject)obj).GetInt("r-id");
		int groupIndex = Convert.ToInt32(((ISFSObject)obj).GetUtfString("r-gname"));
		UnityEngine.Debug.Log("vao phong id = " + @int);
		this.LoadGame(groupIndex, @int, 3);
	}

	public override void OnUserVariablesUpdate(BaseEvent evt)
	{
		SFSUser sfsuser = (SFSUser)evt.Params["user"];
		if (sfsuser.IsItMe)
		{
			this._avatarContainerScipt.UpdateUserInfo(evt);
		}
	}

	public void ChooseGame(string name, bool autoChoose)
	{
		Context.currentGameId = name;
		ChooseGameRequest.SendMessage(Context.currentGameId);
		if (Config.LIST_READY_GAMES.Contains(name))
		{
			this.scrollViewController.LoadListGroupButton(autoChoose);
			this.UpdateGameName();
		}
		else
		{
			Context.OnDeletegateObject onYesClick = delegate(object obj)
			{
				Application.OpenURL(Joker2XConfigUtils.UPDATE_LINK + "?deviceType=" + Context.GameInfo.DeviceType);
			};
			Context.Confirm.AddMessageYesNo(Language.NOTIFY_UPDATE_LINK, onYesClick, null, null, null, string.Empty, string.Empty, false);
		}
	}

	public void LoadGame(int groupIndex, int roomId, int joinRoomType)
	{
	}

	public override void OnJoinRoomError(BaseEvent evt)
	{
		throw new NotImplementedException();
	}

	public GameObject languageButton;

	public GameObject avatarContainer;

	private BaseAvatarContainer _avatarContainerScipt;

	public GameObject soundButton;

	public ScrollViewController scrollViewController;

	public GameObject versionLabel;

	public GameObject hotlineLabel;

	public GameObject gameLabel;

	public GameObject playNowButton;

	public GameObject fbInviteButton;

	public GameObject logoSprite;
}
