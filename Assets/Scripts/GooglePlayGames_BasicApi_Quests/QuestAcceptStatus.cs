// @sonhg: class: GooglePlayGames.BasicApi.Quests.QuestAcceptStatus
using System;

namespace GooglePlayGames.BasicApi.Quests
{
	public enum QuestAcceptStatus
	{
		Success,
		BadInput,
		InternalError,
		NotAuthorized,
		Timeout,
		QuestNoLongerAvailable,
		QuestNotStarted
	}
}
