using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.OceanSlime
{
	public class OceanSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			npc.width = 18;
			npc.height = 14;
			npc.damage = 17;
			npc.defense = 6;
			npc.lifeMax = 60;
			npc.HitSound = SoundID.NPCHit2;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 860f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 1;
			aiType = NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.CoconutSlimeBanner>();
		}
		public override void NPCLoot()
		{
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Coconut>(), Main.rand.Next(3) + 6);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 207;
			int d1 = 207;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OceanSlime1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OceanSlime2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OceanSlime3"), 1f);
			}
		}
	}
}
