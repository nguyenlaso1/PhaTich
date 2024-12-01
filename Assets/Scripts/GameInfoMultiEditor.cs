// @sonhg: class: GameInfoMultiEditor
using System;

public class GameInfoMultiEditor : BaseGameInfo
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
