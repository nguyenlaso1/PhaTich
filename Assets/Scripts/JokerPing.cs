// @sonhg: class: JokerPing
using System;
using UnityEngine;

public class JokerPing
{
	public static void SendPingPong()
	{
		DateTime now = DateTime.Now;
		if ((now - JokerPing.lastPingPong).TotalSeconds >= (double)Joker2XConfigUtils.PINGPONG_TIME)
		{
			if (SmartFoxConnection.IsConnected)
			{
				PingPongV2Request.SendMessage();
			}
			JokerPing.lastPingPong = now;
		}
	}

	public static void SendIPPing(Context.OnDeletegateObject callback)
	{
		DateTime now = DateTime.Now;
		TimeSpan timeSpan = now - JokerPing.lastIPPing;
		if (timeSpan.TotalSeconds >= (double)Joker2XConfigUtils.PING_REPEAT && Joker2XConfigUtils.PING_IP != string.Empty)
		{
			if (JokerPing.ping != null)
			{
				if (JokerPing.ping.isDone)
				{
					callback(JokerPing.ping.time);
					JokerPing.ping.DestroyPing();
					JokerPing.ping = null;
					JokerPing.lastIPPing = now;
				}
				else if (timeSpan.TotalSeconds > (double)Joker2XConfigUtils.PING_DELAY)
				{
					callback(500);
					JokerPing.ping.DestroyPing();
					JokerPing.ping = null;
					JokerPing.lastIPPing = now;
				}
			}
			else
			{
				JokerPing.ping = new Ping(Joker2XConfigUtils.PING_IP);
			}
		}
	}

	private static DateTime lastPingPong = DateTime.Now;

	private static DateTime lastIPPing = DateTime.Now;

	private static Ping ping;
}
