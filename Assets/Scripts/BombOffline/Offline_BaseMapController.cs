// @sonhg: class: BombOffline.Offline_BaseMapController
using System;
using System.Collections;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace BombOffline
{
	public class Offline_BaseMapController : MonoBehaviour
	{
		public void DropItem(int x, int y, string id)
		{
			Item item = ResourcesManager.ItemsDict[id];
			if (item != null && !string.IsNullOrEmpty(item.Path))
			{
				GameObject gameObject = new GameObject();
				Transform transform = this.GenObjectInHolder(this.itemHolder, gameObject, x, y);
				UnityEngine.Object.Destroy(gameObject);
				if (ResourcesManager.SpriteList.ContainsKey(item.Path))
				{
					Offline_ItemController offline_ItemController = this.MakeItem(this.itemPrefab, transform, ResourcesManager.SpriteList[item.Path], item);
					this.mapTiled[x, y].SetItem(id);
				}
				else
				{
					transform.gameObject.SetActive(false);
				}
			}
		}

		protected Transform GenObjectInHolder(Transform parent, GameObject unit, float x, float y)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(unit);
			gameObject.name = x + "-" + y;
			gameObject.transform.SetParent(parent, false);
			gameObject.transform.localPosition = new Vector3((float)x, (float)y, 0f);
			return gameObject.transform;
		}

		protected Offline_ItemController MakeItem(Offline_ItemController prefab, Transform itemContainer, Sprite sprite, Item item)
		{
			Offline_ItemController offline_ItemController = UnityEngine.Object.Instantiate(prefab, itemContainer.position, Quaternion.identity) as Offline_ItemController;
			ISFSObject isfsobject = SFSObject.NewFromJsonData(item.Data);
			SpriteRenderer component = offline_ItemController.GetComponent<SpriteRenderer>();
			if (isfsobject.GetKeys().Length > 0)
			{
				offline_ItemController.type = isfsobject.GetKeys()[0].ToEnum<ItemType>();
				offline_ItemController.value = isfsobject.GetInt(isfsobject.GetKeys()[0]);
			}
			offline_ItemController.transform.SetParent(itemContainer);
			if (sprite != null)
			{
				offline_ItemController.GetComponent<SpriteRenderer>().sprite = sprite;
			}
			string[] array = itemContainer.name.Split(new char[]
			{
				'-'
			});
			int num = Convert.ToInt32(array[0]);
			int num2 = Convert.ToInt32(array[1]);
			this.itemMap[num, num2] = offline_ItemController.transform;
			component.sortingOrder = 101 - 2 * num2;
			return offline_ItemController;
		}

		protected virtual IEnumerator LoadResourceFormXML(string filePath)
		{
			yield return base.StartCoroutine(this.LoadMapFromXML(filePath));
			yield return base.StartCoroutine(this.LoadResource());
			yield return base.StartCoroutine(this.SetInfoFromResource());
			yield break;
		}

		protected virtual IEnumerator LoadMapFromXML(string filePath)
		{
			TextAsset txt = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
			string text = txt.text;
			this.jsonObject = SFSObject.NewFromJsonData(text);
			int width = this.jsonObject.GetInt("width");
			int height = this.jsonObject.GetInt("height");
			string[][] map = new string[height][];
			List<int[]> brick_destroyable = new List<int[]>();
			ISFSArray mapSFS = this.jsonObject.GetSFSArray("map");
			for (int y = 0; y < height; y++)
			{
				string[] rows = mapSFS.GetUtfString(y).Split(new char[]
				{
					','
				});
				map[height - 1 - y] = new string[rows.Length];
				for (int x = 0; x < rows.Length; x++)
				{
					map[height - 1 - y][x] = rows[x];
					if (!this.TilesUsed.ContainsKey(rows[x]) && ResourcesManager.TilesDict.ContainsKey(rows[x]))
					{
						this.TilesUsed.Add(rows[x], ResourcesManager.TilesDict[rows[x]]);
					}
					if (!rows[x].Contains("-") && ResourcesManager.TilesDict.ContainsKey(rows[x]) && ResourcesManager.TilesDict[rows[x]].Category == 11)
					{
						brick_destroyable.Add(new int[]
						{
							height - 1 - y,
							x
						});
					}
				}
			}
			if (this.jsonObject.ContainsKey("foreground"))
			{
				string foreground = this.jsonObject.GetInt("foreground") + string.Empty;
				this.TilesUsed.Add(foreground, ResourcesManager.TilesDict[foreground]);
			}
			string[][] mapbackground = new string[height][];
			ISFSArray mapbackgroundSFS = this.jsonObject.GetSFSArray("background");
			if (mapbackgroundSFS != null)
			{
				for (int y2 = 0; y2 < height; y2++)
				{
					string[] rows2 = mapbackgroundSFS.GetUtfString(y2).Split(new char[]
					{
						','
					});
					mapbackground[height - 1 - y2] = new string[rows2.Length];
					for (int x2 = 0; x2 < rows2.Length; x2++)
					{
						mapbackground[height - 1 - y2][x2] = rows2[x2];
						if (!this.TilesUsed.ContainsKey(rows2[x2]) && ResourcesManager.TilesDict.ContainsKey(rows2[x2]))
						{
							this.TilesUsed.Add(rows2[x2], ResourcesManager.TilesDict[rows2[x2]]);
						}
					}
				}
			}
			yield return 0;
			yield break;
		}

		protected IEnumerator LoadResource()
		{
			foreach (Item _item in this.ItemsUsed.Values)
			{
				string url = _item.Path;
				if (!string.IsNullOrEmpty(url))
				{
					yield return base.StartCoroutine(this.checker.CheckThenFindSprite(url));
				}
			}
			List<string> urlList = new List<string>();
			foreach (Tile _item2 in this.TilesUsed.Values)
			{
				string url2 = _item2.Path;
				if (!string.IsNullOrEmpty(url2))
				{
					urlList.Add(url2);
				}
			}
			this.checker.LoadSpriteInAtlas(urlList);
			yield break;
		}

		protected virtual IEnumerator SetInfoFromResource()
		{
			yield break;
		}

		protected void SetForeGround(int id)
		{
			Tile tile = ResourcesManager.TilesDict[id + string.Empty];
			Sprite sprite = ResourcesManager.SpriteList[tile.Path];
			this.backGroundRender.sprite = sprite;
		}

		[Header("Holder")]
		public Transform mapHolder;

		public Transform itemHolder;

		public Transform backgroundHolder;

		[Header("Prefab")]
		public Offline_ItemController itemPrefab;

		[Header("Other Reference")]
		[SerializeField]
		protected SpriteRenderer backGroundRender;

		public Offline_Tiled[,] mapTiled;

		public string[][] gameMap;

		public Transform[,] tileMap;

		protected Transform[,] itemMap;

		protected Transform[,] backgroundMap;

		protected ResourceChecking checker;

		protected ISFSObject jsonObject;

		protected Dictionary<string, Tile> TilesUsed = new Dictionary<string, Tile>();

		protected Dictionary<string, Item> ItemsUsed = new Dictionary<string, Item>();
	}
}
