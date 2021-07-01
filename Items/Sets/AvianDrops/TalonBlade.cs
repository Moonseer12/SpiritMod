using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AvianDrops
{
	public class TalonBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon Blade");
			Tooltip.SetDefault("Launches fossilized feathers");
		}


		int charger;
		public override void SetDefaults()
		{
			item.damage = 26;
			item.melee = true;
			item.width = 34;
			item.height = 40;
			item.useTime = 55;
			item.useAnimation = 29;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<BoneFeatherSwordProj>();
			item.shootSpeed = 9f;
			item.crit = 6;
			item.autoReuse = true;
		}
		public override bool OnlyShootOnSwing => true;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 8);
			{
				charger++;
				if (charger >= 5) {
					Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 20);
					for (int I = 0; I < 1; I++) {
						int p = Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<GiantFeather>(), damage, knockBack, player.whoAmI, 0f, 0f);
						Main.projectile[p].ranged = false;
						Main.projectile[p].melee = true;
					}
					charger = 0;
				}
				return true;
			}
		}
	}
}

