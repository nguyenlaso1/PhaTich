// @sonhg: class: AdViewTest
using System;
//using AudienceNetwork;
using UnityEngine;

public class AdViewTest : MonoBehaviour
{
	private void Awake()
	{
		//AdView adView = new AdView("YOUR_PLACEMENT_ID", AdSize.BANNER_HEIGHT_50);
		//this.adView = adView;
		//this.adView.Register(base.gameObject);
		//this.adView.AdViewDidLoad = delegate()
		//{
		//	UnityEngine.Debug.Log("Ad view loaded.");
		//	this.adView.Show(100.0);
		//};
		//adView.AdViewDidFailWithError = delegate(string error)
		//{
		//	UnityEngine.Debug.Log("Ad view failed to load with error: " + error);
		//};
		//adView.AdViewWillLogImpression = delegate()
		//{
		//	UnityEngine.Debug.Log("Ad view logged impression.");
		//};
		//adView.AdViewDidClick = delegate()
		//{
		//	UnityEngine.Debug.Log("Ad view clicked.");
		//};
		//adView.LoadAd();
	}

	private void OnDestroy()
	{
		//if (this.adView)
		//{
		//	this.adView.Dispose();
		//}
		//UnityEngine.Debug.Log("AdViewTest was destroyed!");
	}

	public void NextScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("NativeAdScene");
	}

	//private AdView adView;
}
