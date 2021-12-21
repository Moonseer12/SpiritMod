using SpiritMod.Items.Accessory;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Alien
{
	public class Alien : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alien");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 44;
			npc.damage = 70;
			npc.defense = 30;
			npc.lifeMax = 600;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.WeaponImbueVenom] = true;
			npc.HitSound = SoundID.NPCHit6;
			npc.DeathSound = SoundID.NPCDeath8;
			npc.value = 10000f;
			npc.knockBackResist = .25f;
			npc.aiStyle = 26;
			aiType = NPCID.Unicorn;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            if (NPC.downedMechBossAny && Main.eclipse && spawnInfo.player.ZoneOverworldHeight)
            {
                return 0.07f;
            }
			return 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Alien1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Alien2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Alien2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Alien2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Alien2"), 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.40f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 1) {
				target.AddBuff(BuffID.Venom, 260);
			}
		}
	}
}
