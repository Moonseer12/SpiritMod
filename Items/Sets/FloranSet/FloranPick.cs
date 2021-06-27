using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloranSet
{
	public class FloranPick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Pickaxe");
		}


		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 42;
			item.value = 1000;
			item.rare = 1;

			item.pick = 55;

			item.damage = 12;
			item.knockBack = 3;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 15;
			item.useAnimation = 25;

			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;

			item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<FloranBar>(), 15);
			modRecipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 5);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
	}
}
