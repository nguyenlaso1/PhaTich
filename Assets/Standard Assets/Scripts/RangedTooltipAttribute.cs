// @plugin: class: RangedTooltipAttribute
using System;
using UnityEngine;

public class RangedTooltipAttribute : PropertyAttribute
{
	public RangedTooltipAttribute(string text, float min, float max)
	{
		this.text = text;
		this.min = min;
		this.max = max;
	}

	public readonly float min;

	public readonly float max;

	public readonly string text;
}
