// @sonhg: class: GooglePlayGames.BasicApi.Quests.QuestUiResult
using System;

namespace GooglePlayGames.BasicApi.Quests
{
	public enum QuestUiResult
	{
		UserRequestsQuestAcceptance,
		UserRequestsMilestoneClaiming,
		BadInput,
		InternalError,
		UserCanceled,
		NotAuthorized,
		VersionUpdateRequired,
		Timeout,
		UiBusy
	}
}
