using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class HowlingBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Howling Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.penetrate = 1;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
			for (int num621 = 0; num621 < 40; num621++) {
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 2f);
				Main.dust[num622].velocity *= 3f;
				Main.dust[num622].noGravity = true;
				Main.dust[num622].scale = 0.5f;
			}
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
			delegate {
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default, 1.1f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X += (Main.rand.Next(-30, 31) / 20) - 1.5f;
					dust.position.Y += (Main.rand.Next(-30, 31) / 20) - 1.5f;
					if (dust.position != projectile.Center)
						dust.velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			});
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(220, 220, 220, 100);
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3) {
				projectile.tileCollide = false;
				projectile.ai[1] = 0f;
				projectile.alpha = 255;
				projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
				projectile.width = 30;
				projectile.height = 30;
				projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
				projectile.knockBack = 4f;
			}

			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueCrystalShard);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust].scale *= Main.rand.NextFloat(.4f, .6f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Frostburn, 180);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}