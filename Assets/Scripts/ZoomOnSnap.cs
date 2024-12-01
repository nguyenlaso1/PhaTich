// @sonhg: class: ZoomOnSnap
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ZoomOnSnap : MonoBehaviour
{
	public ScrollRect Scroll
	{
		get
		{
			if (this._scroll == null)
			{
				this._scroll = base.GetComponent<ScrollRect>();
			}
			return this._scroll;
		}
	}

	private void Update()
	{
		foreach (object obj in this.Scroll.content.transform)
		{
			Transform transform = (Transform)obj;
			float num = Mathf.Abs(transform.position.x - this.Scroll.transform.position.x);
			float num2 = 1.2f * (this.range - num) / this.range;
			num2 = Mathf.Max(num2, 0.5f);
			transform.localScale = Vector3.one * num2;
		}
	}

	public float range = 100f;

	private ScrollRect _scroll;
}
