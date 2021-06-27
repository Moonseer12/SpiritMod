using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria.ID;
using Terraria.ModLoader;
using ReachBookcaseTile = SpiritMod.Tiles.Furniture.Reach.ReachBookcase;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachBookcase : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Bookcase");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.value = 500;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<ReachBookcaseTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 20);
			recipe.AddIngredient(ItemID.Book, 10);
			recipe.AddTile(TileID.Sawmill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}