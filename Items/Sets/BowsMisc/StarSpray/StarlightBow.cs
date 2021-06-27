using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.StarSpray
{
	public class StarlightBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Spray");
			Tooltip.SetDefault("Converts wooden arrows into stars");
		}



		public override void SetDefaults()
		{
			item.damage = 17;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 40;
			item.useTime = 27;
			item.useAnimation = 27;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Starfury;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shootSpeed = 11f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ProjectileID.Starfury;
			}
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Projectile projectile = Main.projectile[proj];
			for (int k = 0; k < 15; k++) {
				Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
				Vector2 offset = mouse - player.position;
				offset.Normalize();
				offset *= 23f;
				int dust = Dust.NewDust(projectile.Center + offset, projectile.width, projectile.height, 71);

				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				//        Main.dust[dust].scale *= 2f;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				Main.dust[dust].position = (projectile.Center + offset) - vector2_3;
			}
			return false;
		}
	}
}