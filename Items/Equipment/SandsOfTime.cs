using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	public class SandsOfTime : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sands of Time");
			Tooltip.SetDefault("Summons or ends a sandstorm\nOnly usable in a desert");
		}

		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 48;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.noMelee = true;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;
			item.mana = 20;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
		}

		public override bool CanUseItem(Player player) => player.ZoneDesert;

		public override bool UseItem(Player player)
		{
			if(Sandstorm.Happening) {
				Sandstorm.Happening = false;
			}
			else {
				Sandstorm.Happening = true;
				Sandstorm.TimeLeft = 6000;
				Sandstorm.Severity = 1;
				Sandstorm.IntendedSeverity = 1;
			}

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			return true;
		}
	}
}