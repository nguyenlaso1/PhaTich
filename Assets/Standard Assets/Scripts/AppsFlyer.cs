// @plugin: class: AppsFlyer
using System;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyer : MonoBehaviour
{
	public static void trackEvent(string eventName, string eventValue)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AppsFlyer.cls_AppsFlyer.CallStatic("sendTrackingWithEvent", new object[]
				{
					@static,
					eventName,
					eventValue
				});
			}
		}
	}

	public static void setCurrencyCode(string currencyCode)
	{
		AppsFlyer.cls_AppsFlyer.CallStatic("setCurrencyCode", new object[]
		{
			currencyCode
		});
	}

	public static void setCustomerUserID(string customerUserID)
	{
		AppsFlyer.cls_AppsFlyer.CallStatic("setAppUserId", new object[]
		{
			customerUserID
		});
	}

	public static void loadConversionData(string callbackObject, string callbackMethod)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AppsFlyer.cls_AppsFlyerHelper.CallStatic("createConversionDataListener", new object[]
				{
					@static,
					callbackObject,
					callbackMethod
				});
			}
		}
	}

	public static void setCollectIMEI(bool shouldCollect)
	{
		AppsFlyer.cls_AppsFlyer.CallStatic("setCollectIMEI", new object[]
		{
			shouldCollect
		});
	}

	public static void setCollectAndroidID(bool shouldCollect)
	{
		MonoBehaviour.print("AF.cs setCollectAndroidID");
		AppsFlyer.cls_AppsFlyer.CallStatic("setCollectAndroidID", new object[]
		{
			shouldCollect
		});
	}

	public static void setAppsFlyerKey(string key)
	{
		AppsFlyer.cls_AppsFlyer.CallStatic("setAppsFlyerKey", new object[]
		{
			key
		});
	}

	public static void trackAppLaunch()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				AppsFlyer.cls_AppsFlyer.CallStatic("sendTracking", new object[]
				{
					@static
				});
			}
		}
	}

	public static void setAppID(string appleAppId)
	{
	}

	public static void validateReceipt(string eventName, string failedEventName, string eventValue, string productIdentifier, string price, string currency)
	{
	}

	public static void trackRichEvent(string eventName, Dictionary<string, string> eventValues)
	{
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]))
		{
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			object[] array = new object[2];
			foreach (KeyValuePair<string, string> keyValuePair in eventValues)
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
				{
					keyValuePair.Key
				}))
				{
					using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", new object[]
					{
						keyValuePair.Value
					}))
					{
						array[0] = androidJavaObject2;
						array[1] = androidJavaObject3;
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
					}
				}
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					AppsFlyer.cls_AppsFlyer.CallStatic("trackEvent", new object[]
					{
						@static,
						eventName,
						androidJavaObject
					});
				}
			}
		}
	}

	private static AndroidJavaClass cls_AppsFlyer = new AndroidJavaClass("com.appsflyer.AppsFlyerLib");

	private static AndroidJavaClass cls_AppsFlyerHelper = new AndroidJavaClass("com.appsflyer.AppsFlyerUnityHelper");
}
