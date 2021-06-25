using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.InfernonDrops
{
	[AutoloadEquip(EquipType.Shield)]
	public class InfernalShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Shield");
			Tooltip.SetDefault("Double tap a direction to dash in flames\nReduces damage taken by 5%");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.rare = ItemRarityID.Pink;
			item.value = 80000;
			item.damage = 36;
			item.defense = 3;
			item.melee = true;
			item.accessory = true;

			item.knockBack = 5f;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().infernalShield = true;
			player.endurance += 0.05f;
		}
	}
}
