// @sonhg: class: GameInfoMultiEXE
using System;

public class GameInfoMultiEXE : BaseGameInfo
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

	public override string DeviceId
	{
		get
		{
			return DateTime.UtcNow.Ticks + string.Empty;
		}
	}

	public override string DisplayName
	{
		get
		{
			return NameGenerator.GenRandomName();
		}
	}

	public override string UserName
	{
		get
		{
			return DateTime.UtcNow.Ticks + string.Empty;
		}
	}
}
