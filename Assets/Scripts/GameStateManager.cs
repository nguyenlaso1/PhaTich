// @sonhg: class: GameStateManager
using System;
using System.Collections.Generic;
//using Facebook.Unity;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	public static GameStateManager Instance
	{
		get
		{
			return GameStateManager.current();
		}
	}

	public static int Score
	{
		get
		{
			return GameStateManager.Instance.score;
		}
	}

	public static int HighScore
	{
		get
		{
			return (GameStateManager.Instance.highScore == null) ? 0 : GameStateManager.Instance.highScore.Value;
		}
		set
		{
			GameStateManager.Instance.highScore = new int?(value);
		}
	}

	public static int LivesRemaining
	{
		get
		{
			return GameStateManager.Instance.lives;
		}
	}

	public static List<object> Scores
	{
		get
		{
			return GameStateManager.scores;
		}
		set
		{
			GameStateManager.scores = value;
			GameStateManager.ScoresReady = true;
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void StartGame()
	{
		this.lives = GameStateManager.StartingLives;
		this.score = GameStateManager.StartingScore;
		GameStateManager.ScoringLockout = false;
		Time.timeScale = 1f;
	}

	public static void onFriendSmash()
	{
		if (!GameStateManager.ScoringLockout)
		{
			GameStateManager.Instance.score++;
		}
	}

	public static void onFriendDie()
	{
		if (--GameStateManager.Instance.lives == 0)
		{
			GameStateManager.EndGame();
		}
	}

	public static void EndGame()
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"EndGame Instance.highScore = ",
			GameStateManager.Instance.highScore,
			"\nInstance.score = ",
			GameStateManager.Instance.score
		}));
		//FBAppEvents.GameComplete(GameStateManager.Instance.score);
		//if (FB.IsLoggedIn && GameStateManager.Instance.highScore != null)
		//{
		//	int? num = GameStateManager.Instance.highScore;
		//	if (num != null && num.Value < GameStateManager.Instance.score)
		//	{
		//		UnityEngine.Debug.Log("Player has new high score :" + GameStateManager.Instance.score);
		//		GameStateManager.Instance.highScore = new int?(GameStateManager.Instance.score);
		//		GameStateManager.highScorePending = true;
		//	}
		//}
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
	}

	//public static FacebookController FacebookController
	//{
	//	get
	//	{
	//		if (GameStateManager.facebookController == null)
	//		{
	//			GameObject gameObject = GameObject.Find("FacebookBox");
	//			if (gameObject)
	//			{
	//				GameStateManager.facebookController = gameObject.GetComponent<FacebookController>();
	//			}
	//		}
	//		return GameStateManager.facebookController;
	//	}
	//}

	private static GameStateManager instance;

	private static GameStateManager.InstanceStep init = delegate()
	{
		GameObject gameObject = new GameObject("GameStateManagerManager");
		GameStateManager.instance = gameObject.AddComponent<GameStateManager>();
		GameStateManager.instance.lives = GameStateManager.StartingLives;
		GameStateManager.instance.score = GameStateManager.StartingScore;
		GameStateManager.instance.highScore = null;
		GameStateManager.current = GameStateManager.final;
		return GameStateManager.instance;
	};

	private static GameStateManager.InstanceStep final = () => GameStateManager.instance;

	private static GameStateManager.InstanceStep current = GameStateManager.init;

	public static readonly string ServerURL = "https://friendsmash-unity.herokuapp.com/";

	public static readonly int StartingLives = 3;

	public static readonly int StartingScore = 0;

	private int score;

	private int? highScore;

	public static bool ScoringLockout;

	public static bool highScorePending;

	private int lives;

	public static int CoinBalance;

	public static int NumBombs;

	public static string FriendName = "Blue Guy";

	public static string FriendID = null;

	public static Texture FriendTexture = null;

	public static int CelebFriend = -1;

	public static string Username;

	public static Texture UserTexture;

	public static List<object> Friends;

	public static Dictionary<string, Texture> FriendImages = new Dictionary<string, Texture>();

	public static List<object> InvitableFriends = new List<object>();

	public static bool ScoresReady;

	private static List<object> scores;

	//private static FacebookController facebookController;

	private delegate GameStateManager InstanceStep();
}
