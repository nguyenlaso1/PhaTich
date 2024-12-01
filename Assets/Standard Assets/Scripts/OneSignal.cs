// @plugin: class: OneSignal
using System;
using System.Collections.Generic;
using OneSignalPush.MiniJSON;
using UnityEngine;

public class OneSignal : MonoBehaviour
{
	public static void Init(string appId, string googleProjectNumber, OneSignal.NotificationReceived inNotificationDelegate, bool autoRegister)
	{
		if (OneSignal.initialized)
		{
			return;
		}
		OneSignal.oneSignalPlatform = new OneSignalAndroid("OneSignalRuntimeObject_KEEP", googleProjectNumber, appId, OneSignal.logLevel, OneSignal.visualLogLevel);
		OneSignal.notificationDelegate = inNotificationDelegate;
		GameObject gameObject = new GameObject("OneSignalRuntimeObject_KEEP");
		gameObject.AddComponent<OneSignal>();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		OneSignal.initialized = true;
	}

	public static void Init(string appId, string googleProjectNumber, OneSignal.NotificationReceived inNotificationDelegate)
	{
		OneSignal.Init(appId, googleProjectNumber, inNotificationDelegate, true);
	}

	public static void Init(string appId, string googleProjectNumber)
	{
		OneSignal.Init(appId, googleProjectNumber, null, true);
	}

	public static void Init(string appId)
	{
		OneSignal.Init(appId, null, null, true);
	}

	public static void SetLogLevel(OneSignal.LOG_LEVEL inLogLevel, OneSignal.LOG_LEVEL inVisualLevel)
	{
		OneSignal.logLevel = inLogLevel;
		OneSignal.visualLogLevel = inVisualLevel;
	}

	public static void SendTag(string tagName, string tagValue)
	{
		OneSignal.oneSignalPlatform.SendTag(tagName, tagValue);
	}

	public static void SendTags(IDictionary<string, string> tags)
	{
		OneSignal.oneSignalPlatform.SendTags(tags);
	}

	public static void GetTags(OneSignal.TagsReceived inTagsReceivedDelegate)
	{
		OneSignal.tagsReceivedDelegate = inTagsReceivedDelegate;
		OneSignal.oneSignalPlatform.GetTags();
	}

	public static void GetTags()
	{
		OneSignal.oneSignalPlatform.GetTags();
	}

	public static void DeleteTag(string key)
	{
		OneSignal.oneSignalPlatform.DeleteTag(key);
	}

	public static void DeleteTags(IList<string> keys)
	{
		OneSignal.oneSignalPlatform.DeleteTags(keys);
	}

	public static void SendPurchase(double amount)
	{
	}

	public static void RegisterForPushNotifications()
	{
		OneSignal.oneSignalPlatform.RegisterForPushNotifications();
	}

	public static void GetIdsAvailable(OneSignal.IdsAvailable inIdsAvailableDelegate)
	{
		OneSignal.idsAvailableDelegate = inIdsAvailableDelegate;
		OneSignal.oneSignalPlatform.IdsAvailable();
	}

	public static void GetIdsAvailable()
	{
		OneSignal.oneSignalPlatform.IdsAvailable();
	}

	public static void EnableVibrate(bool enable)
	{
		((OneSignalAndroid)OneSignal.oneSignalPlatform).EnableVibrate(enable);
	}

	public static void EnableSound(bool enable)
	{
		((OneSignalAndroid)OneSignal.oneSignalPlatform).EnableSound(enable);
	}

	public static void EnableNotificationsWhenActive(bool enable)
	{
		((OneSignalAndroid)OneSignal.oneSignalPlatform).EnableNotificationsWhenActive(enable);
	}

	public static void EnableInAppAlertNotification(bool enable)
	{
		OneSignal.oneSignalPlatform.EnableInAppAlertNotification(enable);
	}

	public static void SetSubscription(bool enable)
	{
		OneSignal.oneSignalPlatform.SetSubscription(enable);
	}

	public static void PostNotification(Dictionary<string, object> data)
	{
		OneSignal.PostNotification(data, null, null);
	}

	public static void PostNotification(Dictionary<string, object> data, OneSignal.OnPostNotificationSuccess inOnPostNotificationSuccess, OneSignal.OnPostNotificationFailure inOnPostNotificationFailure)
	{
		OneSignal.postNotificationSuccessDelegate = inOnPostNotificationSuccess;
		OneSignal.postNotificationFailureDelegate = inOnPostNotificationFailure;
		OneSignal.oneSignalPlatform.PostNotification(data);
	}

	private void onPushNotificationReceived(string jsonString)
	{
		if (OneSignal.notificationDelegate != null)
		{
			OneSignal.oneSignalPlatform.FireNotificationReceivedEvent(jsonString, OneSignal.notificationDelegate);
		}
	}

	private void onIdsAvailable(string jsonString)
	{
		if (OneSignal.idsAvailableDelegate != null)
		{
			Dictionary<string, object> dictionary = Json.Deserialize(jsonString) as Dictionary<string, object>;
			OneSignal.idsAvailableDelegate((string)dictionary["userId"], (string)dictionary["pushToken"]);
		}
	}

	private void onTagsReceived(string jsonString)
	{
		OneSignal.tagsReceivedDelegate(Json.Deserialize(jsonString) as Dictionary<string, object>);
	}

	private void OnApplicationPause(bool paused)
	{
		OneSignal.oneSignalPlatform.OnApplicationPause(paused);
	}

	private void onPostNotificationSuccess(string response)
	{
		if (OneSignal.postNotificationSuccessDelegate != null)
		{
			OneSignal.postNotificationSuccessDelegate(Json.Deserialize(response) as Dictionary<string, object>);
			OneSignal.postNotificationFailureDelegate = null;
			OneSignal.postNotificationSuccessDelegate = null;
		}
	}

	private void onPostNotificationFailed(string response)
	{
		if (OneSignal.postNotificationFailureDelegate != null)
		{
			OneSignal.postNotificationFailureDelegate(Json.Deserialize(response) as Dictionary<string, object>);
			OneSignal.postNotificationFailureDelegate = null;
			OneSignal.postNotificationSuccessDelegate = null;
		}
	}

	private const string gameObjectName = "OneSignalRuntimeObject_KEEP";

	public static OneSignal.IdsAvailable idsAvailableDelegate;

	public static OneSignal.TagsReceived tagsReceivedDelegate;

	private static OneSignal.LOG_LEVEL logLevel = OneSignal.LOG_LEVEL.INFO;

	private static OneSignal.LOG_LEVEL visualLogLevel;

	private static OneSignalPlatform oneSignalPlatform;

	private static bool initialized;

	internal static OneSignal.NotificationReceived notificationDelegate;

	internal static OneSignal.OnPostNotificationSuccess postNotificationSuccessDelegate;

	internal static OneSignal.OnPostNotificationFailure postNotificationFailureDelegate;

	public enum LOG_LEVEL
	{
		NONE,
		FATAL,
		ERROR,
		WARN,
		INFO,
		DEBUG,
		VERBOSE
	}

	public delegate void NotificationReceived(string message, Dictionary<string, object> additionalData, bool isActive);

	public delegate void IdsAvailable(string playerID, string pushToken);

	public delegate void TagsReceived(Dictionary<string, object> tags);

	public delegate void OnPostNotificationSuccess(Dictionary<string, object> response);

	public delegate void OnPostNotificationFailure(Dictionary<string, object> response);
}
