
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
	[AutoloadEquip(EquipType.Shield)]
	public class ClatterboneShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fervent Protector");
			Tooltip.SetDefault("Increases defense by 2 and reduces movement speed by 4% for every nearby enemy\nThis effect stacks up to five times\n'There is something special between us'");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.rare = ItemRarityID.Green;
			item.defense = 2;
			item.melee = true;
			item.accessory = true;
			item.value = Item.buyPrice(0, 0, 5, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().clatterboneShield = true;

			player.statDefense += 2 * player.GetSpiritPlayer().clatterStacks;
			player.moveSpeed -= .04f * player.GetSpiritPlayer().clatterStacks;
			player.maxRunSpeed -= .04f * player.GetSpiritPlayer().clatterStacks;
			player.runAcceleration -= .005f * player.GetSpiritPlayer().clatterStacks;
		}
	}
}
