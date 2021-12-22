﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Sets.AtlasDrops;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Atlas
{
	[AutoloadBossHead]
	public class Atlas : ModNPC, IBCRegistrable
	{

		int[] arms = new int[2];
		int timer = 0;
		bool secondStage = false;
		bool thirdStage = false;
		bool lastStage = false;
		int collideTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlas");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 0;
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 290;
			npc.height = 450;
			bossBag = ModContent.ItemType<AtlasBag>();
			npc.damage = 100;
			npc.lifeMax = 41000;
			npc.defense = 48;
			npc.knockBackResist = 0f;
			npc.boss = true;
			npc.timeLeft = NPC.activeTime * 30;

			npc.buffImmune[BuffID.Confused] = true;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.buffImmune[ModContent.BuffType<BCorrupt>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;

			npc.noGravity = true;
			npc.alpha = 255;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Atlas");
        }

		private int Counter;
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void AI()
		{
			bool expertMode = Main.expertMode; //expert mode bool
			Player player = Main.player[npc.target]; //player target
			bool aiChange = npc.life <= npc.lifeMax * 0.75f; //ai change to phase 2
			bool aiChange2 = npc.life <= npc.lifeMax * 0.5f; //ai change to phase 3
			bool aiChange3 = npc.life <= npc.lifeMax * 0.25f; //ai change to phase 4
			bool phaseChange = npc.life <= npc.lifeMax * 0.1f; //aggression increase
			player.AddBuff(ModContent.BuffType<UnstableAffliction>(), 2); //buff that causes gravity shit
			int defenseBuff = (int)(35f * (1f - npc.life / npc.lifeMax));
			npc.defense = npc.defDefense + defenseBuff;

			if (npc.ai[0] == 0f) {
				arms[0] = NPC.NewNPC((int)npc.Center.X - 80 - Main.rand.Next(80, 160), (int)npc.position.Y, ModContent.NPCType<AtlasArmRight>(), npc.whoAmI, npc.whoAmI);
				arms[1] = NPC.NewNPC((int)npc.Center.X + 80 + Main.rand.Next(80, 160), (int)npc.position.Y, ModContent.NPCType<AtlasArmLeft>(), npc.whoAmI, npc.whoAmI);
				npc.ai[0] = 1f;
                npc.netUpdate = true;
                Main.npc[arms[0]].netUpdate = true;
                Main.npc[arms[1]].netUpdate = true;
            }
			else if (npc.ai[0] == 1f) {
				npc.ai[1] += 1f;
				if (npc.ai[1] >= 210f) {
					npc.alpha -= 4;
					if (npc.alpha <= 0) {
						npc.ai[0] = 2f;
						npc.ai[1] = 0f;
						npc.alpha = 0;
						npc.velocity.Y = 14f;
						npc.netUpdate = true;
					}
				}
			}
			else if (npc.ai[0] == 2f) {
				if (npc.alpha == 0) {
					Vector2 dist = player.Center - npc.Center;
					Vector2 direction = player.Center - npc.Center;
					npc.netUpdate = true;
					npc.TargetClosest(true);
					if (!player.active || player.dead) {
						npc.TargetClosest(false);
						npc.velocity.Y = -100f;
					}

					#region Dashing mechanics
					//dash if player is too far away
					if (Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) > 155) {
						direction.Normalize();
						npc.velocity *= 0.99f;
						if (Math.Sqrt((npc.velocity.X * npc.velocity.X) + (npc.velocity.Y * npc.velocity.Y)) >= 7f) {
							int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Stone, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
							Main.dust[dust].noGravity = true;
							dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Stone, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
							Main.dust[dust].noGravity = true;
						}

						if (Math.Sqrt((npc.velocity.X * npc.velocity.X) + (npc.velocity.Y * npc.velocity.Y)) < 2f) {
							if (Main.netMode != NetmodeID.MultiplayerClient) {
								if (Main.rand.Next(25) == 1) {
									direction.X *= Main.rand.Next(19, 24);
									direction.Y *= Main.rand.Next(19, 24);
									npc.velocity.X = direction.X;
									npc.velocity.Y = direction.Y;
									npc.netUpdate = true;
								}
							}
						}
					}
					#endregion

					#region Flying Movement
					if (Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) < 325) {
						float speed = expertMode ? 21f : 18f; //made more aggressive.  expert mode is more.  dusking base value is 7
						float acceleration = expertMode ? 0.16f : 0.13f; //made more aggressive.  expert mode is more.  dusking base value is 0.09
						Vector2 vector2 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float xDir = player.position.X + (player.width / 2) - vector2.X;
						float yDir = (float)(player.position.Y + (player.height / 2) - 120) - vector2.Y;
						float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);
						if (length > 400f) {
							++speed;
							acceleration += 0.08F;
							if (length > 600f) {
								++speed;
								acceleration += 0.08F;
								if (length > 800f) {
									++speed;
									acceleration += 0.08F;
								}
							}
						}
						float num10 = speed / length;
						xDir *= num10;
						yDir *= num10;
						if (npc.velocity.X < xDir) {
							npc.velocity.X = npc.velocity.X + acceleration;
							if (npc.velocity.X < 0 && xDir > 0)
								npc.velocity.X = npc.velocity.X + acceleration;
						}
						else if (npc.velocity.X > xDir) {
							npc.velocity.X = npc.velocity.X - acceleration;
							if (npc.velocity.X > 0 && xDir < 0)
								npc.velocity.X = npc.velocity.X - acceleration;
						}
						if (npc.velocity.Y < yDir) {
							npc.velocity.Y = npc.velocity.Y + acceleration;
							if (npc.velocity.Y < 0 && yDir > 0)
								npc.velocity.Y = npc.velocity.Y + acceleration;
						}
						else if (npc.velocity.Y > yDir) {
							npc.velocity.Y = npc.velocity.Y - acceleration;
							if (npc.velocity.Y > 0 && yDir < 0)
								npc.velocity.Y = npc.velocity.Y - acceleration;
						}
					}
					#endregion
					timer += phaseChange ? 2 : 1; //if below 20% life fire more often
					int shootThings = expertMode ? 200 : 250; //fire more often in expert mode
					if (timer > shootThings) {
						direction.Normalize();
						direction.X *= 8f;
						direction.Y *= 8f;
						int amountOfProjectiles = Main.rand.Next(6, 8);
						int damageAmount = expertMode ? 54 : 62; //always account for expert damage values
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 92);
						for (int num621 = 0; num621 < 30; num621++) {
							Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default, 2f);
						}
						if (Main.netMode != NetmodeID.MultiplayerClient) {
							for (int i = 0; i < amountOfProjectiles; ++i) {
								float A = Main.rand.Next(-250, 250) * 0.01f;
								float B = Main.rand.Next(-250, 250) * 0.01f;
								Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<PrismaticBoltHostile>(), damageAmount, 1, Main.myPlayer);
								timer = 0;
							}
						}
					}

					if (aiChange) {
						if (secondStage == false) {
							Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 93);
							float radius = 250;
							float rot = MathHelper.TwoPi / 5;
							for (int I = 0; I < 5; I++) {
								Vector2 position = npc.Center + radius * (I * rot).ToRotationVector2();
								NPC.NewNPC((int)(position.X), (int)(position.Y), ModContent.NPCType<CobbledEye>(), npc.whoAmI, npc.whoAmI, I * rot, radius);
							}
							secondStage = true;
						}
					}

					if (aiChange2) {
						if (thirdStage == false) {
							Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 93);
							float radius = 400;
							float rot = MathHelper.TwoPi / 10;
							for (int I = 0; I < 10; I++) {
								Vector2 position = npc.Center + radius * (I * rot).ToRotationVector2();
								NPC.NewNPC((int)(position.X), (int)(position.Y), ModContent.NPCType<CobbledEye2>(), npc.whoAmI, npc.whoAmI, I * rot, radius);
							}
							thirdStage = true;
						}
					}

					if (aiChange3) {
						if (lastStage == false) {
							Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 93);
							float radius = 1200;
							float rot = MathHelper.TwoPi / 41;
							for (int I = 0; I < 41; I++) {
								Vector2 position = npc.Center + radius * (I * rot).ToRotationVector2();
								NPC.NewNPC((int)(position.X), (int)(position.Y), ModContent.NPCType<CobbledEye3>(), npc.whoAmI, npc.whoAmI, I * rot, radius);
							}
							lastStage = true;
						}
					}
					for (int index1 = 0; index1 < 6; ++index1) {
						float x = (npc.Center.X - 2);
						float xnum2 = (npc.Center.X + 2);
						float y = (npc.Center.Y - 160);
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
				}
			}

			collideTimer++;
			if (collideTimer == 500) {
				npc.noTileCollide = true;
			}

			npc.TargetClosest(true);
			if (!player.active || player.dead) {
				npc.TargetClosest(false);
				npc.velocity.Y = -100f;
				timer = 0;
			}

			Counter++;
			if (Counter > 400) {
				SpiritMod.tremorTime = 20;
				Counter = 0;
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.85f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.65f);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, hitDirection, -1f, 0, default, 1f);
			}
			if (npc.life <= 0) {
				npc.position = npc.Center;
				npc.width = 300;
				npc.height = 500;
				npc.position = npc.Center;
				for (int num621 = 0; num621 < 200; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Stone, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 400; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Stone, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Stone, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
			lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
			=> GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Atlas/Atlas_Glow"));

		public override bool PreNPCLoot()
		{
			MyWorld.downedAtlas = true;
			return true;
		}

		public override void NPCLoot()
		{
			if (Main.expertMode) {
				npc.DropBossBags();
				return;
			}

			npc.DropItem(ModContent.ItemType<ArcaneGeyser>(), Main.rand.Next(32, 44));

			int[] lootTable = {
				ModContent.ItemType<Mountain>(),
				ModContent.ItemType<TitanboundBulwark>(),
				ModContent.ItemType<CragboundStaff>(),
				ModContent.ItemType<QuakeFist>(),
				ModContent.ItemType<Earthshatter>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(lootTable[loot]);
            npc.DropItem(ModContent.ItemType<AtlasMask>(), 1f / 7);
			npc.DropItem(ModContent.ItemType<Trophy8>(), 1f / 10);

            string[] lootTable1 = { "JackHead", "JackBody", "JackLegs" };
            int loot1 = Main.rand.Next(lootTable1.Length);
            if (Main.rand.NextBool(12))
            {
                npc.DropItem(mod.ItemType(lootTable1[loot1]));
            }
        }

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			if (npc.ai[0] < 2f)
				return false;

			return base.CanHitPlayer(target, ref cooldownSlot);
		}

		public override void BossLoot(ref string name, ref int potionType)
			=> potionType = ItemID.GreaterHealingPotion;

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 12.4f;
			name = "Atlas";
			downedCondition = () => MyWorld.downedAtlas;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<Atlas>()
				},
				new List<int> {
					ModContent.ItemType<StoneSkin>()
				},
				new List<int> {
					ModContent.ItemType<Trophy8>(),
					ModContent.ItemType<AtlasMask>(),
					ModContent.ItemType<AtlasBox>()
				},
				new List<int> {
					ModContent.ItemType<AtlasEye>(),
					ModContent.ItemType<Mountain>(),
					ModContent.ItemType<TitanboundBulwark>(),
					ModContent.ItemType<CragboundStaff>(),
					ModContent.ItemType<QuakeFist>(),
					ModContent.ItemType<Earthshatter>(),
					ModContent.ItemType<ArcaneGeyser>(),
					ItemID.GreaterHealingPotion
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<StoneSkin>()}] anywhere at any time.";
			texture = "SpiritMod/Textures/BossChecklist/AtlasTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/Atlas/Atlas_Head_Boss";
		}
	}
}