using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.Fathomless_Chest
{
	public class Visual_Projectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purity Light");
		}
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.hide = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 180;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			if (Main.rand.Next(1) == 0)
			{
				int index2 = Dust.NewDust(projectile.Center, 8, 8, DustID.DungeonSpirit, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center;
				Main.dust[index2].velocity = projectile.velocity;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].scale = projectile.scale;
			}
			Player player = Main.LocalPlayer;
			float x = 0.15f;
			float y = 0.15f;

			Vector2 vector2_1 = projectile.velocity + new Vector2((float)Math.Sign(player.Center.X - projectile.Center.X), (float)Math.Sign(player.Center.Y - projectile.Center.Y)) * new Vector2(x, y);
			projectile.velocity = vector2_1;
			if ((double)projectile.velocity.Length() > 4.0)
			{
				Vector2 vector2_2 = projectile.velocity * (4f / projectile.velocity.Length());
				projectile.velocity = vector2_2;
			}
		}
	}
}