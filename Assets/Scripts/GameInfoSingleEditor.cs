// @sonhg: class: GameInfoSingleEditor
using System;

public class GameInfoSingleEditor : BaseGameInfo
{
	public override string Platform
	{
		get
		{
			return "Editor";
		}
	}

	public override int DeviceType
	{
		get
		{
			return 5;
		}
	}
}
