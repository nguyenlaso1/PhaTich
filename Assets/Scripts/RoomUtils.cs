// @sonhg: class: RoomUtils
using System;
using System.Collections.Generic;
using Sfs2X.Entities;
using UnityEngine;

public class RoomUtils
{
	public static Room JoinedRoom
	{
		get
		{
			return SmartFoxConnection.Connection.LastJoinedRoom;
		}
	}

	public static JokerEnum.ClientGameState GetGameState()
	{
		if (RoomUtils.JoinedRoom.ContainsVariable("r-gstate"))
		{
			return Context.MapServerToClientGameState(RoomUtils.JoinedRoom.GetVariable("r-gstate").GetIntValue());
		}
		return JokerEnum.ClientGameState.GS_IN_GAME_WAITING;
	}

	public static int GetPlayerCount()
	{
		int num = 0;
		if (RoomUtils.JoinedRoom != null)
		{
			foreach (User user in RoomUtils.JoinedRoom.UserList)
			{
				if (user.IsPlayer)
				{
					num++;
				}
			}
		}
		UnityEngine.Debug.Log("GetPlayerCount: " + num);
		return num;
	}

	public static List<User> GetPlayerList()
	{
		List<User> list = new List<User>();
		foreach (User user in RoomUtils.JoinedRoom.UserList)
		{
			if (user.IsPlayer)
			{
				list.Add(user);
			}
		}
		return list;
	}

	public static int GetRoomOwnerId()
	{
		if (RoomUtils.JoinedRoom != null && RoomUtils.JoinedRoom.ContainsVariable("r-ownerid"))
		{
			return RoomUtils.JoinedRoom.GetVariable("r-ownerid").GetIntValue();
		}
		return RoomUtils.INVALID_OWNER_ID;
	}

	public static bool IsRoomOwner(User user)
	{
		return user != null && user.Id == RoomUtils.GetRoomOwnerId();
	}

	public static bool MeIsRoomOwner()
	{
		return RoomUtils.IsRoomOwner(SmartFoxConnection.Connection.MySelf);
	}

	public static List<User> GetUserList()
	{
		if (RoomUtils.JoinedRoom != null)
		{
			return RoomUtils.JoinedRoom.UserList;
		}
		return null;
	}

	private static int INVALID_OWNER_ID = -1;
}
