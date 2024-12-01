// @sonhg: class: Config
using System;
using System.Collections.Generic;

public class Config
{
	public static string[] arrServerNames = new string[]
	{
		"pokerair.com",
		"pokerair.com.vn"
	};

	public static string serverName = Config.arrServerNames[0];

	public static int serverPort = 9933;

	public static string zoneName = "joker2x_bomb";

	public static int backupServerIndex = -1;

	public static bool multiExe = false;

	public static bool logToServer = false;

	public static List<string> LIST_READY_GAMES = new List<string>
	{
		"BOMB_BATTLE",
		"BOMB_BOSS"
	};

	public static List<string> LIST_DEV_GAMES = new List<string>
	{
		"BOMB_BATTLE",
		"BOMB_BOSS"
	};

	public static bool turnOnInapp = true;

	public static bool turnOnSms = true;

	public static bool turnOnCard = true;

	public static int versionCode = 40;

	public static string versionName = "Version:1.40";

	public static int settingVersionCode = 40;

	public static string settingVersionName = "1.40";

	public static string defaultGame = string.Empty;

	public static string package_name = "vn.com.bomberonline";

	public static string default_cp = "default_bigfox";

	public static string pushNotifiactionID = "8c54ed22-7eb8-11e5-8def-a0369f2d9328";

	public static string googleProjectID = "714383265073";

	public static string inappAndroidKeyHash = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyUZ0Au2V32hhP8ECUlVXX9KNkOYps9z/aB65+SDKuLEsCxfxweA4rbZP/gqxVAjSOkE0WqVHGknDxEVLeXcNRoefMRQ1aNPm3nl5Dxd0MVKHUTxHUU6vP5Ef//kEm5w7Mj9IPUmFPlsWpHbQcAFyfyEZyS4mUDLQDAg6Hq/8c14nEBm/9g0VJZ1WspMS1n8RvIss7lhEPJQJq5KCmEa2/ofJioqsq/QhtItLVPdC9UCmRQwbMTfAlvQbb+NvFYotGKOvx/9uiGMSC3VoPFJ8x8eAVH6urXh6n6Gjc+I2f1KOxaPaL3YeziA///PH0XMsWuAuudToF2HyWFiAshgeEQIDAQAB";

	public static string settingKeyStore = "bomber_keystore";

	public static string settingLogo = "LOGO_BOMB";

	public static string settingProductName = "Bomber";

	public static string settingAliasName = "vn.com.bomber";

	public static string FB_APPLINK_URI = "https://fb.me/2163741863766522";

	public static string FB_APPLINK_PICTURE_URI = "http://pokerair.com/joker2x_bomb/share_facebook.png";
}
