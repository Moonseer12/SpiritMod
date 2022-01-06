using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Fish
{
	public class FishChips : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fish n' Chips");
			Tooltip.SetDefault("Minor improvements to all stats\nMakes you sluggish");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 22;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 30;

			item.buffType = BuffID.WellFed;
			item.buffTime = 98000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(ModContent.BuffType<CouchPotato>(), 3600);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ModContent.ItemType<RawFish>(), 5);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();
		}
	}
}
