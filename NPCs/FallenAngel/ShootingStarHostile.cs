﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.FallenAngel
{
	public class ShootingStarHostile : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.height = 30;
			projectile.width = 10;
		}

		int target;

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 215, 105), new Color(105, 213, 255)), new RoundCap(), new SleepingStarTrailPosition(), 14f, 150f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 215, 105) * .5f, new Color(105, 213, 255) * .5f), new RoundCap(), new SleepingStarTrailPosition(), 56f, 250f, new DefaultShader());
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new SleepingStarTrailPosition(), 12f, 80f, new DefaultShader());
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new SleepingStarTrailPosition(), 12f, 80f, new DefaultShader());
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.2f), new RoundCap(), new SleepingStarTrailPosition(), 56f, 30f, new DefaultShader());
		}

		public override void AI()
		{
			{
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
				if (projectile.ai[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
					target = -1;
					float distance = 2000f;
					for (int k = 0; k < 255; k++) {
						if (Main.player[k].active && !Main.player[k].dead) {
							Vector2 center = Main.player[k].Center;
							float currentDistance = Vector2.Distance(center, projectile.Center);
							if (currentDistance < distance || target == -1) {
								distance = currentDistance;
								target = k;
							}
						}
					}
					if (target != -1) {
						projectile.ai[0] = 1;
						projectile.netUpdate = true;
					}
				}
				else if (target >= 0 && target < Main.maxPlayers) {
					Player targetPlayer = Main.player[target];
					if (!targetPlayer.active || targetPlayer.dead) {
						target = -1;
						projectile.ai[0] = 0;
						projectile.netUpdate = true;
					}
					else {
						float currentRot = projectile.velocity.ToRotation();
						Vector2 direction = targetPlayer.Center - projectile.Center;
						float targetAngle = direction.ToRotation();
						if (direction == Vector2.Zero)
							targetAngle = currentRot;

						float desiredRot = currentRot.AngleLerp(targetAngle, 0.1f);
						projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(desiredRot);
					}
				};
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GoldCoin, 0f, -2f, 0, default, .6f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 5.5f;
				}
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
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 100);
		}
	}
}
