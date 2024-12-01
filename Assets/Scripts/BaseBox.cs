// @sonhg: class: BaseBox
using System;
using DG.Tweening;
using UnityEngine;

public abstract class BaseBox : MonoBehaviour
{
	protected virtual void Start()
	{
		BaseBox.currentBaseBox = this;
		this.OnStart();
	}

	public virtual void OnClickCloseButton()
	{
		this.CloseBox();
	}

	public virtual void CloseBox()
	{
		base.transform.DOScale(Vector3.zero, 0.2f).OnComplete(delegate
		{
			this.OnDestroyBox();
		});
	}

	protected virtual void OnStart()
	{
	}

	protected virtual void OnShow()
	{
		base.transform.localScale = Vector3.one;
		base.gameObject.SetActive(true);
	}

	protected virtual void OnDestroyBox()
	{
		BaseBox.currentBaseBox = null;
		if (this.OnCloseBox != null)
		{
			this.OnCloseBox();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected virtual void OnHide()
	{
		base.gameObject.SetActive(false);
	}

	public static bool CloseCurrentBox()
	{
		if (BaseBox.currentBaseBox != null)
		{
			BaseBox.currentBaseBox.CloseBox();
			return true;
		}
		return false;
	}

	public static BaseBox currentBaseBox;

	public BaseBox.BoxCallbackDelegate OnCloseBox;

	public delegate void BoxCallbackDelegate();
}
