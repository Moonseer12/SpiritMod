﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class JellyfishOrbiter : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Energy");
            Main.projFrames[projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
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
            projectile.hide = false;
			projectile.timeLeft = Main.rand.Next(76, 84);
		}

        float alphaCounter;

		public override void AI()
        {
            alphaCounter += .04f;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
            projectile.frameCounter++;
            projectile.spriteDirection = -projectile.direction;
            if (projectile.frameCounter >= 4)
            {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }
            int num1 = ModContent.NPCType<MoonWizard>();
            float num2 = 60f;
            float x = 0.08f;
            float y = 0.1f;
            bool flag2 = false;

            if ((double)projectile.ai[0] < (double)num2)
            {
                bool flag4 = true;
                int index1 = (int)projectile.ai[1];
                if (Main.npc[index1].active && Main.npc[index1].type == num1)
                {
                    if (!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
                        projectile.position = projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
                }
                else
                {
                    projectile.ai[0] = num2;
                    flag4 = false;
                }
                if (flag4 && !flag2)
                {
                    projectile.velocity = projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - projectile.Center.Y)) * new Vector2(x, y);
                    if (projectile.velocity.Length() > 7f)
                    {
                        projectile.velocity *= 7f / projectile.velocity.Length();
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((projectile.Center.X + 10) - Main.screenPosition.X) - (int)(Main.projectileTexture[projectile.type].Width / 2);
            int ypos = (int)((projectile.Center.Y + 10) - Main.screenPosition.Y) - (int)(Main.projectileTexture[projectile.type].Width / 2);
            Texture2D ripple = mod.GetTexture("Effects/Masks/Extra_49");
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), projectile.rotation, ripple.Size() / 2f, .5f, spriteEffects, 0);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            float maxDistance = 1000f; // max distance to search for a player
            int index = -1;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
                if (!target.active || target.dead)
                {
                    continue;
                }
                float curDistance = projectile.Distance(target.Center);
                if (curDistance < maxDistance)
                {
                    index = i;
                    maxDistance = curDistance;
                }
            }

			if (index != -1)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 69);
				float speed = Main.expertMode ? Main.rand.NextFloat(15, 17) : Main.rand.NextFloat(11.5f, 12.5f);
				Vector2 direction = Vector2.Normalize(Main.player[index].Center - projectile.Center) * speed;
				Projectile.NewProjectile(projectile.Center, direction, ModContent.ProjectileType<JellyfishOrbiter_Projectile>(), projectile.damage, 0, Main.myPlayer);
			}
        }
	}
}
