// @sonhg: class: GameInfoWP
using System;
using OnePF;

public class GameInfoWP : BaseGameInfo
{
	public override string Platform
	{
		get
		{
			return "WP";
		}
	}

	public override int DeviceType
	{
		get
		{
			return 4;
		}
	}

	public override string StoreName
	{
		get
		{
			return OpenIAB_WP8.STORE;
		}
	}

	public override string GetSKU(string sku)
	{
		return sku;
	}

	public override void SendSMS(string phoneNo, string text)
	{
	}
}
