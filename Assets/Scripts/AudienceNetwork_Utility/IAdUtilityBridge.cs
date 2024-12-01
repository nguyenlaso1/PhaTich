// @sonhg: class: AudienceNetwork.Utility.IAdUtilityBridge
using System;

namespace AudienceNetwork.Utility
{
	internal interface IAdUtilityBridge
	{
		double deviceWidth();

		double deviceHeight();

		double width();

		double height();

		double convert(double deviceSize);

		void prepare();
	}
}
