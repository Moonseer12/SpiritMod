using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class TerraProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Wrath");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.penetrate = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 180;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = true;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.TerraBlade, 0f, 0f);

			if (projectile.localAI[0] == 0f) {
				AdjustMagnitude(ref projectile.velocity);
				projectile.localAI[0] = 1f;
			}

			Vector2 move = Vector2.Zero;
			float distance = 400f;
			bool target = false;
			for (int k = 0; k < 200; k++) {
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5) {
					Vector2 newMove = Main.npc[k].Center - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance) {
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}

			if (target) {
				AdjustMagnitude(ref move);
				projectile.velocity = (10 * projectile.velocity + move) / 11f;
				AdjustMagnitude(ref projectile.velocity);
			}

			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			Dust.NewDust(projectile.position + projectile.velocity * 0, projectile.width, projectile.height, DustID.GreenFairy, projectile.oldVelocity.X * 0, projectile.oldVelocity.Y * 0);
			return false;
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
				vector *= 6f / magnitude;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int debuff = Main.rand.Next(3);
			if (debuff == 0)
				target.AddBuff(BuffID.Ichor, 300, true);
			else if (debuff == 1)
				target.AddBuff(BuffID.Frostburn, 300, true);
			else
				target.AddBuff(BuffID.CursedInferno, 300, true);
		}

	}
}

