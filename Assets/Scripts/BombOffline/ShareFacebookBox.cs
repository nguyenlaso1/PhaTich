// @sonhg: class: BombOffline.ShareFacebookBox
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	public class ShareFacebookBox : BaseBox
	{
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

		protected virtual void onShow()
		{
			base.gameObject.SetActive(true);
		}

		protected override void OnDestroyBox()
		{
			base.transform.DOScale(Vector3.zero, 0.5f);
			if (this._onAutoHide != null)
			{
				this._onAutoHide(this._hideObject);
			}
			base.gameObject.SetActive(false);
		}

		public ShareFacebookBox AddMessage(string str, string yes = "", string no = "")
		{
			this.onShow();
			this.messageLabel.GetComponent<Text>().text = str;
			this.EnableBackground();
			this.noButton.SetActive(false);
			this.yesButton.SetActive(false);
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

		public ShareFacebookBox AddMessageYes(string str, Context.OnDeletegateObject onYesClick = null, object yesObject = null, string yes = "")
		{
			this.onShow();
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

		public ShareFacebookBox AddOkMessage(string str, Context.OnDeletegateObject onYesClick = null, object yesObject = null, string yes = "")
		{
			this.onShow();
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

		public ShareFacebookBox AddMessageYesNo(string str, Context.OnDeletegateObject onYesClick = null, object yesObject = null, Context.OnDeletegateObject onNoClick = null, object noObject = null, string yes = "", string no = "", bool showClose = false)
		{
			this.onShow();
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

		public ShareFacebookBox EnableBackground()
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

		public GameObject closeButton;

		public GameObject yesLabel;

		public GameObject noLabel;

		public GameObject messageLabel;

		public GameObject blackGround;

		public Image image;
	}
}
