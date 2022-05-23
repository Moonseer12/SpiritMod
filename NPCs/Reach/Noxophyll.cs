using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.Reach
{
	public class Noxophyll : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Noxophyll");
			Main.npcFrameCount[npc.type] = 8;
			Main.npcCatchable[npc.type] = true;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 24;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.catchItem = (short)ModContent.ItemType<Items.Sets.BriarDrops.ReachFishingCatch>();
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			npc.npcSlots = 0;
			aiType = NPCID.Goldfish;
			npc.dontCountMe = true;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.GrassBlades, 1.5f * hitDirection, -1.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, 7, 1.5f * hitDirection, -1.5f, 0, default, .34f);
			}
		}

		public override void AI() => npc.spriteDirection = npc.direction;

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.player.GetSpiritPlayer().ZoneReach && spawnInfo.water ? 0.511f : 0f;
	}
}