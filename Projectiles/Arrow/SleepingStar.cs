﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class SleepingStar : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sleeping Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		private bool purple; //just used for client end things, shouldnt matter if synced or not

		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.Size = new Vector2(10, 10);
			purple = Main.rand.NextBool();
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new StandardColorTrail(purple ? new Color(218, 94, 255) : new Color(120, 217, 255)), new RoundCap(), new SleepingStarTrailPosition(), 8f, 250f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 200, true);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			int dusttype = purple ? 223 : 180;
			float dustscale = purple ? 0.6f : 1.2f;
			{
				for (int i = 0; i < 40; i++)
				{
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, dusttype, 0f, -2f, 0, default(Color), dustscale);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != projectile.Center)
					{
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 2f;
					}
				}
			}
		}

		private bool looping = false;
		private int loopCounter = 0;
		private int loopSize = 9;
		public override void AI()
		{
			Lighting.AddLight((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), 0.396f, 0.170588235f, 0.564705882f);
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (!looping) //change direction slightly
			{
				Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
				projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 40));
			}
			if (Main.rand.Next(100) == 1 && !looping)
			{
				loopCounter = 0;
				looping = true;
				loopSize = Main.rand.Next(8, 14);
			}
			if (looping)
			{
				Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
				projectile.velocity = currentSpeed.RotatedBy(Math.PI / loopSize);
				loopCounter++;
				if (loopCounter >= loopSize * 2)
				{
					looping = false;
					loopSize = Main.rand.Next(8, 13);
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(160, 160, 160, 100);
	}
}