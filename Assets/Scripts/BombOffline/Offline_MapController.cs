// @sonhg: class: BombOffline.Offline_MapController
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace BombOffline
{
	public class Offline_MapController : Offline_BaseMapController
	{
		public Offline_BombScene Scene
		{
			get
			{
				if (this._bombScene == null)
				{
					this._bombScene = base.GetComponent<Offline_BombScene>();
				}
				return this._bombScene;
			}
		}

		private void Awake()
		{
            if (string.IsNullOrEmpty(OfflineMapChooser.CurrentLevel))
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("OfflineMainMenu");
			}
			else
			{
				this.checker = base.GetComponent<ResourceChecking>();
				this.LoadBomb();
				base.StartCoroutine(this.LoadResourceFormXML("Levels/Bomber/" + OfflineMapChooser.CurrentZone + "/" + OfflineMapChooser.CurrentLevel));
			}
		}

		protected override IEnumerator LoadMapFromXML(string filePath)
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
			ISFSArray monsters = this.jsonObject.GetSFSArray("monsters");
			if (monsters != null)
			{
				for (int i = 0; i < monsters.Count; i++)
				{
					ISFSObject monsterObject = monsters.GetSFSObject(i);
					int id = monsterObject.GetInt("id");
					int x2 = monsterObject.GetInt("x");
					int y2 = monsterObject.GetInt("y");
					Monster monster = ResourcesManager.MonsterDict[id.ToString()].Copy();
					monster.AppearTime = 0f;
					if (monsterObject.ContainsKey("appearTime"))
					{
						monster.AppearTime = (float)monsterObject.GetInt("appearTime");
					}
					if (monster.Id == 16 || monster.Id == 17)
					{
						monster.Position = new Vector2((float)x2, (float)y2 + 0.25f);
					}
					else
					{
						monster.Position = new Vector2((float)x2, (float)y2);
					}
					this.Scene.GameController.monsterListPath.Add(monster);
				}
			}
			string[][] mapbackground = new string[height][];
			ISFSArray mapbackgroundSFS = this.jsonObject.GetSFSArray("background");
			if (mapbackgroundSFS != null)
			{
				for (int y3 = 0; y3 < height; y3++)
				{
					string[] rows2 = mapbackgroundSFS.GetUtfString(y3).Split(new char[]
					{
						','
					});
					mapbackground[height - 1 - y3] = new string[rows2.Length];
					for (int x3 = 0; x3 < rows2.Length; x3++)
					{
						mapbackground[height - 1 - y3][x3] = rows2[x3];
						if (!this.TilesUsed.ContainsKey(rows2[x3]) && ResourcesManager.TilesDict.ContainsKey(rows2[x3]))
						{
							this.TilesUsed.Add(rows2[x3], ResourcesManager.TilesDict[rows2[x3]]);
						}
					}
				}
			}
			IEnumerable<Item> itemDrop = from p in ResourcesManager.ItemsDict.Values
			where p.Category == 1 || p.Category == 2
			select p;
			foreach (Item item in itemDrop)
			{
				if (!this.ItemsUsed.ContainsKey(item.Id.ToString()))
				{
					this.ItemsUsed.Add(item.Id.ToString(), ResourcesManager.ItemsDict[item.Id.ToString()]);
				}
			}
			ISFSArray items = this.jsonObject.GetSFSArray("items");
			for (int j = 0; j < items.Size(); j++)
			{
				ISFSObject item2 = items.GetSFSObject(j);
				int id2 = item2.GetInt("id");
				int min = item2.GetInt("min");
				int max = item2.GetInt("max");
				int randomNum = UnityEngine.Random.Range(min, max);
				for (int k = 0; k < randomNum; k++)
				{
					if (brick_destroyable.Count == 0)
					{
						break;
					}
					int index = UnityEngine.Random.Range(0, brick_destroyable.Count);
					int[] position = brick_destroyable[index];
					int x4 = position[0];
					int y4 = position[1];
					map[x4][y4] = map[x4][y4] + "-" + id2;
					brick_destroyable.Remove(position);
				}
			}
			string[] door = this.jsonObject.GetUtfString("door").Split(new char[]
			{
				','
			});
			int doorX = int.Parse(door[0]);
			int doorY = int.Parse(door[1]);
			this.Scene.GameController.door.toPosition = new Vector2((float)doorX, (float)doorY);
			List<int[]> positiontList = new List<int[]>();
			ISFSArray posArr = this.jsonObject.GetSFSArray("positions");
			if (posArr != null)
			{
				for (int l = 0; l < posArr.Count; l++)
				{
					string[] temp = posArr.GetUtfString(l).Split(new char[]
					{
						','
					});
					int X = int.Parse(temp[0].Trim());
					int Y = int.Parse(temp[1].Trim());
					positiontList.Add(new int[]
					{
						X,
						Y
					});
				}
			}
			int[] currentPos = positiontList.GetRandomElement<int[]>();
			this.Scene.GameController.player.transform.position = new Vector3((float)currentPos[0], (float)currentPos[1], 0f);
			this.gameMapArray = map;
			this.background = mapbackground;
			yield break;
		}

		protected override IEnumerator SetInfoFromResource()
		{
			if (this.jsonObject.ContainsKey("foreground"))
			{
				int foreground = this.jsonObject.GetInt("foreground");
				base.SetForeGround(foreground);
			}
			this.LoadMap(this.gameMapArray, this.background);
			float maxTime = (float)this.jsonObject.GetInt("matchTime");
			int maxScore = this.jsonObject.GetInt("maxScore");
			this.Scene.GameController.fiveStarScore = maxScore;
			this.Scene.GameController.StartClock(maxTime);
			this.loadingBackground.SetActive(false);
			this.foregroundhasAnim.SetActive(true);
			this.Scene.GameController.InitPlayerStat();
			this.Scene.GameController.StartGame();
			yield break;
		}

		public void ExplodeBomb(BombModel bombModel)
		{
			int num = Mathf.RoundToInt(bombModel.position.x);
			int num2 = Mathf.RoundToInt(bombModel.position.y);
			if (bombModel != null)
			{
				if (bombModel.isMine)
				{
					this.playerController.DegreeBomb();
				}
				this.Scene.ParticlesController.PlayBombParticle(num, num2);
				if (bombModel.bomb != null)
				{
					BombScript component = bombModel.bomb.GetComponent<BombScript>();
					component.DestroyBomb();
				}
				this.mapTiled[num, num2].Explode();
			}
		}

		public bool ChangeBombPosition(BombModel bomb, Vector3 newPosition)
		{
			Vector3 position = bomb.position;
			Offline_Tiled offline_Tiled = this.mapTiled[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)];
			Offline_Tiled offline_Tiled2 = this.mapTiled[Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y)];
			if (offline_Tiled2.bomb != null)
			{
				return false;
			}
			bomb.position = newPosition;
			offline_Tiled2.bomb = bomb;
			offline_Tiled.bomb = null;
			return true;
		}

		public BombModel PlaceBomb(GameObject bombPrefab, int bombLength, Vector3 position, bool isMine, int userId, int? bombID, bool autoExplode = true)
		{
			int num = Mathf.RoundToInt(position.x);
			int num2 = Mathf.RoundToInt(position.y);
			if (!this.mapTiled[num, num2].CanPlaceBomb())
			{
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(bombPrefab, new Vector3((float)num, (float)num2, 0f), Quaternion.identity) as GameObject;
			BombModel bombModel = new BombModel();
			bombModel.position = new Vector3((float)num, (float)num2, 0f);
			bombModel.bomb = gameObject;
			bombModel.length = bombLength;
			bombModel.userId = userId;
			bombModel.isMine = isMine;
			BombScript component = gameObject.GetComponent<BombScript>();
			component.board = this.Scene.GameController;
			if (bombID != null)
			{
				component.SetSprite(bombID.Value);
			}
			Offline_Bomb offline_Bomb = gameObject.AddComponent<Offline_Bomb>();
			offline_Bomb.scene = this.Scene;
			offline_Bomb.bomb = bombModel;
			offline_Bomb.autoDestroy = autoExplode;
			this.mapTiled[num, num2].PlaceBomb(bombModel);
			return bombModel;
		}

		public bool CanPlaceBomb(Vector3 position)
		{
			int num = Mathf.RoundToInt(position.x);
			int num2 = Mathf.RoundToInt(position.y);
			return this.mapTiled[num, num2].CanPlaceBomb();
		}

		public void LoadMap(string[][] gameMap, string[][] background)
		{
			this.gameMap = gameMap;
			this.itemMap = new Transform[gameMap[0].Length, gameMap.Length];
			this.tileMap = new Transform[gameMap[0].Length, gameMap.Length];
			this.mapTiled = new Offline_Tiled[gameMap[0].Length, gameMap.Length];
			this.backgroundMap = new Transform[gameMap[0].Length, gameMap.Length];
			this.mapHolder.DestroyChildren();
			this.itemHolder.DestroyChildren();
			for (int i = 0; i < gameMap.Length; i++)
			{
				for (int j = 0; j < gameMap[i].Length; j++)
				{
					string text = gameMap[i][j];
					this.AddMapUnit(j, i, text);
					this.mapTiled[j, i] = new Offline_Tiled(text, 0);
					if (text.Contains("-"))
					{
						this.AddItemUnit(j, i, text);
					}
					text = background[i][j];
					this.AddBackground(j, i, text);
				}
			}
			this.Scene.GameController.GenMonster();
			this.DeacviteItem();
			this.HighLightItem();
		}

		public void AddItemUnit(int x, int y, string type)
		{
			string key = type.Split(new char[]
			{
				'-'
			})[1];
			Item item = ResourcesManager.ItemsDict[key];
			if (item != null && !string.IsNullOrEmpty(item.Path))
			{
				GameObject gameObject = new GameObject();
				Transform transform = base.GenObjectInHolder(this.itemHolder, gameObject, x, y);
				UnityEngine.Object.Destroy(gameObject);
				if (ResourcesManager.SpriteList.ContainsKey(item.Path))
				{
					base.MakeItem(this.itemPrefab, transform, ResourcesManager.SpriteList[item.Path], item);
				}
				else
				{
					transform.gameObject.SetActive(false);
				}
			}
		}

		public void BossDropItem(int xBoss, int yBoss, int x, int y, string id)
		{
			Item item = ResourcesManager.ItemsDict[id];
			if (item != null && !string.IsNullOrEmpty(item.Path))
			{
				GameObject gameObject = new GameObject();
				Transform itemTrans = base.GenObjectInHolder(this.itemHolder, gameObject, x, y);
				itemTrans.localScale = Vector3.zero;
				UnityEngine.Object.Destroy(gameObject);
				itemTrans.position = new Vector3((float)xBoss, (float)yBoss);
				if (ResourcesManager.SpriteList.ContainsKey(item.Path))
				{
					Offline_ItemController offline_ItemController = base.MakeItem(this.itemPrefab, itemTrans, ResourcesManager.SpriteList[item.Path], item);
					this.mapTiled[x, y].SetItem(id);
					BoxCollider2D collider = offline_ItemController.gameObject.GetComponent<BoxCollider2D>();
					SpriteRenderer sprite = offline_ItemController.gameObject.GetComponent<SpriteRenderer>();
					sprite.sortingLayerName = "Particle";
					collider.enabled = false;
					itemTrans.DOScale(1f, 0.7f);
					itemTrans.DOJump(new Vector3((float)x, (float)y), 1f, 1, 1f, false).OnComplete(delegate
					{
						collider.enabled = true;
						itemTrans.gameObject.layer = 10;
						sprite.sortingLayerName = "Default";
					});
				}
				else
				{
					itemTrans.gameObject.SetActive(false);
				}
			}
		}

		private void AddMapUnit(int x, int y, string type)
		{
			string text = type.Split(new char[]
			{
				'-'
			})[0];
			Transform transform = base.GenObjectInHolder(this.mapHolder, this.mapUnitPrefab, x, y);
			transform.GetComponent<BoxCollider2D>().enabled = false;
			transform.gameObject.SetActive(true);
			Tile tile = null;
			if (ResourcesManager.TilesDict.ContainsKey(text))
			{
				tile = ResourcesManager.TilesDict[text];
			}
			if (tile != null && !string.IsNullOrEmpty(tile.Path))
			{
				if (ResourcesManager.SpriteList.ContainsKey(tile.Path))
				{
					transform.GetComponent<SpriteRenderer>().sprite = ResourcesManager.SpriteList[tile.Path];
                    if (tile.Id == 54)
                    {
						transform.position += new Vector3(1.5f, 1f, 0);
						transform.GetComponent<BoxCollider2D>().offset = new Vector2(-1, -0.7f);
					}
					else
                    {
						transform.position += new Vector3(0.5f, 0.4f, 0);
						transform.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.08f);
					}
					this.CheckTypeOfColliderById(ResourcesUltis.TileIdToType(text), transform);
					int category = tile.Category;
					switch (category)
					{
					case 21:
						transform.tag = "NonDestroyable";
						break;
					default:
						if (category == 11)
						{
							transform.tag = "Destroyable";
						}
						break;
					case 23:
						transform.tag = "Pit";
						break;
					}
				}
				else
				{
					transform.gameObject.SetActive(false);
					UnityEngine.Debug.LogError("Khong load dc Sprice");
				}
				transform.GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * y;
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
			this.tileMap[x, y] = transform;
		}

		private void AddBackground(int x, int y, string type)
		{
			Tile tile = ResourcesManager.TilesDict[type];
			if (tile != null && !string.IsNullOrEmpty(tile.Path) && ResourcesManager.SpriteList.ContainsKey(tile.Path))
			{
				Transform transform = base.GenObjectInHolder(this.backgroundHolder, this.backGroundUnitPrefab, x, y);
				transform.GetComponent<SpriteRenderer>().sprite = ResourcesManager.SpriteList[tile.Path];
				this.backgroundMap[x, y] = transform;
			}
		}

		protected void CheckTypeOfColliderById(int _id, Transform _object)
		{
			BoxCollider2D component = _object.gameObject.GetComponent<BoxCollider2D>();
			if (11 <= _id && _id <= 20)
			{
				component.enabled = true;
				_object.gameObject.layer = LayerMask.NameToLayer("Wall");
			}
			else if (21 <= _id && _id <= 30)
			{
				component.enabled = true;
				_object.gameObject.layer = LayerMask.NameToLayer("Wall");
			}
			else
			{
				component.enabled = false;
			}
		}

		private Transform FindObjectInHolder(int x, int y)
		{
			return this.tileMap[x, y];
		}

		private Transform FindObjectInHolder(Transform parent, string _name)
		{
			Transform result = null;
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if (transform.name == _name)
				{
					result = transform;
				}
			}
			return result;
		}

		private Transform FindObjectInItemHolder(int x, int y)
		{
			return this.FindObjectInHolder(this.itemHolder, x.ToString() + "-" + y.ToString());
		}

		private void LoadTileFromCache()
		{
			foreach (object obj in this.mapHolder)
			{
				Transform transform = (Transform)obj;
				string[] array = transform.name.Split(new char[]
				{
					'-'
				});
				string text = this.gameMapArray[int.Parse(array[1])][int.Parse(array[0])];
				if (!text.Contains("-"))
				{
					string text2 = ResourcesUltis.TileIdToLink(text);
					if (!string.IsNullOrEmpty(text2))
					{
						string key = text2.Replace(ResourceChecking.BaseIp(), string.Empty);
						if (ResourcesManager.SpriteList.ContainsKey(key))
						{
							transform.gameObject.SetActive(true);
							transform.GetComponent<SpriteRenderer>().sprite = ResourcesManager.SpriteList[key];
							this.CheckTypeOfColliderById(ResourcesUltis.TileIdToType(text), transform);
						}
					}
				}
				else
				{
					string[] array2 = text.Split(new char[]
					{
						'-'
					});
					string text3 = ResourcesUltis.TileIdToLink(array2[0]);
					if (!string.IsNullOrEmpty(text3))
					{
						transform.gameObject.SetActive(true);
						string key2 = text3.Replace(ResourceChecking.BaseIp(), string.Empty);
						if (ResourcesManager.SpriteList.ContainsKey(key2))
						{
							transform.GetComponent<SpriteRenderer>().sprite = ResourcesManager.SpriteList[key2];
							this.CheckTypeOfColliderById(ResourcesUltis.TileIdToType(array2[0]), transform);
						}
					}
				}
			}
		}

		private void DeacviteItem()
		{
			for (int i = 0; i < this.gameMapArray.Length; i++)
			{
				for (int j = 0; j < this.gameMapArray[i].Length; j++)
				{
					if (this.tileMap[j, i] != null && this.tileMap[j, i].gameObject.activeSelf && this.itemMap[j, i] != null)
					{
						this.itemMap[j, i].gameObject.SetActive(false);
					}
				}
			}
		}

		private Offline_ItemController MakeItem(Offline_ItemController prefab, Transform itemContainer, Sprite sprite)
		{
			Offline_ItemController offline_ItemController = UnityEngine.Object.Instantiate(prefab, itemContainer.position, Quaternion.identity) as Offline_ItemController;
			offline_ItemController.transform.SetParent(itemContainer);
			SpriteRenderer component = offline_ItemController.GetComponent<SpriteRenderer>();
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
			component.sortingOrder = 100 - 2 * num2;
			return offline_ItemController;
		}

		public void ResetMap()
		{
			foreach (object obj in this.mapHolder)
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(false);
				transform.tag = "Untagged";
				transform.GetComponent<BoxCollider2D>().enabled = false;
				SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
				component.sprite = null;
				component.color = Color.white;
			}
			foreach (object obj2 in this.itemHolder)
			{
				Transform transform2 = (Transform)obj2;
				List<GameObject> list = new List<GameObject>();
				transform2.tag = "Untagged";
				foreach (object obj3 in transform2)
				{
					Transform transform3 = (Transform)obj3;
					list.Add(transform3.gameObject);
				}
				list.ForEach(delegate(GameObject child)
				{
					UnityEngine.Object.Destroy(child);
				});
			}
			foreach (object obj4 in this.backgroundHolder)
			{
				Transform transform4 = (Transform)obj4;
				List<GameObject> list2 = new List<GameObject>();
				transform4.tag = "Untagged";
				foreach (object obj5 in transform4)
				{
					Transform transform5 = (Transform)obj5;
					list2.Add(transform5.gameObject);
				}
				list2.ForEach(delegate(GameObject child)
				{
					UnityEngine.Object.Destroy(child);
				});
			}
			Offline_Tiled[,] mapTiled = this.mapTiled;
			int length = mapTiled.GetLength(0);
			int length2 = mapTiled.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					Offline_Tiled offline_Tiled = mapTiled[i, j];
					if (offline_Tiled.bomb != null)
					{
						UnityEngine.Object.Destroy(offline_Tiled.bomb.bomb);
					}
				}
			}
		}

		private void MakeBorderWall()
		{
			for (int i = 0; i < this.gameMapArray.Length; i++)
			{
				Transform transform = this.tileMap[0, i];
				if (transform != null)
				{
					this.ConvertToBorderWall(transform);
				}
				transform = this.tileMap[this.gameMapArray[0].Length - 1, i];
				if (transform != null)
				{
					this.ConvertToBorderWall(transform);
				}
			}
			for (int j = 0; j < this.gameMapArray[0].Length; j++)
			{
				Transform transform2 = this.tileMap[j, 0];
				if (transform2 != null)
				{
					this.ConvertToBorderWall(transform2);
				}
				transform2 = this.tileMap[j, this.gameMapArray.Length - 1];
				if (transform2 != null)
				{
					this.ConvertToBorderWall(transform2);
				}
			}
		}

		private void ConvertToBorderWall(Transform t)
		{
			t.tag = "BorderWall";
			t.gameObject.layer = LayerMask.NameToLayer("BorderWall");
			t.GetComponent<SpriteRenderer>().sprite = null;
			t.GetComponent<BoxCollider2D>().enabled = true;
		}

		public void ToggleActive(GameObject go)
		{
			if (go.activeSelf)
			{
				go.SetActive(false);
			}
			else
			{
				go.SetActive(true);
			}
		}

		private void DrawFire(BombModel bombModel)
		{
			FireLazer fireLazer = UnityEngine.Object.Instantiate(this.firePrefab, bombModel.position, Quaternion.identity) as FireLazer;
			fireLazer.DrawFireDirection(bombModel.length);
			UnityEngine.Object.Destroy(fireLazer.gameObject, 0.7f);
		}

		public void DrawFireManual(Vector3 position, GameObject prefab = null)
		{
			float time = 0.7f;
			if (prefab == null)
			{
				prefab = this.firePrefab.firePrefab;
			}
			else if (ObjectPoolManager.Instance.GetPool(prefab.name) == null)
			{
				ObjectPoolManager.Instance.CreatePool(prefab.name, new ObjectPool(prefab, 10, null, null));
			}
			GameObject go = ObjectPoolManager.Instance.GetPool(prefab.name).Spawn(position, Quaternion.identity);
			base.StartCoroutine(this.ClearFireManual(time, go, prefab.name));
		}

		private IEnumerator ClearFireManual(float time, GameObject go, string poolName)
		{
			yield return new WaitForSeconds(time);
			ObjectPoolManager.Instance.GetPool(poolName).Destroy(go);
			yield break;
		}

		public bool DestroyBricks(int x, int y)
		{
			Transform brickObject = this.FindObjectInHolder(x, y);
			if (brickObject.gameObject.activeInHierarchy)
			{
				SpriteRenderer sprite = brickObject.GetComponent<SpriteRenderer>();
				float duration = (!(this.itemMap[x, y] != null)) ? 0.15f : 0.1f;
				int loops = (!(this.itemMap[x, y] != null)) ? 1 : 2;
				bool canDropcoin = this.mapTiled[x, y].IsBrick();
				Sequence s = DOTween.Sequence();
				s.Append(DOTween.To(() => sprite.color, delegate(Color color)
				{
					sprite.color = color;
				}, Color.red, duration)).Append(DOTween.To(() => sprite.color, delegate(Color color)
				{
					sprite.color = color;
				}, Color.yellow, duration)).SetLoops(loops).OnComplete(delegate
				{
					brickObject.gameObject.SetActive(false);
					if (this.itemMap[x, y] != null)
					{
						this.ActiveItem(this.itemMap[x, y]);
					}
					else if (canDropcoin)
					{
						if (UnityEngine.Random.Range(0, 2) != 0)
						{
							this.DropItem(x, y, "2");
						}
						else
						{
							this.DropItem(x, y, "3");
						}
					}
				});
				return true;
			}
			if (this.itemMap[x, y] != null)
			{
				this.ActiveItem(this.itemMap[x, y]);
			}
			return false;
		}

		public bool CanDestroyBricks(int x, int y)
		{
			Transform transform = this.FindObjectInHolder(x, y);
			return transform.gameObject.activeInHierarchy;
		}

		public void DestroyItems(int x, int y)
		{
			Transform transform = this.FindObjectInItemHolder(x, y);
			if (transform != null && transform.childCount > 0)
			{
				Transform child = transform.GetChild(0);
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}

		public void HighLightItem()
		{
			for (int i = 0; i < this.gameMapArray.Length; i++)
			{
				for (int j = 0; j < this.gameMapArray[i].Length; j++)
				{
					if (this.tileMap[j, i] != null && this.itemMap[j, i] != null)
					{
						this.Scene.ParticlesController.PlayHighLightParticle(this.tileMap[j, i]);
					}
				}
			}
		}

		public ItemType GetRandomItem()
		{
			List<ItemType> list = new List<ItemType>();
			list.Add(ItemType.SHIELD);
			list.Add(ItemType.HASTE);
			list.Add(ItemType.RADAR);
			if (OfflineMapChooser.CurrentZone.CompareTo("forest") == 0)
			{
				list.Add(ItemType.SNARE);
			}
			else if (OfflineMapChooser.CurrentZone.CompareTo("frozen") == 0)
			{
				list.Add(ItemType.AUTO_FIRE);
			}
			else if (OfflineMapChooser.CurrentZone.CompareTo("desert") == 0)
			{
				list.Add(ItemType.SLOW);
			}
			else if (OfflineMapChooser.CurrentZone.CompareTo("swamp") == 0)
			{
				list.Add(ItemType.REVERSE);
			}
			return list.GetRandomElement<ItemType>();
		}

		private void ActiveItem(Transform item)
		{
			item.gameObject.SetActive(true);
			BoxCollider2D component = item.GetComponent<BoxCollider2D>();
			component.enabled = true;
			item.gameObject.GetComponent<SpriteRenderer>().SetAlpha(1f);
		}

		private void LoadBomb()
		{
			this.firePrefab.firePrefab = this.bombParticleList[OfflineMapChooser.Index];
			ObjectPoolManager.NewInstance.CreatePool(this.firePrefab.firePrefab.name, new ObjectPool(this.firePrefab.firePrefab, 10, null, null));
		}

		public void ItemVisible()
		{
			for (int i = 0; i < this.gameMapArray.Length; i++)
			{
				for (int j = 0; j < this.gameMapArray[i].Length; j++)
				{
					if (this.tileMap[j, i] != null && this.itemMap[j, i] != null && !this.itemMap[j, i].gameObject.activeInHierarchy)
					{
						GameObject gameObject = this.itemMap[j, i].gameObject;
						SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
						if (component != null)
						{
							BoxCollider2D component2 = gameObject.GetComponent<BoxCollider2D>();
							component2.enabled = false;
							gameObject.SetActive(true);
							component.SetAlpha(0.5f);
							base.StartCoroutine(this.ItemInvisible(this.itemMap[j, i].gameObject, gameObject, component, component2));
						}
					}
				}
			}
		}

		private IEnumerator ItemInvisible(GameObject tiled, GameObject item, SpriteRenderer sprite, BoxCollider2D collider)
		{
			yield return new WaitForSeconds(5f);
			if (item != null && tiled != null && !collider.enabled)
			{
				item.SetActive(false);
				sprite.SetAlpha(1f);
			}
			yield break;
		}

		private Offline_BombScene _bombScene;

		[SerializeField]
		[Header("Map prefab")]
		private FireLazer firePrefab;

		[SerializeField]
		private GameObject mapUnitPrefab;

		[SerializeField]
		private GameObject backGroundUnitPrefab;

		[Header("Other")]
		public Offline_PlayerController playerController;

		[SerializeField]
		private GameObject foregroundhasAnim;

		[SerializeField]
		private GameObject loadingBackground;

		[SerializeField]
		private List<GameObject> bombParticleList;

		[HideInInspector]
		private string[][] gameMapArray;

		private string[][] background;
	}
}
