using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Block;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
	public class UnstableWisp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Wisp");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 32;
			npc.lifeMax = 150;
			npc.knockBackResist = 0f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.friendly = false;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>(), };
				return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY > Main.rockLayer && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f && !spawnInfo.playerSafe && !spawnInfo.invasion ? 2f : 0f;

			}
			return 0f;
		}

		public override bool PreAI()
		{
			bool inRange = false;
			Vector2 target = Vector2.Zero;
			float triggerRange = 280f;
			for (int i = 0; i < 255; i++) {
				if (Main.player[i].active && !Main.player[i].dead) {
					float playerX = Main.player[i].position.X + (float)(Main.player[i].width / 2);
					float playerY = Main.player[i].position.Y + (float)(Main.player[i].height / 2);
					float distOrth = Math.Abs(npc.position.X + (float)(npc.width / 2) - playerX) + Math.Abs(npc.position.Y + (float)(npc.height / 2) - playerY);
					if (distOrth < triggerRange) {
						if (Main.player[i].Hitbox.Intersects(npc.Hitbox)) {
							npc.life = 0;
							npc.HitEffect(0, 10.0);
							npc.checkDead();
							npc.active = false;
							return false;
						}
						triggerRange = distOrth;
						target = Main.player[i].Center;
						inRange = true;
					}
				}
			}
			if (inRange) {
				Vector2 delta = target - npc.Center;
				delta.Normalize();
				delta *= 0.95f;
				npc.velocity = (npc.velocity * 10f + delta) * (1f / 11f);
				return false;
			}
			if (npc.velocity.Length() > 0.2f) {
				npc.velocity *= 0.98f;
			}
			return false;
		}

		public override bool CheckDead()
		{
			Vector2 center = npc.Center;
			Projectile.NewProjectile(center.X, center.Y, 0f, 0f, ModContent.ProjectileType<UnstableWisp_Explosion>(), 100, 0f, Main.myPlayer);
			return true;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.1f;
			if (npc.frameCounter >= Main.npcFrameCount[npc.type])
				npc.frameCounter -= Main.npcFrameCount[npc.type];
			int num = (int)npc.frameCounter;
			npc.frame.Y = num * frameHeight;
			npc.spriteDirection = npc.direction;
		}
	}
}
