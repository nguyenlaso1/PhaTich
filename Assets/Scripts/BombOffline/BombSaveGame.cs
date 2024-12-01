// @sonhg: class: BombOffline.BombSaveGame
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	[Serializable]
	public class BombSaveGame : SaveGame
	{
		public string CurrentLevel { get; set; }

		public Dictionary<string, int> CurrentStar { get; set; }

		public int TotalBomb { get; set; }

		public int CurrentBombLength { get; set; }

		public float BaseMoveSpeed { get; set; }

		public int GetStar(string level)
		{
			if (!this.CurrentStar.ContainsKey(level))
			{
				return 0;
			}
			return this.CurrentStar[level];
		}

		public void SetStar(string level, int currentStar)
		{
			if (this.CurrentStar.ContainsKey(level))
			{
				this.CurrentStar[level] = currentStar;
			}
			else
			{
				this.CurrentStar.Add(level, currentStar);
			}
		}

		public static BombSaveGame CreateNewSaveGame(string saveName, int bestScore = 0)
		{
			BombSaveGame bombSaveGame = new BombSaveGame();
			bombSaveGame.CurrentStar = new Dictionary<string, int>();
			bombSaveGame.TotalBomb = 1;
			bombSaveGame.CurrentBombLength = 2;
			bombSaveGame.BaseMoveSpeed = 2.2f;
			bombSaveGame.CurrentZonePoint = 0;
			bombSaveGame.IsPassed = false;
			bombSaveGame.BestZonePoint = bestScore;
			SaveGameSystem.SaveGame(bombSaveGame, "SAVE_GAME" + saveName);
			return bombSaveGame;
		}

		public static BombSaveGame Copy(BombSaveGame save)
		{
			return new BombSaveGame
			{
				CurrentBombLength = save.CurrentBombLength,
				CurrentLevel = save.CurrentLevel,
				CurrentStar = save.CurrentStar,
				TotalBomb = save.TotalBomb,
				BaseMoveSpeed = save.BaseMoveSpeed,
				BestZonePoint = save.BestZonePoint,
				CurrentZonePoint = save.CurrentZonePoint,
				IsPassed = save.IsPassed
			};
		}

		public static BombSaveGame LoadZoneProgress(string zone)
		{
			BombSaveGame bombSaveGame = SaveGameSystem.LoadGame("SAVE_GAME" + zone) as BombSaveGame;
			if (bombSaveGame == null)
			{
				return BombSaveGame.CreateNewSaveGame(zone, 0);
			}
			return bombSaveGame;
		}

		public static BombSaveGame LoadZoneProgressCache(string zone)
		{
			BombSaveGame bombSaveGame = SaveGameSystem.LoadGame("SAVE_GAME" + zone + "cache") as BombSaveGame;
			if (bombSaveGame == null)
			{
				return BombSaveGame.CreateNewSaveGame(zone, 0);
			}
			return bombSaveGame;
		}

		public static void SaveZoneProgress()
		{
			SaveGameSystem.SaveGame(OfflineMapChooser.CurrentZoneProgress, "SAVE_GAME" + OfflineMapChooser.CurrentZone);
		}

		public static void SaveZoneProgressCache(BombSaveGame cacheSaveGame)
		{
			SaveGameSystem.SaveGame(cacheSaveGame, "SAVE_GAME" + OfflineMapChooser.CurrentZone + "cache");
		}

		public static void ReportScore()
		{
			OfflineMapChooser.CurrentZoneProgress.IsPassed = true;
			if (OfflineMapChooser.CurrentZoneProgress.CurrentZonePoint > OfflineMapChooser.CurrentZoneProgress.BestZonePoint)
			{
				OfflineMapChooser.CurrentZoneProgress.BestZonePoint = OfflineMapChooser.CurrentZoneProgress.CurrentZonePoint;
			}
			if (Social.localUser.authenticated && OfflineMapChooser.CurrentZoneProgress.CurrentZonePoint > 0)
			{
				Social.ReportScore((long)OfflineMapChooser.CurrentZoneProgress.CurrentZonePoint, OfflineMapChooser.CurrentZoneID, delegate(bool success)
				{
				});
			}
		}

		public const string SAVE_GAME = "SAVE_GAME";

		public int BestZonePoint;

		public int CurrentZonePoint;

		public bool IsPassed;
	}
}
