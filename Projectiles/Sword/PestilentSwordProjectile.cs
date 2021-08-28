using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Sword
{
	public class PestilentSwordProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pestilent Shard");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 16;

			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;

			projectile.ranged = true;
			projectile.friendly = true;

			projectile.penetrate = 5;
			projectile.timeLeft = 600;
		}

		public override void AI()
		{
			for (int i = 0; i < 10; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, DustID.GreenTorch, 0f, 0f, 0, default, 1f);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].noGravity = true;
				Main.dust[num].velocity *= 0f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(ModContent.BuffType<BlightedFlames>(), 260, false);

			MyPlayer mp = Main.player[projectile.owner].GetSpiritPlayer();
			mp.PutridHits++;
			if (mp.putridSet && mp.PutridHits >= 4) {
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, ModContent.ProjectileType<CursedFlame>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
				mp.PutridHits = 0;
			}
		}

	}
}
