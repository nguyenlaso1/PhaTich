// @plugin: class: GameControllerExample
using System;
using System.Collections.Generic;
using OneSignalPush.MiniJSON;
using UnityEngine;

public class GameControllerExample : MonoBehaviour
{
	private void Start()
	{
		GameControllerExample.extraMessage = null;
		OneSignal.Init("8c54ed22-7eb8-11e5-8def-a0369f2d9328", "714383265073", new OneSignal.NotificationReceived(GameControllerExample.HandleNotification));
		OneSignal.EnableInAppAlertNotification(true);
	}

	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive)
	{
		MonoBehaviour.print("GameControllerExample:HandleNotification:message" + message);
		GameControllerExample.extraMessage = "Notification opened with text: " + message;
		if (additionalData != null)
		{
			if (additionalData.ContainsKey("discount"))
			{
				GameControllerExample.extraMessage = (string)additionalData["discount"];
			}
			else if (additionalData.ContainsKey("actionSelected"))
			{
				GameControllerExample.extraMessage = "Pressed ButtonId: " + additionalData["actionSelected"];
			}
		}
	}

	private void OnGUI()
	{
		GUIStyle guistyle = new GUIStyle("button");
		guistyle.fontSize = 30;
		GUIStyle guistyle2 = new GUIStyle("box");
		guistyle2.fontSize = 30;
		GUI.Box(new Rect(10f, 10f, 390f, 340f), "Test Menu", guistyle2);
		if (GUI.Button(new Rect(60f, 80f, 300f, 60f), "SendTags", guistyle))
		{
			OneSignal.SendTag("UnityTestKey", "TestValue");
			OneSignal.SendTags(new Dictionary<string, string>
			{
				{
					"UnityTestKey2",
					"value2"
				},
				{
					"UnityTestKey3",
					"value3"
				}
			});
		}
		if (GUI.Button(new Rect(60f, 170f, 300f, 60f), "GetIds", guistyle))
		{
			OneSignal.GetIdsAvailable(delegate(string userId, string pushToken)
			{
				GameControllerExample.extraMessage = "UserID:\n" + userId + "\n\nPushToken:\n" + pushToken;
			});
		}
		if (GUI.Button(new Rect(60f, 260f, 300f, 60f), "TestNotification", guistyle))
		{
			GameControllerExample.extraMessage = "Waiting to get a OneSignal userId. Uncomment OneSignal.SetLogLevel in the Start method if it hangs here to debug the issue.";
			OneSignal.GetIdsAvailable(delegate(string userId, string pushToken)
			{
				if (pushToken != null)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["contents"] = new Dictionary<string, string>
					{
						{
							"en",
							"Test Message"
						}
					};
					dictionary["include_player_ids"] = new List<string>
					{
						userId
					};
					dictionary["send_after"] = DateTime.Now.ToUniversalTime().AddSeconds(30.0).ToString("U");
					GameControllerExample.extraMessage = "Posting test notification now.";
					OneSignal.PostNotification(dictionary, delegate(Dictionary<string, object> responseSuccess)
					{
						GameControllerExample.extraMessage = "Notification posted successful! Delayed by about 30 secounds to give you time to press the home button to see a notification vs an in-app alert.\n" + Json.Serialize(responseSuccess);
					}, delegate(Dictionary<string, object> responseFailure)
					{
						GameControllerExample.extraMessage = "Notification failed to post:\n" + Json.Serialize(responseFailure);
					});
				}
				else
				{
					GameControllerExample.extraMessage = "ERROR: Device is not registered.";
				}
			});
		}
		if (GameControllerExample.extraMessage != null)
		{
			guistyle2.alignment = TextAnchor.UpperLeft;
			guistyle2.wordWrap = true;
			GUI.Box(new Rect(10f, 390f, (float)(Screen.width - 20), (float)(Screen.height - 400)), GameControllerExample.extraMessage, guistyle2);
		}
	}

	private static string extraMessage;
}
