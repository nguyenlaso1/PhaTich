// @sonhg: class: Bomb.Offline_FreeGoldAds
using System;
using AudienceNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace Bomb
{
	public class Offline_FreeGoldAds : MonoBehaviour
	{
		private void Start()
		{
			this.goldlabel.text = Offline_Config.VIEW_ADS_BONUS + string.Empty;
		}

		private void Update()
		{
		}

		public void OnclickViewAds()
		{
			AdsStatus adsStatus = Offline_FreeGoldAds.status;
			if (adsStatus != AdsStatus.UnLoad)
			{
				if (adsStatus == AdsStatus.Loading)
				{
					this.label.text = "Plz Wait";
				}
			}
			else
			{
				this.label.text = "Loading";
				this.loading.SetActive(true);
				Offline_FreeGoldAds.status = AdsStatus.Loading;
				this.LoadAds();
			}
		}

		public void LoadAds()
		{
			//Offline_FreeGoldAds.interstitialAd = new InterstitialAd("2153047734835935_2189625071178201");
			//Offline_FreeGoldAds.interstitialAd.Register(base.gameObject);
			//Offline_FreeGoldAds.interstitialAd.InterstitialAdDidLoad = delegate()
			//{
			//	UnityEngine.Debug.Log("Interstitial ad loaded.");
			//	this.label.text = "View";
			//	this.loading.SetActive(false);
			//	this.ShowAds();
			//};
			//Offline_FreeGoldAds.interstitialAd.InterstitialAdDidFailWithError = delegate(string error)
			//{
			//	UnityEngine.Debug.Log("Interstitial ad failed to load with error: " + error);
			//	this.freeCoinController.shopController.confirmPopUp.AddMessage("Please check your connection", string.Empty, string.Empty);
			//	this.label.text = "View Ads";
			//	this.loading.SetActive(false);
			//	Offline_FreeGoldAds.status = AdsStatus.UnLoad;
			//};
			//Offline_FreeGoldAds.interstitialAd.InterstitialAdWillLogImpression = delegate()
			//{
			//	UnityEngine.Debug.Log("Interstitial ad logged impression.");
			//};
			//Offline_FreeGoldAds.interstitialAd.InterstitialAdDidClick = delegate()
			//{
			//	UnityEngine.Debug.Log("Interstitial ad clicked.");
			//};
			//Offline_FreeGoldAds.status = AdsStatus.Loading;
			//Offline_FreeGoldAds.interstitialAd.LoadAd();
		}

		public void ShowAds()
		{
			//Offline_FreeGoldAds.interstitialAd.Show();
			//this.label.text = "View Ads";
			//this.loading.SetActive(false);
			//Offline_FreeGoldAds.status = AdsStatus.UnLoad;
			//this.freeCoinController.PlusGold(Offline_Config.VIEW_ADS_BONUS);
		}

		//public static InterstitialAd interstitialAd;

		private static AdsStatus status;

		public GameObject loading;

		public Text label;

		public Text goldlabel;

		public Offline_FreeCoinController freeCoinController;
	}
}
