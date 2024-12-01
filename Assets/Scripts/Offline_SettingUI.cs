// @sonhg: class: Offline_SettingUI
using System;
using Bomb;
using BombOffline;
using UnityEngine;

public class Offline_SettingUI : MonoBehaviour
{
	private void Awake()
	{
	}

	public void OnSettingButtonClick()
	{
		MapSettingBox original = Resources.Load<MapSettingBox>("Prefabs/Bomber/Boxs/MapSettingPanel");
		MapSettingBox mapSettingBox = UnityEngine.Object.Instantiate<MapSettingBox>(original);
		mapSettingBox.achivementBox = this.achievementBox;
	}

	public GameObject LeaderboardButton;

	public AchievementBox achievementBox;
}
