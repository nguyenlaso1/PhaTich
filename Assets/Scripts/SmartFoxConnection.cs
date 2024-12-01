// @sonhg: class: SmartFoxConnection
using System;
using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Util;
using UnityEngine;

public class SmartFoxConnection : MonoBehaviour
{
	public static SmartFox Connection
	{
		get
		{
			if (SmartFoxConnection.mInstance == null)
			{
				SmartFoxConnection.mInstance = (new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection);
			}
			return SmartFoxConnection.smartFox;
		}
		set
		{
			if (SmartFoxConnection.mInstance == null)
			{
				SmartFoxConnection.mInstance = (new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection);
			}
			SmartFoxConnection.smartFox = value;
		}
	}

	public static bool IsInitialized
	{
		get
		{
			return SmartFoxConnection.smartFox != null;
		}
	}

	public static bool IsConnected
	{
		get
		{
			return SmartFoxConnection.smartFox != null && SmartFoxConnection.smartFox.IsConnected;
		}
	}

	public static Room LastJoinedRoom
	{
		get
		{
			return SmartFoxConnection.smartFox.LastJoinedRoom;
		}
	}

	public static void Destroy()
	{
		if (SmartFoxConnection.smartFox != null)
		{
			SmartFoxConnection.smartFox.Send(new ExtensionRequest("p-disconnect", new SFSObject()));
			SmartFoxConnection.smartFox.Disconnect();
			SmartFoxConnection.smartFox.RemoveAllEventListeners();
			SmartFoxConnection.smartFox = null;
		}
	}

	public static void UnregisterSFSSceneCallbacks()
	{
		if (SmartFoxConnection.smartFox != null)
		{
			SmartFoxConnection.smartFox.RemoveAllEventListeners();
		}
	}

	public static void EnablePingPong()
	{
		if (SmartFoxConnection.IsConnected)
		{
			SmartFoxConnection.smartFox.EnableLagMonitor(true, 4);
		}
	}

	public static SmartFox ConnectServer(string ServerName, int ServerPort, int[] customErrorCodes = null)
	{
		SmartFoxConnection.Destroy();
		SmartFoxConnection.smartFox = new SmartFox(false);
		if (customErrorCodes != null)
		{
			for (int i = 0; i < customErrorCodes.Length; i++)
			{
				SFSErrorCodes.SetErrorMessage(customErrorCodes[i], "{0}");
			}
		}
		SmartFoxConnection.smartFox.Connect(ServerName, ServerPort);
		Config.serverName = ServerName;
		return SmartFoxConnection.smartFox;
	}

	private static SmartFoxConnection mInstance;

	private static SmartFox smartFox;

	private static bool debug;
}
