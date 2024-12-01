// @sonhg: class: BombOffline.GameProgress
using System;

namespace BombOffline
{
	[Serializable]
	public class GameProgress : SaveGame
	{
		public string CurrentZone { get; set; }

		public static GameProgress CreateNewSaveGame()
		{
			GameProgress gameProgress = new GameProgress();
			gameProgress.CurrentZone = "forest";
			SaveGameSystem.SaveGame(gameProgress, GameProgress.PROGRESS_SAVE);
			return gameProgress;
		}

		public static GameProgress LoadGameProgress()
		{
			GameProgress gameProgress = SaveGameSystem.LoadGame(GameProgress.PROGRESS_SAVE) as GameProgress;
			if (gameProgress == null)
			{
				return GameProgress.CreateNewSaveGame();
			}
			return gameProgress;
		}

		public static void SaveGameProgress()
		{
			SaveGameSystem.SaveGame(OfflineMapChooser.CurrentGameProgress, GameProgress.PROGRESS_SAVE);
		}

		public static string PROGRESS_SAVE = "PROGRESS_SAVE";
	}
}
