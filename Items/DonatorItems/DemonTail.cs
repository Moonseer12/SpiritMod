﻿using SpiritMod.Items.Sets.BloodcourtSet;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class DemonTail : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Tail");
			Tooltip.SetDefault("Summons an eldrich abomination to follow you");
		}

		public override void SetDefaults()
		{
			item.UseSound = SoundID.Item2;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useAnimation = 20;
			item.useTime = 20;

			item.width = 22;
			item.height = 32;

			item.value = Item.sellPrice(0, 0, 54, 0);
			item.rare = ItemRarityID.LightRed;
			item.noMelee = true;

			item.buffType = ModContent.BuffType<LoomingPresence>();
			item.shoot = ModContent.ProjectileType<DemonicBlob>();
		}

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(item.buffType, 10);
			return true;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BlackLens);
			recipe.AddIngredient(ItemID.WaterCandle);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 5);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
