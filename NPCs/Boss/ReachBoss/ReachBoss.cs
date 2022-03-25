﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using SpiritMod.Effects;
using SpiritMod.Items.Sets.VinewrathDrops;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	[AutoloadBossHead]
	public class ReachBoss : ModNPC, IBCRegistrable
	{
		int moveSpeed = 0;
		bool text = false;
		int moveSpeedY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vinewrath Bane");
			Main.npcFrameCount[npc.type] = 5;
			NPCID.Sets.TrailCacheLength[npc.type] = 4;
			NPCID.Sets.TrailingMode[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.Size = new Vector2(80, 120);
			npc.damage = 28;
            npc.boss = true;
			npc.lifeMax = 3400;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 30;
			npc.defense = 9;
			npc.aiStyle = -1;
			music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/ReachBoss");
			npc.buffImmune[20] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[70] = true;

			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
		}

		bool pulseTrail;
		bool pulseTrailPurple;
		bool pulseTrailYellow;
		bool trailbehind = false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(pulseTrail);
			writer.Write(pulseTrailPurple);
			writer.Write(pulseTrailYellow);
			writer.Write(trailbehind);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			pulseTrail = reader.ReadBoolean();
			pulseTrailPurple = reader.ReadBoolean();
			pulseTrailYellow = reader.ReadBoolean();
			trailbehind = reader.ReadBoolean();
		}
		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.301f, 0.110f, 0.126f);
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			npc.rotation = MathHelper.Lerp(npc.rotation, 0, 0.06f);
			npc.noTileCollide = true;

			if (!player.active || player.dead) {
				npc.TargetClosest(false);
				npc.velocity.Y = -2000;
			}
			if (!player.GetSpiritPlayer().ZoneReach) {
				npc.defense = 25;
				npc.damage = 45;
			}
			else {
				npc.defense = 9;
				npc.damage = 28;
			}

			if (npc.life <= (npc.lifeMax / 10 * 4) && npc.ai[3] == 0)
			{
				npc.ai[0] = 0;
			    DustHelper.DrawStar(npc.Center, 235, pointAmount: 7, mainSize: 2.7425f, dustDensity: 6, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 55).WithPitchVariance(0.2f), npc.Center);
				Main.PlaySound(SoundID.Trackable, (int)npc.position.X, (int)npc.position.Y, 180, 1f, -0.9f);
				npc.netUpdate = true;
				npc.ai[3]++;
			}
			if (npc.life <= (npc.lifeMax / 10 * 4))
			{
				npc.ai[0]+= 1.5f;
			}
			else
			{
				npc.ai[0]++;
			}
			if (npc.ai[0] < 470 || npc.ai[0] > 730 && npc.ai[0] < 900 || npc.ai[0] > 1051 && npc.ai[0] < 1120 || npc.ai[0] > 1750) {
				generalMovement(player);
			}

			float[] stoptimes = new float[] { 471, 540, 669, 900, 1051 };
			if (stoptimes.Contains(npc.ai[0]))
			{
				npc.velocity = Vector2.Zero;
				npc.netUpdate = true;
			}

			if (npc.ai[0] >= 480  && npc.ai[0] < 730)
			{
				sideFloat(player);
				pulseTrail = true;
			}	
			else
				pulseTrail = false;

			if (npc.ai[0] == 880)
			{
			    DustHelper.DrawStar(npc.Center, 272, pointAmount: 8, mainSize: 3.7425f, dustDensity: 6, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 55).WithPitchVariance(0.2f), npc.Center);
			}

			if (npc.ai[0] > 900 && npc.ai[0] < 1050)
			{
				pulseTrailPurple = true;
				flowerAttack(player);
			}	
			else
				pulseTrailPurple = false;

			if (npc.ai[0] >= 1120 && npc.ai[0] < 1740)
			{
				DashAttack(player);
				pulseTrailYellow = true;
			}

			else {
				pulseTrailYellow = false;
				trailbehind = false;
				npc.TargetClosest(true);
				npc.spriteDirection = npc.direction;
			}
			if (npc.ai[0] > 1800 && npc.ai[0] < 1970)
			{
				SummonSpores();
				pulseTrailYellow = true;
			}
			else
			{
				pulseTrailYellow = false;
			}
			if (npc.ai[0] > 2000)
			{
				pulseTrailPurple = false;
				pulseTrailYellow = false;
				pulseTrail = false;
				npc.ai[0] = 0;
				npc.ai[1] = 0;
				npc.ai[2] = 0;
				npc.netUpdate = true;
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => npc.ai[0] < 480 || npc.ai[0] >= 730;
		public void generalMovement(Player player)
		{
			float value12 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) * 60f;
			if (npc.Center.X >= player.Center.X && moveSpeed >= -37) // flies to players x position
				moveSpeed--;
			else if (npc.Center.X <= player.Center.X && moveSpeed <= 37)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.13f;
			npc.rotation = npc.velocity.X * 0.04f;

			if (npc.Center.Y >= player.Center.Y - 140f + value12 && moveSpeedY >= -20) //Flies to players Y position
				moveSpeedY--;
			else if (npc.Center.Y <= player.Center.Y - 140f + value12 && moveSpeedY <= 20)
				moveSpeedY++;
			npc.velocity.Y = moveSpeedY * 0.1f;			
		}
		public void sideFloat(Player player)
		{
			bool expertMode = Main.expertMode;
			Vector2 homepos = Main.player[npc.target].Center;
			if ((npc.ai[0] >= 480 && npc.ai[0] < 540) || (npc.ai[0] >= 580 && npc.ai[0] < 670)) {
				homepos += (npc.ai[0] < 540) ? new Vector2(150, -250f) : new Vector2(-150, -250);
				npc.TargetClosest(true);
				float vel = MathHelper.Clamp(npc.Distance(homepos) / 8, 6, 38);
				npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(homepos) * vel, 0.05f);
			}
			if (npc.ai[0] == 561 || npc.ai[0] == 690)
			{
				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y);
				Main.PlaySound(SoundID.Trackable, (int)npc.Center.X, (int)npc.Center.Y, 139, 1f, 0.4f);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
					direction *= 3f;

                    int amountOfProjectiles = 5;
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        int damage = expertMode ? 14 : 19;
                        if(i == 0)
							Projectile.NewProjectileDirect(npc.Center, direction, ModContent.ProjectileType<BossRedSpike>(), damage, 1, Main.myPlayer, 0, 0).netUpdate = true;
						else
							Projectile.NewProjectileDirect(npc.Center, direction.RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(0.5f, 1f), ModContent.ProjectileType<BossRedSpike>(), damage, 1, Main.myPlayer, 0, 0).netUpdate = true;
                    }
                }
			}
		}
		public void flowerAttack(Player player)
		{
			bool expertMode = Main.expertMode;
			int damage = expertMode ? 13 : 16;
			if (npc.ai[0] % 15 == 0)
			{
				Main.PlaySound(new LegacySoundStyle(SoundID.Item, 104).WithPitchVariance(0.2f), npc.Center);
	            if (Main.netMode != NetmodeID.MultiplayerClient)
                {
					int p = Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-60, 60), npc.Center.Y+ Main.rand.Next(-60, 60), Main.rand.NextFloat(-5.3f, 5.3f), Main.rand.NextFloat(-5.3f, 5.3f), ModContent.ProjectileType<ReachBossFlower>(), damage, 1, Main.myPlayer, 0, 0);		
					Main.projectile[p].scale = Main.rand.NextFloat(.6f, .8f);
					DustHelper.DrawStar(Main.projectile[p].Center, 272, pointAmount: 6, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
					if (Main.projectile[p].velocity == Vector2.Zero)
					{
						Main.projectile[p].velocity = new Vector2(2.25f, 2.25f);
					}
					if (Main.projectile[p].velocity.X < 2.25f && Math.Sign(Main.projectile[p].velocity.X) == Math.Sign(1)|| Main.projectile[p].velocity.X > -2.25f && Math.Sign(Main.projectile[p].velocity.X) == Math.Sign(-1))
					{
						Main.projectile[p].velocity.X *= 2.15f;
					}
					Main.projectile[p].netUpdate = true;
				}
			}
		}

		void DashAttack(Player player) //basically just copy pasted from scarabeus mostly
		{
			pulseTrailYellow = true;
			npc.direction = Math.Sign(player.Center.X - npc.Center.X);		
			if (npc.ai[0] < 1280 || npc.ai[0] > 1420 && npc.ai[0] < 1600) {
				npc.ai[1] = 0;
				npc.ai[2] = 0;
				Vector2 homeCenter = player.Center;
				npc.spriteDirection = npc.direction;
				homeCenter.X += (npc.Center.X < player.Center.X) ? -280 : 280;
				homeCenter.Y -= 30;

				float vel = MathHelper.Clamp(npc.Distance(homeCenter) / 12, 8, 30);
				npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(homeCenter) * vel, 0.08f);
			}
			else {
				npc.rotation = npc.velocity.X * 0.04f;
				if (npc.ai[0] < 1320 || npc.ai[0] > 1600 && npc.ai[0] < 1641) {
					npc.velocity.X = -npc.spriteDirection;
					npc.velocity.Y = 0;
				}

				else if (npc.ai[0] == 1320 || npc.ai[0] == 1641) {
					Main.PlaySound(new LegacySoundStyle(SoundID.Roar, 0), npc.Center);
					npc.velocity.X = MathHelper.Clamp(Math.Abs((player.Center.X - npc.Center.X) / 10), 24, 36) * npc.spriteDirection;
					npc.netUpdate = true;
					trailbehind = true;
				}

				else if (npc.direction != npc.spriteDirection || npc.ai[1] > 0) {
					npc.ai[1]++; //ai 1 is used here to store this being triggered at least once, so if direction is equal to sprite direction again after this it will continue this part of the ai
					npc.velocity.X = MathHelper.Lerp(npc.velocity.X, 0, 0.06f);
					npc.noTileCollide = false;

					if (npc.collideX && npc.ai[2] == 0) {
						npc.ai[2]++; //ai 2 is used here as a flag to make sure the tile collide effects only trigger once
						Collision.HitTiles(npc.position, npc.velocity, npc.width, npc.height);
						Main.PlaySound(SoundID.Dig, npc.Center);
						npc.velocity.X *= -0.5f;
						//put other things here for on tile collision effects
					}
				}
			}
		}
		public void SummonSpores()
		{			
			if (npc.ai[0] % 9 == 0)
			{
				
				Main.PlaySound(new LegacySoundStyle(42, 4), npc.Center);
				Main.PlaySound(new LegacySoundStyle(6, 0).WithPitchVariance(0.2f), npc.Center);
				Main.PlaySound(new LegacySoundStyle(4, 55).WithPitchVariance(0.2f), npc.Center);
				int p = NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-100, 100), (int)npc.Center.Y + Main.rand.Next(-200, -100), ModContent.NPCType<ExplodingSpore>());
				DustHelper.DrawStar(new Vector2(Main.npc[p].Center.X, Main.npc[p].Center.Y), DustID.GoldCoin, pointAmount: 4, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
				Main.npc[p].ai[1] = npc.whoAmI;
				Main.npc[p].netUpdate = true;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += .15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.66f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		Vector2 Drawoffset => new Vector2(0, npc.gfxOffY) + Vector2.UnitX * npc.spriteDirection * 12;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            float num366 = num395 + .85f;
			if ((npc.ai[0] > 1290 && npc.ai[0] < 1360 || npc.ai[0] > 1600 && npc.ai[0] < 1690) || npc.life <= (npc.lifeMax/10 * 4))
			{
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, 0f), 0.75f, Color.OrangeRed * .7f, Color.Firebrick * .05f, 0.75f, num366, .65f);
			}	
			var effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + Drawoffset, npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			if (trailbehind) {
				for (int i = 0; i < NPCID.Sets.TrailCacheLength[npc.type]; i++) {
					Vector2 drawpos = npc.oldPos[i] + npc.Size / 2 - Main.screenPosition;
					float opacity = 0.5f * ((NPCID.Sets.TrailCacheLength[npc.type] - i) / (float)NPCID.Sets.TrailCacheLength[npc.type]);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawpos + Drawoffset, npc.frame,
									 drawColor * opacity, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
				}
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;
			Color color1 = Color.White * num107 * .8f;
			var effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(
				mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss_Glow"),
				npc.Center - Main.screenPosition + Drawoffset,
				npc.frame,
				color1,
				npc.rotation,
				npc.frame.Size() / 2,
				npc.scale,
				effects,
				0
			);
			if (pulseTrail)
			{
				SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y) - Main.screenPosition + Drawoffset - npc.velocity;
				Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.Tomato);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = npc.GetAlpha(color28);
					color28 *= 1f - num107;
					Vector2 vector29 = npc.Center + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + Drawoffset - npc.velocity * (float)num103;
					Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
				}
			}
			if (pulseTrailPurple || pulseTrailYellow)
			{
				Color glowcolor = (pulseTrailYellow) ? Color.Gold : Color.Orchid;
		        float num1072 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
				SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y) - Main.screenPosition + Drawoffset - npc.velocity;
				Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(glowcolor);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = npc.GetAlpha(color28);
					color28 *= 1f - num1072;
					Vector2 vector29 = npc.Center + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num1072 + 2f) - Main.screenPosition + Drawoffset - npc.velocity * (float)num103;
					Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss_PurpleGlow"), vector29, npc.frame, color28 * .9f, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
				}
			}
		}
		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);
        public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
        {
            SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int i = 1; i < 10; i++)
            {
                Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
                spriteBatch.Draw(mod.GetTexture("NPCs/Boss/ReachBoss/ReachBoss_Afterimage"), new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)i * trailLengthModifier, npc.frame, color, npc.rotation, npc.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
            }
        }
	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && npc.life <= 0) {
				if (!text) {
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(0, 200, 80, 100),
					"You cannot stop the wrath of nature!");
					text = true;
				}

				Vector2 spawnAt = npc.Center + new Vector2(0f, (float)npc.height / 2f);
				NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<ReachBoss1>());

				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);

				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss1"), 1f);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Grass, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
			for (int j = 0; j < 2; j++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(npc.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50)), npc.velocity, 386, goreScale);
				Main.gore[a].timeLeft = 15;
				Main.gore[a].rotation = 10f;
				Main.gore[a].velocity = new Vector2(hitDirection * 2.5f, Main.rand.NextFloat(1f, 2f));
				
				int a1 = Gore.NewGore(npc.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50)), npc.velocity, 911, goreScale);
				Main.gore[a1].timeLeft = 15;
				Main.gore[a1].rotation = 1f;
				Main.gore[a1].velocity = new Vector2(hitDirection * 2.5f, Main.rand.NextFloat(10f, 20f));
			}
			for (int k = 0; k < 12; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Plantera_Green, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.LavaMoss, 2.5f * hitDirection, -2.5f, 0,  default, 0.7f);
			}
		}
		public override bool PreNPCLoot() => false;

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 3.5f;
			name = "Vinewrath Bane";
			downedCondition = () => MyWorld.downedReachBoss;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<ReachBoss>()
				},
				new List<int> {
					ModContent.ItemType<ReachBossSummon>()
				},
				new List<int> {
					ModContent.ItemType<Trophy5>(),
					ModContent.ItemType<ReachMask>(),
					ModContent.ItemType<VinewrathBox>()
				},
				new List<int> {
					ModContent.ItemType<DeathRose>(),
					ModContent.ItemType<SunbeamStaff>(),
					ModContent.ItemType<ThornBow>(),
					ModContent.ItemType<ReachVineStaff>(),
					ModContent.ItemType<ReachBossSword>(),
					// ModContent.ItemType<ReachKnife>(),
					ItemID.LesserHealingPotion
				});
			spawnInfo =
				$"Right-click the Bloodblossom, a glowing flower found at the bottom of the Briar. The Vinewrath Bane can be fought at any time and any place in progression. If a Bloodblossom is not present, use a [i:{ModContent.ItemType<ReachBossSummon>()}] in the Briar below the surface at any time.";
			texture = "SpiritMod/Textures/BossChecklist/ReachBossTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/ReachBoss/ReachBoss/ReachBoss_Head_Boss";
		}
	}
}
