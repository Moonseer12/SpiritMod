using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria.ID;
using Terraria.ModLoader;
using ReachPlatform = SpiritMod.Tiles.Furniture.Reach.ReachPlatform;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachPlatformTile : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Platform");
		}
		public override void SetDefaults()
		{
			item.width = 8;
			item.height = 10;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<ReachPlatform>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>());
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}