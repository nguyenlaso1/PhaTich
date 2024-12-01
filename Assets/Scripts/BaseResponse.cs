// @sonhg: class: BaseResponse
using System;
using Sfs2X.Core;
using UnityEngine;

public abstract class BaseResponse
{
	public abstract void UpdateBusiness();

	public abstract void UpdateGUI();

	public void SetParams(BaseEvent baseEvent, BaseScene baseScene)
	{
		this.evt = baseEvent;
		this.baseScene = baseScene;
	}

	public virtual void Run(bool isHideWaiting = true)
	{
		UnityEngine.Debug.Log("OnResponse: " + this.Dump);
		if (isHideWaiting)
		{
			Context.Waiting.HideWaiting();
		}
		this.UpdateBusiness();
		this.UpdateGUI();
	}

	protected virtual string Dump
	{
		get
		{
			return base.GetType().Name;
		}
	}

	protected BaseEvent evt;

	protected BaseScene baseScene;
}
