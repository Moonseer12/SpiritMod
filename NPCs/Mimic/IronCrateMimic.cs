using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Mimic
{
	public class IronCrateMimic : ModNPC
	{
		bool jump = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Iron Crate Mimic");
			Main.npcFrameCount[npc.type] = 4;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.width = 48;
			npc.height = 42;
			npc.damage = 17;
			npc.defense = 15;
			npc.lifeMax = 90;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 360f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 25;
			aiType = 85;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.IronCrateMimicBanner>();
		}
		int frame = 2;
		int timer = 0;
		int mimictimer = 0;
		public override void AI()
		{
			mimictimer++;
			if (mimictimer <= 80) {
				frame = 0;
				mimictimer = 81;
			}
			Player target = Main.player[npc.target];
			{
				timer++;
				if (timer == 4) {
					frame++;
					timer = 0;
				}
				if (frame == 4) {
					frame = 1;
				}
			}
			if (npc.collideY && jump && npc.velocity.Y > 0) {
				if (Main.rand.Next(4) == 0) {
					jump = false;
					for (int i = 0; i < 20; i++) {
						int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.SpookyWood, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
						Main.dust[dust].noGravity = true;
					}
				}
			}
			if (!npc.collideY)
				jump = true;

		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 720) {
				Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
				for (int k = 0; k < npc.oldPos.Length; k++) {
					var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
			return true;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				int a = Gore.NewGore(npc.position, npc.velocity / 6, 220);
				int a1 = Gore.NewGore(npc.position, npc.velocity / 6, 221);
				int a2 = Gore.NewGore(npc.position, npc.velocity / 6, 222);
			}
			if (npc.life <= 0 || npc.life >= 0) {
				int d = 8;
				for (int k = 0; k < 10; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .57f);
				}

				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .57f);
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .77f);
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .47f);
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .57f);
			}
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.IronCrate);
		}
	}
}
