// @sonhg: class: LoadingScreenManager
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
	public static void LoadScene(string levelNum, bool isAutoSwitchScene = true)
	{
		Application.backgroundLoadingPriority = ThreadPriority.High;
		LoadingScreenManager.sceneToLoad = levelNum;
		LoadingScreenManager.autoSwitchScene = isAutoSwitchScene;
		UnityEngine.SceneManagement.SceneManager.LoadScene(LoadingScreenManager.loadingSceneIndex);
	}

	private void Start()
	{
		if (string.IsNullOrEmpty(LoadingScreenManager.sceneToLoad))
		{
			return;
		}
		if (LoadingScreenManager.sceneToLoad.CompareTo("BomberMap") == 0)
		{
			Resources.UnloadUnusedAssets();
		}
		this.fadeOverlay.gameObject.SetActive(true);
		base.StartCoroutine(this.LoadAsync(LoadingScreenManager.sceneToLoad));
	}

	private IEnumerator LoadAsync(string levelNum)
	{
		this.ShowLoadingVisuals();
		yield return null;
		this.FadeIn();
		this.StartOperation(levelNum);
		float lastProgress = 0f;
		while (!this.DoneLoading())
		{
			yield return null;
			if (!Mathf.Approximately(this.operation.progress, lastProgress))
			{
				lastProgress = this.operation.progress;
			}
		}
		this.ShowCompletionVisuals();
		yield return new WaitForSeconds(this.waitOnLoadEnd);
		this.FadeOut();
		yield return new WaitForSeconds(this.fadeDuration);
		this.operation.allowSceneActivation = true;
		yield break;
	}

	private void StartOperation(string levelNum)
	{
		Application.backgroundLoadingPriority = this.loadThreadPriority;
		this.operation = Application.LoadLevelAsync(levelNum);
		this.operation.allowSceneActivation = false;
	}

	private bool DoneLoading()
	{
		return this.operation.progress >= 0.9f && LoadingScreenManager.autoSwitchScene;
	}

	private void FadeIn()
	{
		this.fadeOverlay.CrossFadeAlpha(0f, this.fadeDuration, true);
	}

	private void FadeOut()
	{
		this.fadeOverlay.CrossFadeAlpha(1f, this.fadeDuration, true);
	}

	private void ShowLoadingVisuals()
	{
		this.loadingText.text = this.GetTip();
	}

	private void ShowCompletionVisuals()
	{
	}

	private string GetTip()
	{
		return new List<string>
		{
			"Đoán xem điều gì đang chờ đợi bạn ở phía trước nào?"
			//"Stuck on a level ? Try another Land !",
			//"Always find items first !",
			//"Conquer Land Boss to get huge Gold Bags and Achievement Reward!",
			//"You can change controller type in Pause Menu.",
			//"Bees always chase you.",
			//"Mushroom monsters have no brain so they move randomly.",
			//"Egg monsters can eat bombs so surround them.",
			//"Club Penguin : how to be invisible.",
			//"Living Flame monsters have a protect shield.",
			//"Questionmark item is mysterious...",
			//"Collect items to maximum stats \nReady for BOSS battle !",
			//"Place bombs as much as you can.",
			//"WATCH OUT!!!",
			//"Swamp Land : Swamp Mud reverses your control !",
			//"Forest Land : Root trap keeps you in a short time",
			//"Frozen Land : Frozen trap makes that you can't stop place the bombs !",
			//"Desert Land : Quicksand slows your steps !",
			//"Take care of these Toxic boxes and Tourchs !",
			//"You have 3 Lifes each level.",
			//"New Land - New adventure"
		}.GetRandomElement<string>();
	}

	[Header("Loading Visuals")]
	public Text loadingText;

	public Image fadeOverlay;

	[Header("Timing Settings")]
	public float waitOnLoadEnd = 0.25f;

	public float fadeDuration = 0.25f;

	[Header("Loading Settings")]
	public ThreadPriority loadThreadPriority;

	private AsyncOperation operation;

	public static bool autoSwitchScene = true;

	public static string sceneToLoad = "OfflineMainMenu";

	private static string loadingSceneIndex = "LoadingScene";
}
