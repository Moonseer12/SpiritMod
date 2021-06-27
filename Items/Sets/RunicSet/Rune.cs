using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.RunicSet
{
	public class Rune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Rune");
			Tooltip.SetDefault("'It's inscribed in some archaic language'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 5));
			ItemID.Sets.ItemNoGravity[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;
		}


		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 42;
			item.value = 100;
			item.rare = ItemRarityID.Pink;
			item.maxStack = 999;
		}
	}
}