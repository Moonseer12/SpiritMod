using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CoilSet
{
	public class TechDrive : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tech Drive");
			Tooltip.SetDefault("'How does a goblin stumble upon such advanced circuitry?'");
		}


		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 24;
			item.value = 100;
			item.rare = ItemRarityID.Green;

			item.maxStack = 999;
		}
	}
}
