// @sonhg: class: BombOffline.Kamestone
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class Kamestone : Kabuton
	{
		protected override void InitMonsterProperty()
		{
			base.InitMonsterProperty();
			this.unTargetableColor = new Color(1f, 1f, 1f, 0.5f);
			this.spawnCooldown = this.spawnTime;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			this.SpawnMonster();
			if (this.spawnCooldown > 0f)
			{
				this.spawnCooldown -= Time.deltaTime;
			}
		}

		protected override void ProcessBomb()
		{
			if (this.isTargetable)
			{
				this.SetTargetableBody(false);
				base.StartCoroutine(this.WaitThenBecomeTargetable(this.untargetableTime));
			}
		}

		protected virtual void SetTargetableBody(bool isTargetable)
		{
			this.isTargetable = isTargetable;
			if (isTargetable)
			{
				base.transform.ChangeColorRecursive(Color.white);
			}
			else
			{
				base.transform.ChangeColorRecursive(this.unTargetableColor);
			}
		}

		public override bool GetHit(int x, int y)
		{
			return this.isTargetable && base.GetHit(x, y);
		}

		public override bool CheckBombCollision()
		{
			return this.isTargetable && base.CheckBombCollision();
		}

		private IEnumerator WaitThenBecomeTargetable(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			this.SetTargetableBody(true);
			yield break;
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

		[SerializeField]
		protected Offline_BaseMonster spawnMonster;

		[SerializeField]
		protected float spawnTime = 5f;

		[SerializeField]
		protected float untargetableTime = 4f;

		[SerializeField]
		protected bool isTargetable = true;

		[SerializeField]
		protected int maxMonster = 4;

		protected Color unTargetableColor;

		protected float spawnCooldown;

		private List<Offline_BaseMonster> monsterList = new List<Offline_BaseMonster>();
	}
}
