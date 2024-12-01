// @sonhg: class: MainMenuSettingBox
using System;
//using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingBox : IngameSettingBox
{
	protected override void OnStart()
	{
		base.OnStart();
		if (Context.GameInfo.LastAccounType == 0)
		{
			this.facebookButton.sprite = this.facebookSprite[0];
		}
		else
		{
			this.facebookButton.sprite = this.facebookSprite[1];
		}
	}

	public void OnClickFacebook()
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
	private Sprite[] facebookSprite;

	[SerializeField]
	private Image facebookButton;
}
