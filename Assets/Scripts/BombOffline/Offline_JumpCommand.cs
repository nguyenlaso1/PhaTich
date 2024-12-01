// @sonhg: class: BombOffline.Offline_JumpCommand
using System;

namespace BombOffline
{
	public class Offline_JumpCommand : Offline_BaseCommand
	{
		public override void Execute(Offline_PlayerController actor)
		{
			actor.JumpCommand();
		}
	}
}
