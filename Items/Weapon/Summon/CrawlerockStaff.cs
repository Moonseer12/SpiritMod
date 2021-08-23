using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class CrawlerockStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crawlerock Staff");
			Tooltip.SetDefault("Summons bouncing mini crawlers to fight for you");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Green;
			item.mana = 11;
			item.damage = 13;
			item.knockBack = 3;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.buffType = ModContent.BuffType<CrawlerockMinionBuff>();
            item.shoot = ModContent.ProjectileType<Crawlerock>();
			item.UseSound = SoundID.Item44;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			player.AddBuff(item.buffType, 2);
			position = Main.MouseWorld;
			return player.altFunctionUse != 2;
		}
	}
}