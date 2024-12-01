// @sonhg: class: BombOffline.Offline_DailyBonusItem
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	public class Offline_DailyBonusItem : MonoBehaviour
	{
		private void Start()
		{
			this.button = base.GetComponent<Button>();
		}

		private void Update()
		{
		}

		public void OnItemClick()
		{
			IEnumerable<Item> enumerable = from x in ResourcesManager.ItemsDict.Values
			where x.Category > 2
			select x;
			enumerable = enumerable.Skip(UnityEngine.Random.Range(0, enumerable.Count<Item>() - 5)).Take(5);
			int index = UnityEngine.Random.Range(0, enumerable.Count<Item>());
			base.StartCoroutine(this.Spin(enumerable));
			this.j = 0;
			this.i = 20;
			this.daiybonusController.DisableButon();
			string[] array = DataManager.GetMyItem().Split(new char[]
			{
				'-'
			});
			foreach (string text in array)
			{
				if (text.Equals(enumerable.ElementAt(index).Id))
				{
					return;
				}
			}
			DataManager.SetMyItem(DataManager.GetMyItem() + "-" + enumerable.ElementAt(index).Id);
			this.button.image.sprite = Resources.Load<Sprite>("Textures/" + enumerable.ElementAt(index).Icon.Substring(0, enumerable.ElementAt(index).Icon.Length - 4));
		}

		private IEnumerator Spin(IEnumerable<Item> item)
		{
			while (this.i > 0)
			{
				yield return new WaitForSeconds(0.1f);
				this.button.image.sprite = Resources.Load<Sprite>("Textures/" + item.ElementAt(this.j).Icon.Substring(0, item.ElementAt(this.j).Icon.Length - 4));
				this.j++;
				if (this.j > 4)
				{
					this.j = 0;
				}
				this.i--;
			}
			yield break;
		}

		public Offline_DailyBonus daiybonusController;

		private Button button;

		private int i = 20;

		private int j;
	}
}
