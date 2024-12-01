// @sonhg: class: AudienceNetwork.Utility.AdUtilityBridge
using System;
using UnityEngine;

namespace AudienceNetwork.Utility
{
	internal class AdUtilityBridge : IAdUtilityBridge
	{
		internal AdUtilityBridge()
		{
		}

		private static IAdUtilityBridge createInstance()
		{
			if (Application.platform != RuntimePlatform.OSXEditor)
			{
				return new AdUtilityBridgeAndroid();
			}
			return new AdUtilityBridge();
		}

		public virtual double deviceWidth()
		{
			return 2208.0;
		}

		public virtual double deviceHeight()
		{
			return 1242.0;
		}

		public virtual double width()
		{
			return 1104.0;
		}

		public virtual double height()
		{
			return 621.0;
		}

		public virtual double convert(double deviceSize)
		{
			return 2.0;
		}

		public virtual void prepare()
		{
		}

		public static readonly IAdUtilityBridge Instance = AdUtilityBridge.createInstance();
	}
}
