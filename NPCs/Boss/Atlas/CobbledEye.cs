﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.Boss.Atlas
{
	public class CobbledEye : ModNPC
	{
		int timer = 0;
		bool start = true;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Cobbled Eye");

		public override void SetDefaults()
		{
			npc.width = 42;
			npc.height = 42;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.damage = 80;

			npc.buffImmune[BuffID.Confused] = true;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;

			npc.lifeMax = 2700;
		}

		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			if (start) {
				for (int num621 = 0; num621 < 15; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Stone, 0f, 0f, 100, default, 2f);
				}
				start = false;
			}

			npc.TargetClosest(true);
			Vector2 direction = Main.player[npc.target].Center - npc.Center;
			direction.Normalize();
			direction *= 9f;
			npc.rotation = direction.ToRotation();
			timer++;
			if (timer > 60) {
				if (Main.rand.Next(4) == 0) {
					for (int num621 = 0; num621 < 5; num621++) {
						Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default, 2f);
					}
					int damage = expertMode ? 39 : 55;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<MiracleBeam>(), damage, 1f, npc.target);
				}
				timer = 0;
			}
			for (int index1 = 0; index1 < 6; ++index1) {
				float x = (npc.Center.X - 22);
				float xnum2 = (npc.Center.X + 22);
				float y = (npc.Center.Y);
				if (npc.direction == -1) {
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, DustID.Electric, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = .85f;
					Main.dust[index2].velocity *= 0.02f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
				else if (npc.direction == 1) {
					int index2 = Dust.NewDust(new Vector2(xnum2, y), 1, 1, DustID.Electric, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position.X = xnum2;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = .85f;
					Main.dust[index2].velocity *= 0.02f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
			}
			int parent = (int)npc.ai[0];
			if (parent < 0 || parent >= Main.maxNPCs || !Main.npc[parent].active || Main.npc[parent].type != ModContent.NPCType<Atlas>()) {
				npc.active = false;
				return false;
			}

			npc.Center = Main.npc[parent].Center + npc.ai[2] * npc.ai[1].ToRotationVector2();

			npc.ai[1] += .03f;
			return false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, hitDirection, -1f, 0, default, 1f);
			}
			if (npc.life <= 0) {
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 50;
				npc.height = 50;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Stone, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}

				for (int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Stone, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Stone, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.55f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.75f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.velocity != Vector2.Zero) {
				Texture2D texture = Main.npcTexture[npc.type];
				Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				for (int i = 1; i < npc.oldPos.Length; ++i) {
					Vector2 vector2_2 = npc.oldPos[i];
					Color color2 = Color.White * npc.Opacity;
					color2.R = (byte)(0.5 * (double)color2.R * (double)(10 - i) / 20.0);
					color2.G = (byte)(0.5 * (double)color2.G * (double)(10 - i) / 20.0);
					color2.B = (byte)(0.5 * (double)color2.B * (double)(10 - i) / 20.0);
					color2.A = (byte)(0.5 * (double)color2.A * (double)(10 - i) / 20.0);
					Main.spriteBatch.Draw(Main.npcTexture[npc.type],
						new Vector2(npc.oldPos[i].X - Main.screenPosition.X + (npc.width / 2),
							npc.oldPos[i].Y - Main.screenPosition.Y + npc.height / 2),
						new Rectangle?(npc.frame),
						color2, npc.oldRot[i], origin, npc.scale, SpriteEffects.None, 0.0f);
				}
			}
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Atlas/CobbledEye_Glow"));
		}

	}
}