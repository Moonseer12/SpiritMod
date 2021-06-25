using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AtlasDrops
{
	public class AtlasEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlas Eye");
			Tooltip.SetDefault("Under 50% health, defense is increased by 20, but movement speed is reduced by 1/3\nReduces damage taken by 12%");
		}



		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.Lime;
			item.expert = true;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.defense = 2;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.statLife < player.statLifeMax2 / 2) {
				player.moveSpeed *= 0.66f;
				player.statDefense += 20;
			}
			player.endurance += 0.12f;
		}
	}
}
