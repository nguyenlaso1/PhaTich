// @sonhg: class: GameInfoIOS
using System;
using System.Runtime.InteropServices;
using OnePF;

public class GameInfoIOS : BaseGameInfo
{
	[DllImport("__Internal")]
	private static extern void _SendMsg(string phoneNo, string sms);

	[DllImport("__Internal")]
	private static extern string _GetVersionName();

	[DllImport("__Internal")]
	private static extern int _GetLanguage();

	[DllImport("__Internal")]
	private static extern string _GetDeviceId();

	[DllImport("__Internal")]
	private static extern string _GetAccountName();

	public override string Platform
	{
		get
		{
			return "IOS";
		}
	}

	public override string DeviceName
	{
		get
		{
			return NameGenerator.GenRandomName();
		}
	}

	public override int DeviceType
	{
		get
		{
			return 1;
		}
	}

	public override int LanguageId
	{
		get
		{
			return GameInfoIOS._GetLanguage();
		}
	}

	public override void SendSMS(string phoneNo, string text)
	{
		GameInfoIOS._SendMsg(phoneNo, text);
	}

	public override string GetSKU(string sku)
	{
		return Config.package_name + "." + sku;
	}

	public override string StoreName
	{
		get
		{
			return OpenIAB_iOS.STORE;
		}
	}
}
