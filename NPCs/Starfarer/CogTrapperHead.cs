﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor.ProtectorateSet;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Starfarer
{
	public class CogTrapperHead : ModNPC
	{
		public bool flies = true;
		public bool directional = false;
		public float speed = 6.5f;
		public float turnSpeed = 0.125f;
		public bool tail = false;
		public int minLength = 15;
		public int maxLength = 16;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardancer");
		}

		public override void SetDefaults()
		{
			npc.damage = 32; //150
			npc.npcSlots = 17f;
			npc.width = 26; //324
			npc.height = 26; //216
			npc.defense = 0;
			npc.lifeMax = 225; //250000
			npc.aiStyle = 6; //new
			Main.npcFrameCount[npc.type] = 1; //new
			aiType = -1; //new
			animationType = 10; //new
			npc.knockBackResist = 0f;
			npc.value = 540;
			npc.alpha = 255;
			npc.behindTiles = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.netAlways = true;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.StardancerBanner>();
			for (int k = 0; k < npc.buffImmune.Length; k++) {
				npc.buffImmune[k] = true;
			}
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0f, 0.0375f * 2, 0.125f * 2);
			if (npc.ai[3] > 0f)
				npc.realLife = (int)npc.ai[3];

			if (npc.target < 0 || npc.target == 255 || player.dead)
				npc.TargetClosest(true);

			if (npc.alpha != 0) {
				for (int num934 = 0; num934 < 2; num934++) {
					int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num935].noGravity = true;
					Main.dust[num935].noLight = true;
				}
			}
			npc.alpha -= 12;
			if (npc.alpha < 0)
				npc.alpha = 0;

			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (!tail && npc.ai[0] == 0f) {
					int current = npc.whoAmI;
					for (int num36 = 0; num36 < maxLength; num36++) {
						int trailing = 0;
						if (num36 >= 0 && num36 < minLength)
							trailing = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), ModContent.NPCType<CogTrapperBody>(), npc.whoAmI);
						else
							trailing = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), ModContent.NPCType<CogTrapperTail>(), npc.whoAmI);
						Main.npc[trailing].realLife = npc.whoAmI;
						Main.npc[trailing].ai[2] = (float)npc.whoAmI;
						Main.npc[trailing].ai[1] = (float)current;
						Main.npc[current].ai[0] = (float)trailing;
						npc.netUpdate = true;
						current = trailing;
					}
					tail = true;
				}

				if (!npc.active && Main.netMode == NetmodeID.Server) {
					NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
				}
			}

			int num180 = (int)(npc.position.X / 16f) - 1;
			int num181 = (int)((npc.position.X + (float)npc.width) / 16f) + 2;
			int num182 = (int)(npc.position.Y / 16f) - 1;
			int num183 = (int)((npc.position.Y + (float)npc.height) / 16f) + 2;

			if (num180 < 0)
				num180 = 0;
			if (num181 > Main.maxTilesX)
				num181 = Main.maxTilesX;
			if (num182 < 0)
				num182 = 0;
			if (num183 > Main.maxTilesY)
				num183 = Main.maxTilesY;

			bool flag94 = flies;
			npc.localAI[1] = 0f;
			if (directional) {
				if (npc.velocity.X < 0f)
					npc.spriteDirection = 1;
				else if (npc.velocity.X > 0f)
					npc.spriteDirection = -1;
			}

			if (player.dead) {
				npc.TargetClosest(false);
				flag94 = false;
				npc.velocity.Y = npc.velocity.Y + 10f;
				if ((double)npc.position.Y > Main.worldSurface * 16.0)
					npc.velocity.Y = npc.velocity.Y + 10f;
				if ((double)npc.position.Y > Main.rockLayer * 16.0) {
					for (int num957 = 0; num957 < 200; num957++) {
						if (Main.npc[num957].aiStyle == npc.aiStyle)
							Main.npc[num957].active = false;
					}
				}
			}

			float num188 = speed;
			float num189 = turnSpeed;
			Vector2 vector18 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num191 = player.position.X + (float)(player.width / 2);
			float num192 = player.position.Y + (float)(player.height / 2);
			int num42 = -1;
			int num43 = (int)(player.Center.X / 16f);
			int num44 = (int)(player.Center.Y / 16f);
			for (int num45 = num43 - 2; num45 <= num43 + 2; num45++) {
				for (int num46 = num44; num46 <= num44 + 15; num46++) {
					if (WorldGen.SolidTile2(num45, num46)) {
						num42 = num46;
						break;
					}
				}
				if (num42 > 0)
					break;
			}
			if (num42 > 0) {
				npc.defense = 5;
				num42 *= 16;
				float num47 = (float)(num42 - 200); //was 800
				if (player.position.Y > num47) {
					num192 = num47;
					if (Math.Abs(npc.Center.X - player.Center.X) < 125f) //was 500
					{
						if (npc.velocity.X > 0f)
							num191 = player.Center.X + 150f; //was 600
						else
							num191 = player.Center.X - 150f; //was 600
					}
				}
			}
			else {
				npc.defense = 0;
				num188 = expertMode ? 10.83f : 8.66f; //added 2.5
				num189 = expertMode ? 0.208f : 0.166f; //added 0.05
			}

			float num48 = num188 * 1.5f;
			float num49 = num188 * 0.8f;
			float num50 = npc.velocity.Length();
			if (num50 > 0f) {
				if (num50 > num48) {
					npc.velocity.Normalize();
					npc.velocity *= num48;
				}
				else if (num50 < num49) {
					npc.velocity.Normalize();
					npc.velocity *= num49;
				}
			}

			if (num42 > 0) {
				for (int num51 = 0; num51 < 200; num51++) {
					if (Main.npc[num51].active && Main.npc[num51].type == npc.type && num51 != npc.whoAmI) {
						Vector2 vector3 = Main.npc[num51].Center - npc.Center;
						if (vector3.Length() < 400f) {
							vector3.Normalize();
							vector3 *= 1000f;
							num191 -= vector3.X;
							num192 -= vector3.Y;
						}
					}
				}
			}
			else {
				for (int num52 = 0; num52 < 200; num52++) {
					if (Main.npc[num52].active && Main.npc[num52].type == npc.type && num52 != npc.whoAmI) {
						Vector2 vector4 = Main.npc[num52].Center - npc.Center;
						if (vector4.Length() < 60f) {
							vector4.Normalize();
							vector4 *= 200f;
							num191 -= vector4.X;
							num192 -= vector4.Y;
						}
					}
				}
			}

			num191 = (float)((int)(num191 / 16f) * 16);
			num192 = (float)((int)(num192 / 16f) * 16);
			vector18.X = (float)((int)(vector18.X / 16f) * 16);
			vector18.Y = (float)((int)(vector18.Y / 16f) * 16);
			num191 -= vector18.X;
			num192 -= vector18.Y;
			float num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
			if (npc.ai[1] > 0f && npc.ai[1] < (float)Main.npc.Length) {
				try {
					vector18 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					num191 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - vector18.X;
					num192 = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - vector18.Y;
				}
				catch {
				}
				npc.rotation = (float)System.Math.Atan2((double)num192, (double)num191) + 1.57f;
				num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
				int num194 = npc.width;
				num193 = (num193 - (float)num194) / num193;
				num191 *= num193;
				num192 *= num193;
				npc.velocity = Vector2.Zero;
				npc.position.X = npc.position.X + num191;
				npc.position.Y = npc.position.Y + num192;
				if (directional) {
					if (num191 < 0f)
						npc.spriteDirection = 1;
					if (num191 > 0f)
						npc.spriteDirection = -1;
				}
			}
			else {
				num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
				float num196 = System.Math.Abs(num191);
				float num197 = System.Math.Abs(num192);
				float num198 = num188 / num193;
				num191 *= num198;
				num192 *= num198;
				bool flag21 = false;
				if (!flag21) {
					if ((npc.velocity.X > 0f && num191 > 0f) || (npc.velocity.X < 0f && num191 < 0f) || (npc.velocity.Y > 0f && num192 > 0f) || (npc.velocity.Y < 0f && num192 < 0f)) {
						if (npc.velocity.X < num191)
							npc.velocity.X = npc.velocity.X + num189;
						else if (npc.velocity.X > num191)
							npc.velocity.X = npc.velocity.X - num189;

						if (npc.velocity.Y < num192)
							npc.velocity.Y = npc.velocity.Y + num189;
						else if (npc.velocity.Y > num192)
							npc.velocity.Y = npc.velocity.Y - num189;

						if ((double)System.Math.Abs(num192) < (double)num188 * 0.2 && ((npc.velocity.X > 0f && num191 < 0f) || (npc.velocity.X < 0f && num191 > 0f))) {
							if (npc.velocity.Y > 0f)
								npc.velocity.Y = npc.velocity.Y + num189 * 2f;
							else
								npc.velocity.Y = npc.velocity.Y - num189 * 2f;
						}

						if ((double)System.Math.Abs(num191) < (double)num188 * 0.2 && ((npc.velocity.Y > 0f && num192 < 0f) || (npc.velocity.Y < 0f && num192 > 0f))) {
							if (npc.velocity.X > 0f)
								npc.velocity.X = npc.velocity.X + num189 * 2f; //changed from 2
							else
								npc.velocity.X = npc.velocity.X - num189 * 2f; //changed from 2
						}
					}
					else {
						if (num196 > num197) {
							if (npc.velocity.X < num191)
								npc.velocity.X = npc.velocity.X + num189 * 1.1f; //changed from 1.1
							else if (npc.velocity.X > num191)
								npc.velocity.X = npc.velocity.X - num189 * 1.1f; //changed from 1.1

							if ((double)(System.Math.Abs(npc.velocity.X) + System.Math.Abs(npc.velocity.Y)) < (double)num188 * 0.5) {
								if (npc.velocity.Y > 0f)
									npc.velocity.Y = npc.velocity.Y + num189;
								else
									npc.velocity.Y = npc.velocity.Y - num189;
							}
						}
						else {
							if (npc.velocity.Y < num192)
								npc.velocity.Y = npc.velocity.Y + num189 * 1.1f;
							else if (npc.velocity.Y > num192)
								npc.velocity.Y = npc.velocity.Y - num189 * 1.1f;

							if ((double)(System.Math.Abs(npc.velocity.X) + System.Math.Abs(npc.velocity.Y)) < (double)num188 * 0.5) {
								if (npc.velocity.X > 0f)
									npc.velocity.X = npc.velocity.X + num189;
								else
									npc.velocity.X = npc.velocity.X - num189;
							}
						}
					}
				}
			}
			npc.rotation = (float)System.Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + 1.57f;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(1) == 400) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GravityModulator>());
			}
            if (Main.rand.Next(2) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StarEnergy>());
			}
			int[] lootTable = {
				ModContent.ItemType<ProtectorateBody>(),
				ModContent.ItemType<ProtectorateLegs>()
			};
			if (Main.rand.Next(30) == 0) {
				int loot = Main.rand.Next(lootTable.Length);
				{
					npc.DropItem(lootTable[loot]);
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Stardancer/Stardancer1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Stardancer/Stardancer2"), 1f);
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 20;
				npc.height = 20;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 5; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default(Color), .5f);
					Main.dust[num622].velocity *= 2f;
				}
				for (int num623 = 0; num623 < 10; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 4f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.DungeonSpirit, 0f, 0f, 100, default(Color), .5f);
					Main.dust[num624].velocity *= 1f;
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
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			{
				Microsoft.Xna.Framework.Color color1 = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
				Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
				int r1 = (int)color1.R;
				drawOrigin.Y += 30f;
				drawOrigin.Y += 8f;
				--drawOrigin.X;
				Vector2 position1 = npc.Bottom - Main.screenPosition;
				Texture2D texture2D2 = Main.glowMaskTexture[239];
				float num11 = (float)((double)Main.GlobalTime % 1.0 / 1.0);
				float num12 = num11;
				if ((double)num12 > 0.5)
					num12 = 1f - num11;
				if ((double)num12 < 0.0)
					num12 = 0.0f;
				float num13 = (float)(((double)num11 + 0.5) % 1.0);
				float num14 = num13;
				if ((double)num14 > 0.5)
					num14 = 1f - num13;
				if ((double)num14 < 0.0)
					num14 = 0.0f;
				Microsoft.Xna.Framework.Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
				drawOrigin = r2.Size() / 2f;
				Vector2 position3 = position1 + new Vector2(0.0f, -20f);
				Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color(84, 207, 255) * 1.6f;
				Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3, npc.rotation, drawOrigin, npc.scale * 0.35f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num15 = 1f + num11 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num12, npc.rotation, drawOrigin, npc.scale * 0.5f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num16 = 1f + num13 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num14, npc.rotation, drawOrigin, npc.scale * 0.5f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				Texture2D texture2D3 = Main.extraTexture[89];
				Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
				drawOrigin = r3.Size() / 2f;
				Vector2 scale = new Vector2(0.75f, 1f + num16) * 1.5f;
				float num17 = 1f + num13 * 0.75f;
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Starfarer/CogTrapperHead_Glow"));

			}
		}
	}
}