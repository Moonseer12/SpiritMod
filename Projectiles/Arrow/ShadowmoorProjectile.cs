﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Arrow
{
	public class ShadowmoorProjectile : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darklight Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
            projectile.alpha = 100;
			projectile.timeLeft = 200;
			projectile.height = 20;
			projectile.width = 10;
			projectile.ranged = true;
			aiType = ProjectileID.DeathLaser;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			switch (Main.rand.Next(3)) {
				case 0:
					tManager.CreateTrail(projectile, new GradientTrail(new Color(142, 8, 255), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 90f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
					break;
				case 1:
					tManager.CreateTrail(projectile, new GradientTrail(new Color(222, 84, 128), new Color(190, 72, 194)), new RoundCap(), new SleepingStarTrailPosition(), 90f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
					break;
				case 2:
					tManager.CreateTrail(projectile, new GradientTrail(new Color(126, 55, 250), new Color(230, 117, 255)), new RoundCap(), new SleepingStarTrailPosition(), 90f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
					break;
			}
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new SleepingStarTrailPosition(), 12f, 80f, new DefaultShader());
		}

		float num;
        bool escaped = false;
        Color colorField;
        bool checkColor = false;
        public override void AI()
		{
			if (!checkColor)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        colorField = new Color(191, 145, 255);
                        break;
                    case 1:
                        colorField = new Color(215, 135, 255);
                        break;
                    case 2:
                        colorField = new Color(223, 94, 255);
                        break;
                }
                checkColor = true;
            }
			if (projectile.timeLeft >= 290) {
				projectile.tileCollide = false;
			}
			else {
				projectile.tileCollide = true;
			}
            Lighting.AddLight(projectile.position, 0.205f*1.85f, 0.135f*1.85f, 0.255f*1.85f);
            num += .4f;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            float distance = Vector2.Distance(projectile.Center, Main.MouseWorld);
			if (distance < 20f)
            {
                DustHelper.DrawDiamond(new Vector2(projectile.Center.X, projectile.Center.Y), 173, 4, .8f, .75f);
                Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 6);
                if (!escaped && Main.rand.NextBool(2))
                {
                    projectile.ai[1] = 10;
                }
                escaped = true;
            }
			if (projectile.ai[1] == 10)
            {
                Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
                projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-3, 3) * (Math.PI / 40));
            }
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(5) == 0)
                target.AddBuff(BuffID.ShadowFlame, 180);
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
		public override Color? GetAlpha(Color lightColor)
		{
            return colorField;
		}
		public override void Kill (int timLeft)
        {
            Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
        }
	}
}
