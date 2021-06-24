using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Bismite
{
	public class BismiteHamaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Hamaxe");
		}


		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 30;
			item.value = 1000;
			item.rare = ItemRarityID.Blue;
			item.hammer = 40;
			item.axe = 6;
			item.damage = 6;
			item.knockBack = 4;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 18;
			item.useAnimation = 23;
			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
