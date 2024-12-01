// @sonhg: class: Bomb.MapController
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Bomb
{
	public class MapController : MonoBehaviour
	{
		public BombGameScene Scene
		{
			get
			{
				if (this._bombScene == null)
				{
					this._bombScene = base.GetComponent<BombGameScene>();
				}
				return this._bombScene;
			}
		}

		private void Start()
		{
			this.destroyableObjectCount = 0;
		}

		public List<BombModel> GetBombList()
		{
			return this.bombList;
		}

		public void SetForeGround(int id)
		{
		}

		public void LoadMap(string[][] gameMap, string[][] background)
		{
			this.gameMap = gameMap;
			this.LoadMapResource(gameMap, background);
			this.itemMap = new Transform[gameMap[0].Length, gameMap.Length];
			this.tileMap = new Transform[gameMap[0].Length, gameMap.Length];
			this.backgroundMap = new Transform[gameMap[0].Length, gameMap.Length];
			for (int i = 0; i < gameMap.Length; i++)
			{
				for (int j = 0; j < gameMap[i].Length; j++)
				{
					this.AddMapUnit(j, i, gameMap[i][j]);
					this.AddItemUnit(j, i, gameMap[i][j]);
					this.AddBackground(j, i, background[i][j]);
				}
			}
			this.LoadItemsFromCache();
			this.DeacviteItem();
			foreach (object obj in this.mapHolder)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeSelf)
				{
					string[] array = transform.name.Split(new char[]
					{
						'-'
					});
					this.aStarGameManager.addWall(int.Parse(array[0]), int.Parse(array[1]));
				}
			}
		}

		private void AddItemUnit(int x, int y, string type)
		{
			if (type.Contains("-"))
			{
				string[] array = type.Split(new char[]
				{
					'-'
				});
				if (ResourcesUltis.ItemIdToLink(array[1]) != null && ResourcesUltis.ItemIdToLink(array[1]) != string.Empty)
				{
					string empty = string.Empty;
					string text = ResourcesUltis.ItemIdToLink(array[1]).Replace(ResourceChecking.BaseIp(), string.Empty);
					text = text.Substring(0, text.Length - 4);
					Texture2D texture2D = Resources.Load<Texture2D>("Textures/" + text);
					Transform transform = this.FindObjectInHolder(this.itemHolder, x + "-" + y);
					if (texture2D == null)
					{
						transform.tag = "MissingTexture";
					}
					else
					{
						this.MakeItem(this.itemPrefab, transform, TextureCutter.TextureToSprite(texture2D));
					}
				}
			}
		}

		private void AddMapUnit(int x, int y, string type)
		{
			string text = string.Empty;
			if (!type.Contains("-"))
			{
				text = type;
			}
			else
			{
				string[] array = type.Split(new char[]
				{
					'-'
				});
				text = array[0];
			}
			Transform transform = this.FindObjectInHolder(this.mapHolder, x.ToString() + "-" + y.ToString());
			if (transform.GetComponent<SpriteRenderer>() != null)
			{
				transform.GetComponent<SpriteRenderer>().sortingOrder = 100 - 2 * y;
			}
			transform.GetComponent<BoxCollider2D>().enabled = false;
			transform.gameObject.SetActive(true);
			if (ResourcesUltis.TileIdToLink(text) != null && ResourcesUltis.TileIdToLink(text) != string.Empty)
			{
				Sprite sprite = null;
				string key = ResourcesUltis.TileIdToLink(text).Replace(ResourceChecking.BaseIp(), string.Empty);
				if (ResourcesManager.SpriteList.ContainsKey(key))
				{
					sprite = ResourcesManager.SpriteList[key];
				}
				if (sprite == null)
				{
					transform.tag = "MissingTexture";
				}
				else
				{
					transform.GetComponent<SpriteRenderer>().sprite = sprite;
					this.CheckTypeOfColliderById(ResourcesUltis.TileIdToType(text), transform);
					if (ResourcesManager.TilesDict[text].Category >= 11 && ResourcesManager.TilesDict[text].Category < 20)
					{
						transform.tag = "Destroyable";
						this.destroyableObjectCount++;
					}
				}
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
			this.tileMap[x, y] = transform;
		}

		private void AddBackground(int x, int y, string type)
		{
			Transform transform = this.FindObjectInHolder(this.backgroundHolder, x + "-" + y);
			if (ResourcesUltis.TileIdToLink(type) != null && ResourcesUltis.TileIdToLink(type) != string.Empty)
			{
				Sprite sprite = null;
				string key = ResourcesUltis.TileIdToLink(type).Replace(ResourceChecking.BaseIp(), string.Empty);
				if (ResourcesManager.SpriteList.ContainsKey(key))
				{
					sprite = ResourcesManager.SpriteList[key];
				}
				if (sprite == null)
				{
					transform.tag = "MissingTexture";
				}
				else
				{
					transform.GetComponent<SpriteRenderer>().sprite = sprite;
				}
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
			this.backgroundMap[x, y] = transform;
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
				string text = this.gameMap[int.Parse(array[1])][int.Parse(array[0])];
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
							if (ResourcesManager.TilesDict[text].Category >= 11 && ResourcesManager.TilesDict[text].Category < 20)
							{
								transform.tag = "Destroyable";
								this.destroyableObjectCount++;
							}
						}
						else
						{
							transform.gameObject.SetActive(true);
							transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/tileds/default_tile");
							this.CheckTypeOfColliderById(ResourcesUltis.TileIdToType(text), transform);
							if (ResourcesManager.TilesDict[text].Category >= 11 && ResourcesManager.TilesDict[text].Category < 20)
							{
								transform.tag = "Destroyable";
								this.destroyableObjectCount++;
							}
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
							if (ResourcesManager.TilesDict[array2[1]].Category >= 11 && ResourcesManager.TilesDict[array2[1]].Category < 20)
							{
								transform.tag = "Destroyable";
								this.destroyableObjectCount++;
							}
						}
						else
						{
							transform.gameObject.SetActive(true);
							transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/tileds/default_tile");
							this.CheckTypeOfColliderById(ResourcesUltis.TileIdToType(text), transform);
							if (ResourcesManager.TilesDict[array2[1]].Category >= 11 && ResourcesManager.TilesDict[array2[1]].Category < 20)
							{
								transform.tag = "Destroyable";
								this.destroyableObjectCount++;
							}
						}
					}
				}
			}
		}

		private void LoadItemsFromCache()
		{
			foreach (object obj in this.itemHolder)
			{
				Transform transform = (Transform)obj;
				if (transform.tag.Contains("MissingTexture"))
				{
					string[] array = transform.name.Split(new char[]
					{
						'-'
					});
					string text = this.gameMap[int.Parse(array[1])][int.Parse(array[0])];
					string key = string.Empty;
					if (!text.Contains("-"))
					{
						if (!string.IsNullOrEmpty(ResourcesUltis.ItemIdToLink(text)))
						{
							key = ResourcesUltis.ItemIdToLink(text).Replace(ResourceChecking.BaseIp(), string.Empty);
							if (ResourcesManager.SpriteList.ContainsKey(key))
							{
								this.MakeItem(this.itemPrefab, transform, ResourcesManager.SpriteList[key]);
							}
						}
					}
					else
					{
						string[] array2 = text.Split(new char[]
						{
							'-'
						});
						if (!string.IsNullOrEmpty(ResourcesUltis.ItemIdToLink(array2[1])))
						{
							key = ResourcesUltis.ItemIdToLink(array2[1]).Replace(ResourceChecking.BaseIp(), string.Empty);
							if (ResourcesManager.SpriteList.ContainsKey(key))
							{
								this.MakeItem(this.itemPrefab, transform, ResourcesManager.SpriteList[key]);
							}
						}
					}
				}
			}
		}

		private void DeacviteItem()
		{
			for (int i = 0; i < this.gameMap.Length; i++)
			{
				for (int j = 0; j < this.gameMap[i].Length; j++)
				{
					if (this.tileMap[j, i] != null && this.tileMap[j, i].gameObject.activeSelf && this.itemMap[j, i] != null)
					{
						this.itemMap[j, i].gameObject.SetActive(false);
					}
				}
			}
		}

		private ItemController MakeItem(ItemController prefab, Transform itemContainer, Sprite sprite)
		{
			ItemController itemController = UnityEngine.Object.Instantiate(prefab, itemContainer.position, Quaternion.identity) as ItemController;
			itemController.transform.SetParent(itemContainer);
			if (sprite != null)
			{
				itemController.GetComponent<SpriteRenderer>().sprite = sprite;
			}
			string[] array = itemContainer.name.Split(new char[]
			{
				'-'
			});
			int num = Convert.ToInt32(array[0]);
			int num2 = Convert.ToInt32(array[1]);
			this.itemMap[num, num2] = itemController.transform;
			return itemController;
		}

		public void ExplodeBomb(Vector3 position)
		{
			int num = Mathf.RoundToInt(position.x);
			int num2 = Mathf.RoundToInt(position.y);
			BombModel bombModel = null;
			foreach (BombModel bombModel2 in this.bombList)
			{
				if (bombModel2.position.x == (float)num && bombModel2.position.y == (float)num2)
				{
					bombModel = bombModel2;
				}
			}
			if (bombModel != null)
			{
				this.bombList.Remove(bombModel);
				if (MMOUserUtils.IsBot(MMOUserUtils.GetUserByID(bombModel.userId)))
				{
					foreach (GameObject gameObject in base.GetComponent<GameController>().listBotObjects)
					{
						if (gameObject != null && gameObject.GetComponent<BotStupidController>() != null && gameObject.GetComponent<BotStupidController>().userID == bombModel.userId)
						{
							gameObject.GetComponent<BotStupidController>().botAI.canPlaceBomb = true;
						}
					}
				}
				this.Scene.ParticlesController.PlayBombParticle(num, num2);
				this.DrawFire(bombModel);
				int layerMask = 1 << LayerMask.NameToLayer("Character") | 1 << LayerMask.NameToLayer("Wall");
				RaycastHit2D raycastHit2D = Physics2D.Raycast(bombModel.bomb.transform.position, base.transform.right, (float)bombModel.length + 0.1f, layerMask);
				if (raycastHit2D.collider == null || !raycastHit2D.collider.CompareTag("Player"))
				{
					raycastHit2D = Physics2D.Raycast(bombModel.bomb.transform.position, -base.transform.right, (float)bombModel.length + 0.1f, layerMask);
				}
				if (raycastHit2D.collider == null || !raycastHit2D.collider.CompareTag("Player"))
				{
					raycastHit2D = Physics2D.Raycast(bombModel.bomb.transform.position, base.transform.up, (float)bombModel.length + 0.1f, layerMask);
				}
				if (raycastHit2D.collider == null || !raycastHit2D.collider.CompareTag("Player"))
				{
					raycastHit2D = Physics2D.Raycast(bombModel.bomb.transform.position, -base.transform.up, (float)bombModel.length + 0.1f, layerMask);
				}
				if (raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player"))
				{
					BaseCharactersController component = raycastHit2D.collider.GetComponent<BaseCharactersController>();
					if (!(component is EnemyController))
					{
						component.GetHit(bombModel);
					}
				}
				UnityEngine.Object.Destroy(bombModel.bomb);
			}
		}

		private void DrawFire(BombModel bombModel)
		{
			FireLazer fireLazer = UnityEngine.Object.Instantiate(this.firePrefab, bombModel.position, Quaternion.identity) as FireLazer;
			fireLazer.DrawFireDirection(bombModel.length);
			UnityEngine.Object.Destroy(fireLazer.gameObject, 0.7f);
		}

		public bool DestroyBricks(int x, int y)
		{
			Transform brickObject = this.FindObjectInHolder(x, y);
			if (brickObject.gameObject.activeInHierarchy)
			{
				SpriteRenderer sprite = brickObject.GetComponent<SpriteRenderer>();
				float duration = (!(this.itemMap[x, y] != null)) ? 0.15f : 0.1f;
				int loops = (!(this.itemMap[x, y] != null)) ? 1 : 2;
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
					this.destroyableObjectCount--;
					if (this.itemMap[x, y] != null)
					{
						this.itemMap[x, y].gameObject.SetActive(true);
					}
				});
				return true;
			}
			return false;
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

		public int MyBombCount()
		{
			int num = 0;
			foreach (BombModel bombModel in this.bombList)
			{
				if (bombModel.isMine)
				{
					num++;
				}
			}
			return num;
		}

		public BombModel PlaceBomb(GameObject bombPrefab, int bombLength, Vector3 position, bool isMine, int userId, int bombID)
		{
			MusicManager.instance.PlaySingle(this.releaseBomb, 1f);
			int num = Mathf.RoundToInt(position.x);
			int num2 = Mathf.RoundToInt(position.y);
			foreach (BombModel bombModel in this.bombList)
			{
				if (bombModel.position.x == (float)num && bombModel.position.y == (float)num2)
				{
					return null;
				}
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(bombPrefab, new Vector3((float)num, (float)num2, 0f), Quaternion.identity) as GameObject;
			gameObject.GetComponent<BombScript>().SetSprite(bombID);
			BombModel bombModel2 = new BombModel();
			bombModel2.position = new Vector3((float)num, (float)num2, 0f);
			bombModel2.bomb = gameObject;
			bombModel2.length = bombLength;
			bombModel2.userId = userId;
			bombModel2.isMine = isMine;
			this.bombList.Add(bombModel2);
			this.addedBomb = true;
			this.aStarGameManager.addWall(num, num2);
			return bombModel2;
		}

		public void RemoveBomb(int x, int y)
		{
			foreach (BombModel bombModel in this.bombList)
			{
				if (bombModel.position.x == (float)x && bombModel.position.y == (float)y)
				{
					this.bombList.Remove(bombModel);
					UnityEngine.Object.Destroy(bombModel.bomb);
					break;
				}
			}
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
				transform2.tag = "Untagged";
				transform2.DestroyChildren();
			}
			foreach (object obj3 in this.backgroundHolder)
			{
				Transform transform3 = (Transform)obj3;
				transform3.tag = "Untagged";
				transform3.DestroyChildren();
			}
			this.bombList.ForEach(delegate(BombModel item)
			{
				UnityEngine.Object.Destroy(item.bomb);
			});
			this.bombList.Clear();
			this.isDoomMode = false;
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

		public void ActiveDoomMode()
		{
			if (!this.isDoomMode)
			{
				this.Scene.StartCoroutine(this.GenerateDeathTile());
				this.isDoomMode = true;
			}
		}

		public void DeactiveDoomMode()
		{
			if (this.isDoomMode)
			{
				this.isDoomMode = false;
				base.StopAllCoroutines();
			}
		}

		private IEnumerator GenerateDeathTile()
		{
			int i = this.gameMap.Length;
			int j = this.gameMap[0].Length;
			int[,] map = new int[j, i];
			for (int k = 0; k < j; k++)
			{
				for (int l = 0; l < i; l++)
				{
					map[k, l] = 1;
				}
			}
			Vector2 current = Vector2.zero;
			Vector2 next = Vector2.up;
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"(int)current.x, (int)current.y",
				(int)current.x,
				" ---- ",
				(int)current.y,
				"aaa: ",
				map[(int)current.x, (int)current.y]
			}));
			while (map[(int)current.x, (int)current.y] == 1)
			{
				this.MakeItem(this.deathItemPrefab, this.backgroundMap[(int)current.x, (int)current.y], null);
				this.DestroyBricks((int)current.x, (int)current.y);
				this.DestroyItems((int)current.x, (int)current.y);
				map[(int)current.x, (int)current.y] = 0;
				Vector2 temp = current + next;
				bool valid = true;
				if (temp.x >= (float)j || temp.x < 0f || temp.y >= (float)i || temp.y < 0f)
				{
					valid = false;
				}
				else
				{
					int t = map[(int)temp.x, (int)temp.y];
					if (t != 1)
					{
						valid = false;
					}
				}
				if (valid)
				{
					current = temp;
				}
				else
				{
					if (next == Vector2.right)
					{
						next = Vector2.down;
					}
					else if (next == Vector2.down)
					{
						next = Vector2.left;
					}
					else if (next == Vector2.left)
					{
						next = Vector2.up;
					}
					else
					{
						next = Vector2.right;
					}
					current += next;
				}
				yield return new WaitForSeconds(0.4f);
			}
			yield break;
		}

		private void LoadMapResource(string[][] gameMap, string[][] background)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < gameMap.Length; i++)
			{
				for (int j = 0; j < gameMap[i].Length; j++)
				{
					string id = gameMap[i][j].Split(new char[]
					{
						'-'
					})[0];
					if (ResourcesUltis.TileIdToLink(id) != null && ResourcesUltis.TileIdToLink(id) != string.Empty)
					{
						string item = ResourcesUltis.TileIdToLink(id).Replace(ResourceChecking.BaseIp(), string.Empty);
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
					string id2 = background[i][j];
					if (ResourcesUltis.TileIdToLink(id2) != null && ResourcesUltis.TileIdToLink(id2) != string.Empty)
					{
						string item2 = ResourcesUltis.TileIdToLink(id2).Replace(ResourceChecking.BaseIp(), string.Empty);
						if (!list.Contains(item2))
						{
							list.Add(item2);
						}
					}
				}
				this.LoadSpriteInAtlas(list);
			}
		}

		private void LoadSpriteInAtlas(List<string> urlList)
		{
			Dictionary<string, SpritePool> dictionary = new Dictionary<string, SpritePool>();
			foreach (string text in urlList)
			{
				if (!ResourcesManager.SpriteList.ContainsKey(text))
				{
					string[] array = text.Split(new char[]
					{
						'/'
					});
					string text2 = array[array.Length - 2];
					string name = array[array.Length - 1].Split(new char[]
					{
						'.'
					})[0];
					SpritePool spritePool;
					if (!dictionary.ContainsKey(text2))
					{
						spritePool = SpritePool.GetAtlasByName(text2);
						dictionary.Add(text2, spritePool);
					}
					else
					{
						spritePool = dictionary[text2];
					}
					Sprite value = spritePool.FindByName(name);
					ResourcesManager.SpriteList.Add(text, value);
				}
			}
			dictionary.Clear();
		}

		private BombGameScene _bombScene;

		[Header("Prefab")]
		public ItemController itemPrefab;

		public ItemController deathItemPrefab;

		public FireLazer firePrefab;

		public GameObject mapUnitPrefab;

		[Header("Holder")]
		public Transform mapHolder;

		public Transform itemHolder;

		public Transform backgroundHolder;

		public SpriteRenderer foregroundHolder;

		[Header("Sound")]
		public AudioClip releaseBomb;

		public GameManager aStarGameManager;

		private List<BombModel> bombList = new List<BombModel>();

		public string[][] gameMap;

		private Transform[,] itemMap;

		private Transform[,] tileMap;

		private Transform[,] backgroundMap;

		public bool addedBomb;

		private string[] names;

		public int destroyableObjectCount;

		private bool isDoomMode;
	}
}
