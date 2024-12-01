// @sonhg: class: ScrollViewCommon
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewCommon : MonoBehaviour
{
	private void Awake()
	{
		this._scrollview = base.gameObject.GetComponent<UIScrollView>();
		this._panel = base.gameObject.GetComponent<UIPanel>();
	}

	private void Start()
	{
	}

	public bool IsName(string name)
	{
		if (this.listItem != null)
		{
			for (int i = 0; i < this.listItem.Count; i++)
			{
				if (this.listItem[i].strName == name)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void AddGameObject(GameObject go, string name, float cellWidth = 0f, float cellHeigh = 0f, bool isUpdate = false)
	{
		float num = cellHeigh;
		float num2 = cellWidth;
		if (num <= 0f)
		{
			num = cellHeigh;
		}
		if (num2 <= 0f)
		{
			num2 = cellWidth;
		}
		if (this.listItem == null)
		{
			this.listItem = new List<scrollviewItem>();
		}
		if (go.GetComponent<UIDragScrollView>() == null)
		{
			go.AddComponent<UIDragScrollView>();
		}
		if (go.GetComponent<BoxCollider>() == null)
		{
			BoxCollider boxCollider = go.AddComponent<BoxCollider>();
			Vector3 vector = new Vector3(num2, num, 1f);
			boxCollider.size = vector;
			if (this.isTop)
			{
				boxCollider.center = vector * -1f / 2f;
			}
		}
		this.listItem.Add(new scrollviewItem(go, name, num2, num));
		if (isUpdate)
		{
			this.UpdateScrollView(true);
		}
	}

	public Vector3 GetNextPosition(float cellWidth = 0f, float cellHeigh = 0f)
	{
		Vector3 zero = Vector3.zero;
		float num = cellHeigh;
		float num2 = cellWidth;
		if (num <= 0f)
		{
			num = cellHeigh;
		}
		if (num2 <= 0f)
		{
			num2 = cellWidth;
		}
		if (this._scrollview.movement == UIScrollView.Movement.Vertical)
		{
			float num3 = this._panel.GetViewSize().y / 2f;
			float num4 = 0f;
			int num5 = 0;
			if (this.listItem != null)
			{
				num5 = this.listItem.Count;
			}
			int num6 = num5 % this.column;
			int num7 = Mathf.CeilToInt((float)(num5 / this.column));
			if (num7 > 0)
			{
				int index = (num7 - 1) * this.column;
				num3 = this.listItem[index].go.transform.localPosition.y;
				num4 = num;
			}
			num3 -= num4 / 2f;
			if (this.isTop)
			{
				zero.y = num3;
			}
			else
			{
				zero.y = num3 - num / 2f;
			}
			zero.x = (float)num6 * num2;
			if (this.column > 1)
			{
				zero.x -= this._panel.width / 2f - num2 / 2f;
			}
		}
		else if (this._scrollview.movement == UIScrollView.Movement.Horizontal)
		{
		}
		return zero;
	}

	public void UpdateWidthHeigh(float heigh, float width = 0f)
	{
		scrollviewItem scrollviewItem = this.listItem[this.listItem.Count - 1];
		scrollviewItem.cellHeigh = heigh;
		if (width != 0f)
		{
			scrollviewItem.cellWidth = width;
		}
		BoxCollider component = scrollviewItem.go.GetComponent<BoxCollider>();
		Vector3 vector = new Vector3(scrollviewItem.cellWidth, scrollviewItem.cellHeigh, 1f);
		component.size = vector;
		if (this.isTop)
		{
			component.center = vector * -1f / 2f;
			component.size = new Vector3(scrollviewItem.cellWidth * 2f, scrollviewItem.cellHeigh, 1f);
		}
	}

	public void UpdateAllGameObject(float heigh, float width = 0f, bool isUpdate = false)
	{
		if (this.listItem == null)
		{
			return;
		}
		for (int i = 0; i < this.listItem.Count; i++)
		{
			scrollviewItem scrollviewItem = this.listItem[i];
			if (heigh > 0f)
			{
				scrollviewItem.cellHeigh = heigh;
			}
			if (width != 0f)
			{
				scrollviewItem.cellWidth = width;
			}
		}
		if (isUpdate)
		{
			this.UpdateScrollView(true);
		}
	}

	public void UpdateGameObject(string strname, float heigh, float width = 0f, bool isUpdate = false)
	{
		if (this.listItem == null)
		{
			return;
		}
		for (int i = 0; i < this.listItem.Count; i++)
		{
			scrollviewItem scrollviewItem = this.listItem[i];
			if (scrollviewItem.strName == strname)
			{
				if (heigh > 0f)
				{
					scrollviewItem.cellHeigh = heigh;
				}
				if (width != 0f)
				{
					scrollviewItem.cellWidth = width;
				}
			}
		}
		if (isUpdate)
		{
			this.UpdateScrollView(true);
		}
	}

	public bool RemoveGameObject(string strname, bool isUpdate = true)
	{
		if (this.listItem == null)
		{
			return true;
		}
		for (int i = 0; i < this.listItem.Count; i++)
		{
			if (this.listItem[i].strName == strname)
			{
				NGUITools.Destroy(this.listItem[i].go);
				this.listItem.RemoveAt(i);
			}
		}
		if (isUpdate)
		{
			this.UpdateScrollView(true);
		}
		return this.listItem.Count <= 0;
	}

	public void RemoveallChild()
	{
		while (base.transform.childCount > 0)
		{
			NGUITools.Destroy(base.transform.GetChild(0).gameObject);
		}
		this.listItem = null;
	}

	private void HorizontalUpdate()
	{
		float num = 0f;
		float num2 = -this._panel.GetViewSize().x / 2f;
		Vector3 zero = Vector3.zero;
		this._scrollview.ResetPosition();
		for (int i = 0; i < this.listItem.Count; i++)
		{
			zero.x = num2 + this.listItem[i].cellWidth / 2f;
			num2 += this.listItem[i].cellWidth;
			num += this.listItem[i].cellWidth;
			this.listItem[i].go.transform.localPosition = zero;
		}
	}

	private void VerticalUpdate(bool isReset = true)
	{
		float num = 0f;
		float num2 = this._panel.GetViewSize().y / 2f;
		Vector3 zero = Vector3.zero;
		float num3 = 0f;
		float num4 = 0f;
		for (int i = 0; i < this.listItem.Count; i++)
		{
			float num5 = (float)(i % this.column);
			if (num3 != (float)Mathf.CeilToInt((float)(i / this.column)))
			{
				num2 -= num4;
				num += num4;
				num4 = this.listItem[i].cellHeigh;
				num3 = (float)Mathf.CeilToInt((float)(i / this.column));
			}
			else if (this.listItem[i].cellHeigh > num4)
			{
				num4 = this.listItem[i].cellHeigh;
			}
			if (this.isTop)
			{
				zero.y = num2;
			}
			else
			{
				zero.y = num2 - this.listItem[i].cellHeigh / 2f;
			}
			zero.x = num5 * this.listItem[i].cellWidth;
			if (this.column > 1)
			{
				zero.x -= this._panel.width / 2f - this.listItem[i].cellWidth / 2f;
			}
			this.listItem[i].go.transform.localPosition = zero;
		}
		this.heightScrollLists = this._panel.GetViewSize().y / 2f - num2;
		if (this.listItem.Count > 0 && this.listItem[this.listItem.Count - 1] != null)
		{
			this.heightScrollLists += this.listItem[this.listItem.Count - 1].cellHeigh;
		}
		if (isReset)
		{
			this._scrollview.ResetPosition();
		}
		if (this.heightScrollLists >= this._panel.height)
		{
			this._scrollview.enabled = true;
		}
		else
		{
			this._scrollview.enabled = false;
		}
	}

	public void UpdateScroll()
	{
		if (this.listItem == null)
		{
			return;
		}
		float y = this.listItem[this.listItem.Count - 1].go.transform.localPosition.y;
		this.heightScrollLists = this._panel.GetViewSize().y / 2f - y;
		if (this.listItem.Count > 0 && this.listItem[this.listItem.Count - 1] != null)
		{
			this.heightScrollLists += this.listItem[this.listItem.Count - 1].cellHeigh;
		}
		this._scrollview.ResetPosition();
		if (this.heightScrollLists >= this._panel.height)
		{
			this._scrollview.enabled = true;
		}
		else
		{
			this._scrollview.enabled = false;
		}
	}

	public void UpdateScrollView(bool isReset = true)
	{
		if (this.listItem == null)
		{
			return;
		}
		if (this._scrollview.movement == UIScrollView.Movement.Horizontal)
		{
			this.HorizontalUpdate();
		}
		else if (this._scrollview.movement == UIScrollView.Movement.Vertical)
		{
			this.VerticalUpdate(isReset);
		}
	}

	public void Reset()
	{
		this.UpdateScrollView(true);
	}

	public void ScrollBottom()
	{
		this._scrollview.SetDragAmount(0f, 1f, false);
		this._scrollview.SetDragAmount(0f, 1f, true);
	}

	public float cellWidth;

	public float cellHeigh;

	public int column = 1;

	public bool isTop;

	public float heightScrollLists;

	private UIPanel _panel;

	private UIScrollView _scrollview;

	public List<scrollviewItem> listItem;
}
