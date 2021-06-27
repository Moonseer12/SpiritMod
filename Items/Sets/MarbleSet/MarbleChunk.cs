using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet
{
	public class MarbleChunk : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Marble Chunk");
			Tooltip.SetDefault("'Contains fragments of past civilizations'");
		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 36;
			item.maxStack = 999;
			item.rare = ItemRarityID.Green;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<MarbleOre>();
		}
	}
}