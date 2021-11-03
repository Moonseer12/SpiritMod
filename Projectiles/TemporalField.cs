﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class TemporalField : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Temporal Field");

		public override void SetDefaults()
		{
			projectile.width = 600;       //projectile width
			projectile.height = 300;  //projectile height
			projectile.friendly = true;      //make that the projectile will not damage you
			projectile.melee = true;         // 
			projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 1;      //how many npc will penetrate
			projectile.timeLeft = 600;   //how many time projectile projectile has before disepire
			projectile.light = 0.75f;    // projectile light
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
			projectile.ignoreWater = true;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			var rect = new Rectangle((int)projectile.Center.X, (int)projectile.position.Y, 200, 200);
			for (int index1 = 0; index1 < Main.maxNPCs; index1++)
				if (rect.Contains(Main.npc[index1].Center.ToPoint()))
					Main.npc[index1].AddBuff(ModContent.BuffType<Buffs.Slow>(), 240);
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 40; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 2f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
				}
			}
		}
	}
}
