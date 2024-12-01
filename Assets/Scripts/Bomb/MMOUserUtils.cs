// @sonhg: class: Bomb.MMOUserUtils
using System;
using System.Collections.Generic;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

namespace Bomb
{
	public class MMOUserUtils : JokerUserUtils
	{
		public static User GetUserByID(int id)
		{
			List<User> userList = RoomUtils.GetUserList();
			foreach (User user in userList)
			{
				if (user.Id == id)
				{
					return user;
				}
			}
			return null;
		}

		public static bool IsReady(User user)
		{
			return user.ContainsVariable("mmo-ready") && user.GetVariable("mmo-ready").GetBoolValue();
		}

		public static int GetUserPosition(User user)
		{
			if (user != null && user.ContainsVariable("position"))
			{
				return user.GetVariable("position").GetIntValue();
			}
			return -1;
		}

		public static SFSObject GetCharacter(User user)
		{
			if (user.ContainsVariable("character"))
			{
				return (SFSObject)user.GetVariable("character").GetSFSObjectValue();
			}
			return null;
		}

		public static int GetHead(User user)
		{
			SFSObject character = MMOUserUtils.GetCharacter(user);
			return character.GetInt("face");
		}

		public static int GetBody(User user)
		{
			SFSObject character = MMOUserUtils.GetCharacter(user);
			return character.GetInt("body");
		}

		public static int GetHair(User user)
		{
			SFSObject character = MMOUserUtils.GetCharacter(user);
			return character.GetInt("hair");
		}

		public static string GetHairColor(User user)
		{
			SFSObject character = MMOUserUtils.GetCharacter(user);
			return character.GetUtfString("hair_color");
		}

		public static int GetBomb(User user)
		{
			SFSObject character = MMOUserUtils.GetCharacter(user);
			return character.GetInt("bomb");
		}

		public static bool IsBot(User user)
		{
			return user.ContainsVariable("is-bot") && user.GetVariable("is-bot").GetBoolValue();
		}

		public const int INVALID = -1;

		public const int DEFAULT = 1;

		public const int DEFAULT_ZERO = 0;
	}
}
