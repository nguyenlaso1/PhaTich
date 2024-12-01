// @sonhg: class: BaseRequest
using System;
using System.Collections;
using Sfs2X.Requests;
using UnityEngine;

public abstract class BaseRequest
{
	protected abstract IRequest Request { get; }

	public void Send()
	{
		SmartFoxConnection.Connection.Send(this.Request);
		UnityEngine.Debug.Log("Request: " + this.Dump);
		if (this.isShowWaiting)
		{
			Context.Waiting.ShowWaiting(30f, delegate(Hashtable input)
			{
				Context.currentMono.RequestTimeOut();
			});
		}
	}

	public string Dump
	{
		get
		{
			return base.GetType().Name;
		}
	}

	protected bool isShowWaiting = true;
}
