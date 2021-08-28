﻿using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	class BeetleArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beetle Arrow");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.owner == Main.myPlayer)
				Main.LocalPlayer.AddBuff(ModContent.BuffType<BeetleFortitude>(), 180);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			if (projectile.owner == Main.myPlayer)
				Main.LocalPlayer.AddBuff(ModContent.BuffType<BeetleFortitude>(), 180);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_Purple);
			}
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}
