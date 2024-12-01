// @sonhg: class: ProfileBox
using System;
using System.Collections.Generic;
//using Facebook.Unity;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine;

public class ProfileBox : BaseBox
{
	protected override void Start()
	{
		base.Start();
		this.ShowInfo();
	}

	protected override void OnStart()
	{
		base.OnStart();
		if (Context.arrUserItemList != null)
		{
			this.ShowListAvatar(Context.userItemList.GetListItem(2));
		}
		else
		{
			GetUserItemRequest.SendMessage(-1);
		}
	}

	private void ShowInfo()
	{
		User mySelf = SmartFoxConnection.Connection.MySelf;
		int num = JokerUserUtils.GetWin(mySelf) + JokerUserUtils.GetLose(mySelf);
		if (num <= 0)
		{
			num = 1;
		}
		this._dbUserIdLabel.GetComponent<UILabel>().text = "UID: " + JokerUserUtils.GetUserId(mySelf).ToString();
		this._lvLabel.GetComponent<UILabel>().text = JokerUserUtils.GetLevel(mySelf).ToString();
		this._chipLabel.GetComponent<UILabel>().text = JokerUserUtils.GetFormatChip(mySelf);
		this._nameInput.GetComponent<UIInput>().defaultText = JokerUserUtils.GetFormatDisplayName(mySelf, 15);
		this._avatarContainer.GetComponent<BaseAvatarContainer>().SetButtonParams(mySelf, null, null);
		this._winRatioLabel.GetComponent<UILabel>().text = Language.PROFILE_WIN_RATING + JokerUserUtils.GetWin(mySelf) * 100 / num + "%";
		if (Context.GameInfo.LastAccounType == 2)
		{
			this._faceBookLabel.GetComponent<UILabel>().text = Language.PROFILE_FB_LOGOUT;
		}
	}

	public void UpdateAvatar(string avatar)
	{
		Joker2XUtils.LoadImage(this._avatarContainer.GetComponent<BaseAvatarContainer>().avatarTexture, avatar, true);
		for (int i = 0; i < this._scrollViewCommon.listItem.Count; i++)
		{
			this._scrollViewCommon.listItem[i].go.GetComponent<AvatarItem>().ShowChoose(false);
		}
	}

	public void ShowListAvatar(List<GameItem> listGameItem)
	{
		this._scrollViewCommon = this._scrollView.GetComponent<ScrollViewCommon>();
		this._scrollViewCommon.RemoveallChild();
		for (int i = 0; i < listGameItem.Count; i++)
		{
			GameObject gameObject = NGUITools.AddChild(this._scrollView, StaticPrefab.GetPrefab("Prefabs/Joker2x/GameItem/AvatarItem"));
			gameObject.transform.localPosition = new Vector3((float)(i * 125), 0f, 0f);
			gameObject.GetComponent<AvatarItem>().AddInfo(listGameItem[i]);
			this._scrollViewCommon.AddGameObject(gameObject, i.ToString(), 120f, 125f, false);
		}
		this._scrollViewCommon.UpdateScroll();
		this._scrollViewCommon.Reset();
	}

	private void Update()
	{
	}

	public void OnKeyPress()
	{
		string text = StringUtils.RemoveEnter(this._nameInput.GetComponent<UIInput>().value);
		this._avatarContainer.GetComponent<BaseAvatarContainer>().UserName.GetComponent<UILabel>().text = text;
		this._isChange = true;
	}

	public void OnSubmit()
	{
		string val = StringUtils.RemoveEnter(this._nameInput.GetComponent<UIInput>().value);
		SFSObject sfsobject = new SFSObject();
		sfsobject.PutUtfString("displayname", val);
		sfsobject.PutInt("sex", JokerUserUtils.GetSex(SmartFoxConnection.Connection.MySelf));
		UpdateUserInfoRequest.SendMessage(sfsobject);
		this.CloseBox();
	}

	public void OnClickCloseButtonNew()
	{
		if (this._isChange)
		{
			this.OnSubmit();
		}
		this.CloseBox();
	}

	public void OnClickFacebookButton()
	{
		if (Context.GameInfo.LastAccounType == 0)
		{
			Context.Waiting.ShowWaiting();
			//FacebookAPI.Instance.LoginFB(new FacebookDelegate<ILoginResult>(Context.currentMono.MergeFBCallBack), Context.currentMono.onLoginFBSuccess);
		}
		else
		{
			Context.currentMono.Logout(JokerEnum.ClientGameState.GS_LOGOUT_NORMAL);
		}
	}

	[SerializeField]
	private GameObject _winRatioLabel;

	[SerializeField]
	private GameObject _dbUserIdLabel;

	[SerializeField]
	private GameObject _lvLabel;

	[SerializeField]
	private GameObject _faceBookLabel;

	[SerializeField]
	private GameObject _chipLabel;

	[SerializeField]
	private GameObject _nameInput;

	[SerializeField]
	private GameObject _avatarContainer;

	[SerializeField]
	private GameObject _scrollView;

	private ScrollViewCommon _scrollViewCommon;

	private bool _isChange;
}
