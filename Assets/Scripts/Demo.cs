// @sonhg: class: Demo
using System;
using UnityEngine;

public class Demo : MonoBehaviour
{
	private void Awake()
	{
		if (base.enabled)
		{
			GUIAnimSystemFREE.Instance.m_GUISpeed = 1f;
			GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_WaitTimeCount > 0f && this.m_WaitTimeCount <= this.m_WaitTime)
		{
			this.m_WaitTimeCount -= Time.deltaTime;
			if (this.m_WaitTimeCount <= 0f)
			{
				this.m_WaitTimeCount = 0f;
				this.m_ShowMoveInButton = !this.m_ShowMoveInButton;
			}
		}
	}

	private void OnGUI()
	{
		if (this.m_WaitTimeCount <= 0f)
		{
			Rect position = new Rect((float)((Screen.width - 100) / 2), (float)((Screen.height - 50) / 2), 100f, 50f);
			if (this.m_ShowMoveInButton)
			{
				if (GUI.Button(position, "MoveIn"))
				{
					GUIAnimSystemFREE.Instance.MoveIn(base.transform, true);
					this.m_WaitTimeCount = this.m_WaitTime;
				}
			}
			else if (GUI.Button(position, "MoveOut"))
			{
				GUIAnimSystemFREE.Instance.MoveOut(base.transform, true);
				this.m_WaitTimeCount = this.m_WaitTime;
			}
		}
	}

	private float m_WaitTime = 4f;

	private float m_WaitTimeCount;

	private bool m_ShowMoveInButton = true;
}
