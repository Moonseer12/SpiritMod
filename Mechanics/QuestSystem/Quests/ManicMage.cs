﻿using System.Collections.Generic;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ManicMage : Quest
    {
        public override string QuestName => "The Manic Mage";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Scouts at the far ends of the world have reported that a witch seems to be terrorizin' the area. Apparently, it's some type of harpy with a real dangerous staff. Mission's real simple this time. Bring me its head! Er... I promise I'm not unhinged. I mean, bring me its hat! Yeah.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Sets.MagicMisc.Lightspire.AkaviriStaff>(), 1),
			(Terraria.ID.ItemID.GoldCoin, 3)
		};

		private ManicMage()
        {
			_tasks.AddParallelTasks(
					new SlayTask(ModContent.NPCType<NPCs.DarkfeatherMage.DarkfeatherMage>(), 1), 
					new RetrievalTask(ModContent.ItemType<Items.Accessory.DarkfeatherVisage.DarkfeatherVisage>(), 1));
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

		private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) => ModifySpawnRateUnique(pool, ModContent.NPCType<NPCs.DarkfeatherMage.DarkfeatherMage>(), 0.75f);
    }
}