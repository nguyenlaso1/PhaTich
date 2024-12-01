// @sonhg: class: ResourcesUltis
using System;
using UnityEngine;

public class ResourcesUltis : MonoBehaviour
{
	public static string MapIdToLink(string _id)
	{
		if (ResourcesManager.MapDict.ContainsKey(_id))
		{
			return ResourcesManager.MapDict[_id].Thumb;
		}
		return string.Empty;
	}

	public static string TileIdToLink(string _id)
	{
		if (ResourcesManager.TilesDict.ContainsKey(_id))
		{
			return ResourcesManager.TilesDict[_id].Path;
		}
		return string.Empty;
	}

	public static int TileIdToType(string _id)
	{
		if (ResourcesManager.TilesDict.ContainsKey(_id))
		{
			return ResourcesManager.TilesDict[_id].Category;
		}
		return -1;
	}

	public static string ItemIdToLink(string _id)
	{
		if (ResourcesManager.ItemsDict.ContainsKey(_id.ToString()))
		{
			return ResourcesManager.ItemsDict[_id.ToString()].Path;
		}
		return string.Empty;
	}

	public static string ItemIdToIconLink(string _id)
	{
		if (ResourcesManager.ItemsDict.ContainsKey(_id.ToString()))
		{
			return ResourcesManager.ItemsDict[_id.ToString()].Icon;
		}
		return string.Empty;
	}
}
