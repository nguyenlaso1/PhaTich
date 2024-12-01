// @sonhg: class: BombOffline.Offline_BaseCommand
using System;

namespace BombOffline
{
	public abstract class Offline_BaseCommand
	{
		public abstract void Execute(Offline_PlayerController actor);
	}
}
