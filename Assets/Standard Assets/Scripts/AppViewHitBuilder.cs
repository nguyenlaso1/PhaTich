// @plugin: class: AppViewHitBuilder
using System;
using UnityEngine;

public class AppViewHitBuilder : HitBuilder<AppViewHitBuilder>
{
	public string GetScreenName()
	{
		return this.screenName;
	}

	public AppViewHitBuilder SetScreenName(string screenName)
	{
		if (screenName != null)
		{
			this.screenName = screenName;
		}
		return this;
	}

	public override AppViewHitBuilder GetThis()
	{
		return this;
	}

	public override AppViewHitBuilder Validate()
	{
		if (string.IsNullOrEmpty(this.screenName))
		{
			UnityEngine.Debug.Log("No screen name provided - App View hit cannot be sent.");
			return null;
		}
		return this;
	}

	private string screenName = string.Empty;
}
