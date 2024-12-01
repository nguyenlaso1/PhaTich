// @sonhg: class: BombOffline.OfflineAdmob
using System;
//using GoogleMobileAds.Api;

namespace BombOffline
{
	public class OfflineAdmob
	{
		public static void ShowInterAds()
		{
			//if (OfflineAdmob.interstitial == null)
			//{
			//	OfflineAdmob.RequestNewAds();
			//}
			//if (OfflineAdmob.CreateAndLoadInterstitial().IsLoaded())
			//{
			//	Context.googleAnalytics.LogEvent(Analystics.C_IN_GAME, Analystics.A_ADMOB, Analystics.L_SHOW_SUCCESS, 0L);
			//	OfflineAdmob.interstitial.Show();
			//	BomberAds.play_count = 0;
			//}
			//else
			//{
			//	OfflineAdmob.retryTime++;
			//	if (OfflineAdmob.retryTime > 5)
			//	{
			//		OfflineAdmob.interstitial.Destroy();
			//		OfflineAdmob.interstitial = null;
			//		OfflineAdmob.retryTime = 0;
			//	}
			//}
		}

		//public static InterstitialAd CreateAndLoadInterstitial()
		//{
		//	if (OfflineAdmob.interstitial == null)
		//	{
		//		OfflineAdmob.interstitial = new InterstitialAd("ca-app-pub-5380959492831074/2872374543");
		//		OfflineAdmob.interstitial.AdLoaded += delegate(object sender, EventArgs args)
		//		{
		//		};
		//		OfflineAdmob.interstitial.AdFailedToLoad += delegate(object sender, AdFailedToLoadEventArgs args)
		//		{
		//			OfflineAdmob.interstitial.Destroy();
		//			OfflineAdmob.interstitial = null;
		//		};
		//		OfflineAdmob.interstitial.AdOpened += delegate(object sender, EventArgs args)
		//		{
		//		};
		//		OfflineAdmob.interstitial.AdClosing += delegate(object sender, EventArgs args)
		//		{
		//		};
		//		OfflineAdmob.interstitial.AdClosed += delegate(object sender, EventArgs args)
		//		{
		//			OfflineAdmob.interstitial.Destroy();
		//			OfflineAdmob.interstitial = null;
		//			OfflineAdmob.RequestNewAds();
		//		};
		//		OfflineAdmob.interstitial.AdLeftApplication += delegate(object sender, EventArgs args)
		//		{
		//		};
		//		OfflineAdmob.interstitial.LoadAd(new AdRequest.Builder().Build());
		//	}
		//	return OfflineAdmob.interstitial;
		//}

		private static void RequestNewAds()
		{
			//Context.googleAnalytics.LogEvent(Analystics.C_IN_GAME, Analystics.A_ADMOB, Analystics.L_REQUEST, 0L);
			//OfflineAdmob.interstitial = OfflineAdmob.CreateAndLoadInterstitial();
		}

		//private static InterstitialAd interstitial;

		private static int retryTime;
	}
}
