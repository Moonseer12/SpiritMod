﻿
using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Glyph;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class SlicingGust : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Slicing Gust");

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.timeLeft = 220;
			projectile.height = 14;
			projectile.width = 14;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			if (projectile.localAI[0] == 0f)
			{
				ProjectileExtras.LookAlongVelocity(this);
				projectile.localAI[0] = 1f;
			}

			if (Main.rand.NextDouble() < 0.5)
			{
				Dust dust = Dust.NewDustDirect(projectile.position - new Vector2(4, 4), projectile.width + 8, projectile.height + 8, ModContent.DustType<Wind>());
				dust.velocity = projectile.velocity * 0.2f;
				dust.customData = new WindAnchor(projectile.Center, projectile.velocity, dust.position);
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			hitDirection = 0;
			knockback = 0.12f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.velocity.Y -= projectile.knockBack * target.knockBackResist;
			target.AddBuff(ModContent.BuffType<WindBurst>(), 300);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 6; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<Wind>());
				dust.customData = new WindAnchor(projectile.Center, projectile.velocity, dust.position);
			}
		}
	}
}
