// @plugin: class: TimingHitBuilder
using System;
using UnityEngine;

public class TimingHitBuilder : HitBuilder<TimingHitBuilder>
{
	public string GetTimingCategory()
	{
		return this.timingCategory;
	}

	public TimingHitBuilder SetTimingCategory(string timingCategory)
	{
		if (timingCategory != null)
		{
			this.timingCategory = timingCategory;
		}
		return this;
	}

	public long GetTimingInterval()
	{
		return this.timingInterval;
	}

	public TimingHitBuilder SetTimingInterval(long timingInterval)
	{
		this.timingInterval = timingInterval;
		return this;
	}

	public string GetTimingName()
	{
		return this.timingName;
	}

	public TimingHitBuilder SetTimingName(string timingName)
	{
		if (timingName != null)
		{
			this.timingName = timingName;
		}
		return this;
	}

	public string GetTimingLabel()
	{
		return this.timingLabel;
	}

	public TimingHitBuilder SetTimingLabel(string timingLabel)
	{
		if (timingLabel != null)
		{
			this.timingLabel = timingLabel;
		}
		return this;
	}

	public override TimingHitBuilder GetThis()
	{
		return this;
	}

	public override TimingHitBuilder Validate()
	{
		if (string.IsNullOrEmpty(this.timingCategory))
		{
			UnityEngine.Debug.LogError("No timing category provided - Timing hit cannot be sent");
			return null;
		}
		if (this.timingInterval == 0L)
		{
			UnityEngine.Debug.Log("Interval in timing hit is 0.");
		}
		return this;
	}

	private string timingCategory = string.Empty;

	private long timingInterval;

	private string timingName = string.Empty;

	private string timingLabel = string.Empty;
}
