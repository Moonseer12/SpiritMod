using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Tiles.Block;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
	public class SpiritSkull : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Skull");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 52;
			npc.damage = 35;
			npc.defense = 10;
			npc.knockBackResist = 0.2f;
			npc.lifeMax = 295;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 0.75f;
		}
        public override void NPCLoot()
        {
              if (Main.rand.NextBool(90))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Sets.SpiritBiomeDrops.SpiritFlameStaff>());
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Spirit/SpiritSkull_Glow"));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
            if (!player.GetSpiritPlayer().ZoneSpirit)
            {
                return 0f;
            }
            if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>(), };
				return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY > Main.rockLayer && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f && !spawnInfo.playerSafe && !spawnInfo.invasion ? 4f : 0f;
			}
			return 0f;
		}


		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			float velMax = 1f;
			float acceleration = 0.011f;
			npc.TargetClosest(true);
			Vector2 center = npc.Center;
			float deltaX = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - center.X;
			float deltaY = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - center.Y;
			float distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
			npc.ai[1] += 1f;
			if ((double)npc.ai[1] > 600.0) {
				acceleration *= 8f;
				velMax = 4f;
				if ((double)npc.ai[1] > 650.0) {
					npc.ai[1] = 0f;
				}
			}
			else if ((double)distance < 250.0) {
				npc.ai[0] += 0.9f;
				if (npc.ai[0] > 0f) {
					npc.velocity.Y = npc.velocity.Y + 0.019f;
				}
				else {
					npc.velocity.Y = npc.velocity.Y - 0.019f;
				}
				if (npc.ai[0] < -100f || npc.ai[0] > 100f) {
					npc.velocity.X = npc.velocity.X + 0.019f;
				}
				else {
					npc.velocity.X = npc.velocity.X - 0.019f;
				}
				if (npc.ai[0] > 200f) {
					npc.ai[0] = -200f;
				}
			}
			if ((double)distance > 350.0) {
				velMax = 5f;
				acceleration = 0.3f;
			}
			else if ((double)distance > 300.0) {
				velMax = 3f;
				acceleration = 0.2f;
			}
			else if ((double)distance > 250.0) {
				velMax = 1.5f;
				acceleration = 0.1f;
			}
			float stepRatio = velMax / distance;
			float velLimitX = deltaX * stepRatio;
			float velLimitY = deltaY * stepRatio;
			if (Main.player[npc.target].dead) {
				velLimitX = (float)((double)((float)npc.direction * velMax) / 2.0);
				velLimitY = (float)((double)(-(double)velMax) / 2.0);
			}
			if (npc.velocity.X < velLimitX) {
				npc.velocity.X = npc.velocity.X + acceleration;
			}
			else if (npc.velocity.X > velLimitX) {
				npc.velocity.X = npc.velocity.X - acceleration;
			}
			if (npc.velocity.Y < velLimitY) {
				npc.velocity.Y = npc.velocity.Y + acceleration;
			}
			else if (npc.velocity.Y > velLimitY) {
				npc.velocity.Y = npc.velocity.Y - acceleration;
			}
			if ((double)velLimitX > 0.0) {
				npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX);
			}
			if ((double)velLimitX < 0.0) {
				npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX) + 3.14f;
			}
			npc.spriteDirection = -npc.direction;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.05f, 0.09f, 0.4f);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
			}
		}
	}
}
