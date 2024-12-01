// @sonhg: class: BombOffline.AchievementBox
using System;
using System.Collections.Generic;

namespace BombOffline
{
	public class AchievementBox : BaseBox
	{
		private void Awake()
		{
			this.UpdateStatus();
		}

		public override void OnClickCloseButton()
		{
			this.OnHide();
		}

		protected override void OnDestroyBox()
		{
			base.gameObject.SetActive(false);
		}

		public void UpdateStatus()
		{
			foreach (AchievementItem achievementItem in this.listItem)
			{
				achievementItem.UpdateItem();
			}
			this.shopController.UpdateAchievementStatus();
		}

		public List<AchievementItem> listItem;

		public Offline_ShopController shopController;
	}
}
