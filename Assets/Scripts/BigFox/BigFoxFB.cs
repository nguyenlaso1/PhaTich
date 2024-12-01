// @sonhg: class: BigFox.BigFoxFB
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BombOffline;
//using Facebook.MiniJSON;
//using Facebook.Unity;
using UnityEngine;

namespace BigFox
{
	public class BigFoxFB
	{
		public static BigFoxFB Instance
		{
			get
			{
				if (BigFoxFB._instance == null)
				{
					BigFoxFB._instance = new BigFoxFB();
				}
				return BigFoxFB._instance;
			}
		}

		public void Login()
		{
			//if (!FB.IsLoggedIn)
			//{
			//	FB.LogInWithReadPermissions(new List<string>
			//	{
			//		"public_profile,user_friends"
			//	}, new FacebookDelegate<ILoginResult>(this.LoginCallback));
			//}
		}

		public void LoginForPublishFeed()
		{
			//if (!FB.IsLoggedIn)
			//{
			//	FB.LogInWithPublishPermissions(new List<string>
			//	{
			//		"public_profile,user_friends"
			//	}, new FacebookDelegate<ILoginResult>(this.LoginFeedCallback));
			//}
			//else
			//{
			//	FB.API("/me/permissions", HttpMethod.GET, delegate(IGraphResult response)
			//	{
			//		if (response.RawResult.Contains("publish_actions"))
			//		{
			//			this.hasPublishFeedRole = true;
			//			this.OnLoggedIn();
			//		}
			//		else
			//		{
			//			FB.LogInWithPublishPermissions(new List<string>
			//			{
			//				"publish_actions"
			//			}, new FacebookDelegate<ILoginResult>(this.LoginFeedCallback));
			//		}
			//	}, null);
			//}
		}

		//private void LoginFeedCallback(ILoginResult result)
		//{
		//	FB.API("/me/permissions", HttpMethod.GET, delegate(IGraphResult response)
		//	{
		//		if (response.RawResult.Contains("publish_actions"))
		//		{
		//			this.hasPublishFeedRole = true;
		//			this.OnLoggedIn();
		//		}
		//	}, null);
		//}

		//private void LoginCallback(ILoginResult result)
		//{
		//	if (FB.IsLoggedIn)
		//	{
		//		this.OnLoggedIn();
		//	}
		//}

		private void OnLoggedIn()
		{
			switch (this.taskAfterLogin)
			{
			case 0:
				this.ShareScreenShot(this.content, this.screenShot);
				break;
			case 1:
				this.InviteFB(null);
				break;
			case 2:
				this.PostFeed(this.message, this.name, this.description, this.link, this.picture);
				break;
			case 3:
				this.InviteFB2();
				break;
			}
		}

		public void InviteFB(Context.OnDeletegateNone callback)
		{
			this.callback = callback;
			//if (!FB.IsLoggedIn)
			//{
			//	this.taskAfterLogin = 1;
			//	this.Login();
			//}
			//else
			//{
			//	FB.LogInWithPublishPermissions(new List<string>
			//	{
			//		"publish_actions"
			//	}, delegate(ILoginResult result)
			//	{
			//		if (this.friends == null)
			//		{
			//			this.friends = new ArrayList();
			//			FB.API("/me/invitable_friends?limit=1000", HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.GetListFriendCallBack), null);
			//		}
			//		else
			//		{
			//			this.CallAppRequest();
			//		}
			//	});
			//}
		}

		public void InviteFB2()
		{
			//if (!FB.IsLoggedIn)
			//{
			//	this.taskAfterLogin = 3;
			//	this.Login();
			//}
			//else if (this.friends == null)
			//{
			//	this.friends = new ArrayList();
			//	//FB.API("/me/invitable_friends?limit=1000", HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.GetListFriendCallBack2), null);
			//}
			//else
			//{
			//	this.CallAppRequest2();
			//}
		}

		//public void GetListFriendCallBack2(IGraphResult result)
		//{
		//	if (!string.IsNullOrEmpty(result.Error))
		//	{
		//		Offline_Context.Confirm.AddMessage("Invite FB friends ERROR", string.Empty, string.Empty);
		//	}
		//	else
		//	{
		//		Dictionary<string, object> dictionary = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
		//		List<object> list = (from d in (List<object>)dictionary["data"]
		//		orderby Guid.NewGuid()
		//		select d).ToList<object>();
		//		for (int i = 0; i < list.Count; i++)
		//		{
		//			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list[i];
		//			string value = (string)dictionary2["id"];
		//			string text = (string)dictionary2["name"];
		//			Dictionary<string, object> dictionary3 = (Dictionary<string, object>)dictionary2["picture"];
		//			Dictionary<string, object> dictionary4 = (Dictionary<string, object>)dictionary3["data"];
		//			string text2 = (string)dictionary4["url"];
		//			this.friends.Add(value);
		//		}
		//		for (int j = 0; j < this.friends.Count; j++)
		//		{
		//			string value2 = (string)this.friends[j];
		//			int index = UnityEngine.Random.Range(j, this.friends.Count);
		//			this.friends[j] = this.friends[index];
		//			this.friends[index] = value2;
		//		}
		//		this.CallAppRequest2();
		//	}
		//}

		//public void GetListFriendCallBack(IGraphResult result)
		//{
		//	if (string.IsNullOrEmpty(result.Error))
		//	{
		//		UnityEngine.Debug.Log(result.RawResult);
		//		Dictionary<string, object> dictionary = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
		//		List<object> list = (from d in (List<object>)dictionary["data"]
		//		orderby Guid.NewGuid()
		//		select d).ToList<object>();
		//		for (int i = 0; i < list.Count; i++)
		//		{
		//			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list[i];
		//			string value = (string)dictionary2["id"];
		//			string text = (string)dictionary2["name"];
		//			Dictionary<string, object> dictionary3 = (Dictionary<string, object>)dictionary2["picture"];
		//			Dictionary<string, object> dictionary4 = (Dictionary<string, object>)dictionary3["data"];
		//			string text2 = (string)dictionary4["url"];
		//			this.friends.Add(value);
		//			UnityEngine.Debug.LogError(value);
		//		}
		//		for (int j = 0; j < this.friends.Count; j++)
		//		{
		//			string value2 = (string)this.friends[j];
		//			int index = UnityEngine.Random.Range(j, this.friends.Count);
		//			this.friends[j] = this.friends[index];
		//			this.friends[index] = value2;
		//		}
		//		this.CallAppRequest();
		//	}
		//}

		private void CallAppRequest2()
		{
			int num = Mathf.Min(this.friends.Count, 50);
			List<string> list = new List<string>();
			for (int i = 0; i < num; i++)
			{
				list.Add((string)this.friends[(i + this.offset) % this.friends.Count]);
			}
			this.offset += num;
			//FB.AppRequest("Invite FB friends", list, null, null, new int?(20), string.Empty, "1", new FacebookDelegate<IAppRequestResult>(this.CallbackAppRequest2));
		}

		private void CallAppRequest()
		{
			int num = Mathf.Min(this.friends.Count, 50);
			string[] array = new string[num];
			string str = string.Empty;
			for (int i = 0; i < num; i++)
			{
				array[i] = (string)this.friends[(i + this.offset) % this.friends.Count];
				str += (string)this.friends[(i + this.offset) % this.friends.Count];
			}
			this.offset += num;
			//FB.AppRequest("Invite FB friends", array, null, null, new int?(20), string.Empty, "Free Coins", new FacebookDelegate<IAppRequestResult>(this.CallbackAppRequest));
		}

		//private void CallbackAppRequest(IAppRequestResult result)
		//{
		//	if (result.RawResult.Contains("cancelled"))
		//	{
		//		UnityEngine.Debug.Log("invite cancel");
		//	}
		//	else
		//	{
		//		if (result == null)
		//		{
		//			return;
		//		}
		//		if (!string.IsNullOrEmpty(result.Error))
		//		{
		//			Offline_Context.Confirm.AddMessage("Invite unsuccess!", string.Empty, string.Empty);
		//			Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_INVITE_FACEBOOK, Analystics.L_ERROR + "-" + result.Error, 0L);
		//		}
		//		else if (result.Cancelled)
		//		{
		//			Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_INVITE_FACEBOOK, Analystics.L_CANCEL, 0L);
		//		}
		//		else if (!string.IsNullOrEmpty(result.RawResult))
		//		{
		//			if ((DateTime.Now - DataManager.LastInvite).Days > 0)
		//			{
		//				DataManager.InviteCount = 0;
		//			}
		//			Offline_ShopController.SetCharacterCoint(Offline_ShopController.GetCharacterCoin() + Offline_Config.INVITE_COIN_BONUS);
		//			DataManager.InviteCount++;
		//			Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_INVITE_FACEBOOK, Analystics.L_SUCCESS, (long)DataManager.InviteCount);
		//			Offline_Context.Confirm.AddMessage("Invite success receive " + Offline_Config.INVITE_COIN_BONUS + " gold", string.Empty, string.Empty);
		//			DataManager.LastInvite = DateTime.Now;
		//			if (this.callback != null)
		//			{
		//				this.callback();
		//			}
		//		}
		//	}
		//}

		//private void CallbackAppRequest2(IAppRequestResult result)
		//{
		//}

		public void ShareScreenShot(string content, Texture2D screenShot)
		{
			if (!this.hasPublishFeedRole)
			{
				this.content = content;
				this.screenShot = screenShot;
				this.taskAfterLogin = 0;
				this.LoginForPublishFeed();
			}
			else
			{
				WWWForm wwwform = new WWWForm();
				wwwform.AddBinaryData("image", screenShot.EncodeToPNG());
				wwwform.AddField("message", content);
				//FB.API("/me/photos", HttpMethod.POST, new FacebookDelegate<IGraphResult>(this.PostPhotoCallBack), wwwform);
			}
		}

		//private void PostPhotoCallBack(IGraphResult result)
		//{
		//}

		public void PostFeed(string message, string name, string description, string link, string picture)
		{
			if (!this.hasPublishFeedRole)
			{
				this.message = message;
				this.name = name;
				this.description = description;
				this.link = link;
				this.picture = picture;
				this.taskAfterLogin = 2;
				this.LoginForPublishFeed();
			}
			else
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("message", message);
				dictionary.Add("name", name);
				dictionary.Add("description", description);
				dictionary.Add("link", link);
				dictionary.Add("picture", picture);
				//FB.API("/me/feed", HttpMethod.POST, new FacebookDelegate<IGraphResult>(this.PostFeedCallback), dictionary);
			}
		}

		//private void PostFeedCallback(IGraphResult result)
		//{
		//}

		public const int TASK_POST_SCREEN_SHOT = 0;

		public const int TASK_INVITE_FRIEND = 1;

		public const int TASK_POST_FEED = 2;

		public const int TASK_INVITE_FRIEND_2 = 3;

		public const int TYPE_FACEBOOK_ACTION_POST_SCREEN_SHOT = 0;

		public const int TYPE_FACEBOOK_ACTION_POST_FEED = 1;

		public const int TYPE_FACEBOOK_ACTION_INVITE_FRIEND = 2;

		public const int TYPE_FACEBOOK_ACTION_INVITE_FRIEND_2 = 3;

		private static BigFoxFB _instance;

		private int taskAfterLogin = -1;

		private ArrayList friends;

		private int offset;

		private Context.OnDeletegateNone callback;

		private string content = string.Empty;

		private Texture2D screenShot;

		private string message = string.Empty;

		private string name = string.Empty;

		private string description = string.Empty;

		private string link = string.Empty;

		private string picture = string.Empty;

		private bool hasPublishFeedRole;
	}
}
