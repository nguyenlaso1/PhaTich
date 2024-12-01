// @plugin: class: EventHitBuilder
using System;
using UnityEngine;

public class EventHitBuilder : HitBuilder<EventHitBuilder>
{
	public string GetEventCategory()
	{
		return this.eventCategory;
	}

	public EventHitBuilder SetEventCategory(string eventCategory)
	{
		if (eventCategory != null)
		{
			this.eventCategory = eventCategory;
		}
		return this;
	}

	public string GetEventAction()
	{
		return this.eventAction;
	}

	public EventHitBuilder SetEventAction(string eventAction)
	{
		if (eventAction != null)
		{
			this.eventAction = eventAction;
		}
		return this;
	}

	public string GetEventLabel()
	{
		return this.eventLabel;
	}

	public EventHitBuilder SetEventLabel(string eventLabel)
	{
		if (eventLabel != null)
		{
			this.eventLabel = eventLabel;
		}
		return this;
	}

	public long GetEventValue()
	{
		return this.eventValue;
	}

	public EventHitBuilder SetEventValue(long eventValue)
	{
		this.eventValue = eventValue;
		return this;
	}

	public override EventHitBuilder GetThis()
	{
		return this;
	}

	public override EventHitBuilder Validate()
	{
		if (string.IsNullOrEmpty(this.eventCategory))
		{
			UnityEngine.Debug.LogWarning("No event category provided - Event hit cannot be sent.");
			return null;
		}
		if (string.IsNullOrEmpty(this.eventAction))
		{
			UnityEngine.Debug.LogWarning("No event action provided - Event hit cannot be sent.");
			return null;
		}
		return this;
	}

	private string eventCategory = string.Empty;

	private string eventAction = string.Empty;

	private string eventLabel = string.Empty;

	private long eventValue;
}
