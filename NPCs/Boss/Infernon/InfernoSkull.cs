﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.InfernonDrops;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	[AutoloadBossHead]
	public class InfernoSkull : ModNPC
	{

		//bool txt = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernus Skull");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 100;
			npc.height = 80;
			npc.knockBackResist = 0f;
			npc.defense = 20;
			npc.damage = 50;
			npc.lifeMax = 5500;
			bossBag = ModContent.ItemType<InfernonBag>();
			npc.aiStyle = -1;
			npc.npcSlots = 10;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon");
			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.10f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}


		public override bool PreNPCLoot()
		{
			MyWorld.downedInfernon = true;
			return true;
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
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Infernon/InfernonSkull_Glow"));
		}
		public override void NPCLoot()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int centerX = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
				int centerY = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
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
						{
							NetMessage.SendTileSquare(-1, x, y, 1);
						}
						else
						{
							WorldGen.SquareTileFrame(x, y, true);
						}
					}
				}
			}
			if (Main.expertMode)
				npc.DropBossBags();
		}

		//int timer1 = 0;
		int timer = 0;
		public override void AI()
		{
			if (npc.localAI[0] == 0f) {
				npc.localAI[0] = npc.Center.Y;
				npc.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if (npc.Center.Y >= npc.localAI[0]) {
				npc.localAI[1] = -1f;
				npc.netUpdate = true;
			}
			if (npc.Center.Y <= npc.localAI[0] - 10f) {
				npc.localAI[1] = 1f;
				npc.netUpdate = true;
			}
			npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y + 0.05f * npc.localAI[1], -2f, 2f);
			npc.ai[0] += 1f;
			npc.netUpdate = true;

			//Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			int damage = expertMode ? 16 : 30;

			timer++;
			if (timer == 0 || timer == 200) {
				float spread = 45f * 0.0174f;
				double startAngle = Math.Atan2(1, 0) - spread / 2;
				double deltaAngle = spread / 8f;
				double offsetAngle;
				int i;
				for (i = 0; i < 4; i++) {
					offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
					npc.netUpdate = true;
				}
			}
			if (timer == 210 || timer == 220 || timer == 230 || timer == 240 || timer == 250 || timer == 260 || timer == 270 || timer == 280 || timer == 290 || timer == 300 || timer == 310 || timer == 320 || timer == 340 || timer == 350) {
				if (npc.life >= (npc.lifeMax / 3)) {
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 33);
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 9.5f;
					direction.Y *= 9.5f;

					int amountOfProjectiles = 1;
					for (int z = 0; z < amountOfProjectiles; ++z) {
						float A = Main.rand.Next(-200, 200) * 0.03f;
						float B = Main.rand.Next(-200, 200) * 0.03f;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<InfernalBlastHostile>(), damage, 1, Main.myPlayer, 0, 0);

					}
				}
			}


			if (timer == 400) {
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 200, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X + 200, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X - 200, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 200, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);

				timer = 0;
			}
			else if (Main.rand.Next(90) == 1 && npc.life <= (npc.lifeMax / 3)) {
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 500, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X + 500, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X - 500, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);

				Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 500, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, hitDirection, -1f, 0, default, 1f);
			}
			if (npc.life <= 0) {
				npc.position.X = npc.position.X + (npc.width / 2);
				npc.position.Y = npc.position.Y + (npc.height / 2);
				npc.width = 80;
				npc.height = 80;
				npc.position.X = npc.position.X - (npc.width / 2);
				npc.position.Y = npc.position.Y - (npc.height / 2);
				for (int num621 = 0; num621 < 200; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Fire, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 400; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Fire, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Fire, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

	}
}