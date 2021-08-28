using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class OakHeart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oak Heart");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 16;

			projectile.aiStyle = 1;
			projectile.aiStyle = 113;

			projectile.friendly = true;

			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 1;
			projectile.spriteDirection = Main.rand.NextBool() ? 1 : -1;
			aiType = ProjectileID.BoneJavelin;
			projectile.melee = true;
			projectile.netUpdate = true;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + ((projectile.spriteDirection == 1) ? 1.57f : 0);
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(6) == 0) {
				for (int k = 0; k < 5; k++) {
					int p = Projectile.NewProjectile(target.Center.X + Main.rand.Next(-20, 20), target.position.Y - 60, 0f, 8f, ModContent.ProjectileType<PoisonCloud>(), projectile.damage / 2, 0f, projectile.owner, 0f, 0f);
					Main.projectile[p].penetrate = 2;

				}
			}
			MyPlayer mp = Main.player[projectile.owner].GetSpiritPlayer();
			if (mp.sacredVine && Main.rand.Next(2) == 0)
				target.AddBuff(ModContent.BuffType<PollinationPoison>(), 200, true);

			else
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Poisoned, 200, true);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GrassBlades);
			}
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
		    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		    for (int k = 0; k < projectile.oldPos.Length; k++)
		    {
		        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		        Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		       spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.oldRot[k], drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		    }
		    return true;
		}
	}
}