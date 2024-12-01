// @sonhg: class: InControl.TinyJSON.DecodeException
using System;

namespace InControl.TinyJSON
{
	public sealed class DecodeException : Exception
	{
		public DecodeException(string message) : base(message)
		{
		}

		public DecodeException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
