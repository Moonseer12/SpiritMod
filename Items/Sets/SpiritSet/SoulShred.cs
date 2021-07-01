
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class SoulShred : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Ember");
			Tooltip.SetDefault("'A part of the everburning Soul'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 6));
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void SetDefaults()
		{
			item.rare = ItemRarityID.Pink;
			item.width = 14;
			item.maxStack = 99;
			item.height = 36;
		}
	}
}