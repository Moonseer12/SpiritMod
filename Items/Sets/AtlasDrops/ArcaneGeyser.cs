using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AtlasDrops
{
	public class ArcaneGeyser : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Geyser");
			Tooltip.SetDefault("'The rock overflows with energy'");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.maxStack = 999;
			item.rare = ItemRarityID.Cyan;
			item.value = Item.sellPrice(0, 0, 15, 0);
		}
	}
}
