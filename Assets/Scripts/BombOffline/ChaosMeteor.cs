// @sonhg: class: BombOffline.ChaosMeteor
using System;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class ChaosMeteor : Offline_BaseMonster, ISpecialAction
	{
		protected override void CreateBrain()
		{
			SequenceNode sequenceNode = new SequenceNode();
			sequenceNode.AddRoutine(new Routine[]
			{
				new Wait(this.time),
				new SpecialAction(this)
			});
			this.brain = new Repeat(sequenceNode, -1);
		}

		public override bool GetHit(int x, int y)
		{
			return false;
		}

		public bool DoSpecialAction()
		{
			base.transform.position = new Vector3((float)this.target.currentX, 13f);
			base.transform.DOMoveY(-7f, 21f / this.baseMoveSpeed, false);
			return true;
		}

		[SerializeField]
		[Header("ChaosMeteor")]
		private float time = 11f;
	}
}
