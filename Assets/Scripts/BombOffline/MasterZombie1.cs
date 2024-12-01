// @sonhg: class: BombOffline.MasterZombie1
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class MasterZombie1 : BigDespider
	{
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.spawnCooldown > 0f)
			{
				this.spawnCooldown -= Time.deltaTime;
			}
		}

		protected override void InitMonsterProperty()
		{
			base.InitMonsterProperty();
			this.spawnCooldown = this.spawnTime;
		}

		public override bool DoSpecialAction()
		{
			this.ShootBullet();
			this.SpawnMonster();
			return true;
		}

		protected virtual void SpawnMonster()
		{
			if (this.CanSpawn())
			{
				this.spawnCooldown = this.spawnTime;
				this.monsterList.RemoveAll((Offline_BaseMonster monster) => monster == null);
				for (int i = 0; i < this.maxMonster - this.monsterList.Count; i++)
				{
					Offline_BaseMonster offline_BaseMonster = this.board.PutMonsterAt(base.currentX, base.currentY, this.spawnMonster);
					offline_BaseMonster.transform.ChangeColorRecursive(Color.red);
					this.monsterList.Add(offline_BaseMonster);
				}
			}
		}

		public bool CanSpawn()
		{
			return this.spawnCooldown <= 0f;
		}

		[Header("MasterZombie1")]
		[SerializeField]
		protected Offline_BaseMonster spawnMonster;

		[SerializeField]
		protected float spawnTime = 5f;

		[SerializeField]
		protected int maxMonster = 4;

		protected float spawnCooldown;

		private List<Offline_BaseMonster> monsterList = new List<Offline_BaseMonster>();
	}
}
