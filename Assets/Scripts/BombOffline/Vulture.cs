// @sonhg: class: BombOffline.Vulture
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class Vulture : Offline_BaseMonster, ISpecialAction
	{
		protected override void CreateBrain()
		{
			Repeat repeat = new Repeat(new Wander(this), -1);
			Repeat repeat2 = new Repeat(new SpecialAction(this), -1);
			Parallel brain = new Parallel(3, 3, new Routine[]
			{
				repeat,
				repeat2
			});
			this.brain = brain;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.placeBombCooldown > 0f)
			{
				this.placeBombCooldown -= Time.deltaTime;
			}
		}

		public override IEnumerable<MapNode> GetMapRoute()
		{
			int toX = 0;
			int toY = 0;
			this.board.GetRandomPositionAt(ref toX, ref toY);
			return this.board.SearchFromTo(base.currentX, base.currentY, toX, toY, this);
		}

		protected bool CanPlaceBomb()
		{
			return this.placeBombCooldown <= 0f;
		}

		protected void PlaceBomb()
		{
			if (this.CanPlaceBomb())
			{
				BombModel bombModel = this.board.Scene.MapController.PlaceBomb(this.bombPrefab, this.bombLength, base.transform.position, false, 0, new int?(30), true);
				if (bombModel != null)
				{
					this.placeBombCooldown = this.cooldown;
				}
			}
		}

		public bool DoSpecialAction()
		{
			this.PlaceBomb();
			return true;
		}

		[Header("Vulture")]
		public int bombLength = 1;

		[SerializeField]
		protected float cooldown = 3f;

		[SerializeField]
		protected GameObject bombPrefab;

		protected float placeBombCooldown;
	}
}
