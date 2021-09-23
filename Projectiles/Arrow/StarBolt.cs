using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Projectiles.Arrow
{
	public class StarBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(82);
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.width = 28;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.damage = 10;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 180);
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.UnusedWhiteBluePurple);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);

			for (int num623 = 0; num623 < 20; num623++) {
				int num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.UnusedWhiteBluePurple);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 1.5f;
				num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.UnusedWhiteBluePurple);
				Main.dust[num624].velocity *= 2f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 100);
	}
}