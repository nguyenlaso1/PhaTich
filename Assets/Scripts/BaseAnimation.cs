// @sonhg: class: BaseAnimation
using System;
using UnityEngine;

public abstract class BaseAnimation
{
	public abstract void Run();

	protected abstract void CompeleteDotween();

	public void ForceComplete()
	{
		UnityEngine.Debug.Log("ForceComplete" + base.GetType());
		this.CompeleteDotween();
		this.onForceComplete();
	}

	public BaseResponse baseResponse;

	protected Context.OnDeletegateNone onNormalComplete;

	protected Context.OnDeletegateNone onForceComplete;
}
