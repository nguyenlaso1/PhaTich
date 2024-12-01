// @sonhg: class: BombOffline.IAlert
using System;

namespace BombOffline
{
	public interface IAlert
	{
		int AlertRange { get; }

		bool IsPlayerInRange();
	}
}
