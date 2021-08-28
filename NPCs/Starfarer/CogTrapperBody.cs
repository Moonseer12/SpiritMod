﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.NPCs.Starfarer
{
	public class CogTrapperBody : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardancer");
		}

		public override void SetDefaults()
		{
			npc.damage = 25; //70
			npc.npcSlots = 0f;
			npc.width = 14; //324
			npc.height = 20; //216
			npc.defense = 14;
			npc.lifeMax = 300; //250000
			npc.aiStyle = 6; //new
			Main.npcFrameCount[npc.type] = 1; //new
			aiType = -1; //new
			animationType = 10; //new
			npc.dontCountMe = true;
			npc.knockBackResist = 0f;
			npc.alpha = 255;
			npc.behindTiles = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.netAlways = true;
			for (int k = 0; k < npc.buffImmune.Length; k++) {
				npc.buffImmune[k] = true;
			}
			npc.dontCountMe = true;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0f, 0.0375f * 2, 0.125f * 2);
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				npc.localAI[0] += Main.rand.Next(4);
				if (npc.localAI[0] >= (float)Main.rand.Next(700, 1000)) {
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 9);
					npc.localAI[0] = 0f;
					npc.TargetClosest(true);
					if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)) {
						float num941 = 1f; //speed
						Vector2 vector104 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)(npc.height / 2));
						float num942 = player.position.X + (float)player.width * 0.5f - vector104.X + (float)Main.rand.Next(-20, 21);
						float num943 = player.position.Y + (float)player.height * 0.5f - vector104.Y + (float)Main.rand.Next(-20, 21);
						float num944 = (float)Math.Sqrt((double)(num942 * num942 + num943 * num943));
						num944 = num941 / num944;
						num942 *= num944;
						num943 *= num944;
						num942 += (float)Main.rand.Next(-10, 11) * 0.05f;
						num943 += (float)Main.rand.Next(-10, 11) * 0.05f;
						int num945 = expertMode ? 10 : 15;
						int num946 = ModContent.ProjectileType<Starshock>();
						vector104.X += num942 * 5f;
						vector104.Y += num943 * 5f;
						int num947 = Projectile.NewProjectile(vector104.X, vector104.Y, num942, num943, num946, num945, 0f, Main.myPlayer, 0f, 0f);
						Main.projectile[num947].timeLeft = 180;
						npc.netUpdate = true;
					}
				}
			}

			if (!Main.npc[(int)npc.ai[1]].active) {
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.active = false;
			}

			if (Main.npc[(int)npc.ai[1]].alpha < 128) {
				if (npc.alpha != 0) {
					for (int num934 = 0; num934 < 2; num934++) {
						int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default, 2f);
						Main.dust[num935].noGravity = true;
						Main.dust[num935].noLight = true;
					}
				}
				npc.alpha -= 42;
				if (npc.alpha < 0)
					npc.alpha = 0;
			}
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override bool PreNPCLoot()
		{
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{

			for (int k = 0; k < 5; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, hitDirection, -1f, 0, default, 1f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Stardancer/Stardancer3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Stardancer/Stardancer4"), 1f);
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 20;
				npc.height = 20;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 5; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default, .5f);
					Main.dust[num622].velocity *= 2f;
				}
				for (int num623 = 0; num623 < 10; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default, 1f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 4f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.DungeonSpirit, 0f, 0f, 100, default, .5f);
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

			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Starfarer/CogTrapperBody_Glow"));

		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.65f);
		}
	}
}