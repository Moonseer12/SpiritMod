using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class PlagueSkullProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Skull");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;

			projectile.magic = true;
			projectile.friendly = true;
			projectile.hostile = false;

			projectile.penetrate = 5;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
			if (Main.rand.Next(5) == 0) {
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GreenTorch, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
			}

			projectile.rotation += 0.5f;

			if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3) {
				projectile.tileCollide = false;
				projectile.ai[1] = 0f;
				projectile.alpha = 255;
				projectile.position.X = projectile.position.X + (projectile.width / 3f);
				projectile.position.Y = projectile.position.Y + (projectile.height / 3f);
				projectile.width = 12;
				projectile.height = 12;
				projectile.position.X = projectile.position.X - (projectile.width / 3f);
				projectile.position.Y = projectile.position.Y - (projectile.height / 3f);
				projectile.knockBack = 4f;
				projectile.damage = 40;
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			{
				for (int num621 = 0; num621 < 40; num621++) {
					int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.CursedTorch, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 1.5f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}

				Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 6);
				for (int I = 0; I < 8; I++)
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.CursedTorch, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<FelBrand>(), 180);
		}

	}
}
