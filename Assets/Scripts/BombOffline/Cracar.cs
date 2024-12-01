// @sonhg: class: BombOffline.Cracar
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class Cracar : Offline_BaseMonster, ISpecialAction
	{
		protected override void CreateBrain()
		{
			SequenceNode sequenceNode = new SequenceNode();
			sequenceNode.AddRoutine(new Routine[]
			{
				new Wait(7f),
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
			this.OldPosition = base.transform.position;
			int num = base.currentX;
			this.carAnimator.enabled = true;
			if (this.direct == MoveDirection.LEFT)
			{
				if (num < 0)
				{
					base.transform.rotation = Quaternion.Euler(0, 180, 0);
				}
				else if (num > 12)
				{
					base.transform.rotation = Quaternion.Euler(0, 0, 0);
				}
				this.carAnimator.Play("Right");
				num = 6 - (base.currentX - 6);
			}
			else
			{
				this.carAnimator.Play("Left");
				num += 6 + Mathf.Abs(base.currentX - 6);
			}
			this.Show(num);
			return true;
		}

		private void Hide()
		{
			this.audioSource.Stop();
			for (int i = 0; i < this.spriteList.Count; i++)
			{
				this.spriteList[i].DoAlpha(0f, 2f).OnComplete(delegate
				{
					base.transform.position = this.OldPosition;
					this.carAnimator.enabled = false;
					for (int j = 0; j < this.particleList.Count; j++)
					{
						this.particleList[j].SetActive(false);
					}
				});
			}
		}

		private void Show(int NextPosition)
		{
			for (int i = 0; i < this.spriteList.Count; i++)
			{
				this.spriteList[i].DoAlpha(1f, 2f).OnComplete(delegate
				{
					this.Move(NextPosition);
				});
			}
		}

		private void Move(int NextPosition)
		{
			this.audioSource.Play();
			float duration = Mathf.Abs((base.transform.position.x - (float)NextPosition) / this.baseMoveSpeed);
			base.transform.DOMoveX((float)NextPosition, duration, false).OnComplete(delegate
			{
				this.Hide();
			}).SetEase(Ease.Linear);
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.tag != "Item" && collider.gameObject.tag != "BorderWall")
			{
				if (collider.gameObject.CompareTag("Player"))
				{
					Offline_BaseCharactersController component = collider.gameObject.GetComponent<Offline_BaseCharactersController>();
					component.GetHit(null);
					base.transform.DOKill(false);
					this.Hide();
				}
				if (collider.gameObject.CompareTag("Destroyable"))
				{
					this.board.TriggerSpecialBomb(collider.transform.localPosition, 0, null);
					base.transform.DOKill(false);
					this.Hide();
				}
				if (collider.gameObject.CompareTag("Bomb"))
				{
					this.board.TriggerBomb(collider.transform.position);
					base.transform.DOKill(false);
					this.Hide();
				}
			}
		}

		[Header("Car")]
		public MoveDirection direct;

		public List<SpriteRenderer> spriteList;

		public Animator carAnimator;

		public List<GameObject> particleList;

		[SerializeField]
		protected AudioSource audioSource;

		private Vector3 OldPosition;
	}
}
