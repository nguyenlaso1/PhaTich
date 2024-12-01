// @sonhg: class: OfflineClockController
using System;
using DG.Tweening;
using UnityEngine;

public class OfflineClockController : ClockTimerController
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
		base.gameObject.SetActive(true);
	}

	public string GetRemainString()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)base.RemainTime);
		return string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
	}

	public int GetProgessStar()
	{
		int num = Mathf.CeilToInt(base.RemainTime / this._maxRaiseTime * 10f);
		if (num > 6)
		{
			return 3;
		}
		if (num > 2)
		{
			return 2;
		}
		return 1;
	}

	public void AddTimes(float seconds)
	{
		this._maxRaiseTime += seconds;
	}

	[Header("OfflineClock")]
	private Sequence sequence;

	[SerializeField]
	private Animator sandClockAnimator;

	private Color originalColor;
}
