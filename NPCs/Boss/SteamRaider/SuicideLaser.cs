using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class SuicideLaser : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Bomber");
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		int timer = 0;
		public override void SetDefaults()
		{
			npc.width = 56;
			npc.height = 46;
			npc.damage = 44;
			npc.defense = 4;
			npc.noGravity = true;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit4;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.value = 0f;
			npc.knockBackResist = .0f;
			npc.noTileCollide = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
			{
				float sineAdd = (float)Math.Sin(alphaCounter) + 3;
				Vector2 drawPos1 = npc.Center - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Main.spriteBatch.Draw(SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);
			}
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.alpha != 255) {
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/SteamRaider/LaserBase_Glow"));
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 117, new Color(0, 255, 142), .6f);
			}
			if (npc.life <= 0) {
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 14);
				Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 44);
				Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 4);
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Ultrabright, 0f, -2f, 117, new Color(0, 255, 142), .6f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X += ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					dust.position.Y += ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != npc.Center) {
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
			}
		}
		float alphaCounter;
		public override void AI()
		{
			alphaCounter += .08f;
			timer++;
			if (timer >= 90) {
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 14);
				Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 44);
				Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 4);
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, 0f, -2f, 117, new Color(0, 255, 142), .6f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					dust.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					if (Main.dust[num].position != npc.Center) {
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
				npc.active = false;
			}
			npc.TargetClosest(true);
			float speed = 16f;
			float acceleration = 0.16f;
			Vector2 vector2 = new Vector2(npc.position.X + npc.width * 0.5F, npc.position.Y + npc.height * 0.5F);
			float xDir = Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5F) - vector2.X;
			float yDir = Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5F) - vector2.Y;
			float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);

			float num10 = speed / length;
			xDir = xDir * num10;
			yDir = yDir * num10;
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
			npc.noTileCollide = true;
			npc.localAI[0] += 1f;
			if (npc.localAI[0] == 12f) {
				npc.localAI[0] = 0f;
				for (int j = 0; j < 12; j++) {
					Vector2 vector21 = Vector2.UnitX * -npc.width / 2f;
					vector21 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector21 = Utils.RotatedBy(vector21, (npc.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(npc.Center, 0, 0, DustID.Electric, 0f, 0f, 160, default, 1f);
					Main.dust[num8].scale = .8f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = npc.Center;
					Main.dust[num8].velocity = npc.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(npc.Center - npc.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			Vector2 direction9 = Main.player[npc.target].Center - npc.Center;
			direction9.Normalize();
			npc.rotation = direction9.ToRotation() + 1.57f + 3.14f;
			Player target = Main.player[npc.target];
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(3, 252, 215, 100);
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			npc.life = 0;
			Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 14);
			Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 44);
			Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 4);
			for (int i = 0; i < 40; i++) {
				int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, 0f, -2f, 117, new Color(0, 255, 142), .6f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X = dust.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				if (Main.dust[num].position != npc.Center) {
					Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
	}
}
