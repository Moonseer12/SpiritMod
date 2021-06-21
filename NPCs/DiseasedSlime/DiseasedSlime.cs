using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Sets.Bismite;
using SpiritMod.Items.Consumable.Food;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.DiseasedSlime
{
	public class DiseasedSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diseased Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 32;
			npc.damage = 18;
			npc.defense = 5;
			npc.lifeMax = 65;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath22;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.value = 20f;
			npc.alpha = 60;
			npc.knockBackResist = .6f;
			npc.aiStyle = 1;
			aiType = NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.DiseasedSlimeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe) {
				return 0f;
			}
			return SpawnCondition.Underground.Chance * 0.24f;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BismiteCrystal>(), Main.rand.Next(2, 4) + 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1, 3) + 1);

			if (Main.rand.Next(10000) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SlimeStaff);
			}
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Cake>());
            }
        }

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(3) == 0) {
				target.AddBuff(ModContent.BuffType<FesteringWounds>(), 240);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 || npc.life >= 0) {
				int d = 193;
				for (int k = 0; k < 12; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.Green, 0.7f);
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.Green, 0.7f);
				}
			}
		}
	}
}
