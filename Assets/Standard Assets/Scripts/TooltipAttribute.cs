// @plugin: class: TooltipAttribute
using System;
using UnityEngine;

public class TooltipAttribute : PropertyAttribute
{
	public TooltipAttribute(string text)
	{
		this.text = text;
	}

	public readonly string text;
}
