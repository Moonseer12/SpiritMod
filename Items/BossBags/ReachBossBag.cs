using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
	public class ReachBossBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Consumable\nRight Click to open");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.rare = -2;

			item.maxStack = 30;

			item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(1, 3));
			player.QuickSpawnItem(mod.ItemType("DeathRose"));
			player.QuickSpawnItem(mod.ItemType("ReachFlowers"), Main.rand.Next(14, 20));
			if (Main.rand.Next(73) < 3)
				player.QuickSpawnItem(mod.ItemType("RootPod"));

			string[] lootTable = { "ThornBow", "SunbeamStaff", "ReachVineStaff", "ReachBossSword", "ReachKnife" };
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(mod.ItemType(lootTable[loot]));

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(Armor.Masks.ReachMask._type);
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(Boss.Trophy5._type);
		}
	}
}
