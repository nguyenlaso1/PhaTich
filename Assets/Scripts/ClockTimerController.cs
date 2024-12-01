// @sonhg: class: ClockTimerController
using System;
using UnityEngine;
using UnityEngine.UI;

public class ClockTimerController : BaseTimerController
{
	public override void StartRaise(float thoughtTime, float maxTime, Context.OnDeletegateObject onThinkingTimeout, object thinkingParam)
	{
		base.StartRaise(thoughtTime, maxTime, onThinkingTimeout, thinkingParam);
		this.SetFillNumber(maxTime - thoughtTime);
	}

	public override void StopRaising(string debug)
	{
		base.StopRaising(debug);
		this.SetFillNumber(0f);
	}

	protected override void DoUpdateTimer()
	{
		this.SetFillNumber(this._maxRaiseTime - this._thoughtTime);
	}

	protected virtual void SetFillNumber(float timeleft)
	{
		int num = (int)timeleft / 10;
		int num2 = (int)timeleft % 10;
		this.numbers.text = num + string.Empty + num2;
		int num3 = num * 10 + num2;
		if (this.old != num3)
		{
			this.old = num3;
		}
	}

	[Header("Clock")]
	public Text numbers;

	private int old;
}
