// @sonhg: class: Bomb.Offline_FreeCoinItem
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bomb
{
	public class Offline_FreeCoinItem : MonoBehaviour
	{
		private void Start()
		{
			switch (this.type)
			{
			case FreeCoinType.Rate:
				this.gold.text = Offline_Config.RATE_COIN_BONUS + string.Empty;
				break;
			case FreeCoinType.Invite:
				this.gold.text = Offline_Config.INVITE_COIN_BONUS + string.Empty;
				break;
			case FreeCoinType.Like:
				this.gold.text = Offline_Config.LIKE_COIN_BONUS + string.Empty;
				break;
			case FreeCoinType.Share:
				this.gold.text = Offline_Config.SHARE_COIN_BONUS + string.Empty;
				break;
			}
			if (DataManager.Rated(this.uri))
			{
				base.gameObject.SetActive(false);
			}
		}

		private void OnEnable()
		{
			if (DataManager.Rated(this.uri))
			{
				base.gameObject.SetActive(false);
			}
		}

		private void Update()
		{
		}

		public void OnFreeCoinClick()
		{
			switch (this.type)
			{
			case FreeCoinType.Rate:
				this.freeCoinController.Rate(this.uri);
				break;
			case FreeCoinType.Invite:
				this.freeCoinController.InviteFacebook(this.uri, this.picture_uri);
				break;
			case FreeCoinType.Like:
				this.freeCoinController.InviteFacebook(this.uri, this.picture_uri);
				break;
			case FreeCoinType.Share:
				this.freeCoinController.shareFacebook(this.uri, this.title, this.description, this.picture_uri);
				break;
			}
		}

		public FreeCoinType type;

		public Offline_FreeCoinController freeCoinController;

		public string uri;

		public string picture_uri;

		public string title;

		public string description;

		public Text gold;
	}
}
