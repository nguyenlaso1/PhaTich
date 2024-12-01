// @sonhg: class: Bomb.MapSettingBox
using System;
using System.Collections.Generic;
using BombOffline;
//using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Bomb
{
	public class MapSettingBox : BaseSettingBox
	{
		protected override void OnStart()
		{
			base.OnStart();
			//if (FB.IsLoggedIn)
			//{
			//	this.fbLogin.gameObject.SetActive(false);
			//}
			//else
			{
				this.fbLogout.gameObject.SetActive(false);
			}
		}

		public void OnClickLogoutFB()
		{
			//FB.LogOut();
			this.OnClickCloseButton();
		}

		public void OnClickLoginFB()
		{
			Offline_Context.Waitting.ShowWaiting();
			//if (FB.IsLoggedIn)
			//{
			//	this.achivementBox.UpdateStatus();
			//}
			//else
			//{
			//	FB.LogInWithReadPermissions(new List<string>(), new FacebookDelegate<ILoginResult>(this.AuthCallback));
			//}
		}

		//private void AuthCallback(ILoginResult result)
		//{
		//	Offline_Context.Waitting.HideWaiting();
		//	if (result.Cancelled)
		//	{
		//		return;
		//	}
		//	if (result.Error == null)
		//	{
		//		Offline_Context.Confirm.AddMessageYes("Login Success!", null, null, string.Empty);
		//		if (DataManager.AchievementCount("LOGIN_FACEBOOK") == 0)
		//		{
		//			DataManager.AchievementCountPlus("LOGIN_FACEBOOK", 1);
		//			this.achivementBox.UpdateStatus();
		//		}
		//		this.OnDestroyBox();
		//	}
		//}

		[SerializeField]
		private Button fbLogin;

		[SerializeField]
		private Button fbLogout;

		public AchievementBox achivementBox;
	}
}
