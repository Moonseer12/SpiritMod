using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class CursedBone : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pestilent Bone");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 3;
			projectile.alpha = 255;
			projectile.timeLeft = 150;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<FelBrand>(), 300);
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.CursedTorch, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].noGravity = true;

			return true;
		}

	}
}
