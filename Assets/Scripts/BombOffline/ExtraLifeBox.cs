// @sonhg: class: BombOffline.ExtraLifeBox
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	public class ExtraLifeBox : BaseBox
	{
		public void OnClickYesButton()
		{
			if (this._onYesClick != null)
			{
				this._onYesClick(this._yesObject);
			}
			this.OnDestroyBox();
		}

		public void OnClickNoButton()
		{
			this.noButton.GetComponent<Button>().interactable = false;
			if (this._onNoClick != null)
			{
				this._onNoClick(this._noObject);
			}
			this.OnDestroyBox();
			this.noButton.GetComponent<Button>().interactable = true;
		}

		public override void OnClickCloseButton()
		{
			if (this._onCloseClick != null)
			{
				this._onCloseClick();
			}
			this.OnHide();
		}

		public void Show()
		{
			this.OnShow();
		}

		protected override void OnDestroyBox()
		{
			this.OnHide();
		}

		public ExtraLifeBox AddExtraHeart(string str, string bonus_gold, string bonus_free, Context.OnDeletegateObject onYesClick = null, object yesObject = null, Context.OnDeletegateObject onNoClick = null, object noObject = null, Context.OnDeletegateNone onCloseClick = null, string yes = "", string no = "", bool showClose = false)
		{
			this.OnShow();
			this.EnableBackground();
			this.messageLabel.GetComponent<Text>().text = str;
			this._onYesClick = onYesClick;
			this._onNoClick = onNoClick;
			this._yesObject = yesObject;
			this._noObject = noObject;
			this._onCloseClick = onCloseClick;
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
			this.IconList[0].SetActive(false);
			this.IconList[1].SetActive(true);
			this.textBonus[0].text = bonus_gold;
			this.textBonus[1].text = bonus_free;
			if (this.disable)
			{
				this.noButton.GetComponent<Button>().interactable = false;
			}
			return this;
		}

		public ExtraLifeBox AddExtraLife(string str, string bonus_gold, string bonus_free, Context.OnDeletegateObject onYesClick = null, object yesObject = null, Context.OnDeletegateObject onNoClick = null, object noObject = null, Context.OnDeletegateNone onCloseClick = null, string yes = "", string no = "", bool showClose = false)
		{
			this.OnShow();
			this.EnableBackground();
			this.messageLabel.GetComponent<Text>().text = str;
			this._onYesClick = onYesClick;
			this._onNoClick = onNoClick;
			this._yesObject = yesObject;
			this._noObject = noObject;
			this._onCloseClick = onCloseClick;
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
			this.IconList[0].SetActive(true);
			this.IconList[1].SetActive(false);
			this.textBonus[0].text = bonus_gold;
			this.textBonus[1].text = bonus_free;
			if (this.disable)
			{
				this.noButton.GetComponent<Button>().interactable = false;
			}
			return this;
		}

		public ExtraLifeBox EnableBackground()
		{
			this.blackGround.SetActive(true);
			return this;
		}

		private Context.OnDeletegateObject _onYesClick;

		private Context.OnDeletegateObject _onNoClick;

		private Context.OnDeletegateNone _onCloseClick;

		private object _yesObject;

		private object _noObject;

		private object _hideObject;

		public GameObject noButton;

		public GameObject yesButton;

		public GameObject yesLabel;

		public GameObject noLabel;

		public GameObject messageLabel;

		public GameObject blackGround;

		public GameObject[] IconList;

		public Text[] textBonus;

		public bool disable;
	}
}
