// @sonhg: class: Bomb.GameClockTimerController
using System;
using DG.Tweening;
using UnityEngine;

namespace Bomb
{
	public class GameClockTimerController : ClockTimerController
	{
		private void Awake()
		{
			this.originalColor = this.numbers.color;
		}

		protected override void DoUpdateTimer()
		{
			base.DoUpdateTimer();
			if (base.RemainTime <= 6f && this.sequence == null)
			{
				this.sequence = DOTween.Sequence();
				this.sequence.Append(this.numbers.DoColor(Color.red, 0.2f));
				this.sequence.Append(this.numbers.DoColor(Color.white, 0.2f));
				this.sequence.SetLoops(-1);
				this.sandClockAnimator.SetBool("isHurry", true);
			}
			if (base.RemainTime <= 0f)
			{
				this.mapController.ActiveDoomMode();
			}
		}

		protected override void SetFillNumber(float timeleft)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)timeleft);
			this.numbers.text = string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		}

		public override void StartRaise(float thoughtTime, float maxTime, Context.OnDeletegateObject onThinkingTimeout, object thinkingParam)
		{
			base.StartRaise(thoughtTime, maxTime, onThinkingTimeout, thinkingParam);
			this.numbers.color = this.originalColor;
			this.sandClockAnimator.SetBool("isHurry", false);
			this.sequence.Kill(false);
			this.sequence = null;
		}

		public override void StopRaising(string debug)
		{
			base.StopRaising(debug);
			this.sandClockAnimator.SetBool("isHurry", false);
			base.gameObject.SetActive(true);
		}

		public override void PauseRaising()
		{
			base.PauseRaising();
			this.sandClockAnimator.SetBool("isHurry", false);
		}

		[SerializeField]
		private MapController mapController;

		[SerializeField]
		private Animator sandClockAnimator;

		private Sequence sequence;

		private Color originalColor;
	}
}
