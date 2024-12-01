// @sonhg: class: BombOffline.AchievementItem
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	public class AchievementItem : MonoBehaviour
	{
		private void Start()
		{
			this.UpdateItem();
			this.bonusButton.onClick.AddListener(delegate()
			{
				int num = DataManager.AchievementCount(this.id);
				int index = DataManager.AchievementProgess(this.id);
				//List<Achievement> list = Achievements.listAchievement[this.id];
				//if (num >= list[index].total)
				//{
				//	DataManager.PlusGold(list[index].bonus);
				//	this.shopController.UpdateCoin();
				//	DataManager.AchievementProgess(this.id, DataManager.AchievementProgess(this.id) + 1);
				//	this.UpdateItem();
				//}
				//this.shopController.UpdateAchievementStatus();
			});
		}

		private void Update()
		{
		}

		public void UpdateItem()
		{
			int num = DataManager.AchievementCount(this.id);
			int num2 = DataManager.AchievementProgess(this.id);
			//List<Achievement> list = Achievements.listAchievement[this.id];
			//if (num2 >= list.Count)
			//{
			//	base.gameObject.SetActive(false);
			//	return;
			//}
			//this.achievementName.text = list[num2].name;
			//this.description.text = list[num2].description;
			//this.processText.text = num + "/" + list[num2].total;
			//this.bonus.text = list[num2].bonus + string.Empty;
			//float num3 = (float)num / (float)list[num2].total;
			//this.processFill.fillAmount = num3;
			//this.percentText.text = Mathf.RoundToInt(num3 * 100f) + "%";
			//if (num2 <= list.Count && num >= list[num2].total)
			//{
			//	this.bonusButton.gameObject.SetActive(true);
			//	this.processPanel.SetActive(false);
			//}
			//else
			//{
			//	this.bonusButton.gameObject.SetActive(false);
			//	this.processPanel.SetActive(true);
			//}
		}

		public string id;

		public Text achievementName;

		public Text description;

		public Text processText;

		public Text percentText;

		public Text bonus;

		public Button bonusButton;

		public GameObject processPanel;

		public Image processFill;

		public Offline_ShopController shopController;
	}
}
