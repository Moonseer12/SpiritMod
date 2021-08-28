using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Pet
{
	public class Lantern : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lantern");
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ZephyrFish);
			aiType = ProjectileID.ZephyrFish;
			projectile.width = 46;
			projectile.height = 46;
			projectile.scale = 0.9f;
		}

		public override bool PreAI()
		{
			int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Clentaminator_Green, 0, -1f, 0, default(Color), 1f);
			Main.dust[d].scale *= 0.5f;
			Main.dust[d].noGravity = true;
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.75f/2, 1.5f/2, 0.75f/2);

			Player player = Main.player[projectile.owner];
			player.zephyrfish = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.dead)
				modPlayer.lanternPet = false;

			if (modPlayer.lanternPet)
				projectile.timeLeft = 2;
		}

	}
}