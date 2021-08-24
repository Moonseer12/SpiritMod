using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Thrown.Charge
{
	public class TikiJavelinProj1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiki Javelin Tracer");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 39;
			projectile.aiStyle = 1;
			projectile.friendly = false;
			projectile.thrown = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 600;
			projectile.alpha = 255;
			projectile.extraUpdates = 16;
			projectile.light = 0;
			projectile.ignoreWater = true;
			aiType = ProjectileID.ThrowingKnife;
		}
		public override bool PreAI()
		{
			if (projectile.owner != Main.myPlayer) return true;
			int num = 5;
			int index2 = Dust.NewDust(projectile.position, 1, 1, DustID.Fire, 0.0f, 0.0f, 0, new Color(), 1.3f);
			Main.dust[index2].position = projectile.Center - projectile.velocity / num;
			Main.dust[index2].velocity *= 0f;
			Main.dust[index2].noGravity = true;
			Main.dust[index2].noLight = true;
			return true;
		}
	}
}
