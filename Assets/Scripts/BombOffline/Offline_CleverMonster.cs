// @sonhg: class: BombOffline.Offline_CleverMonster
using System;
using UnityEngine;

namespace BombOffline
{
	public class Offline_CleverMonster : Offline_StupidMonster
	{
		protected override void Start()
		{
			base.transform.ChangeColorRecursive(this.monsterColor);
			base.CurrentDirection = this.defaultDirect;
			base.StartCoroutine(base.RandomChangeDirection());
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
		}

		[Header("CleverBot")]
		private bool shouldChase;
	}
}
