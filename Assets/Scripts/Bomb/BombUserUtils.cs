// @sonhg: class: Bomb.BombUserUtils
using System;
using Sfs2X.Entities;

namespace Bomb
{
	public class BombUserUtils : MMOUserUtils
	{
		public static int GetTotalBomb()
		{
			User mySelf = SmartFoxConnection.Connection.MySelf;
			if (mySelf.ContainsVariable("total-bomb"))
			{
				return mySelf.GetVariable("total-bomb").GetIntValue();
			}
			return 1;
		}

		public static int GetCurrentBomb()
		{
			User mySelf = SmartFoxConnection.Connection.MySelf;
			if (mySelf.ContainsVariable("current-bomb"))
			{
				return mySelf.GetVariable("current-bomb").GetIntValue();
			}
			return 0;
		}

		public static int GetCurrentBombLength()
		{
			User mySelf = SmartFoxConnection.Connection.MySelf;
			if (mySelf.ContainsVariable("bomb-length"))
			{
				return mySelf.GetVariable("bomb-length").GetIntValue();
			}
			return 1;
		}

		public static int GetBombLength(int userId)
		{
			User userById = SmartFoxConnection.Connection.UserManager.GetUserById(userId);
			if (userById.ContainsVariable("bomb-length"))
			{
				return userById.GetVariable("bomb-length").GetIntValue();
			}
			return 1;
		}

		public static int GetSpeed(User user)
		{
			if (user.ContainsVariable("speed"))
			{
				return user.GetVariable("speed").GetIntValue();
			}
			return 0;
		}

		public static bool IsAlive()
		{
			User mySelf = SmartFoxConnection.Connection.MySelf;
			return mySelf.ContainsVariable("is-alive") && mySelf.GetVariable("is-alive").GetBoolValue();
		}

		public static double GetX(User user)
		{
			if (user.ContainsVariable("x"))
			{
				return user.GetVariable("x").GetDoubleValue();
			}
			return 1.0;
		}

		public static double GetY(User user)
		{
			if (user.ContainsVariable("y"))
			{
				return user.GetVariable("y").GetDoubleValue();
			}
			return 1.0;
		}

		public static int GetDirection(User user)
		{
			if (user.ContainsVariable("direction"))
			{
				return user.GetVariable("direction").GetIntValue();
			}
			return 1;
		}

		public static int GetTotalBomb(User user)
		{
			if (user.ContainsVariable("total-bomb"))
			{
				return user.GetVariable("total-bomb").GetIntValue();
			}
			return 1;
		}

		public static int GetTeam(User user)
		{
			if (user.ContainsVariable("team"))
			{
				return user.GetVariable("team").GetIntValue();
			}
			return -1;
		}

		public const int TEAM_A = 0;

		public const int TEAM_B = 1;
	}
}
