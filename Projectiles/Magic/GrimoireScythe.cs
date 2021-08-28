using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class GrimoireScythe : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grimoire Scythe");
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;

			projectile.magic = true;
			projectile.friendly = true;
			projectile.hostile = false;

			projectile.penetrate = 5;
		}

		public override bool PreAI()
		{
			projectile.rotation += 0.1f;
			if (projectile.ai[0]++ >= 60)
				projectile.velocity *= 1.2f;

			if (Main.rand.Next(5) == 0) {
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GreenTorch, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 3f;
				Main.dust[dust].noGravity = true;
			}
			return false;
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
