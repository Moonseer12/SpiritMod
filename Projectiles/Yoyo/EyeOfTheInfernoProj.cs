using SpiritMod.Buffs;
using SpiritMod.Projectiles.Flail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class EyeOfTheInfernoProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye of the Inferno");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Yelets);
			projectile.extraUpdates = 1;
			aiType = ProjectileID.Yelets;
		}

		public override void AI()
		{
			projectile.localAI[1] += 1f;
			int num = 1;
			int num2 = 1;
			if (projectile.localAI[1] <= 1.0) {
				return;
			}

			int num4 = (int)projectile.localAI[1];
			if (num4 <= 30) {
				if (num4 != 10) {
					if (num4 == 30) {
						num2--;
					}
				}
				else {
					num2--;
				}
			}
			else if (num4 != 50) {
				if (num4 == 70) {
					num2--;
				}
			}
			else {
				num2--;
			}

			if ((int)projectile.localAI[1] == 20) {
				int num5 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, num, num2, ModContent.ProjectileType<SpikeBall>(), 50, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num5].localAI[0] = projectile.whoAmI;
			}

			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<StackingFireBuff>(), 280);
			projectile.velocity *= 0f;
		}

	}
}
