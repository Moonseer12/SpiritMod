using SpiritMod.Tiles.Furniture.NeonLights;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Placeable.Furniture.Neon
{
	public class BlueFairyLights : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Fairy Lights");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 28;
			item.value = item.value = Terraria.Item.buyPrice(0, 0, 1, 0);
			item.rare = ItemRarityID.Blue;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<BlueLightsSmall>();
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Material.SynthMaterial>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}