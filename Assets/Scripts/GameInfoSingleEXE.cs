// @sonhg: class: GameInfoSingleEXE
using System;

public class GameInfoSingleEXE : BaseGameInfo
{
	public override string Platform
	{
		get
		{
			return "EXE";
		}
	}

	public override int DeviceType
	{
		get
		{
			return 3;
		}
	}
}
