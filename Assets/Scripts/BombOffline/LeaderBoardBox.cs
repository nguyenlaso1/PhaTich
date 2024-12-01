using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	public class LeaderBoardBox : BaseBox
	{
		private void Awake()
		{
			//this.UpdateStatus();
		}

		public override void OnClickCloseButton()
		{
			this.OnHide();
		}

		protected override void OnDestroyBox()
		{
			base.gameObject.SetActive(false);
		}

		public void DestroyItem()
		{
			Transform transform = this.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0);
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}

		//public void UpdateStatus()
		//{
		//	foreach (AchievementItem achievementItem in this.listItem)
		//	{
		//		achievementItem.UpdateItem();
		//	}
		//	this.shopController.UpdateAchievementStatus();
		//}

		public List<AchievementItem> listItem;

		public Offline_ShopController shopController;
	}
}
