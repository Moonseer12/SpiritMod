using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria.ID;
using Terraria.ModLoader;
using ReachCandelabraTile = SpiritMod.Tiles.Furniture.Reach.ReachCandelabra;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachCandelabra : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Candelabra");
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

			item.createTile = ModContent.TileType<ReachCandelabraTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 5);
			recipe.AddIngredient(ItemID.Torch, 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}