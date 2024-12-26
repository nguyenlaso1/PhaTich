// @sonhg: class: ConfirmBox
using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBox : BaseBox
{
	public static ConfirmBox Setup()
	{
		if (ConfirmBox.instance == null)
		{
			ConfirmBox.instance = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Joker2x/Boxs/ConfirmPopup") as GameObject);
			GameObject gameObject = GameObject.Find("Canvas");
			ConfirmBox.instance.transform.SetParent(gameObject.transform);
			ConfirmBox.instance.transform.localScale = Vector3.one;
			ConfirmBox.instance.transform.localPosition = Vector3.zero;
			ConfirmBox.instance.GetComponent<RectTransform>().anchorMin = Vector2.zero;
			ConfirmBox.instance.GetComponent<RectTransform>().anchorMax = Vector2.one;
			ConfirmBox.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		}
		return ConfirmBox.instance.GetComponent<ConfirmBox>();
	}

	protected override void OnDestroyBox()
	{
		this.OnHide();
	}

	public void OnClickYesButton()
	{
		this.OnDestroyBox();
		if (this._onYesClick != null)
		{
			this._onYesClick(this._yesObject);
		}
	}

	public void OnClickNoButton()
	{
		this.OnDestroyBox();
		if (this._onNoClick != null)
		{
			this._onNoClick(this._noObject);
		}
	}

	public override void OnClickCloseButton()
	{
		this.OnHide();
	}

	public ConfirmBox AddMessage(string str, string yes = "", string no = "")
	{
		this.OnShow();
		this.messageLabel.GetComponent<Text>().text = str;
		this.EnableBackground();
		this.noButton.SetActive(false);
		this.yesButton.SetActive(false);
		this.buttonsPanel.SetActive(false);
		if (this.closeButton != null)
		{
			this.closeButton.SetActive(true);
		}
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

	public ConfirmBox AddMessageYes(string str, Context.OnDeletegateObject onYesClick = null, object yesObject = null, string yes = "")
	{
		this.OnShow();
		this.EnableBackground();
		this.messageLabel.GetComponent<Text>().text = str;
		this.noButton.SetActive(false);
		this.yesButton.SetActive(true);
		this.closeButton.SetActive(false);
		this._onYesClick = onYesClick;
		this._yesObject = yesObject;
		if (StringUtils.CheckNullOrEmpty(yes))
		{
			yes = Language.MSG_YES;
		}
		this.yesLabel.GetComponent<Text>().text = yes;
		return this;
	}

	public ConfirmBox AddOkMessage(string str, Context.OnDeletegateObject onYesClick = null, object yesObject = null, string yes = "")
	{
		this.OnShow();
		this.EnableBackground();
		this.messageLabel.GetComponent<Text>().text = str;
		this.noButton.SetActive(false);
		this.yesButton.SetActive(true);
		this.closeButton.SetActive(false);
		this._onYesClick = onYesClick;
		this._yesObject = yesObject;
		if (StringUtils.CheckNullOrEmpty(yes))
		{
			yes = Language.MSG_OK;
		}
		this.yesLabel.GetComponent<Text>().text = yes;
		return this;
	}

	public void SelftDestroy()
	{
	}

	public ConfirmBox AddMessageYesNo(string str, Context.OnDeletegateObject onYesClick = null, object yesObject = null, Context.OnDeletegateObject onNoClick = null, object noObject = null, string yes = "", string no = "", bool showClose = false)
	{
		this.OnShow();
		this.EnableBackground();
		this.messageLabel.GetComponent<Text>().text = str;
		this.noButton.SetActive(true);
		this.yesButton.SetActive(true);
		if (this.closeButton != null)
		{
			this.closeButton.SetActive(showClose);
		}
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

	public ConfirmBox EnableBackground()
	{
		this.blackGround.SetActive(true);
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

	public GameObject buttonsPanel;

	public GameObject closeButton;

	public GameObject yesLabel;

	public GameObject noLabel;

	public GameObject messageLabel;

	public GameObject blackGround;

	private static GameObject instance;
}
