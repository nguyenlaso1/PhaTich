// @sonhg: class: SFSErrorCode
using System;

public class SFSErrorCode
{
	public const int HANDSHAKE_API_OBSOLETE = 0;

	public const int LOGIN_BAD_ZONENAME = 1;

	public const int LOGIN_BAD_USERNAME = 2;

	public const int LOGIN_BAD_PASSWORD = 3;

	public const int LOGIN_BANNED_USER = 4;

	public const int LOGIN_ZONE_FULL = 5;

	public const int LOGIN_ALREADY_LOGGED = 6;

	public const int LOGIN_SERVER_FULL = 7;

	public const int LOGIN_INACTIVE_ZONE = 8;

	public const int LOGIN_NAME_CONTAINS_BAD_WORDS = 9;

	public const int LOGIN_GUEST_NOT_ALLOWED = 10;

	public const int LOGIN_BANNED_IP = 11;

	public const int ROOM_DUPLICATE_NAME = 12;

	public const int CREATE_ROOM_BAD_GROUP = 13;

	public const int ROOM_NAME_BAD_SIZE = 14;

	public const int ROOM_NAME_CONTAINS_BADWORDS = 15;

	public const int CREATE_ROOM_ZONE_FULL = 16;

	public const int CREATE_ROOM_EXCEED_USER_LIMIT = 17;

	public const int CREATE_ROOM_WRONG_PARAMETER = 18;

	public const int JOIN_ALREADY_JOINED = 19;

	public const int JOIN_ROOM_FULL = 20;

	public const int JOIN_BAD_PASSWORD = 21;

	public const int JOIN_BAD_ROOM = 22;

	public const int JOIN_ROOM_LOCKED = 23;

	public const int SUBSCRIBE_GROUP_ALREADY_SUBSCRIBED = 24;

	public const int SUBSCRIBE_GROUP_NOT_FOUND = 25;

	public const int UNSUBSCRIBE_GROUP_NOT_SUBSCRIBED = 26;

	public const int UNSUBSCRIBE_GROUP_NOT_FOUND = 27;

	public const int GENERIC_ERROR = 28;

	public const int ROOM_NAME_CHANGE_PERMISSION_ERR = 29;

	public const int ROOM_PASS_CHANGE_PERMISSION_ERR = 30;

	public const int ROOM_CAPACITY_CHANGE_PERMISSION_ERR = 31;

	public const int SWITCH_NO_PLAYER_SLOTS_AVAILABLE = 32;

	public const int SWITCH_NO_SPECTATOR_SLOTS_AVAILABLE = 33;

	public const int SWITCH_NOT_A_GAME_ROOM = 34;

	public const int SWITCH_NOT_JOINED_IN_ROOM = 35;

	public const int BUDDY_LIST_LOAD_FAILURE = 36;

	public const int BUDDY_LIST_FULL = 37;

	public const int BUDDY_BLOCK_FAILURE = 38;

	public const int BUDDY_TOO_MANY_VARIABLES = 39;

	public const int JOIN_GAME_ACCESS_DENIED = 40;

	public const int JOIN_GAME_NOT_FOUND = 41;

	public const int INVITATION_NOT_VALID = 42;
}
