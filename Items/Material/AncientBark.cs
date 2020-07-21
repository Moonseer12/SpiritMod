using SpiritMod.Tiles.Block;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class AncientBark : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark");

		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.maxStack = 999;
			item.value = 800;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 7;
			item.useAnimation = 15;
			item.rare = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<BarkTileTile>();
		}
	}
}
