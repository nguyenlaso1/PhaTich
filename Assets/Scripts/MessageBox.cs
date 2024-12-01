// @sonhg: class: MessageBox
using System;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : BaseBox
{
	protected override void Start()
	{
		base.Start();
	}

	private void Update()
	{
	}

	public bool IsShow()
	{
		return base.gameObject.activeSelf;
	}

	public bool IsHide()
	{
		return !base.gameObject.activeSelf;
	}

	private void ShowBox()
	{
		base.gameObject.SetActive(true);
		base.transform.localScale = Vector3.zero;
	}

	private void HideBox()
	{
		base.gameObject.SetActive(false);
	}

	public void OnClickYesButton()
	{
		if (this._onYesClick != null)
		{
			this._onYesClick(this._yesObject);
		}
		this.CloseBox();
	}

	public void OnClickNoButton()
	{
		if (this._onNoClick != null)
		{
			this._onNoClick(this._noObject);
		}
		this.CloseBox();
	}

	public void DestroyBox()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void AddOkMessage(string message, Context.OnDeletegateObject onYesClick = null, object yesObject = null, string title = "", string ok = "")
	{
		if (StringUtils.CheckNullOrEmpty(ok))
		{
			ok = Language.MSG_OK;
		}
		this.mesaageLabel.GetComponent<Text>().text = message;
		this.noLabel.transform.parent.gameObject.SetActive(false);
		Transform parent = this.yesLabel.transform.parent;
		this.yesLabel.GetComponent<Text>().text = ok;
		this._onYesClick = onYesClick;
		this._yesObject = yesObject;
		this._onNoClick = null;
		this._noObject = null;
		base.gameObject.SetActive(true);
	}

	public void AddYesNoMessage(string message, Context.OnDeletegateObject onYesClick, Context.OnDeletegateObject onNoClick = null, object yesObject = null, object noObject = null, string title = "", string yes = "", string no = "", bool IsShowCloseButton = true)
	{
		UnityEngine.Debug.Log("AddYesNoMessage");
		if (StringUtils.CheckNullOrEmpty(yes))
		{
			yes = Language.MSG_YES;
		}
		if (StringUtils.CheckNullOrEmpty(no))
		{
			no = Language.MSG_NO;
		}
		this.mesaageLabel.GetComponent<Text>().text = message;
		this._onYesClick = onYesClick;
		this._onNoClick = onNoClick;
		this._noObject = noObject;
		this._yesObject = yesObject;
		this.yesLabel.GetComponent<Text>().text = yes;
		this.noLabel.GetComponent<Text>().text = no;
		base.gameObject.SetActive(true);
	}

	private Context.OnDeletegateObject _onYesClick;

	private Context.OnDeletegateObject _onNoClick;

	private object _noObject;

	private object _yesObject;

	public GameObject yesLabel;

	public GameObject noLabel;

	public GameObject mesaageLabel;

	private bool _isCheck = true;
}
