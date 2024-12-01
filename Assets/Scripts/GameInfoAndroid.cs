// @sonhg: class: GameInfoAndroid
using System;
using OnePF;
using UnityEngine;

public class GameInfoAndroid : BaseGameInfo
{
	public override string Platform
	{
		get
		{
			return "Android";
		}
	}

	public override string CP
	{
		get
		{
			string text = this.GetCPNameFromReferer();
			if (text == null || text.Equals(string.Empty))
			{
				text = this.GetCPFromRawFile();
			}
			if (text == null || text.Equals(string.Empty))
			{
				return Config.default_cp;
			}
			return text;
		}
	}

	public override string DeviceName
	{
		get
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("vn.com.joker.smsplugin.Poker");
			string text = androidJavaClass.CallStatic<string>("getAccountName", new object[]
			{
				this.CurrentActivity()
			});
			if (text != null && text.Contains("@"))
			{
				return text.Substring(0, text.IndexOf("@"));
			}
			return text;
		}
	}

	public override int DeviceType
	{
		get
		{
			return 0;
		}
	}

	public override int LanguageId
	{
		get
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("vn.com.joker.smsplugin.Poker");
			string text = androidJavaClass.CallStatic<string>("getLanguage", new object[]
			{
				this.CurrentActivity()
			});
			if (text != null)
			{
				if (text.StartsWith("vi"))
				{
					return 1;
				}
				if (text.StartsWith("en"))
				{
					return 0;
				}
			}
			return 0;
		}
	}

	public AndroidJavaObject CurrentActivity()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		return androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
	}

	private string GetCPNameFromReferer()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("vn.com.joker.smsplugin.Poker");
		return androidJavaClass.CallStatic<string>("getCPNameFromReferer", new object[]
		{
			this.CurrentActivity()
		});
	}

	private string GetCPFromRawFile()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("vn.com.joker.smsplugin.Poker");
		return androidJavaClass.CallStatic<string>("getCPFromRawFile", new object[]
		{
			this.CurrentActivity(),
			"raw/cp"
		});
	}

	public override void Vibrate()
	{
		Handheld.Vibrate();
	}

	public override void SendSMS(string phoneNo, string text)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("vn.com.joker.smsplugin.SMS");
		androidJavaClass.CallStatic("composeMessage", new object[]
		{
			this.CurrentActivity(),
			phoneNo,
			text
		});
	}

	public override void Toast(string text)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("vn.com.joker.smsplugin.SMS");
		androidJavaClass.CallStatic("toast", new object[]
		{
			this.CurrentActivity(),
			text
		});
	}

	public override string StoreName
	{
		get
		{
			return OpenIAB_Android.STORE_GOOGLE;
		}
	}

	public override void InitAppsFlyer()
	{
		AppsFlyer.setAppsFlyerKey("Cu8qkEJaJAZ9LENAu6Jumn");
		AppsFlyer.setAppID(Config.package_name);
		AppsFlyer.setCollectIMEI(false);
		AppsFlyer.setCollectAndroidID(false);
		AppsFlyer.trackAppLaunch();
	}
}
