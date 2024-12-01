// @sonhg: class: ImageManager
using System;
using System.Collections.Generic;

public class ImageManager
{
	public static void AddToMap(string link, LoadAndCache loadAndCache, bool cache)
	{
		if (!loadAndCache.isCached(link))
		{
			ImageManager._arrCache.Add(cache);
			ImageManager._arrLoadAndCache.Add(loadAndCache);
			ImageManager._arrLinkd.Add(link);
			ImageManager.loadImage();
		}
	}

	public static void Remove()
	{
		if (ImageManager._arrLoadAndCache.Count > 0)
		{
			ImageManager._arrLinkd.RemoveAt(0);
			ImageManager._arrLoadAndCache.RemoveAt(0);
			ImageManager._arrCache.RemoveAt(0);
			ImageManager.isLoading = false;
			ImageManager.loadImage();
		}
	}

	public static void loadImage()
	{
		if (ImageManager._arrLoadAndCache.Count > 0 && !ImageManager.isLoading)
		{
			ImageManager.isLoading = true;
			if (ImageManager._arrLoadAndCache[0] != null)
			{
				if (ImageManager._arrLoadAndCache[0].isActiveAndEnabled)
				{
					ImageManager._arrLoadAndCache[0].Load(ImageManager._arrLinkd[0], ImageManager._arrCache[0]);
				}
				else
				{
					ImageManager.Remove();
				}
			}
			else
			{
				ImageManager.Remove();
			}
		}
	}

	public static bool isLoading = false;

	private static List<string> _arrLinkd = new List<string>();

	private static List<LoadAndCache> _arrLoadAndCache = new List<LoadAndCache>();

	private static List<bool> _arrCache = new List<bool>();
}
