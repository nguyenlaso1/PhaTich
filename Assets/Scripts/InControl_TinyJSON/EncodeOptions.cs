// @sonhg: class: InControl.TinyJSON.EncodeOptions
using System;

namespace InControl.TinyJSON
{
	[Flags]
	public enum EncodeOptions
	{
		None = 0,
		PrettyPrint = 1,
		NoTypeHints = 2
	}
}
