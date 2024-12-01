// @sonhg: class: SettlersEngine.IPathNode<TUserContext>
using System;

namespace SettlersEngine
{
	public interface IPathNode<TUserContext>
	{
		bool IsWalkable(TUserContext inContext);
	}
}
