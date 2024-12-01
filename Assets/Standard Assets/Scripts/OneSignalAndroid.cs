// @plugin: class: OneSignalAndroid
using System;
using System.Collections.Generic;
using OneSignalPush.MiniJSON;
using UnityEngine;

public class OneSignalAndroid : OneSignalPlatform
{
	public OneSignalAndroid(string gameObjectName, string googleProjectNumber, string appId, OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel)
	{
		OneSignalAndroid.mOneSignal = new AndroidJavaObject("com.onesignal.OneSignalUnityProxy", new object[]
		{
			gameObjectName,
			googleProjectNumber,
			appId,
			(int)logLevel,
			(int)visualLevel
		});
	}

	public void SetLogLevel(OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel)
	{
		OneSignalAndroid.mOneSignal.Call("setLogLevel", new object[]
		{
			(int)logLevel,
			(int)logLevel
		});
	}

	public void SendTag(string tagName, string tagValue)
	{
		OneSignalAndroid.mOneSignal.Call("sendTag", new object[]
		{
			tagName,
			tagValue
		});
	}

	public void SendTags(IDictionary<string, string> tags)
	{
		OneSignalAndroid.mOneSignal.Call("sendTags", new object[]
		{
			Json.Serialize(tags)
		});
	}

	public void GetTags()
	{
		OneSignalAndroid.mOneSignal.Call("getTags", new object[0]);
	}

	public void DeleteTag(string key)
	{
		OneSignalAndroid.mOneSignal.Call("deleteTag", new object[]
		{
			key
		});
	}

	public void DeleteTags(IList<string> keys)
	{
		OneSignalAndroid.mOneSignal.Call("deleteTags", new object[]
		{
			Json.Serialize(keys)
		});
	}

	public void IdsAvailable()
	{
		OneSignalAndroid.mOneSignal.Call("idsAvailable", new object[0]);
	}

	public void FireNotificationReceivedEvent(string jsonString, OneSignal.NotificationReceived notificationReceived)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
		Dictionary<string, object> additionalData = null;
		if (dictionary.ContainsKey("custom"))
		{
			additionalData = (dictionary["custom"] as Dictionary<string, object>);
		}
		notificationReceived((string)dictionary["alert"], additionalData, (bool)dictionary["isActive"]);
	}

	public void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			OneSignalAndroid.mOneSignal.Call("onPause", new object[0]);
		}
		else
		{
			OneSignalAndroid.mOneSignal.Call("onResume", new object[0]);
		}
	}

	public void RegisterForPushNotifications()
	{
	}

	public void EnableVibrate(bool enable)
	{
		OneSignalAndroid.mOneSignal.Call("enableVibrate", new object[]
		{
			enable
		});
	}

	public void EnableSound(bool enable)
	{
		OneSignalAndroid.mOneSignal.Call("enableSound", new object[]
		{
			enable
		});
	}

	public void EnableInAppAlertNotification(bool enable)
	{
		OneSignalAndroid.mOneSignal.Call("enableInAppAlertNotification", new object[]
		{
			enable
		});
	}

	public void EnableNotificationsWhenActive(bool enable)
	{
		OneSignalAndroid.mOneSignal.Call("enableNotificationsWhenActive", new object[]
		{
			enable
		});
	}

	public void SetSubscription(bool enable)
	{
		OneSignalAndroid.mOneSignal.Call("setSubscription", new object[]
		{
			enable
		});
	}

	public void PostNotification(Dictionary<string, object> data)
	{
		OneSignalAndroid.mOneSignal.Call("postNotification", new object[]
		{
			Json.Serialize(data)
		});
	}

	private static AndroidJavaObject mOneSignal;
}
