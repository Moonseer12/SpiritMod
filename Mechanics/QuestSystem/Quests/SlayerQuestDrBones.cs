﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestDrBones : Quest
    {
        public override string QuestName => "Zombies... Why Zombies";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "My colleague, an expert archaeologist, went roamin' the jungle for some ancient temple. He didn't make it, though. Reports have told me that he's still roamin' the Jungle surface as a zombie. Mind going out there and puttin' him to rest for me? He's been exploring enough.";
		public override int Difficulty => 2;
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.ArchaeologistsJacket, 1),
            (Terraria.ID.ItemID.ArchaeologistsPants, 1),
            (Terraria.ID.ItemID.TigerSkin, 1),
			(ModContent.ItemType<Items.Weapon.Thrown.TargetBottle>(), 25),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};
        public SlayerQuestDrBones()
        {
            _tasks.AddTask(new SlayTask(Terraria.ID.NPCID.DoctorBones, 1));    
        }
        public override void OnActivate()
		{
			QuestGlobalNPC.OnEditSpawnPool += QuestGlobalNPC_OnEditSpawnPool;
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			QuestGlobalNPC.OnEditSpawnPool -= QuestGlobalNPC_OnEditSpawnPool;
			base.OnDeactivate();
		}
        private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			if (!Main.dayTime && spawnInfo.player.ZoneJungle && !spawnInfo.playerSafe && spawnInfo.spawnTileY < Main.worldSurface && !NPC.AnyNPCs(Terraria.ID.NPCID.DoctorBones))
			{
				pool[Terraria.ID.NPCID.DoctorBones] = 0.09f;
			}
		}
    }
}