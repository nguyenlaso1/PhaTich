// @sonhg: class: BombOffline.Kabuton
using System;
using UnityEngine;

namespace BombOffline
{
	public class Kabuton : BaseEggMonster, ISpecialAction
	{
		protected override void CreateBrain()
		{
			SequenceNode sequenceNode = new SequenceNode();
			sequenceNode.AddRoutine(new Routine[]
			{
				new CheckCanMove(this),
				new AIMove(this)
			});
			Repeat repeat = new Repeat(sequenceNode, -1);
			SequenceNode sequenceNode2 = new SequenceNode();
			sequenceNode2.AddRoutine(new Routine[]
			{
				new CheckBombCollider(this),
				new SpecialAction(this)
			});
			Repeat repeat2 = new Repeat(sequenceNode2, -1);
			Parallel brain = new Parallel(3, 3, new Routine[]
			{
				repeat2,
				repeat
			});
			this.brain = brain;
		}

		protected virtual void ProcessBomb()
		{
			this.TriggerBomb();
		}

		private void TriggerBomb()
		{
			if (this.collisionObject != null && this.collisionObject.CompareTag("Bomb"))
			{
				BombModel bomb = this.collisionObject.GetComponent<Offline_Bomb>().bomb;
				MoveDirection pushDirection = this.faceDirection;
				int num = Mathf.RoundToInt(bomb.position.x);
				int num2 = Mathf.RoundToInt(bomb.position.y);
				if (num < base.currentX)
				{
					pushDirection = MoveDirection.LEFT;
				}
				else if (num > base.currentX)
				{
					pushDirection = MoveDirection.RIGHT;
				}
				else if (num2 < base.currentY)
				{
					pushDirection = MoveDirection.DOWN;
				}
				else
				{
					pushDirection = MoveDirection.UP;
				}
				this.board.PushBomb(bomb, pushDirection, true);
				this.collisionObject = null;
			}
		}

		public bool DoSpecialAction()
		{
			this.ProcessBomb();
			return true;
		}
	}
}
