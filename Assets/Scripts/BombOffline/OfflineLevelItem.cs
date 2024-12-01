// @sonhg: class: BombOffline.OfflineLevelItem
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	public class OfflineLevelItem : MonoBehaviour
	{
		public void SetLevel(string level)
		{
			this.levelText.text = level;
			this.level = level;
		}

		public int ShowStar()
		{
			int star = OfflineMapChooser.CurrentZoneProgress.GetStar(this.level);
			for (int i = 0; i < star; i++)
			{
				this.starList[i].DoColor(Color.white, 0.5f).SetDelay((float)i * 0.5f + 0.9f);
			}
			return star;
		}

		public void LoadLevel()
		{
			OfflineMapChooser.CurrentLevel = this.level;
			Application.LoadLevelAsync("OfflineMainScene");
		}

		public void Unblock()
		{
			this.button.interactable = true;
			this.lockImage.DoColor(new Color(1f, 1f, 1f, 0f), 0.5f).SetDelay(1f);
		}

		public void Block()
		{
			this.button.interactable = false;
		}

		public string level;

		[SerializeField]
		private List<Image> starList;

		[SerializeField]
		private Image lockImage;

		[SerializeField]
		private Text levelText;

		[SerializeField]
		private Button button;
	}
}
