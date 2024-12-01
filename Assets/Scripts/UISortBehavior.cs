// @sonhg: class: UISortBehavior
using System;
using UnityEngine;

public class UISortBehavior : MonoBehaviour
{
	private void Awake()
	{
		this.m_renderer = base.GetComponent<Renderer>();
	}

	private void LateUpdate()
	{
		if (this.widgetBehindMe != null && this.widgetBehindMe.drawCall != null && !this.isDone)
		{
			int renderQueue = this.widgetBehindMe.drawCall.renderQueue + this.subDepth;
			this.isDone = true;
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				foreach (Material material in renderer.materials)
				{
					material.renderQueue = renderQueue;
				}
			}
		}
	}

	public UIWidget widgetBehindMe;

	public int subDepth = 1;

	private bool isDone;

	[NonSerialized]
	private Renderer m_renderer;
}
