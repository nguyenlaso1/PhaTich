// @sonhg: class: MMORoomUtils
using System;

public class MMORoomUtils : RoomUtils
{
	public static long GetEndThingingTime()
	{
		if (RoomUtils.JoinedRoom.ContainsVariable("r-ewaitingTime"))
		{
			return Convert.ToInt64(RoomUtils.JoinedRoom.GetVariable("r-ewaitingTime").GetDoubleValue());
		}
		return (long)MMORoomUtils.INVALID_END_THINKING;
	}

	public static int GetRemainWaitingTime()
	{
		long endThingingTime = MMORoomUtils.GetEndThingingTime();
		if (endThingingTime == (long)MMORoomUtils.INVALID_END_THINKING)
		{
			return MMORoomUtils.INVALID_REMAIN_THINKING;
		}
		long currentTime = DateUtils.CurrentTime;
		return (int)(endThingingTime - currentTime) / 1000;
	}

	public static int GetMapID()
	{
		if (RoomUtils.JoinedRoom.ContainsVariable("r-map-id"))
		{
			return RoomUtils.JoinedRoom.GetVariable("r-map-id").GetIntValue();
		}
		return MMORoomUtils.RANDOM_MAP_ID;
	}

	private static int INVALID_END_THINKING = -1;

	private static int INVALID_REMAIN_THINKING;

	private static int RANDOM_MAP_ID;
}
