// @sonhg: class: OnlineAdmob
using System;
//using GoogleMobileAds.Api;

public class OnlineAdmob
{
	//private static AdRequest getAdRequest()
	//{
	//	AdRequest.Builder builder = new AdRequest.Builder();
	//	return builder.Build();
	//}

	public static void ShowBannerAds(bool isTop)
	{
		//if (Joker2XConfigUtils.TURN_ON_BANNER)
		//{
		//	AdPosition position = AdPosition.Bottom;
		//	if (isTop)
		//	{
		//		position = AdPosition.Top;
		//	}
		//	string adUnitId = "ca-app-pub-5380959492831074/6110437748";
		//	OnlineAdmob.bannerView = new BannerView(adUnitId, AdSize.Banner, position);
		//	OnlineAdmob.bannerView.LoadAd(OnlineAdmob.getAdRequest());
		//	OnlineAdmob.bannerView.Show();
		//}
	}

	public static void HideBannerAds()
	{
		//if (OnlineAdmob.bannerView != null)
		//{
		//	OnlineAdmob.bannerView.Hide();
		//}
	}

	public static void LoadInterAds()
	{
		//OnlineAdmob.interstitial = null;
		//OnlineAdmob.GetInterstitialAd().LoadAd(OnlineAdmob.getAdRequest());
	}

	//public static InterstitialAd GetInterstitialAd()
	//{
	//	if (OnlineAdmob.interstitial == null)
	//	{
	//		OnlineAdmob.interstitial = new InterstitialAd("ca-app-pub-5380959492831074/7587170941");
	//	}
	//	return OnlineAdmob.interstitial;
	//}

	public static void DestroyInterstitialAd()
	{
		//OnlineAdmob.interstitial.Destroy();
	}

	public static void ShowInterstitialAd()
	{
		//if (OnlineAdmob.GetInterstitialAd().IsLoaded())
		//{
		//	OnlineAdmob.GetInterstitialAd().Show();
		//	OnlineAdmob.LoadInterAds();
		//}
		//else
		//{
		//	OnlineAdmob.LoadInterAds();
		//}
	}

	//public static BannerView bannerView;

	//private static InterstitialAd interstitial;
}
