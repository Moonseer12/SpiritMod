﻿namespace SpiritMod
{
	public enum MessageType : byte
	{
		None = 0,
		ProjectileData,
		Dodge,
		Dash,
		PlayerGlyph,
		AuroraData,
		BossSpawnFromClient,
		StartTide,
		TideData,
		TameAuroraStag,
		SpawnTrail,
		PlaceSuperSunFlower,
		DestroySuperSunFlower,
		SpawnExplosiveBarrel,
		StarjinxData,
		BoonData,
		RequestQuestManager,
		RecieveQuestManager,
		Quest,
	}

	public enum QuestMessageType : byte
	{
		Deactivate = 0,
		Activate,
		ProgressOrComplete,
		SyncOnNPCLoot,
		SyncOnEditSpawnPool,
		ObtainQuestBook,
		Unlock,
		SyncNPCQueue
	}
}
