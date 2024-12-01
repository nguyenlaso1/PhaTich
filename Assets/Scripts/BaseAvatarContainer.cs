// @sonhg: class: BaseAvatarContainer
using System;
using System.Collections;
using Sfs2X.Core;
using Sfs2X.Entities;
using UnityEngine;

public class BaseAvatarContainer : MonoBehaviour
{
	public bool IsEmpty()
	{
		return this._user == null;
	}

	public virtual void ShowAvatarButton(bool isShow)
	{
		this.avatarButton.SetActive(isShow);
	}

	public void SetButtonParams(User user, Context.OnDeletegateObject onAvatarClick = null, object avatarParam = null)
	{
		base.gameObject.SetActive(true);
		this._onAvatarClick = onAvatarClick;
		this._avatarParam = avatarParam;
		this.UpdateUserInfo(user);
	}

	public void LoadAvatar()
	{
		Joker2XUtils.LoadImage(this.avatarTexture, JokerUserUtils.GetAvatar(this._user), false);
	}

	public virtual void UpdateUserChip()
	{
		this.UserChip.GetComponent<UILabel>().text = JokerUserUtils.GetFormatChip(this._user);
	}

	public virtual void UpdateUserName()
	{
		this.UserName.GetComponent<UILabel>().text = JokerUserUtils.GetFormatDisplayName(this._user, 0);
	}

	public virtual void UpdateUserInfo(User user)
	{
		if (user != null)
		{
			this._user = user;
			this.ShowAvatarButton(true);
			this.LoadAvatar();
			this.UpdateUserName();
			this.UpdateUserChip();
		}
	}

	public void UpdateUserInfo()
	{
		if (this._user != null)
		{
			this.UpdateUserInfo(this._user);
		}
	}

	public virtual void UpdateUserInfo(BaseEvent variables)
	{
		if (this._user != null)
		{
			ArrayList arrayList = (ArrayList)variables.Params["changedVars"];
			if (arrayList.Contains("avatar"))
			{
				this.LoadAvatar();
			}
			if (arrayList.Contains("displayname"))
			{
				this.UpdateUserName();
			}
			if (arrayList.Contains("chip"))
			{
				this.UpdateUserChip();
			}
		}
	}

	public void OnClickAvatarButton()
	{
		if (this._onAvatarClick != null)
		{
			this._onAvatarClick(this._avatarParam);
		}
	}

	public User _user;

	private Context.OnDeletegateObject _onAvatarClick;

	private object _avatarParam;

	public GameObject avatarTexture;

	public GameObject UserName;

	public GameObject UserChip;

	public GameObject avatarButton;

	private bool _onGameRoom = true;
}
