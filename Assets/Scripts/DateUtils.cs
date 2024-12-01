// @sonhg: class: DateUtils
using System;
using System.Globalization;

public class DateUtils
{
	public static void SetDeltaTime(long serverCurrentTime)
	{
		long num = (DateTime.UtcNow.Ticks - DateUtils.Epoch.Ticks) / 10000L;
		DateUtils.DELTA_TIME = serverCurrentTime - num;
	}

	public static long CurrentTime
	{
		get
		{
			long num = (DateTime.UtcNow.Ticks - DateUtils.Epoch.Ticks) / 10000L;
			return num + DateUtils.DELTA_TIME;
		}
	}

	private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), DateTimeKind.Utc);

	private static long DELTA_TIME = 0L;
}
