using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ToolsMisc.OreWarhammersSet
{
	public class PalladiumWarhammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Warhammer");
		}


		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 38;
			item.value = 10000;
			item.rare = ItemRarityID.LightRed;

			item.hammer = 85;

			item.damage = 41;
			item.knockBack = 6;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;

			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;

			item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PalladiumBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}