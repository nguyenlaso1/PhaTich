// @sonhg: class: Offline_FreeCoinController
using System;
using BigFox;
//using Facebook.Unity;
using UnityEngine;

public class Offline_FreeCoinController : BaseBox
{
	protected override void Start()
	{
		if (PlayerPrefs.GetInt("FreeCoinRate", 0) == 1)
		{
			this.rateItem.SetActive(false);
		}
	}

	private void Update()
	{
	}

	public void InviteFacebook(string uri, string picture_uri)
	{
		if (DataManager.InviteCount < Offline_Config.INVITE_TIME)
		{
			this.shopController.confirmPopUp.AddMessageYesNo(string.Format("Invite friends\n get {0} golds free", Offline_Config.INVITE_COIN_BONUS), delegate(object e)
			{
				BigFoxFB.Instance.InviteFB(delegate
				{
					this.shopController.UpdateCoin();
				});
			}, null, null, null, string.Empty, string.Empty, false);
		}
		else
		{
			this.shopController.confirmPopUp.AddMessage("Invite Bonus Limited today!", string.Empty, string.Empty);
			Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_INVITE_FACEBOOK, Analystics.L_FAILD + " - Invite Limited!", (long)DataManager.InviteCount);
		}
	}

	//protected void HandleResult(IResult result)
	//{
	//	if (result == null)
	//	{
	//		return;
	//	}
	//	if (!string.IsNullOrEmpty(result.Error))
	//	{
	//		this.shopController.confirmPopUp.AddMessage("Invite unsuccess!", string.Empty, string.Empty);
	//		Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_INVITE_FACEBOOK, Analystics.L_ERROR + "-" + result.Error, 0L);
	//	}
	//	else if (result.Cancelled)
	//	{
	//		Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_INVITE_FACEBOOK, Analystics.L_CANCEL, 0L);
	//	}
	//	else if (!string.IsNullOrEmpty(result.RawResult))
	//	{
	//		if ((DateTime.Now - DataManager.LastInvite).Days > 0)
	//		{
	//			DataManager.InviteCount = 0;
	//		}
	//		Offline_ShopController.SetCharacterCoint(Offline_ShopController.GetCharacterCoin() + Offline_Config.INVITE_COIN_BONUS);
	//		DataManager.InviteCount++;
	//		Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_INVITE_FACEBOOK, Analystics.L_SUCCESS, (long)DataManager.InviteCount);
	//		this.shopController.UpdateCoin();
	//		this.shopController.confirmPopUp.AddMessage("Invite success receive " + Offline_Config.INVITE_COIN_BONUS + " gold", string.Empty, string.Empty);
	//		DataManager.LastInvite = DateTime.Now;
	//	}
	//}

	public void shareFacebook(string uri, string title, string description, string picture_uri)
	{
		if (DataManager.SharedCount < Offline_Config.SHARE_TIME)
		{
			this.shopController.confirmPopUp.AddMessageYesNo(string.Format("Share\n TO SUPPORT US\nGET {0} GOLD FREE", Offline_Config.SHARE_COIN_BONUS), delegate(object e)
			{
				//FB.ShareLink(new Uri(uri), title, description, new Uri(picture_uri), new FacebookDelegate<IShareResult>(this.HandleShareResult));
			}, null, null, null, string.Empty, string.Empty, false);
		}
		else
		{
			this.shopController.confirmPopUp.AddMessage("Share Bonus Limited today!", string.Empty, string.Empty);
			Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_SHARE_FACEBOOK, Analystics.L_FAILD + " - Share Limited!", (long)DataManager.SharedCount);
		}
	}

	//protected void HandleShareResult(IResult result)
	//{
	//	if (result == null)
	//	{
	//		return;
	//	}
	//	if (!string.IsNullOrEmpty(result.Error))
	//	{
	//		this.shopController.confirmPopUp.AddMessage("Share unsuccess!", string.Empty, string.Empty);
	//		Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_SHARE_FACEBOOK, Analystics.L_ERROR + "-" + result.Error, 0L);
	//	}
	//	else if (result.Cancelled)
	//	{
	//		Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_SHARE_FACEBOOK, Analystics.L_CANCEL, 0L);
	//	}
	//	else if (!string.IsNullOrEmpty(result.RawResult))
	//	{
	//		if ((DateTime.Now - DataManager.LastShare).Days > 0)
	//		{
	//			DataManager.SharedCount = 0;
	//		}
	//		Offline_ShopController.SetCharacterCoint(Offline_ShopController.GetCharacterCoin() + Offline_Config.SHARE_COIN_BONUS);
	//		DataManager.SharedCount++;
	//		Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_SHARE_FACEBOOK, Analystics.L_SUCCESS, (long)DataManager.SharedCount);
	//		this.shopController.UpdateCoin();
	//		this.shopController.confirmPopUp.AddMessage("Share success receive " + Offline_Config.SHARE_COIN_BONUS + " gold", string.Empty, string.Empty);
	//		DataManager.LastShare = DateTime.Now;
	//	}
	//}

	public void Rate(string uri)
	{
		this.shopController.confirmPopUp.AddMessageYesNo("Please Rate us\n Bonus " + Offline_Config.RATE_COIN_BONUS + "  gold FREE", delegate(object e)
		{
			Application.OpenURL(uri);
			Offline_ShopController.SetCharacterCoint(Offline_ShopController.GetCharacterCoin() + Offline_Config.RATE_COIN_BONUS);
			Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_RATE, Analystics.L_SUCCESS, 0L);
			this.shopController.UpdateCoin();
			PlayerPrefs.SetInt("FreeCoinRate", 1);
			this.rateItem.SetActive(false);
			DataManager.Rate(uri);
		}, null, null, null, string.Empty, string.Empty, false);
	}

	public void PlusGold(int gold)
	{
		Offline_ShopController.SetCharacterCoint(Offline_ShopController.GetCharacterCoin() + gold);
		this.shopController.UpdateCoin();
	}

	public Offline_ShopController shopController;

	public GameObject rateItem;
}
