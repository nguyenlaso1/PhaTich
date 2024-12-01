// @sonhg: class: AdsController
using System;
using DG.Tweening;
using UnityEngine;

public class AdsController : MonoBehaviour
{
	private void Awake()
	{
		//if (Joker2XConfigUtils.TURN_ON_ADMOB_ONLINE && !OnlineAdmob.GetInterstitialAd().IsLoaded())
		//{
		//	OnlineAdmob.LoadInterAds();
		//}
	}

	private void OnDestroy()
	{
		//if (OnlineAdmob.bannerView != null && Joker2XConfigUtils.TURN_ON_ADMOB_ONLINE)
		//{
		//	OnlineAdmob.bannerView.AdLoaded -= this.LoadedBannerAds;
		//}
	}

	private void LoadedBannerAds(object sender, EventArgs e)
	{
		this.mainCamera.transform.DOLocalMoveY(5.7f, 0.2f, false);
	}

	public void ShowBannerAds(bool isScale)
	{
		OnlineAdmob.ShowBannerAds(true);
		if (Joker2XConfigUtils.TURN_ON_BANNER && isScale)
		{
			//OnlineAdmob.bannerView.AdLoaded += this.LoadedBannerAds;
		}
	}

	public void ShowInterAds(bool isTme)
	{
		if (!Joker2XConfigUtils.TURN_ON_ADMOB_ONLINE)
		{
			return;
		}
		//if (isTme)
		//{
		//	if (AdsController.play_count >= Joker2XConfigUtils.ONLINE_ADMOB_TIME)
		//	{
		//		if (OnlineAdmob.bannerView != null)
		//		{
		//			OnlineAdmob.bannerView.Hide();
		//		}
		//		OnlineAdmob.ShowInterstitialAd();
		//		AdsController.play_count = 0;
		//	}
		//}
		//else
		//{
		//	DateTime now = DateTime.Now;
		//	if ((now - AdsController.lastShowAdmob).TotalSeconds >= (double)Joker2XConfigUtils.ONLINE_ADMOB_FREQUENCE)
		//	{
		//		if (OnlineAdmob.bannerView != null)
		//		{
		//			OnlineAdmob.bannerView.Hide();
		//		}
		//		OnlineAdmob.ShowInterstitialAd();
		//		AdsController.lastShowAdmob = DateTime.Now;
		//	}
		//}
	}

	private void HideBannerAds(object sender, EventArgs e)
	{
		OnlineAdmob.HideBannerAds();
	}

	[SerializeField]
	private Camera mainCamera;

	public static DateTime lastShowAdmob = DateTime.Now;

	public static int play_count;
}
