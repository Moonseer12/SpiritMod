using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
	public class IlluminantBag : ModItem
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(4, 10));
			player.QuickSpawnItem(mod.ItemType("CrystalShield"));
			player.QuickSpawnItem(mod.ItemType("IlluminatedCrystal"), Main.rand.Next(32, 44));
			if (Main.rand.Next(8) < 1)
				player.QuickSpawnItem(mod.ItemType("RadiantEmblem"));

			string[] lootTable = { "SylphBow", "FairystarStaff", "FaeSaber", "GastropodStaff", };
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(mod.ItemType(lootTable[loot]));

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(Armor.Masks.IlluminantMask._type);
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(Boss.Trophy7._type);
		}
	}
}
