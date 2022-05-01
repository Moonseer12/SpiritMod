﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.InfernonDrops;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	[AutoloadBossHead]
	public class Infernon : ModNPC, IBCRegistrable
	{
		public int currentSpread;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Infernon");

		public override void SetDefaults()
		{
			npc.width = 160;
			npc.height = 250;
			npc.damage = 36;
			npc.defense = 13;
			npc.lifeMax = 13000;
			npc.knockBackResist = 0;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.CursedInferno] = true;
			Main.npcFrameCount[npc.type] = 8;
			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			bossBag = ModContent.ItemType<InfernonBag>();
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon");
			npc.npcSlots = 10;

			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
		}

		public override bool PreAI()
		{
			npc.spriteDirection = npc.direction;

			if (!Main.player[npc.target].active || Main.player[npc.target].dead)
			{
				npc.TargetClosest(false);
				npc.velocity.Y = -100;
			}

			if (!NPC.AnyNPCs(ModContent.NPCType<InfernonSkull>()))
			{
				if (Main.expertMode || npc.life <= 7000)
					NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<InfernonSkull>(), 0, 2, 1, 0, npc.whoAmI, npc.target);
			}

			if (npc.ai[0] == 0)
			{
				// Get the proper direction to move towards the current targeted player.
				if (npc.ai[2] == 0)
				{
					npc.TargetClosest(true);
					npc.ai[2] = npc.Center.X >= Main.player[npc.target].Center.X ? -1f : 1f;
				}
				npc.TargetClosest(true);

				Player player = Main.player[npc.target];
				if (!player.active || player.dead)
				{
					npc.TargetClosest(false);
					npc.velocity.Y = -100;
				}

				float currentXDist = Math.Abs(npc.Center.X - player.Center.X);
				if (npc.Center.X < player.Center.X && npc.ai[2] < 0)
					npc.ai[2] = 0;
				if (npc.Center.X > player.Center.X && npc.ai[2] > 0)
					npc.ai[2] = 0;

				float accelerationSpeed = 0.13F;
				float maxXSpeed = 9;
				npc.velocity.X = npc.velocity.X + npc.ai[2] * accelerationSpeed;
				npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -maxXSpeed, maxXSpeed);

				float yDist = player.position.Y - (npc.position.Y + npc.height);
				if (yDist < 0)
					npc.velocity.Y = npc.velocity.Y - 0.2F;
				if (yDist > 150)
					npc.velocity.Y = npc.velocity.Y + 0.2F;
				npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -6, 6);
				npc.rotation = npc.velocity.X * 0.03f;

				// If the NPC is close enough
				if ((currentXDist < 500 || npc.ai[3] < 0) && npc.position.Y < player.position.Y)
				{
					++npc.ai[3];
					int cooldown = 15;
					if (npc.life < npc.lifeMax * 0.75)
						cooldown = 154;
					if (npc.life < npc.lifeMax * 0.5)
						cooldown = 13;
					if (npc.life < npc.lifeMax * 0.25)
						cooldown = 12;
					cooldown++;
					if (npc.ai[3] > cooldown)
						npc.ai[3] = -cooldown;

					if (npc.ai[3] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 position = npc.Center;
						position.X += npc.velocity.X * 7;

						float speedX = player.Center.X - npc.Center.X;
						float speedY = player.Center.Y - npc.Center.Y;
						float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						float speed = 6;

						if (npc.life < npc.lifeMax * 0.25f)
							speed = 10f;
						else if (npc.life < npc.lifeMax * 0.5f)
							speed = 8f;
						else if (npc.life < npc.lifeMax * 0.75f)
							speed = 6f;

						float num12 = speed / length;
						speedX *= num12;
						speedY *= num12;
						Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
					}
				}
				else if (npc.ai[3] < 0)
					npc.ai[3]++;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.ai[1] += Main.rand.Next(1, 4);
					if (npc.ai[1] > 800 && currentXDist < 600)
						npc.ai[0] = -1;
				}
			}
			else if (npc.ai[0] == 1)
			{
				if (npc.ai[2] == 0)
				{
					npc.TargetClosest(true);
					npc.ai[2] = npc.Center.X >= Main.player[npc.target].Center.X ? -1f : 1f;
				}
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];

				if (npc.Center.X < player.Center.X && npc.ai[2] < 0)
					npc.ai[2] = 0;
				if (npc.Center.X > player.Center.X && npc.ai[2] > 0)
					npc.ai[2] = 0;

				float accelerationSpeed = 0.1F;
				float maxXSpeed = 7;
				npc.velocity.X = npc.velocity.X + npc.ai[2] * accelerationSpeed;
				npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -maxXSpeed, maxXSpeed);

				float yDist = player.position.Y - (npc.position.Y + npc.height);
				if (yDist < 0)
					npc.velocity.Y = npc.velocity.Y - 0.2F;
				if (yDist > 150)
					npc.velocity.Y = npc.velocity.Y + 0.2F;
				npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -6, 6);

				npc.rotation = npc.velocity.X * 0.03f;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.ai[3]++;
					if (npc.ai[3] % 5 == 0 && npc.ai[3] <= 25)
					{
						Vector2 pos = new Vector2(npc.Center.X, (npc.position.Y + npc.height - 14));
						if (!WorldGen.SolidTile((int)(pos.X / 16), (int)(pos.Y / 16)))
						{
							Vector2 dir = player.Center - pos;
							dir.Normalize();
							dir *= 12;
							Projectile.NewProjectile(pos.X, pos.Y, dir.X, dir.Y, ModContent.ProjectileType<FireSpike>(), 24, 0, Main.myPlayer);
							currentSpread++;
						}
					}

					int cooldown = 80;
					if (npc.life < npc.lifeMax * 0.75)
						cooldown = 70;
					if (npc.life < npc.lifeMax * 0.5)
						cooldown = 60;
					if (npc.life < npc.lifeMax * 0.25)
						cooldown = 50;
					if (npc.life < npc.lifeMax * 0.1)
						cooldown = 35;
					if (npc.ai[3] >= cooldown)
						npc.ai[3] = 0;

					npc.ai[1] += Main.rand.Next(1, 4);
					if (npc.ai[1] > 600.0)
						npc.ai[0] = -1f;
				}
			}
			else if (npc.ai[0] == 2)
			{
				if (npc.velocity.X > 0)
					npc.velocity.X -= 0.1f;
				if (npc.velocity.X < 0)
					npc.velocity.X += 0.1f;
				if (npc.velocity.X > -0.2f && npc.velocity.X < 0.2f)
					npc.velocity.X = 0;
				if (npc.velocity.Y > 0)
					npc.velocity.Y -= 0.1f;
				if (npc.velocity.Y < 0)
					npc.velocity.Y += 0.1f;
				if (npc.velocity.Y > -0.2f && npc.velocity.Y < 0.2f)
					npc.velocity.Y = 0;

				npc.rotation = npc.velocity.X * 0.03F;

				npc.ai[3]++;
				if (npc.ai[3] >= 60)
				{
					if (npc.ai[3] % 20 == 0)
					{
						int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = 1.9f;
						int dust1 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire);
						Main.dust[dust1].noGravity = true;
						Main.dust[dust1].scale = 1.9f;
						int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire);
						Main.dust[dust2].noGravity = true;
						Main.dust[dust2].scale = 1.9f;
						int dust3 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire);
						Main.dust[dust3].noGravity = true;
						Main.dust[dust3].scale = 1.9f;
						Vector2 direction = Vector2.One.RotatedByRandom(MathHelper.ToRadians(100));
						int newNPC = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<InfernonSkullMini>());
						Main.npc[newNPC].velocity = direction * 8;
					}
					// Shoot mini skulls.
				}

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.ai[1] += Main.rand.Next(1, 4);
					if (npc.ai[1] > 500)
						npc.ai[0] = -1f;
				}
			}
			else if (npc.ai[0] == 3)
			{
				npc.velocity.Y -= 0.1F;
				npc.alpha += 2;
				if (npc.alpha >= 255)
					npc.active = false;
				if (npc.velocity.X > 0)
					npc.velocity.X -= 0.2F;
				if (npc.velocity.X < 0)
					npc.velocity.X += 0.2F;
				if (npc.velocity.X > -0.2F && npc.velocity.X < 0.2F)
					npc.velocity.X = 0;

				npc.rotation = npc.velocity.X * 0.03f;
			}

			int dust4 = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Fire, npc.velocity.X * 1.5f, npc.velocity.Y * 1.5f);
			int dust5 = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Fire, npc.velocity.X * 1.5f, npc.velocity.Y * 1.5f);
			Main.dust[dust4].velocity *= 0f;
			Main.dust[dust5].velocity *= 0f;

			if (!Main.player[npc.target].active || Main.player[npc.target].dead)
			{
				npc.TargetClosest(true);
				if (!Main.player[npc.target].active || Main.player[npc.target].dead)
				{
					npc.ai[0] = 3;
					npc.ai[3] = 0;
				}
			}

			if (npc.ai[0] != -1)
				return false;

			int num = Main.rand.Next(3);
			npc.TargetClosest(true);
			if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > 1000)
				num = 0;
			npc.ai[0] = num;
			npc.ai[1] = 0;
			npc.ai[2] = 0;
			npc.ai[3] = 0;

			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, hitDirection, -1f, 0, default, 1f);
			}
			if (npc.life <= 0)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient && npc.life <= 0)
				{
					if (Main.expertMode)
					{

						Main.NewText("You have yet to defeat the true master of Hell...", 220, 100, 100, true);
						Vector2 spawnAt = npc.Center + new Vector2(0f, (float)npc.height);
						NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<InfernoSkull>());
					}
				}
				npc.position.X = npc.position.X + (npc.width / 2);
				npc.position.Y = npc.position.Y + (npc.height / 2);
				npc.width = 156;
				npc.height = 180;
				npc.position.X = npc.position.X - (npc.width / 2);
				npc.position.Y = npc.position.Y - (npc.height / 2);
				for (int num621 = 0; num621 < 200; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Fire, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 400; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Fire, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Fire, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Infernon/Infernon_Glow"));

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override bool PreNPCLoot()
		{
			if (Main.expertMode)
				return false;

			MyWorld.downedInfernon = true;
			return true;
		}

		public override void NPCLoot()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int centerX = (int)(npc.position.X + (npc.width / 2)) / 16;
				int centerY = (int)(npc.position.Y + (npc.height / 2)) / 16;
				int halfLength = npc.width / 2 / 16 + 1;
				for (int x = centerX - halfLength; x <= centerX + halfLength; x++)
				{
					for (int y = centerY - halfLength; y <= centerY + halfLength; y++)
					{
						if ((x == centerX - halfLength || x == centerX + halfLength || y == centerY - halfLength || y == centerY + halfLength) && !Main.tile[x, y].active())
						{
							Main.tile[x, y].type = TileID.HellstoneBrick;
							Main.tile[x, y].active(true);
						}
						Main.tile[x, y].lava(false);
						Main.tile[x, y].liquid = 0;
						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendTileSquare(-1, x, y, 1);
						else
							WorldGen.SquareTileFrame(x, y, true);
					}
				}
			}
			npc.DropItem(ModContent.ItemType<InfernalAppendage>(), 25, 36);

			int[] lootTable = {
				ModContent.ItemType<InfernalJavelin>(),
				ModContent.ItemType<InfernalSword>(),
				ModContent.ItemType<DiabolicHorn>(),
				ModContent.ItemType<SevenSins>(),
				ModContent.ItemType<InfernalStaff>(),
				ModContent.ItemType<EyeOfTheInferno>(),
				ModContent.ItemType<InfernalShield>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(lootTable[loot]);

			npc.DropItem(ModContent.ItemType<InfernonMask>(), 1f / 7);
			npc.DropItem(ModContent.ItemType<Trophy4>(), 1f / 10);
		}

		public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.GreaterHealingPotion;

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 6.8f;
			name = "Infernon";
			downedCondition = () => MyWorld.downedInfernon;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<Infernon>()
				},
				new List<int> {
					ModContent.ItemType<CursedCloth>()
				},
				new List<int> {
					ModContent.ItemType<Trophy4>(),
					ModContent.ItemType<InfernonMask>(),
					ModContent.ItemType<InfernonBox>()
				},
				new List<int> {
					ModContent.ItemType<HellsGaze>(),
					ModContent.ItemType<InfernalAppendage>(),
					ModContent.ItemType<InfernalJavelin>(),
					ModContent.ItemType<InfernalSword>(),
					ModContent.ItemType<DiabolicHorn>(),
					ModContent.ItemType<SevenSins>(),
					ModContent.ItemType<InfernalStaff>(),
					ModContent.ItemType<EyeOfTheInferno>(),
					ModContent.ItemType<InfernalShield>(),
					ItemID.GreaterHealingPotion
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<CursedCloth>()}] in the Underworld at any time.";
			texture = "SpiritMod/Textures/BossChecklist/InfernonTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/Infernon/Infernon_Head_Boss";
		}
	}
}