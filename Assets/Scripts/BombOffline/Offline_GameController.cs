// @sonhg: class: BombOffline.Offline_GameController
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using InControl;
using SettlersEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BombOffline
{
	public class Offline_GameController : MonoBehaviour
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

		public void SettingSelectClassic()
		{
			PlayerPrefs.SetInt("Joystick", 1);
		}

		public void SettingSelectJoyStick()
		{
			PlayerPrefs.SetInt("Joystick", 0);
		}

		public void SlectClassicController()
		{
			PlayerPrefs.SetInt("Joystick", 1);
		}

		public void SlectJoyStickController()
		{
			PlayerPrefs.SetInt("Joystick", 0);
		}

		public void TriggerBomb(Vector3 position)
		{
			int x = Mathf.RoundToInt(position.x);
			int y = Mathf.RoundToInt(position.y);
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			int[,] array = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array2 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array3 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array4 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			BombModel bombModelAt = this.GetBombModelAt(x, y);
			if (bombModelAt != null)
			{
				this.ProcessMetaExplode(mapTiled, x, y, ref array, ref array2, ref array3, ref array4, bombModelAt, true);
			}
			int num = 0;
			List<Offline_BaseMonster> listKill = new List<Offline_BaseMonster>();
			for (int i = 0; i < mapTiled.GetLength(0); i++)
			{
				for (int j = 0; j < mapTiled.GetLength(1); j++)
				{
					if (array2[i, j] == 1)
					{
						this.Scene.MapController.ExplodeBomb(this.GetBombModelAt(i, j));
						this.Scene.MapController.DestroyItems(i, j);
					}
					if (array[i, j] == 1)
					{
						this.Scene.MapController.DestroyBricks(i, j);
						this.Scene.MapController.mapTiled[i, j].Explode();
					}
					if (array3[i, j] == 1)
					{
						this.Scene.MapController.DestroyItems(i, j);
						this.Scene.MapController.mapTiled[i, j].Explode();
					}
					if (array4[i, j] > 0)
					{
						if (this.KillMonsterAt(i, j, ref listKill))
						{
							num++;
						}
						this.HitPlayerAt(i, j);
						this.Scene.MapController.DrawFireManual(new Vector2((float)i, (float)j), null);
					}
				}
			}
			this.PlayKillStreak(listKill);
		}

		public void TriggerSpecialBomb(Vector3 position, int length, GameObject prefab = null)
		{
			int num = Mathf.RoundToInt(position.x - 0.5f);
			int num2 = Mathf.RoundToInt(position.y - 0.5f);
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			int[,] array = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array2 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array3 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array4 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int num3 = Mathf.Max(num - length, 0);
			int num4 = Mathf.Max(num2 - length, 0);
			int num5 = Mathf.Min(num + length, mapTiled.GetLength(0) - 1);
			int num6 = Mathf.Min(num2 + length, mapTiled.GetLength(1) - 1);
			for (int i = num3; i <= num5; i++)
			{
				for (int j = num4; j <= num6; j++)
				{
					Offline_Tiled offline_Tiled = mapTiled[i, j];
					if (offline_Tiled.bomb != null)
					{
						this.ProcessMetaExplode(mapTiled, i, j, ref array, ref array2, ref array3, ref array4, offline_Tiled.bomb, true);
					}
					else if (offline_Tiled.IsBrick())
					{
						array[i, j] = 1;
					}
					else if (offline_Tiled.IsItem())
					{
						array3[i, j] = 1;
					}
					else if (offline_Tiled.IsEmptyTiled())
					{
						array4[i, j]++;
					}
				}
			}
			for (int k = 0; k < mapTiled.GetLength(0); k++)
			{
				for (int l = 0; l < mapTiled.GetLength(1); l++)
				{
					if (array2[k, l] == 1)
					{
						this.Scene.MapController.ExplodeBomb(this.GetBombModelAt(k, l));
						this.Scene.MapController.DestroyItems(k, l);
					}
					if (array[k, l] == 1)
					{
						this.Scene.MapController.DestroyBricks(k, l);
						this.Scene.MapController.mapTiled[k, l].Explode();
					}
					if (array3[k, l] == 1)
					{
						this.Scene.MapController.DestroyItems(k, l);
						this.Scene.MapController.mapTiled[k, l].Explode();
						this.Scene.MapController.DrawFireManual(new Vector3((float)k, (float)l, 0f), prefab);
					}
					if (array4[k, l] > 0)
					{
						this.Scene.MapController.DrawFireManual(new Vector3((float)k, (float)l, 0f), prefab);
						List<Offline_BaseMonster> list = null;
						this.KillMonsterAt(k, l, ref list);
						this.HitPlayerAt(k, l);
					}
				}
			}
		}

		public void TriggerSquareBomb(Vector3 position, int length = 2, GameObject prefab = null)
		{
			int num = Mathf.RoundToInt(position.x);
			int num2 = Mathf.RoundToInt(position.y);
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			int[,] array = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array2 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array3 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array4 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int num3 = Mathf.Max(num - length, 0);
			int num4 = Mathf.Max(num2 - length, 0);
			int num5 = Mathf.Min(num + length, mapTiled.GetLength(0) - 1);
			int num6 = Mathf.Min(num2 + length, mapTiled.GetLength(1) - 1);
			for (int i = num3; i <= num5; i++)
			{
				for (int j = num4; j <= num6; j++)
				{
					if (Math.Abs(num - i) * Math.Abs(num2 - j) < length)
					{
						Offline_Tiled offline_Tiled = mapTiled[i, j];
						if (offline_Tiled.bomb != null)
						{
							this.ProcessMetaExplode(mapTiled, i, j, ref array, ref array2, ref array3, ref array4, offline_Tiled.bomb, true);
						}
						else if (offline_Tiled.IsBrick())
						{
							array[i, j] = 1;
						}
						else if (offline_Tiled.IsItem())
						{
							array3[i, j] = 1;
						}
						else if (offline_Tiled.IsEmptyTiled())
						{
							array4[i, j]++;
						}
					}
				}
			}
			for (int k = 0; k < mapTiled.GetLength(0); k++)
			{
				for (int l = 0; l < mapTiled.GetLength(1); l++)
				{
					if (array2[k, l] == 1)
					{
						this.Scene.MapController.ExplodeBomb(this.GetBombModelAt(k, l));
						this.Scene.MapController.DestroyItems(k, l);
					}
					if (array[k, l] == 1)
					{
						this.Scene.MapController.DestroyBricks(k, l);
						this.Scene.MapController.mapTiled[k, l].Explode();
					}
					if (array3[k, l] == 1)
					{
						this.Scene.MapController.DestroyItems(k, l);
						this.Scene.MapController.mapTiled[k, l].Explode();
						this.Scene.MapController.DrawFireManual(new Vector3((float)k, (float)l, 0f), prefab);
					}
					if (array4[k, l] > 0)
					{
						this.Scene.MapController.DrawFireManual(new Vector3((float)k, (float)l, 0f), prefab);
						List<Offline_BaseMonster> list = null;
						this.KillMonsterAt(k, l, ref list);
						this.HitPlayerAt(k, l);
					}
				}
			}
		}

		public void TriggerCustomBomb(List<Vector3> positionList, Dictionary<string, bool> parameter = null, GameObject prefab = null)
		{
			bool flag = true;
			bool flag2 = true;
			bool flag3 = true;
			bool flag4 = true;
			bool flag5 = true;
			if (parameter != null)
			{
				if (parameter.ContainsKey("isDestroyBomb"))
				{
					flag = parameter["isDestroyBomb"];
				}
				if (parameter.ContainsKey("isDestroyTile"))
				{
					flag2 = parameter["isDestroyTile"];
				}
				if (parameter.ContainsKey("isDestroyItem"))
				{
					flag3 = parameter["isDestroyItem"];
				}
				if (parameter.ContainsKey("isHitMonster"))
				{
					flag4 = parameter["isHitMonster"];
				}
				if (parameter.ContainsKey("isHitPlayer"))
				{
					flag5 = parameter["isHitPlayer"];
				}
			}
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			int[,] array = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array2 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array3 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			int[,] array4 = new int[mapTiled.GetLength(0), mapTiled.GetLength(1)];
			foreach (Vector3 vector in positionList)
			{
				int num = Mathf.RoundToInt(vector.x);
				int num2 = Mathf.RoundToInt(vector.y);
				if (this.ValidateArrayPosition(num, num2))
				{
					Offline_Tiled offline_Tiled = mapTiled[num, num2];
					if (offline_Tiled.bomb != null)
					{
						this.ProcessMetaExplode(mapTiled, num, num2, ref array, ref array2, ref array3, ref array4, offline_Tiled.bomb, flag);
					}
					else if (offline_Tiled.IsBrick())
					{
						array[num, num2] = 1;
					}
					else if (offline_Tiled.IsItem())
					{
						array3[num, num2] = 1;
					}
					else if (offline_Tiled.IsEmptyTiled())
					{
						array4[num, num2]++;
					}
				}
			}
			for (int i = 0; i < mapTiled.GetLength(0); i++)
			{
				for (int j = 0; j < mapTiled.GetLength(1); j++)
				{
					if (array2[i, j] == 1 && flag)
					{
						this.Scene.MapController.ExplodeBomb(this.GetBombModelAt(i, j));
						this.Scene.MapController.DestroyItems(i, j);
					}
					if (array[i, j] == 1 && flag2)
					{
						this.Scene.MapController.DestroyBricks(i, j);
						this.Scene.MapController.mapTiled[i, j].Explode();
					}
					if (array3[i, j] == 1 && flag3)
					{
						this.Scene.MapController.DestroyItems(i, j);
						this.Scene.MapController.mapTiled[i, j].Explode();
						this.Scene.MapController.DrawFireManual(new Vector3((float)i, (float)j, 0f), prefab);
					}
					if (array4[i, j] > 0)
					{
						this.Scene.MapController.DrawFireManual(new Vector3((float)i, (float)j, 0f), prefab);
						if (flag4)
						{
							List<Offline_BaseMonster> list = null;
							this.KillMonsterAt(i, j, ref list);
						}
						if (flag5)
						{
							this.HitPlayerAt(i, j);
						}
					}
				}
			}
		}

		public void StartClock(float maxTime)
		{
			this._maxTime = (int)maxTime;
			this.clock.StartRaise(0f, maxTime, delegate(object x)
			{
				this.ShowExtraLife();
			}, null);
		}

		private void ShowExtraLife()
		{
			if (this.extraHearCount < this.priceList.Length)
			{
				int num = this.priceList[this.extraHearCount];
			}
			this.EnableController(false);
			this.EndGame(false);
		}

		public void AddClockTime(float time)
		{
			this.clock.AddTimes(time);
		}

		public void GenMonster()
		{
			this.monsterList = new List<Offline_BaseMonster>();
			this.PutMonster();
		}

		private void ProcessMetaMark(ref int[,] map, int x, int y, ref int[,] markHit)
		{
			if (!this.ValidateArrayPosition(map, x, y))
			{
				return;
			}
			int num = map[x, y];
			if (num == 0 || num == 4)
			{
				markHit[x, y] = 1;
				map[x, y] = 1;
				this.ProcessMetaMark(ref map, x + 1, y, ref markHit);
				this.ProcessMetaMark(ref map, x - 1, y, ref markHit);
				this.ProcessMetaMark(ref map, x, y + 1, ref markHit);
				this.ProcessMetaMark(ref map, x, y - 1, ref markHit);
			}
		}

		private bool ProcessMetaExplode(Offline_Tiled[,] map, int x, int y, ref int[,] tileHit, ref int[,] bombHit, ref int[,] itemHit, ref int[,] markHit, BombModel bomb = null, bool isTriggerAnotherBomb = true)
		{
			if (!this.ValidateArrayPosition(map, x, y))
			{
				return false;
			}
			bool result = true;
			switch (map[x, y].status)
			{
			case 0:
			case 6:
				if (bomb == null || bomb.isMine)
				{
					markHit[x, y] += 2;
				}
				break;
			case 1:
				result = false;
				break;
			case 2:
				tileHit[x, y] = 1;
				result = false;
				break;
			case 3:
				if (bombHit[x, y] == 0 && isTriggerAnotherBomb)
				{
					bombHit[x, y] = 1;
					if (bomb == null || bomb.isMine)
					{
						markHit[x, y] += 2;
					}
					BombModel bombModelAt = this.GetBombModelAt(x, y);
					for (int i = 1; i <= bombModelAt.length; i++)
					{
						if (!this.ProcessMetaExplode(map, x + i, y, ref tileHit, ref bombHit, ref itemHit, ref markHit, bombModelAt, true))
						{
							break;
						}
					}
					for (int j = 1; j <= bombModelAt.length; j++)
					{
						if (!this.ProcessMetaExplode(map, x - j, y, ref tileHit, ref bombHit, ref itemHit, ref markHit, bombModelAt, true))
						{
							break;
						}
					}
					for (int k = 1; k <= bombModelAt.length; k++)
					{
						if (!this.ProcessMetaExplode(map, x, y + k, ref tileHit, ref bombHit, ref itemHit, ref markHit, bombModelAt, true))
						{
							break;
						}
					}
					for (int l = 1; l <= bombModelAt.length; l++)
					{
						if (!this.ProcessMetaExplode(map, x, y - l, ref tileHit, ref bombHit, ref itemHit, ref markHit, bombModelAt, true))
						{
							break;
						}
					}
				}
				break;
			case 4:
				itemHit[x, y] = 1;
				if (bomb == null || bomb.isMine)
				{
					markHit[x, y] += 2;
				}
				break;
			}
			return result;
		}

		public void GetRandomPositionAt(int x, int y, ref int randomX, ref int randomY)
		{
			int[,] mapInt = this.GetMapInt();
			int[,] array = new int[mapInt.GetLength(0), mapInt.GetLength(1)];
			this.ProcessMetaMark(ref mapInt, x, y, ref array);
			List<Offline_GameController.position> list = new List<Offline_GameController.position>();
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					int num = array[i, j];
					if (num == 1)
					{
						list.Add(new Offline_GameController.position
						{
							x = i,
							y = j
						});
					}
				}
			}
			Offline_GameController.position randomElement = list.GetRandomElement<Offline_GameController.position>();
			randomX = randomElement.x;
			randomY = randomElement.y;
		}

		public void GetRandomPositionAt(ref int randomX, ref int randomY)
		{
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			randomX = UnityEngine.Random.Range(1, mapTiled.GetLength(0));
			randomY = UnityEngine.Random.Range(1, mapTiled.GetLength(1));
		}

		public List<Vector3> GetRandomTilePosition(int amount)
		{
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			List<Offline_GameController.position> list = new List<Offline_GameController.position>();
			for (int i = 0; i < mapTiled.GetLength(0); i++)
			{
				for (int j = 0; j < mapTiled.GetLength(1); j++)
				{
					if (mapTiled[i, j].IsEmptyTiled())
					{
						list.Add(new Offline_GameController.position
						{
							x = i,
							y = j
						});
					}
				}
			}
			List<Vector3> list2 = new List<Vector3>();
			while (list2.Count < amount)
			{
				Offline_GameController.position randomElement = list.GetRandomElement<Offline_GameController.position>();
				list2.Add(new Vector3((float)randomElement.x, (float)randomElement.y));
				list.Remove(randomElement);
			}
			return list2;
		}

		public LinkedList<MapNode> SearchFromTo(int fromX, int fromY, int toX, int toY, Offline_BaseCharactersController go = null)
		{
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			MapNode[,] inGrid = this.CreateMapGrid(mapTiled);
			SpatialAStar<MapNode, Offline_BaseCharactersController> spatialAStar = new SpatialAStar<MapNode, Offline_BaseCharactersController>(inGrid);
			return spatialAStar.Search(new Vector2((float)fromX, (float)fromY), new Vector2((float)toX, (float)toY), go);
		}

		public LinkedList<MapNode> SearchFromToPlayer(int fromX, int fromY, Offline_BaseCharactersController go = null)
		{
			int currentX = this.player.currentX;
			int currentY = this.player.currentY;
			return this.SearchFromTo(fromX, fromY, currentX, currentY, go);
		}

		public bool IsInRanceWithPlayer(int x, int y, float range)
		{
			return (new Vector3((float)x, (float)y, 0f) - this.player.transform.position).magnitude <= range;
		}

		public bool KillMonsterAt(int x, int y, ref List<Offline_BaseMonster> listkillMonster)
		{
			List<Offline_BaseMonster> list = new List<Offline_BaseMonster>();
			bool result = false;
			foreach (Offline_BaseMonster offline_BaseMonster in this.monsterList)
			{
				if (offline_BaseMonster.IsAtPosition(x, y, null, null) && !list.Contains(offline_BaseMonster))
				{
					list.Add(offline_BaseMonster);
				}
			}
			foreach (Offline_BaseMonster offline_BaseMonster2 in list)
			{
				if (offline_BaseMonster2.GetHit(x, y))
				{
					this.monsterList.Remove(offline_BaseMonster2);
					offline_BaseMonster2.DestroyMonster();
					result = true;
					if (listkillMonster != null)
					{
						listkillMonster.Add(offline_BaseMonster2);
					}
					if (this.bossList.Contains(offline_BaseMonster2))
					{
						this.bossList.Remove(offline_BaseMonster2);
						if (this.bossList.Count == 0)
						{
							this.PutDoor();
						}
					}
					if (this.Scene.MapController.mapTiled[x, y].IsEmptyTiled() && offline_BaseMonster2.Drop != null)
					{
						int @int = offline_BaseMonster2.Drop.GetInt("2");
						int int2 = offline_BaseMonster2.Drop.GetInt("3");
						int int3 = offline_BaseMonster2.Drop.GetInt("4");
						if (offline_BaseMonster2.IsBoss)
						{
							int num = UnityEngine.Random.Range(Mathf.Min(@int - 1, 1), @int);
							int num2 = UnityEngine.Random.Range(Mathf.Min(int2 - 1, 1), int2);
							int num3 = UnityEngine.Random.Range(Mathf.Min(int3 - 1, 1), int3);
							List<Vector3> randomTilePosition = this.Scene.GameController.GetRandomTilePosition(num + num2 + num3);
							foreach (Vector3 vector in randomTilePosition)
							{
								int x2 = Mathf.RoundToInt(vector.x);
								int y2 = Mathf.RoundToInt(vector.y);
								if (num > 0)
								{
									this.Scene.MapController.BossDropItem(x, y, x2, y2, "2");
									num--;
								}
								else if (num2 > 0)
								{
									this.Scene.MapController.BossDropItem(x, y, x2, y2, "3");
									num2--;
								}
								else if (num3 > 0)
								{
									this.Scene.MapController.BossDropItem(x, y, x2, y2, "4");
									num3--;
								}
							}
						}
						else if (UnityEngine.Random.Range(0, 100) <= @int)
						{
							this.Scene.MapController.DropItem(x, y, "2");
						}
						else if (UnityEngine.Random.Range(0, 100) <= int2)
						{
							this.Scene.MapController.DropItem(x, y, "3");
						}
						else if (UnityEngine.Random.Range(0, 100) <= int3)
						{
							this.Scene.MapController.DropItem(x, y, "4");
						}
					}
				}
			}
			if (this.monsterList.Count == 0)
			{
				this.PutDoor();
			}
			return result;
		}

		public bool HitPlayerAt(int x, int y)
		{
			if (this.player.currentX == x && this.player.currentY == y)
			{
				this.player.GetHit(null);
				return true;
			}
			return false;
		}

		public void StartGame()
		{
			MusicManager.instance.RandomizeMusic(this.musicIngame.ToArray());
			this.UnlockLevel();
		}

		public void EndGame(bool isWin)
		{
			if (!this.isEndGame)
			{
				this.player.gameObject.SetActive(false);
				this.isEndGame = true;
				UnityEngine.Debug.Log("EndGame " + isWin);
				this.inControlPanel.controlsEnabled = false;
				this.inControlPanel.gameObject.SetActive(false);
				string currentLevel = OfflineMapChooser.CurrentLevel;
				int progessStar = this.clock.GetProgessStar();
				int star = OfflineMapChooser.CurrentZoneProgress.GetStar(currentLevel);
				string[] array = this.clock.GetRemainString().Split(new char[]
				{
					':'
				});
				int num = int.Parse(array[0]) * 60 + int.Parse(array[1]);
				this.endGamePanel.ShowEndGameBoard(this.clock.GetRemainString(), isWin, num * 10, this.monsterKill, this.monsterKillPoint, this.doubleKill, this.doubleKill * 20, this.tripleKill, this.tripleKill * 50, this.ultraKill, this.ultraKill * 100, this.fiveStarScore, this.cacheBombSaveGame);
				this.endGamePanel.gameObject.SetActive(true);
				this.endGamePanel.PlayAnimation(isWin);
				if (isWin)
				{
					MusicManager.instance.PlayOneShot(this._winSound, 1f);
					OfflineMapChooser.CurrentZoneProgress.CurrentZonePoint += Mathf.RoundToInt(this.clock.RemainTime) * 10;
					this.SaveGame(progessStar, currentLevel);
					//Context.googleAnalytics.LogEvent(Analystics.C_IN_GAME, Analystics.A_PASS_LEVEL_SUCCESS, "LEVEL: " + OfflineMapChooser.CurrentLevel, 0L);
				}
				else
				{
					MusicManager.instance.PlayOneShot(this._loseSound, 1f);
					Context.googleAnalytics.LogEvent(Analystics.C_IN_GAME, Analystics.A_PASS_LEVEL_FAILD, "LEVEL: " + OfflineMapChooser.CurrentLevel, 0L);
				}
				this.clock.PauseRaising();
			}
			foreach (Offline_BaseMonster offline_BaseMonster in this.monsterList)
			{
				offline_BaseMonster.canAct = false;
			}
			foreach (Offline_BaseMonster offline_BaseMonster2 in this.bossList)
			{
				offline_BaseMonster2.canAct = false;
			}
			if (BomberAds.play_count >= Offline_Config.OFFLINE_ADMOB_TIME)
			{
				base.StartCoroutine(this.ShowAdmob());
			}
			else
			{
				Time.timeScale = 0f;
			}
			if (Offline_Config.OFFLINE_PLAY_COUNT_TYPE == 0 && !isWin)
			{
				BomberAds.play_count++;
			}
			if (Offline_Config.OFFLINE_PLAY_COUNT_TYPE == 1 && isWin)
			{
				BomberAds.play_count++;
			}
		}

		private IEnumerator ShowAdmob()
		{
			yield return new WaitForSeconds(1.5f);
			BomberAds.ShowAds();
			Time.timeScale = 0f;
			yield break;
		}

		public Offline_BaseMonster PutMonsterAt(int x, int y, Offline_BaseMonster monsterPrefab)
		{
			Offline_BaseMonster offline_BaseMonster = UnityEngine.Object.Instantiate<Offline_BaseMonster>(monsterPrefab);
			offline_BaseMonster.RandomCycle();
			offline_BaseMonster.transform.position = new Vector3((float)x, (float)y, 0f);
			offline_BaseMonster.board = this;
			offline_BaseMonster.target = this.player;
			this.monsterList.Add(offline_BaseMonster);
			return offline_BaseMonster;
		}

		public bool PushBomb(BombModel bomb, MoveDirection pushDirection, bool isInstantTrigger = false)
		{
			int layerMask = 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Bomb") | 1 << LayerMask.NameToLayer("BorderWall") | 1 << LayerMask.NameToLayer("Character") | 1 << LayerMask.NameToLayer("Monster");
			int num = 0;
			RaycastHit2D[] array = Physics2D.RaycastAll(bomb.bomb.transform.position, pushDirection.GetDircetionVector(), 300f, layerMask);
			RaycastHit2D raycastHit2D = array[1];
			if (raycastHit2D.collider != null)
			{
				num = Mathf.FloorToInt(raycastHit2D.distance);
			}
			Offline_Bomb bombController = bomb.bomb.GetComponent<Offline_Bomb>();
			if (num > 0 && !bombController.isKicked)
			{
				bombController.isKicked = true;
				bombController.ResetCount();
				this.Scene.MapController.ChangeBombPosition(bomb, bomb.position + pushDirection.GetDircetionVector() * (float)num);
				bomb.bomb.transform.DOLocalMove(bomb.position, 0.1f * (float)num, false).OnComplete(delegate
				{
					bombController.isKicked = false;
					if (isInstantTrigger)
					{
						this.TriggerBomb(bombController.transform.position);
					}
				});
				return true;
			}
			return false;
		}

		public bool ThrowBomb(BombModel bomb, MoveDirection throwDirection)
		{
			int num = 8;
			Offline_Bomb bombController = bomb.bomb.GetComponent<Offline_Bomb>();
			if (bombController.isThrown)
			{
				return false;
			}
			Vector3[] throwPath = this.GetThrowPath(throwDirection, ref num);
			if (throwPath != null)
			{
				bombController.isThrown = true;
				bombController.ResetCount();
				this.Scene.MapController.ChangeBombPosition(bomb, bomb.position + throwDirection.GetDircetionVector() * (float)num);
				bomb.bomb.transform.DOPath(throwPath, 0.1f * (float)num, PathType.CatmullRom, PathMode.TopDown2D, 10, null).OnComplete(delegate
				{
					bombController.isThrown = false;
				});
				return true;
			}
			return false;
		}

		public void StopAllMonster()
		{
			foreach (Offline_BaseMonster offline_BaseMonster in this.monsterList)
			{
				offline_BaseMonster.canAct = false;
			}
			base.StartCoroutine(this.ResumeAllMonster());
		}

		public Vector3[] GetJumpPath(MoveDirection jumpDirection)
		{
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			int num = 0;
			int num2 = 0;
			switch (jumpDirection)
			{
			case MoveDirection.RIGHT:
				num = 1;
				break;
			case MoveDirection.LEFT:
				num = -1;
				break;
			case MoveDirection.DOWN:
				num2 = -1;
				break;
			case MoveDirection.UP:
				num2 = 1;
				break;
			}
			if (!this.ValidateArrayPosition(mapTiled, this.player.currentX + num, this.player.currentY + num2))
			{
				return null;
			}
			int status = mapTiled[this.player.currentX + num, this.player.currentY + num2].status;
			if (status == 1 || status == 2 || status == 3 || status == 6)
			{
				if (!this.ValidateArrayPosition(mapTiled, this.player.currentX + 2 * num, this.player.currentY + 2 * num2))
				{
					return null;
				}
				status = mapTiled[this.player.currentX + 2 * num, this.player.currentY + 2 * num2].status;
				if (status == 0 || status == 4)
				{
					if (num != 0)
					{
						return DrawCurveUtil.CreateEllipse(1f, 1f, (float)this.player.currentX, (float)this.player.currentY, 8, jumpDirection.GetDircetionVector(), -1f);
					}
					if (num2 > 0)
					{
						return new Vector3[]
						{
							Vector3.up * 2.2f + new Vector3((float)this.player.currentX, (float)this.player.currentY, 0f),
							Vector3.up * 2f + new Vector3((float)this.player.currentX, (float)this.player.currentY, 0f)
						};
					}
					return new Vector3[]
					{
						Vector3.up * 0.2f + new Vector3((float)this.player.currentX, (float)this.player.currentY, 0f),
						Vector3.up * -2f + new Vector3((float)this.player.currentX, (float)this.player.currentY, 0f)
					};
				}
			}
			return null;
		}

		public bool CanDestroyBrick(MoveDirection jumpDirection)
		{
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			int num = 0;
			int num2 = 0;
			switch (jumpDirection)
			{
			case MoveDirection.RIGHT:
				num = 1;
				break;
			case MoveDirection.LEFT:
				num = -1;
				break;
			case MoveDirection.DOWN:
				num2 = -1;
				break;
			case MoveDirection.UP:
				num2 = 1;
				break;
			}
			if (!this.ValidateArrayPosition(mapTiled, this.player.currentX + num, this.player.currentY + num2))
			{
				return false;
			}
			int status = mapTiled[this.player.currentX + num, this.player.currentY + num2].status;
			return status == 2 && this.ValidateArrayPosition(mapTiled, this.player.currentX + 1 * num, this.player.currentY + 1 * num2) && this.Scene.MapController.CanDestroyBricks(this.player.currentX + num, this.player.currentY + num2);
		}

		public bool DestroyBrick(MoveDirection jumpDirection)
		{
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			int num = 0;
			int num2 = 0;
			switch (jumpDirection)
			{
			case MoveDirection.RIGHT:
				num = 1;
				break;
			case MoveDirection.LEFT:
				num = -1;
				break;
			case MoveDirection.DOWN:
				num2 = -1;
				break;
			case MoveDirection.UP:
				num2 = 1;
				break;
			}
			if (!this.ValidateArrayPosition(mapTiled, this.player.currentX + num, this.player.currentY + num2))
			{
				return false;
			}
			int status = mapTiled[this.player.currentX + num, this.player.currentY + num2].status;
			if (status == 2)
			{
				if (!this.ValidateArrayPosition(mapTiled, this.player.currentX + 1 * num, this.player.currentY + 1 * num2))
				{
					return false;
				}
				Transform transform = this.Scene.MapController.tileMap[this.player.currentX + num, this.player.currentY + num2];
				if (transform != null && transform.gameObject.activeInHierarchy)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.shovelsPrefab);
					gameObject.transform.SetParent(this.Scene.MapController.tileMap[this.player.currentX + num, this.player.currentY + num2]);
					gameObject.transform.localScale = Vector3.one * 1.5f;
					gameObject.transform.localPosition = Vector3.right / 2f;
				}
				if (this.Scene.MapController.DestroyBricks(this.player.currentX + num, this.player.currentY + num2))
				{
					return true;
				}
			}
			return false;
		}

		public Vector3[] GetThrowPath(MoveDirection throwDirection, ref int length)
		{
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			int num = 0;
			int num2 = 0;
			switch (throwDirection)
			{
			case MoveDirection.RIGHT:
				num = 1;
				break;
			case MoveDirection.LEFT:
				num = -1;
				break;
			case MoveDirection.DOWN:
				num2 = -1;
				break;
			case MoveDirection.UP:
				num2 = 1;
				break;
			}
			while (!this.ValidateArrayPosition(mapTiled, this.player.currentX + num * length, this.player.currentY + num2 * length))
			{
				length--;
				if (length == 0)
				{
					return null;
				}
			}
			int status = mapTiled[this.player.currentX + num * length, this.player.currentY + num2 * length].status;
			while (status != 0 && status != 4)
			{
				length--;
				status = mapTiled[this.player.currentX + num * length, this.player.currentY + num2 * length].status;
				if (length == 0)
				{
					return null;
				}
			}
			if (status != 0 && status != 4)
			{
				return null;
			}
			if (num != 0)
			{
				return DrawCurveUtil.CreateEllipse((float)length / 2f, 1.5f, (float)this.player.currentX, (float)this.player.currentY, 8, throwDirection.GetDircetionVector(), -1f);
			}
			if (num2 > 0)
			{
				return new Vector3[]
				{
					Vector3.up * ((float)length + 0.2f) + new Vector3((float)this.player.currentX, (float)this.player.currentY, 0f),
					Vector3.up * (float)length + new Vector3((float)this.player.currentX, (float)this.player.currentY, 0f)
				};
			}
			return new Vector3[]
			{
				Vector3.up * 0.2f + new Vector3((float)this.player.currentX, (float)this.player.currentY, 0f),
				Vector3.up * (float)(-(float)length) + new Vector3((float)this.player.currentX, (float)this.player.currentY, 0f)
			};
		}

		public BombModel GetBombModelAt(int x, int y)
		{
			return this.Scene.MapController.mapTiled[x, y].bomb;
		}

		public BombModel GetBombModelAt(Vector3 position)
		{
			return this.Scene.MapController.mapTiled[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)].bomb;
		}

		public void RemoveBomb(Vector3 position)
		{
			this.Scene.MapController.mapTiled[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)].bomb = null;
		}

		public void InitPlayerStat()
		{
			if (OfflineMapChooser.CurrentZoneProgress.IsPassed)
			{
				OfflineMapChooser.CurrentZoneProgress = BombSaveGame.LoadZoneProgressCache(OfflineMapChooser.CurrentZone);
			}
			this.cacheBombSaveGame = BombSaveGame.Copy(OfflineMapChooser.CurrentZoneProgress);
			int[] id = new int[]
			{
				PlayerPrefs.GetInt("PlayerHead", 53),
				PlayerPrefs.GetInt("PlayerBody", 57),
				PlayerPrefs.GetInt("PlayerHair", 49)
			};
			this.SetSkinForPlayer(id, this.player.transform, TextureCutter.Parse("FFFFFF"));
			this.player.TotalBomb = OfflineMapChooser.CurrentZoneProgress.TotalBomb;
			this.player.CurrentBombLength = OfflineMapChooser.CurrentZoneProgress.CurrentBombLength;
			this.player.SetMoveSpeed(OfflineMapChooser.CurrentZoneProgress.BaseMoveSpeed);
			this.player.InitItemHelper();
		}

		public void SetSkinForPlayer(int[] _id, Transform _char, Color _hairColor)
		{
			Texture2D[] textureArr = new Texture2D[]
			{
				TextureCutter.GetSkinTexture(_id[0]),
				TextureCutter.GetSkinTexture(_id[1]),
				TextureCutter.GetSkinTexture(_id[2])
			};
			TextureCutter.CutAll(textureArr, _char, _hairColor);
		}

		public Offline_Obstacle PlaceObstacle(Offline_Obstacle obstaclePrefab, Vector3 position, Vector3? eulerAngle = null)
		{
			int num = Mathf.RoundToInt(position.x);
			int num2 = Mathf.RoundToInt(position.y);
			if (!this.ValidateArrayPosition(num, num2))
			{
				return null;
			}
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			if (!(mapTiled[num, num2].obstacle != null))
			{
				if (ObjectPoolManager.Instance.GetPool(obstaclePrefab.name) == null)
				{
					ObjectPoolManager.Instance.CreatePool(obstaclePrefab.name, new ObjectPool(obstaclePrefab.gameObject, 5, null, delegate(GameObject go)
					{
						go.GetComponent<Offline_Obstacle>().Reset();
					}));
				}
				Offline_Obstacle component = ObjectPoolManager.Instance.GetPool(obstaclePrefab.name).Spawn(new Vector3((float)num, (float)num2, 0f), Quaternion.identity).GetComponent<Offline_Obstacle>();
				component.poolName = obstaclePrefab.name;
				if (eulerAngle != null)
				{
					Vector3 value = eulerAngle.Value;
					component.transform.localRotation = Quaternion.FromToRotation(Vector3.up, value);
				}
				component.scene = this.Scene;
				mapTiled[num, num2].obstacle = component;
				return component;
			}
			if (obstaclePrefab.type == mapTiled[num, num2].obstacle.type)
			{
				mapTiled[num, num2].obstacle.Reset();
				return mapTiled[num, num2].obstacle;
			}
			return null;
		}

		public void RemoveObstacle(Offline_Obstacle obstacle)
		{
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			for (int i = 0; i < mapTiled.GetLength(0); i++)
			{
				for (int j = 0; j < mapTiled.GetLength(1); j++)
				{
					if (mapTiled[i, j].obstacle != null && mapTiled[i, j].obstacle == obstacle)
					{
						ObjectPoolManager.Instance.GetPool(mapTiled[i, j].obstacle.poolName).Destroy(mapTiled[i, j].obstacle.gameObject);
						mapTiled[i, j].obstacle = null;
					}
				}
			}
		}

		public void PauseGame(bool isPause)
		{
			if (!this.isEndGame && isPause)
			{
				Time.timeScale = 0f;
				this.EnableController(isPause);
				IngameSettingBox original = Resources.Load<IngameSettingBox>("Prefabs/Bomber/Boxs/GameSettingBox");
				IngameSettingBox ingameSettingBox = UnityEngine.Object.Instantiate<IngameSettingBox>(original);
				ingameSettingBox.transform.SetParent(base.transform, false);
				ingameSettingBox.AddClickEvent(new UnityAction(this.Scene.OnClickExit), new UnityAction(this.Scene.OnClickResetButton));
				ingameSettingBox.OnCloseBox = delegate()
				{
					Time.timeScale = 1f;
					this.EnableController(false);
				};
			}
		}

		public void EnableController(bool isEndable)
		{
			if (isEndable)
			{
				this.inControlPanel.controlsEnabled = false;
			}
			else
			{
				this.inControlPanel.controlsEnabled = true;
				if (PlayerPrefs.GetInt("Joystick", 0) == 1)
				{
					this.inputHandler.InitFixedJoystick();
				}
				else
				{
					PlayerPrefs.SetInt("Joystick", 0);
					this.inputHandler.InitDynamicJoystick();
				}
			}
		}

		private void PlayKillStreak(List<Offline_BaseMonster> listKill)
		{
			int count = listKill.Count;
			this.monsterKill += count;
			DataManager.AchievementCountPlus("MONSTER_KILL", count);
			foreach (Offline_BaseMonster offline_BaseMonster in listKill)
			{
				this.monsterKillPoint += offline_BaseMonster.Point;
				OfflineMapChooser.CurrentZoneProgress.CurrentZonePoint += offline_BaseMonster.Point;
			}
			if (count > 1)
			{
				MusicManager.instance.PlayOneShot(this._streakSound, 2f);
			}
			if (count == 2)
			{
				base.StartCoroutine(this.ShowAndShakeObject(0.2f, this.killingStatus[0]));
				this.doubleKill++;
				DataManager.AchievementCountPlus("DOUBLE_KILL", 1);
				OfflineMapChooser.CurrentZoneProgress.CurrentZonePoint += 20;
			}
			if (count == 3)
			{
				base.StartCoroutine(this.ShowAndShakeObject(0.2f, this.killingStatus[1]));
				this.tripleKill++;
				DataManager.AchievementCountPlus("TRIPLE_KILL", 1);
				OfflineMapChooser.CurrentZoneProgress.CurrentZonePoint += 50;
			}
			if (count >= 4)
			{
				base.StartCoroutine(this.ShowAndShakeObject(0.2f, this.killingStatus[2]));
				this.ultraKill++;
				DataManager.AchievementCountPlus("ULTRA_KILL", 1);
				OfflineMapChooser.CurrentZoneProgress.CurrentZonePoint += 100;
			}
		}

		public void ShowBossName(string name)
		{
			this.bossNameText.text = name;
			base.StartCoroutine(this.ShowAndShakeObject(0.2f, this.bossNameText.gameObject));
		}

		private IEnumerator ShowAndShakeObject(float _time, GameObject _obj)
		{
			_obj.SetActive(true);
			_obj.GetComponent<Animator>().Play("ActionState");
			yield return new WaitForSeconds(_time);
			_obj.transform.DOShakePosition(0.3f, new Vector3(0.5f, 0.5f, 0.5f), 10, 90f, false);
			yield break;
		}

		private void PutMonster()
		{
			if (this.monsterPrefabList.Count > 0)
			{
				Offline_BaseMonster offline_BaseMonster = UnityEngine.Object.Instantiate<Offline_BaseMonster>(this.monsterPrefabList[0]);
				offline_BaseMonster.transform.position = Vector3.one * 5f;
				offline_BaseMonster.board = this;
				offline_BaseMonster.target = this.player;
				this.monsterList.Add(offline_BaseMonster);
			}
			this.bossList = new List<Offline_BaseMonster>();
			bool flag = true;
			foreach (Monster monster in this.monsterListPath)
			{
				Offline_BaseMonster original = Resources.Load<Offline_BaseMonster>(monster.Path);
				Offline_BaseMonster offline_BaseMonster2 = UnityEngine.Object.Instantiate<Offline_BaseMonster>(original);
				flag = !flag;
				offline_BaseMonster2.isUpdateCycle = flag;
				offline_BaseMonster2.transform.position = monster.Position;
				offline_BaseMonster2.board = this;
				offline_BaseMonster2.target = this.player;
				if (!monster.IsSpecial)
				{
					this.monsterList.Add(offline_BaseMonster2);
				}
				offline_BaseMonster2.IsBoss = monster.IsBoss;
				if (monster.IsBoss)
				{
					this.bossList.Add(offline_BaseMonster2);
				}
				offline_BaseMonster2.gameObject.SetActive(false);
				offline_BaseMonster2.Point = monster.Point;
				offline_BaseMonster2.Drop = monster.Drop;
				base.StartCoroutine(this.ShowMonster(offline_BaseMonster2, monster.AppearTime));
			}
		}

		private IEnumerator ShowMonster(Offline_BaseMonster monster, float time)
		{
			yield return new WaitForSeconds(time);
			monster.ShowMonster();
			yield break;
		}

		private bool ValidateArrayPosition(int[,] map, int x, int y)
		{
			return x >= 0 && x <= map.GetLength(0) - 1 && y >= 0 && y <= map.GetLength(1) - 1;
		}

		public bool ValidateArrayPosition(int x, int y)
		{
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			return this.ValidateArrayPosition(mapTiled, x, y);
		}

		private bool ValidateArrayPosition(Offline_Tiled[,] map, int x, int y)
		{
			return x >= 0 && x <= map.GetLength(0) - 1 && y >= 0 && y <= map.GetLength(1) - 1;
		}

		private MapNode[,] CreateMapGrid(Offline_Tiled[,] map)
		{
			MapNode[,] array = new MapNode[map.GetLength(0), map.GetLength(1)];
			for (int i = 0; i < map.GetLength(0); i++)
			{
				for (int j = 0; j < map.GetLength(1); j++)
				{
					int status = map[i, j].status;
					array[i, j] = new MapNode(i, j);
					if (status == 1 || status == 2 || status == 3 || status == 5 || status == 6)
					{
						array[i, j].IsWall = true;
					}
					else
					{
						array[i, j].IsWall = false;
					}
					if (status == 5)
					{
						array[i, j].IsBorderWall = true;
					}
				}
			}
			return array;
		}

		private IEnumerator ResumeAllMonster()
		{
			yield return new WaitForSeconds(4f);
			foreach (Offline_BaseMonster monster in this.monsterList)
			{
				monster.canAct = true;
			}
			yield break;
		}

		private void SaveGame(int currentStar, string currentLevel)
		{
			int star = OfflineMapChooser.CurrentZoneProgress.GetStar(currentLevel);
			if (star < currentStar)
			{
				OfflineMapChooser.CurrentZoneProgress.SetStar(currentLevel, currentStar);
			}
			OfflineMapChooser.CurrentZoneProgress.TotalBomb = this.player.TotalBomb;
			OfflineMapChooser.CurrentZoneProgress.CurrentBombLength = this.player.CurrentBombLength;
			OfflineMapChooser.CurrentZoneProgress.BaseMoveSpeed = this.player.baseMoveSpeed;
			BombSaveGame.SaveZoneProgress();
		}

		private void UnlockLevel()
		{
			string nextLevel = OfflineMapChooser.GetNextLevel();
			if (!string.IsNullOrEmpty(nextLevel))
			{
				OfflineMapChooser.CurrentZoneProgress.CurrentLevel = nextLevel;
				if (OfflineMapChooser.CanUnlockNextZone())
				{
					OfflineMapChooser.isNextZone = true;
				}
			}
			else
			{
				BombSaveGame.SaveZoneProgressCache(this.cacheBombSaveGame);
				BombSaveGame.ReportScore();
			}
		}

		private int[,] GetMapInt()
		{
			string[][] gameMap = this.Scene.MapController.gameMap;
			Offline_Tiled[,] mapTiled = this.Scene.MapController.mapTiled;
			int[,] array = new int[gameMap[0].Length, gameMap.Length];
			for (int i = 0; i < gameMap.Length; i++)
			{
				for (int j = 0; j < gameMap[i].Length; j++)
				{
					array[j, i] = mapTiled[j, i].status;
				}
			}
			return array;
		}

		public void PutDoor()
		{
			this.door.enabled = true;
		}

		public Offline_PlayerController player;

		[Header("Monster Prefabs")]
		public List<Offline_BaseMonster> monsterPrefabList;

		private List<Offline_BaseMonster> monsterList;

		[SerializeField]
		[Header("Music")]
		private List<AudioClip> musicIngame;

		[SerializeField]
		private AudioClip _winSound;

		[SerializeField]
		private AudioClip _loseSound;

		[SerializeField]
		private AudioClip _streakSound;

		[Header("Containers")]
		[SerializeField]
		private OfflineEndGamePanel endGamePanel;

		[SerializeField]
		private OfflineClockController clock;

		[SerializeField]
		private GameObject[] killingStatus;

		[SerializeField]
		private GameObject selectControllerPanel;

		[SerializeField]
		private Offline_InputHandler inputHandler;

		[SerializeField]
		private TouchManager inControlPanel;

		[SerializeField]
		private Text bossNameText;

		[SerializeField]
		public DoorSpawn door;

		[HideInInspector]
		public int fiveStarScore;

		[HideInInspector]
		public BombSaveGame cacheBombSaveGame;

		private bool isEndGame;

		private int _maxTime;

		public List<Monster> monsterListPath = new List<Monster>();

		private List<Offline_BaseMonster> bossList;

		private int monsterKill;

		private int monsterKillPoint;

		private int doubleKill;

		private int tripleKill;

		private int ultraKill;

		public ExtraLifeBox extraLifeBox;

		public ConfirmBox confirmBox;

		public GameObject shovelsPrefab;

		private Offline_BombScene _bombScene;

		private int[] priceList = new int[]
		{
			500,
			1000
		};

		private int extraHearCount;

		private struct position
		{
			public int x;

			public int y;
		}
	}
}
