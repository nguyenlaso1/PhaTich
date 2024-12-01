// @sonhg: class: TooltipBox
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TooltipBox : MonoBehaviour
{
	public Animator animator
	{
		get
		{
			if (this._animator == null)
			{
				this._animator = base.GetComponent<Animator>();
			}
			return this._animator;
		}
	}

	private void ShowDropdown()
	{
		this.animator.SetBool("isShow", true);
	}

	private void HideDropdown()
	{
		this.animator.SetBool("isShow", false);
	}

	public void OnClickYesButton()
	{
		this.onDestroy();
		if (this._onYesClick != null)
		{
			this._onYesClick(this._yesObject);
		}
	}

	public void OnClickNoButton()
	{
		this.onDestroy();
		if (this._onNoClick != null)
		{
			this._onNoClick(this._noObject);
		}
	}

	public void OnClickCloseButton()
	{
		this.onDestroy();
	}

	private IEnumerator OnHide(float time)
	{
		this.ShowDropdown();
		yield return new WaitForSeconds(time);
		this.HideDropdown();
		if (this._onAutoHide != null)
		{
			this._onAutoHide(this._hideObject);
		}
		yield break;
	}

	public virtual void onDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public TooltipBox AddMessage(string str, int _time = 5, string yes = "", string no = "")
	{
		if (_time == 0)
		{
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.OnHide((float)_time));
		this.messageLabel.GetComponent<Text>().text = str;
		this.noButton.SetActive(false);
		this.yesButton.SetActive(false);
		if (StringUtils.CheckNullOrEmpty(yes))
		{
			yes = Language.MSG_YES;
		}
		if (StringUtils.CheckNullOrEmpty(no))
		{
			no = Language.MSG_NO;
		}
		this.yesLabel.GetComponent<Text>().text = yes;
		this.noLabel.GetComponent<Text>().text = no;
		return this;
	}

	public TooltipBox AddMessageYes(string str, int _time = 6, Context.OnDeletegateObject onYesClick = null, object yesObject = null, string yes = "")
	{
		if (_time == 0)
		{
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.OnHide((float)_time));
		this.messageLabel.GetComponent<Text>().text = str;
		this.noButton.SetActive(false);
		this.yesButton.SetActive(true);
		this._onYesClick = onYesClick;
		this._yesObject = yesObject;
		if (StringUtils.CheckNullOrEmpty(yes))
		{
			yes = Language.MSG_YES;
		}
		this.yesLabel.GetComponent<Text>().text = yes;
		return this;
	}

	public void SelftDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public TooltipBox AddMessageYesNo(string str, int _time = 6, Context.OnDeletegateObject onYesClick = null, object yesObject = null, Context.OnDeletegateObject onNoClick = null, object noObject = null, string yes = "", string no = "", bool showClose = false)
	{
		if (_time == 0)
		{
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.OnHide((float)_time));
		this.messageLabel.GetComponent<Text>().text = str;
		this.noButton.SetActive(true);
		this.yesButton.SetActive(true);
		this._onYesClick = onYesClick;
		this._onNoClick = onNoClick;
		this._yesObject = yesObject;
		this._noObject = noObject;
		if (StringUtils.CheckNullOrEmpty(yes))
		{
			yes = Language.MSG_YES;
		}
		if (StringUtils.CheckNullOrEmpty(no))
		{
			no = Language.MSG_NO;
		}
		this.yesLabel.GetComponent<Text>().text = yes;
		this.noLabel.GetComponent<Text>().text = no;
		return this;
	}

	private void Destroy()
	{
	}

	private Context.OnDeletegateObject _onYesClick;

	private Context.OnDeletegateObject _onNoClick;

	private Context.OnDeletegateObject _onAutoHide;

	private object _yesObject;

	private object _noObject;

	private object _hideObject;

	public GameObject noButton;

	public GameObject yesButton;

	public GameObject yesLabel;

	public GameObject noLabel;

	public GameObject messageLabel;

	private Animator _animator;
}
