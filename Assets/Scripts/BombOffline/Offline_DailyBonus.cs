// @sonhg: class: BombOffline.Offline_DailyBonus
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BombOffline
{
	public class Offline_DailyBonus : BaseBox
	{
		public void DisableButon()
		{
			foreach (Button button in this.dailyItems)
			{
				button.interactable = false;
			}
		}

		public List<Button> dailyItems;

		public Offline_ShopController shopController;
	}
}
