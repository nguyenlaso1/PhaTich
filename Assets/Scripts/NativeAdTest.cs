// @sonhg: class: NativeAdTest
using System;
using AudienceNetwork;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(RectTransform))]
public class NativeAdTest : MonoBehaviour
{
	private void Awake()
	{
		//NativeAd nativeAd = new NativeAd("YOUR_PLACEMENT_ID");
		//this.nativeAd = nativeAd;
		//nativeAd.RegisterGameObjectForImpression(base.gameObject, new Button[]
		//{
		//	this.callToActionButton
		//});
		//nativeAd.NativeAdDidLoad = delegate()
		//{
		//	UnityEngine.Debug.Log("Native ad loaded.");
		//	UnityEngine.Debug.Log("Loading images...");
		//	this.StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
		//	this.StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
		//	UnityEngine.Debug.Log("Images loaded.");
		//	this.title.text = nativeAd.Title;
		//	this.socialContext.text = nativeAd.SocialContext;
		//	this.callToAction.text = nativeAd.CallToAction;
		//};
		//nativeAd.NativeAdDidFailWithError = delegate(string error)
		//{
		//	UnityEngine.Debug.Log("Native ad failed to load with error: " + error);
		//};
		//nativeAd.NativeAdWillLogImpression = delegate()
		//{
		//	UnityEngine.Debug.Log("Native ad logged impression.");
		//};
		//nativeAd.NativeAdDidClick = delegate()
		//{
		//	UnityEngine.Debug.Log("Native ad clicked.");
		//};
		//nativeAd.LoadAd();
	}

	private void OnGUI()
	{
		//this.coverImage.sprite = this.nativeAd.CoverImage;
		//this.iconImage.sprite = this.nativeAd.IconImage;
	}

	private void OnDestroy()
	{
		//if (this.nativeAd)
		//{
		//	this.nativeAd.Dispose();
		//}
		//UnityEngine.Debug.Log("NativeAdTest was destroyed!");
	}

	public void NextScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("InterstitialAdScene");
	}

	//private NativeAd nativeAd;

	[Header("Text:")]
	public Text title;

	public Text socialContext;

	[Header("Images:")]
	public Image coverImage;

	public Image iconImage;

	[Header("Buttons:")]
	public Text callToAction;

	public Button callToActionButton;
}
