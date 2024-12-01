// @plugin: class: SocialHitBuilder
using System;
using UnityEngine;

public class SocialHitBuilder : HitBuilder<SocialHitBuilder>
{
	public string GetSocialNetwork()
	{
		return this.socialNetwork;
	}

	public SocialHitBuilder SetSocialNetwork(string socialNetwork)
	{
		if (socialNetwork != null)
		{
			this.socialNetwork = socialNetwork;
		}
		return this;
	}

	public string GetSocialAction()
	{
		return this.socialAction;
	}

	public SocialHitBuilder SetSocialAction(string socialAction)
	{
		if (socialAction != null)
		{
			this.socialAction = socialAction;
		}
		return this;
	}

	public string GetSocialTarget()
	{
		return this.socialTarget;
	}

	public SocialHitBuilder SetSocialTarget(string socialTarget)
	{
		if (socialTarget != null)
		{
			this.socialTarget = socialTarget;
		}
		return this;
	}

	public override SocialHitBuilder GetThis()
	{
		return this;
	}

	public override SocialHitBuilder Validate()
	{
		if (string.IsNullOrEmpty(this.socialNetwork))
		{
			UnityEngine.Debug.LogError("No social network provided - Social hit cannot be sent");
			return null;
		}
		if (string.IsNullOrEmpty(this.socialAction))
		{
			UnityEngine.Debug.LogError("No social action provided - Social hit cannot be sent");
			return null;
		}
		if (string.IsNullOrEmpty(this.socialTarget))
		{
			UnityEngine.Debug.LogError("No social target provided - Social hit cannot be sent");
			return null;
		}
		return this;
	}

	private string socialNetwork = string.Empty;

	private string socialAction = string.Empty;

	private string socialTarget = string.Empty;
}
