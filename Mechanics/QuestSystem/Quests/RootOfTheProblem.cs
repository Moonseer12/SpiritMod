﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class RootOfTheProblem : Quest
    {
        public override string QuestName => "Root of the Problem";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Ever since I was captured by those savages from the Briar, I've been doin' some research on the place. That altar you found me at is supposed to harbor a really venegeful nature spirit. Mind investigating? ";
		public override int Difficulty => 3;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.Masks.GladeWraithMask>(), 1),
			(ModContent.ItemType<Items.Material.FloranOre>(), 15),
			(Terraria.ID.ItemID.HealingPotion, 5),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

        public RootOfTheProblem()
        {
			_tasks.AddParallelTasks(new SlayTask(new int[] { ModContent.NPCType<NPCs.Reach.ForestWraith>()}, 1, "Glade Wraith"), new RetrievalTask(ModContent.ItemType<Items.Consumable.Quest.SacredVine>(), 1));
        }

		public override void OnQuestComplete()
		{
			QuestManager.UnlockQuest<ReturnToYourRoots>(true);
			QuestManager.UnlockQuest<RootOfTheProblem>(true);
            QuestManager.UnlockQuest<SlayerQuestValkyrie>(true);
			QuestManager.UnlockQuest<SlayerQuestDrBones>(true);
			QuestManager.UnlockQuest<SlayerQuestNymph>(true);
			QuestManager.UnlockQuest<SlayerQuestUGDesert>(true);
			QuestManager.UnlockQuest<SlayerQuestCavern>(true);
			base.OnQuestComplete();
		}

		public override void OnActivate()
		{
			QuestGlobalNPC.OnNPCLoot += QuestGlobalNPC_OnNPCLoot;
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			QuestGlobalNPC.OnNPCLoot -= QuestGlobalNPC_OnNPCLoot;
			base.OnDeactivate();
		}

		private void QuestGlobalNPC_OnNPCLoot(NPC npc)
		{
			if (npc.type == ModContent.NPCType<NPCs.Reach.ForestWraith>())
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Consumable.Quest.SacredVine>());
			}
		}
    }
}