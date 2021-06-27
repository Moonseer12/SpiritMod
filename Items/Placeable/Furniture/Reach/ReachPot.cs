using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria.ID;
using Terraria.ModLoader;
using ReachPotTile = SpiritMod.Tiles.Furniture.Reach.ReachPotTile;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachPot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Pot");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.value = 200;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<ReachPotTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}