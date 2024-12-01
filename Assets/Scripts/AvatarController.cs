// @sonhg: class: AvatarController
using System;
using Bomb;
using DG.Tweening;
using Sfs2X.Entities;
using UnityEngine;
using UnityEngine.UI;

public class AvatarController : MonoBehaviour
{
	public void SetUser(User user)
	{
		this._user = user;
		this.SetInfo(JokerUserUtils.GetFormatDisplayName(user, 10), JokerUserUtils.GetCountry(user), user);
		if (this.readyImage.Length > 0)
		{
			if (RoomUtils.GetRoomOwnerId() != user.Id)
			{
				this.SetReady(MMOUserUtils.IsReady(user));
				this.readyImage[2].gameObject.SetActive(false);
			}
			else
			{
				this.readyImage[2].gameObject.SetActive(true);
				this.readyImage[0].gameObject.SetActive(false);
				this.readyImage[1].gameObject.SetActive(false);
			}
		}
		this.SetSkin();
	}

	public User GetUser()
	{
		return this._user;
	}

	public int GetUserId()
	{
		if (this._user != null)
		{
			return this._user.Id;
		}
		return -1;
	}

	public void ShowLevelUp()
	{
		UnityEngine.Debug.LogError("ShowLevelUp");
		this.levelUpImage.gameObject.SetActive(true);
	}

	public void HideLevelUp()
	{
		this.levelUpImage.gameObject.SetActive(false);
	}

	public void ShowChatMessage(string content)
	{
		this.chatText.text = content;
		this.chatPopUp.SetActive(true);
		this.chatPopUp.transform.localScale = Vector3.zero;
		this.chatPopUp.transform.DOKill(false);
		this.chatPopUp.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce).OnComplete(delegate
		{
			this.chatPopUp.transform.DOScale(Vector3.zero, 0.3f).SetDelay(5f);
		});
	}

	public void GraySubAvatar()
	{
		this.subAvatar.GrayCanvas();
	}

	public void ResetSubAvatar()
	{
		this.subAvatar.ResetCanvas();
		this.subAvatar.gameObject.SetActive(true);
	}

	private void SetInfo(string name, string country, User user)
	{
		if (base.gameObject.GetComponent<Image>() != null)
		{
			base.gameObject.GetComponent<Image>().enabled = false;
		}
		if (base.transform.Find("CharacterAvarta") != null)
		{
			base.transform.Find("CharacterAvarta").GetComponent<Image>().enabled = true;
		}
		if (this.inviteButton != null && this.skin != null && this.nameLabel != null && this.iconCountry != null)
		{
			this.inviteButton.SetActive(false);
			this.skin.SetActive(true);
			this.nameLabel.gameObject.SetActive(true);
			this.nameLabel.text = name;
			this.subAvatar.gameObject.SetActive(false);
			this.subAvatar.ingameName.text = name;
			this.subAvatar.LoadAvatar(user);
			this.iconCountry.sprite = TextureCutter.TextureToSprite(Resources.Load("Textures/Flags/" + country) as Texture2D);
			this.subAvatar.ingameFlag.sprite = this.iconCountry.sprite;
		}
	}

	private void SetSkin()
	{
		TextureCutter.CutHead(TextureCutter.GetSkinTexture(MMOUserUtils.GetHead(this._user)), this.skin.transform);
		TextureCutter.CutBody(TextureCutter.GetSkinTexture(MMOUserUtils.GetBody(this._user)), this.skin.transform);
		TextureCutter.CutHair(TextureCutter.GetSkinTexture(MMOUserUtils.GetHair(this._user)), this.skin.transform, TextureCutter.Parse(MMOUserUtils.GetHairColor(this._user)));
	}

	public void SetHairColor()
	{
		if (this.skin.transform.Find("Head").GetComponent<SpriteRenderer>() != null)
		{
			this.skin.transform.Find("Head").Find("Hair").GetComponent<SpriteRenderer>().color = TextureCutter.Parse(MMOUserUtils.GetHairColor(this._user));
		}
		else
		{
			this.skin.transform.Find("Head").Find("Hair").GetComponent<Image>().color = TextureCutter.Parse(MMOUserUtils.GetHairColor(this._user));
		}
		if (this.skin.transform.Find("Head_Up") != null)
		{
			this.skin.transform.Find("Head_Up").Find("Hair").GetComponent<SpriteRenderer>().color = TextureCutter.Parse(MMOUserUtils.GetHairColor(this._user));
		}
		if (this.skin.transform.Find("Head_Left") != null)
		{
			this.skin.transform.Find("Head_Left").Find("Hair").GetComponent<SpriteRenderer>().color = TextureCutter.Parse(MMOUserUtils.GetHairColor(this._user));
		}
		if (this.skin.transform.Find("Head_Right") != null)
		{
			this.skin.transform.Find("Head_Right").Find("Hair").GetComponent<SpriteRenderer>().color = TextureCutter.Parse(MMOUserUtils.GetHairColor(this._user));
		}
	}

	public void SetHead()
	{
		TextureCutter.CutHead(TextureCutter.GetSkinTexture(MMOUserUtils.GetHead(this._user)), this.skin.transform);
	}

	public void SetBody(int _bodyId)
	{
		TextureCutter.CutBody(TextureCutter.GetSkinTexture(_bodyId), this.skin.transform);
	}

	public void SetHairById(int _id)
	{
		TextureCutter.CutHair(TextureCutter.GetSkinTexture(_id), this.skin.transform, Color.white);
	}

	public void SetHeadById(int _id)
	{
		TextureCutter.CutHead(TextureCutter.GetSkinTexture(_id), this.skin.transform);
	}

	public void SetHair()
	{
		TextureCutter.CutHair(TextureCutter.GetSkinTexture(MMOUserUtils.GetHair(this._user)), this.skin.transform, TextureCutter.Parse(MMOUserUtils.GetHairColor(this._user)));
	}

	public void ResetInfo()
	{
		base.gameObject.GetComponent<Image>().enabled = true;
		base.transform.Find("CharacterAvarta").GetComponent<Image>().enabled = false;
		this.kickButton.SetActive(false);
		this.skin.SetActive(false);
		this.inviteButton.SetActive(true);
		this.nameLabel.gameObject.SetActive(false);
		this.nameLabel.text = string.Empty;
		this.HideLevelUp();
		this.ResetSubAvatar();
		this.subAvatar.ingameName.text = string.Empty;
		this.subAvatar.gameObject.SetActive(false);
		foreach (Image image in this.readyImage)
		{
			image.gameObject.SetActive(false);
		}
		this._user = null;
	}

	public void SetReady(bool _isReady)
	{
		if (RoomUtils.GetRoomOwnerId() != this._user.Id)
		{
			if (_isReady)
			{
				this.readyImage[0].gameObject.SetActive(true);
				this.readyImage[1].gameObject.SetActive(false);
			}
			else
			{
				this.readyImage[0].gameObject.SetActive(false);
				this.readyImage[1].gameObject.SetActive(true);
			}
			if (RoomUtils.GetRoomOwnerId() == SmartFoxConnection.Connection.MySelf.Id)
			{
				this.kickButton.SetActive(true);
			}
			else
			{
				this.kickButton.SetActive(false);
			}
		}
	}

	public void ResetReady()
	{
		this.SetReady(MMOUserUtils.IsReady(this._user));
	}

	public void SetKickButton(bool active)
	{
		this.kickButton.SetActive(active);
	}

	public void OnKickButtonClick()
	{
		Context.Confirm.AddMessageYesNo("Do you want to kick this player ?", delegate(object x)
		{
			KickRequest.SendMessage(this._user.Id);
		}, null, null, null, string.Empty, string.Empty, false);
	}

	public void SetTeam(User user)
	{
		int team = BombUserUtils.GetTeam(user);
		if (team == 0)
		{
			this.backgroundPanel.color = Color.red;
		}
		else if (team == 1)
		{
			this.backgroundPanel.color = Color.green;
		}
		else
		{
			this.backgroundPanel.color = Color.white;
		}
	}

	public void ChoosePosition()
	{
		ChoosePositionRequest.SendMessage(this.position);
	}

	public void OnClickInvite()
	{
		OnlineAdmob.HideBannerAds();
		InviteListRequest.SendMessage();
		Context.position = this.position;
	}

	public Text nameLabel;

	public Image iconCountry;

	public int position;

	public GameObject inviteButton;

	public GameObject skin;

	public Image[] readyImage;

	public GameObject chatPopUp;

	public GameObject kickButton;

	public Text chatText;

	public Image backgroundPanel;

	[SerializeField]
	protected Image levelUpImage;

	private User _user;

	[SerializeField]
	private SubAvatarController subAvatar;
}
