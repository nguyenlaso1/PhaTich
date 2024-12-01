// @plugin: class: OneSignalPlatform
using System;
using System.Collections.Generic;

public interface OneSignalPlatform
{
	void SetLogLevel(OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel);

	void RegisterForPushNotifications();

	void SendTag(string tagName, string tagValue);

	void SendTags(IDictionary<string, string> tags);

	void GetTags();

	void DeleteTag(string key);

	void DeleteTags(IList<string> keys);

	void OnApplicationPause(bool paused);

	void IdsAvailable();

	void EnableInAppAlertNotification(bool enable);

	void SetSubscription(bool enable);

	void PostNotification(Dictionary<string, object> data);

	void FireNotificationReceivedEvent(string jsonString, OneSignal.NotificationReceived notificationReceived);
}
