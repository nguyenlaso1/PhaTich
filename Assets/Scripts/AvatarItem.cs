// @sonhg: class: AvatarItem
using System;
using Sfs2X.Entities;
using UnityEngine;

public class AvatarItem : MonoBehaviour
{
	private void addUserInfo(string linkAvatar, string name = "")
	{
		UnityEngine.Debug.Log(linkAvatar);
		Joker2XUtils.LoadImage(this.avatarTexture, linkAvatar, true);
		this.ShowChoose(JokerUserUtils.GetAvatar(SmartFoxConnection.Connection.MySelf) == this._gameItem.itemUrl);
	}

	public void ShowChoose(bool ishow)
	{
		this.chooseAvata.SetActive(ishow);
	}

	public void AddInfo(GameItem gameItem)
	{
		this._gameItem = gameItem;
		this.addUserInfo(gameItem.itemUrl, string.Empty);
	}

	public void OnClickAvatarButton()
	{
		User mySelf = SmartFoxConnection.Connection.MySelf;
		if (JokerUserUtils.GetChip(mySelf) < this._gameItem.itemPrice)
		{
			Context.Tooltip.AddMessage(Language.NOT_ENOUGH_MONEY, 5, string.Empty, string.Empty);
			return;
		}
		if (JokerUserUtils.GetAvatar(mySelf) == this._gameItem.itemUrl)
		{
			return;
		}
		BuyUserItemRequest.SendMessage(this._gameItem.itemId);
		StaticGameObject.GetGameObject("Prefabs/Bomber/Boxs/ProfileBox").GetComponent<ProfileBox>().UpdateAvatar(this._gameItem.itemUrl);
		this.ShowChoose(true);
	}

	public GameObject avatarTexture;

	public GameObject chooseAvata;

	public GameObject UserName;

	private GameItem _gameItem;
}
