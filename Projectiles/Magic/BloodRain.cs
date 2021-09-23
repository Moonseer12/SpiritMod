using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BloodRain : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Rain");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 120;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 6);
			for (int I = 0; I < 8; I++)
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Blood, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
			for (int i = 0; i < 3; ++i) {
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
					Main.rand.Next(-400, 400) / 100, Main.rand.Next(-4, 4),
					ModContent.ProjectileType<Terrorflame>(), projectile.damage, 0, projectile.owner);
			}
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(4) == 1)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Blood, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<Wither>(), 180);
		}
	}
}
