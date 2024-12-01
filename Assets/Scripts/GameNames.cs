// @sonhg: class: GameNames
using System;
using System.Collections.Generic;
using System.Linq;

public class GameNames
{
	public static List<string> RemoveUnused(string[] list)
	{
		List<string> list2 = new List<string>();
		foreach (string text in list)
		{
			if (Config.LIST_DEV_GAMES.Contains(text.Trim()))
			{
				list2.Add(text.Trim());
			}
		}
		return list2.Distinct<string>().ToList<string>();
	}

	public const string BOMB_BATTLE = "BOMB_BATTLE";

	public const string BATTLE_TEAM = "BATTLE_TEAM";

	public const string BOMB_BOSS = "BOMB_BOSS";

	public const string NOT_SET = "";

	public const string ALL = "";
}
