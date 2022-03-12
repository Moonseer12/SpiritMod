﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Sets.DuskingDrops;
using SpiritMod.Items.Consumable;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs.Boss.Dusking
{
	[AutoloadBossHead]
	public class Dusking : ModNPC, IBCRegistrable
	{
		// npc.ai[0] = State Manager.
		// npc.ai[1] = Additional timer (charge timer, state timer, etc).
		// npc.localAI[0] = Outer Circle Opacity.
		// npc.localAI[1] = Outer Circle Rotation.

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusking");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 120;
			npc.height = 160;
			npc.damage = 45;
			npc.defense = 32;
			npc.lifeMax = 21000;
			npc.knockBackResist = 0;
			npc.boss = true;

			npc.buffImmune[BuffID.Confused] = true;
			npc.buffImmune[BuffID.ShadowFlame] = true;

			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 5;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;

			bossBag = ModContent.ItemType<DuskingBag>();
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/DuskingTheme");
		}

		public override bool CheckActive() => npc.Center.Y < -2000;

		public override bool PreAI()
		{
			npc.netUpdate = true;
			npc.TargetClosest(true);

			Lighting.AddLight(npc.Center, 0.7f, 0.3f, 0.7f);
			Player player = Main.player[npc.target];

			if (!player.active || player.dead || Main.dayTime || player.DistanceSQ(npc.Center) > 20000 * 20000)
			{
				npc.TargetClosest(false);
				npc.velocity.Y = -100;
			}

			if (npc.ai[0] == 0) // Flying around and shooting projectiles
			{
				#region Flying Movement
				float speed = 7f;
				float acceleration = 0.09f;
				Vector2 vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float xDir = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector2.X;
				float yDir = (float)(Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - 120) - vector2.Y;
				float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);
				if (length > 400 && Main.expertMode)
				{
					++speed;
					acceleration += 0.05F;
					if (length > 600)
					{
						++speed;
						acceleration += 0.05F;
						if (length > 800)
						{
							++speed;
							acceleration += 0.05F;
						}
					}
				}

				float num10 = speed / length;
				xDir *= num10;
				yDir *= num10;

				if (npc.velocity.X < xDir)
				{
					npc.velocity.X = npc.velocity.X + acceleration;
					if (npc.velocity.X < 0 && xDir > 0)
						npc.velocity.X = npc.velocity.X + acceleration;
				}
				else if (npc.velocity.X > xDir)
				{
					npc.velocity.X = npc.velocity.X - acceleration;
					if (npc.velocity.X > 0 && xDir < 0)
						npc.velocity.X = npc.velocity.X - acceleration;
				}

				if (npc.velocity.Y < yDir)
				{
					npc.velocity.Y = npc.velocity.Y + acceleration;
					if (npc.velocity.Y < 0 && yDir > 0)
						npc.velocity.Y = npc.velocity.Y + acceleration;
				}
				else if (npc.velocity.Y > yDir)
				{
					npc.velocity.Y = npc.velocity.Y - acceleration;
					if (npc.velocity.Y > 0 && yDir < 0)
						npc.velocity.Y = npc.velocity.Y - acceleration;
				}
				#endregion
				// Shadow Ball Shoot
				if (npc.ai[1] % 45 == 0)
				{
					Vector2 dir = Main.player[npc.target].Center - npc.Center;
					dir.Normalize();
					dir *= 14;
					int newNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<ShadowBall>(), npc.whoAmI);
					Main.npc[newNPC].velocity = dir;
				}

				// Crystal Shadow Shoot.
				if (npc.ai[1] == 150)
				{
					for (int i = 0; i < 8; ++i)
					{
						bool expertMode = Main.expertMode;
						Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
						targetDir.Normalize();
						targetDir *= 3;
						int dmg = expertMode ? 23 : 37;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<CrystalShadow>(), dmg, 0.5F, Main.myPlayer);
					}
				}

				// Shadowflamer Shoot
				if (npc.ai[1] % 110 == 0)
				{
					Vector2 dir = Main.player[npc.target].Center - npc.Center;
					dir += new Vector2(Main.rand.Next(-40, 41), Main.rand.Next(-40, 41));
					dir.Normalize();
					dir *= 12;
					int newNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Shadowflamer>(), npc.whoAmI);
					Main.npc[newNPC].velocity = dir;
				}

				npc.ai[1]++;
				if (npc.ai[1] >= 300)
				{
					npc.ai[0] = 1;
					npc.ai[1] = 60;
					npc.ai[2] = 0;
					npc.ai[3] = 0;
				}

				// Rage Phase Switch
				if (npc.life <= 9000)
				{
					npc.ai[0] = 2;
					npc.ai[1] = 0;
					npc.ai[2] = 0;
					npc.ai[3] = 0;
				}
			}
			else if (npc.ai[0] == 1) // Charging.
			{
				npc.ai[1]++;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (npc.ai[1] % 45 == 0)
					{
						npc.TargetClosest(true);
						float speed = 10 + (2 * (int)(npc.life / 5000));
						Vector2 vector2_1 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float dirX = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - vector2_1.X;
						float dirY = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - vector2_1.Y;
						float targetVel = Math.Abs(Main.player[npc.target].velocity.X) + Math.Abs(Main.player[npc.target].velocity.Y) / 4f;

						float speedMultiplier = targetVel + (10f - targetVel);
						if (speedMultiplier < 6.0)
							speedMultiplier = 6f;
						if (speedMultiplier > 16.0)
							speedMultiplier = 16f;
						float speedX = dirX - Main.player[npc.target].velocity.X * speedMultiplier;
						float speedY = dirY - (Main.player[npc.target].velocity.Y * speedMultiplier / 4);
						speedX *= (float)(1 + Main.rand.Next(-10, 11) * 0.01);
						speedY *= (float)(1 + Main.rand.Next(-10, 11) * 0.01);
						float speedLength = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						float actualSpeed = speed / speedLength;
						npc.velocity.X = speedX * actualSpeed;
						npc.velocity.Y = speedY * actualSpeed;
						npc.velocity.X = npc.velocity.X + Main.rand.Next(-40, 41) * 0.1f;
						npc.velocity.Y = npc.velocity.Y + Main.rand.Next(-40, 41) * 0.1f;
						npc.netUpdate = true;
					}
				}
				if (npc.ai[1] >= 270)
				{
					npc.ai[0] = 0;
					npc.ai[1] = 0;
					npc.ai[2] = 0;
					npc.ai[3] = 0;
					npc.velocity *= 0.3F;
				}
			}
			else if (npc.ai[0] == 2) // Continuous Charging.
			{
				if (npc.ai[1] == 0) // Flying Movement
				{
					bool expertMode = Main.expertMode;
					float speed = 38f;
					float acceleration = 1.55f;
					float num7 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - npc.Center.X;
					float num8 = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - 120 - npc.Center.Y;
					float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);

					if (Main.rand.Next(100) == 6)
					{
						for (int i = 0; i < 8; ++i)
						{
							Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
							targetDir.Normalize();
							targetDir *= 3;
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<CrystalShadow>(), 26, 0.5F, Main.myPlayer);
						}
					}

					if (npc.life >= (npc.lifeMax / 2))
					{
						if (Main.rand.Next(100) == 10)
						{
							Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 21);
							Vector2 direction = Main.player[npc.target].Center - npc.Center;
							direction.Normalize();
							direction.X *= 12f;
							direction.Y *= 12f;

							int amountOfProjectiles = 1;
							for (int i = 0; i < amountOfProjectiles; ++i)
							{
								float A = Main.rand.Next(-200, 200) * 0.01f;
								float B = Main.rand.Next(-200, 200) * 0.01f;
								int damage = expertMode ? 23 : 37;
								Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<CrystalShadow>(), damage, 1, Main.myPlayer, 0, 0);
							}
						}
					}

					if (num9 > 400 && Main.expertMode)
					{
						++speed;
						acceleration += 0.25F;
						if (num9 > 600)
						{
							++speed;
							acceleration += 0.25F;
							if (num9 > 800)
							{
								++speed;
								acceleration += 0.25F;
							}
						}
					}

					float num10 = speed / num9;
					float num11 = num7 * num10;
					float num12 = num8 * num10;

					if (npc.velocity.X < num11)
					{
						npc.velocity.X = npc.velocity.X + acceleration;
						if (npc.velocity.X < 0 && num11 > 0)
							npc.velocity.X = npc.velocity.X + acceleration;
					}
					else if (npc.velocity.X > num11)
					{
						npc.velocity.X = npc.velocity.X - acceleration;
						if (npc.velocity.X > 0 && num11 < 0)
							npc.velocity.X = npc.velocity.X - acceleration;
					}

					if (npc.velocity.Y < num12)
					{
						npc.velocity.Y = npc.velocity.Y + acceleration;
						if (npc.velocity.Y < 0 && num12 > 0)
							npc.velocity.Y = npc.velocity.Y + acceleration;
					}
					else if (npc.velocity.Y > num12)
					{
						npc.velocity.Y = npc.velocity.Y - acceleration;
						if (npc.velocity.Y > 0 && num12 < 0)
							npc.velocity.Y = npc.velocity.Y - acceleration;
					}

					npc.ai[2]++;
					if (npc.ai[2] >= 120)
					{
						npc.ai[1] = 1;
						npc.ai[2] = 0;
					}
				}

				else if (npc.ai[1] == 1)
				{
					npc.ai[2]++;
					if (npc.ai[2] % 45 == 0)
					{
						npc.TargetClosest(true);
						float speed = 15 + (2 * (int)(npc.life / 10000));
						Vector2 vector2_1 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float dirX = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - vector2_1.X;
						float dirY = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - vector2_1.Y;
						float targetVel = Math.Abs(Main.player[npc.target].velocity.X) + Math.Abs(Main.player[npc.target].velocity.Y) / 4f;
						float speedMultiplier = targetVel + (10f - targetVel);
						if (speedMultiplier < 35.0)
							speedMultiplier = 35f;
						if (speedMultiplier > 25.0)
							speedMultiplier = 25f;
						float speedX = dirX - Main.player[npc.target].velocity.X * speedMultiplier;
						float speedY = dirY - (Main.player[npc.target].velocity.Y * speedMultiplier / 4);
						speedX *= 1 + Main.rand.Next(-10, 11) * 0.019f;
						speedY *= 1 + Main.rand.Next(-10, 11) * 0.019f;
						float speedLength = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						float actualSpeed = speed / speedLength;
						npc.velocity.X = speedX * actualSpeed;
						npc.velocity.Y = speedY * actualSpeed;
						npc.velocity.X = npc.velocity.X + Main.rand.Next(-20, 21) * 0.5f;
						npc.velocity.Y = npc.velocity.Y + Main.rand.Next(-20, 21) * 0.5f;
					}

					if (Main.rand.Next(100) == 6)
					{
						for (int i = 0; i < 8; ++i)
						{
							Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
							targetDir.Normalize();
							targetDir *= 3;
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<CrystalShadow>(), 26, 0.5F, Main.myPlayer);
						}
					}
					if (npc.ai[2] >= 270)
					{
						npc.ai[1] = 0;
						npc.ai[2] = 0;
						npc.velocity *= 0.3F;
					}
				}

				// Circle code.
				if (npc.localAI[0] < 1)
					npc.localAI[0] += 0.01F;
				npc.localAI[1] += 0.03F;
				npc.ai[3]++;

				if (npc.ai[3] >= 6)
				{
					for (int i = 0; i < 255; ++i)
					{
						if (Main.player[i].active && !Main.player[i].dead)
						{
							if ((Main.player[i].Center - npc.Center).Length() <= 200)
							{
								//Main.player[i].Hurt(1, 0, false, false, " was evaporated...", false, 1); commed out because this needs work
								Main.player[i].AddBuff(BuffID.Darkness, 330);
							}
						}
					}
					npc.ai[3] = 0;
				}
			}
			else if (npc.ai[0] == 3)
			{
				npc.velocity *= 0.97F;
				npc.alpha += 3;

				if (npc.alpha >= 255)
					npc.active = false;
			}
			if (!Main.player[npc.target].active || Main.player[npc.target].dead)
			{
				npc.TargetClosest(true);

				if (!Main.player[npc.target].active || Main.player[npc.target].dead)
					npc.ai[0] = 3;
			}
			return true;
		}

		public override void AI()
		{
			npc.netUpdate = true;
			npc.TargetClosest(true);
			Lighting.AddLight(npc.Center, 0.7F, 0.3F, 0.7F);
			Player player = Main.player[npc.target];

			if (Main.expertMode)
			{
				if (Main.rand.Next(100) == 2 && npc.life >= (npc.lifeMax / 2))
				{
					Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 33);
					Vector2 direction = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 12f;

					bool expertMode = Main.expertMode;
					int amountOfProjectiles = 1;
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = Main.rand.Next(-80, 80) * 0.01f;
						float B = Main.rand.Next(-80, 80) * 0.01f;
						int damage = expertMode ? 23 : 37;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<ShadowPulse>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.frameCounter >= (npc.ai[0] == 2 ? 3 : 6))
			{
				npc.frameCounter = 0;
				npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
			}
			npc.spriteDirection = npc.direction;
		}

		public override bool PreNPCLoot()
		{
			MyWorld.downedDusking = true;
			for (int i = 0; i < 15; ++i)
			{
				int newDust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame, 0f, 0f, 100, default, 2.5f);
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].velocity *= 5f;
				newDust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame, 0f, 0f, 100, default, 1.5f);
				Main.dust[newDust].velocity *= 3f;
			}
			return true;
		}

		public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.GreaterHealingPotion;

		public override void NPCLoot()
		{
			if (Main.expertMode)
			{
				npc.DropBossBags();
				return;
			}

			npc.DropItem(ModContent.ItemType<DuskStone>(), Main.rand.Next(25, 36));

			int[] lootTable = {
				ModContent.ItemType<ShadowflameSword>(),
				ModContent.ItemType<UmbraStaff>(),
				ModContent.ItemType<ShadowSphere>(),
				ModContent.ItemType<Shadowmoor>(),
			};
			int loot = Main.rand.Next(lootTable.Length);
			if (loot == 0)
				npc.DropItem(lootTable[0], Main.rand.Next(74, 121));
			else
				npc.DropItem(lootTable[loot]);

			npc.DropItem(ModContent.ItemType<DuskingMask>(), 1f / 7);
			npc.DropItem(ModContent.ItemType<Trophy6>(), 1f / 10);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(ModContent.BuffType<Shadowflame>(), 150);

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (npc.localAI[0] > 0)
			{
				Texture2D ring = mod.GetTexture("Effects/Glowmasks/Dusking_Circle");
				Vector2 origin = new Vector2(ring.Width * 0.5F, ring.Height * 0.5F);
				spriteBatch.Draw(ring, (npc.Center) - Main.screenPosition, null, Color.White * npc.localAI[0], npc.localAI[1], origin, 1, SpriteEffects.None, 0);
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 7.3f;
			name = "Dusking";
			downedCondition = () => MyWorld.downedDusking;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> { ModContent.NPCType<Dusking>() },
				new List<int> { ModContent.ItemType<DuskCrown>() },
				new List<int>
				{
					ModContent.ItemType<Trophy6>(),
					ModContent.ItemType<DuskingMask>()
				},
				new List<int>
				{
					ModContent.ItemType<DuskPendant>(),
					ModContent.ItemType<ShadowflameSword>(),
					ModContent.ItemType<UmbraStaff>(),
					ModContent.ItemType<ShadowSphere>(),
					ModContent.ItemType<Shadowmoor>(),
					ModContent.ItemType<DuskStone>(),
					ItemID.GreaterHealingPotion
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<DuskCrown>()}] anywhere at nighttime.";
			texture = "SpiritMod/Textures/BossChecklist/DuskingTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/Dusking/Dusking_Head_Boss";
		}
	}
}