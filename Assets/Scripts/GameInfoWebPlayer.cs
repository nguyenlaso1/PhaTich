// @sonhg: class: GameInfoWebPlayer
using System;

public class GameInfoWebPlayer : BaseGameInfo
{
	public override string Platform
	{
		get
		{
			return "WebPlayer";
		}
	}

	public override int DeviceType
	{
		get
		{
			return 2;
		}
	}
}
