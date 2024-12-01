// @sonhg: class: GoogleAnalytic
using System;
using BombOffline;
using UnityEngine;

public class GoogleAnalytic : MonoBehaviour
{
	private void Awake()
	{
		this.googleAnalytics = base.GetComponent<GoogleAnalyticsV3>();
	}

	private void Start()
	{
		this.googleAnalytics.StartSession();
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("OfflineMainScene") && OfflineMapChooser.CurrentLevel != null)
		{
			this.googleAnalytics.LogScreen(new AppViewHitBuilder().SetScreenName(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) + OfflineMapChooser.CurrentLevel);
		}
		else
		{
			this.googleAnalytics.LogScreen(new AppViewHitBuilder().SetScreenName(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
		}
	}

	private void Update()
	{
	}

	public GoogleAnalyticsV3 googleAnalytics;
}
