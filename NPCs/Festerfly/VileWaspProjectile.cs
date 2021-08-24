﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Festerfly
{
	public class VileWaspProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pesterfly Hatchling");
			Main.projFrames[projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.penetrate = 2;
			projectile.timeLeft = 600;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			projectile.spriteDirection = -projectile.direction;
			if (projectile.frameCounter >= 4) {
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
			int num1 = ModContent.NPCType<Vilemoth>();
			if (!Main.npc[(int)projectile.ai[1]].active) {
				projectile.velocity.X = -4 * Main.npc[(int)projectile.ai[1]].spriteDirection;
				projectile.velocity.Y = -3;
			}
			float num2 = 120f;
			float x = 0.85f;
			float y = 0.35f;
			bool flag2 = false;
			if ((double)projectile.ai[0] < (double)num2) {
				bool flag4 = true;
				int index1 = (int)projectile.ai[1];
				if (Main.npc[index1].active && Main.npc[index1].type == num1) {
					if (!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
						projectile.position = projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
				}
				else {
					projectile.ai[0] = num2;
					flag4 = false;
				}
				if (flag4 && !flag2) {
					projectile.velocity = projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - projectile.Center.Y)) * new Vector2(x, y);
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, (projectile.height / Main.projFrames[projectile.type]) * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				var effects = projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{

		}
	}
}
