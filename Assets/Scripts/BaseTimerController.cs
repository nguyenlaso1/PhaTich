// @sonhg: class: BaseTimerController
using System;
using UnityEngine;

public abstract class BaseTimerController : MonoBehaviour
{
	protected abstract void DoUpdateTimer();

	public float RemainTime
	{
		get
		{
			return this._maxRaiseTime - this._thoughtTime;
		}
	}

	protected void Update()
	{
		this.UpdateRaising();
	}

	public virtual void StartRaise(float thoughtTime, float maxTime, Context.OnDeletegateObject onThinkingTimeout = null, object thinkingParam = null)
	{
		base.gameObject.SetActive(true);
		this._isRaising = true;
		this._thoughtTime = thoughtTime;
		this._maxRaiseTime = maxTime;
		this._remind = true;
		this._onThinkingTimeout = onThinkingTimeout;
		this._thinkingParam = thinkingParam;
	}

	public virtual void StopRaising(string debug)
	{
		this._isRaising = false;
		this._thoughtTime = 0f;
		this._remind = false;
		this._onThinkingTimeout = null;
		this._thinkingParam = null;
		base.gameObject.SetActive(false);
	}

	public virtual void PauseRaising()
	{
		this._isRaising = false;
	}

	public virtual void ResumeRaising()
	{
		this._isRaising = true;
	}

	public void OnRaisingTimeOut()
	{
		UnityEngine.Debug.Log("OnRaiseTimeOut");
		if (this._onThinkingTimeout != null)
		{
			this._onThinkingTimeout(this._thinkingParam);
		}
		base.gameObject.SetActive(false);
	}

	protected void UpdateRaising()
	{
		if (this._isRaising)
		{
			if (this._thoughtTime < this._maxRaiseTime)
			{
				this._thoughtTime += Time.deltaTime;
				this.DoUpdateTimer();
				if (this._remind && this._maxRaiseTime - this._thoughtTime < 5f)
				{
					this._remind = false;
					this.RemindRaising();
				}
			}
			else
			{
				this.OnRaisingTimeOut();
				this.StopRaising("UpdateRaising");
			}
		}
	}

	private void RemindRaising()
	{
		if (Context.GameInfo.IsVibrate)
		{
			Context.GameInfo.Vibrate();
		}
	}

	private Gradient FillGradient
	{
		get
		{
			if (this._gradient == null)
			{
				GradientColorKey[] array = new GradientColorKey[3];
				GradientAlphaKey[] array2 = new GradientAlphaKey[2];
				this._gradient = new Gradient();
				array[0].color = Color.red;
				array[0].time = 0f;
				array[1].color = Color.yellow;
				array[1].time = 0.5f;
				array[2].color = Color.green;
				array[2].time = 1f;
				array2[0].alpha = 0.6f;
				array2[0].time = 0f;
				array2[1].alpha = 0.6f;
				array2[1].time = 1f;
				this._gradient.SetKeys(array, array2);
			}
			return this._gradient;
		}
	}

	[Header("Base")]
	protected Context.OnDeletegateObject _onThinkingTimeout;

	protected object _thinkingParam;

	protected bool _isRaising;

	protected float _thoughtTime;

	protected float _maxRaiseTime;

	protected bool _remind;

	private Gradient _gradient;
}
