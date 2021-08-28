using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Projectiles
{
	public class MechBat : ModProjectile
	{
		int moveSpeed = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mech Bat");
			Main.projFrames[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 20;
			projectile.height = 20;
			projectile.timeLeft = 1000;
			;
			projectile.friendly = false;
			projectile.penetrate = 1;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			projectile.Kill();
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			for (int k = 0; k < 15; k++) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
            Gore.NewGore(projectile.position, projectile.velocity, mod.GetGoreSlot("Gores/Mech7"), 1f);
            Gore.NewGore(projectile.position, projectile.velocity, mod.GetGoreSlot("Gores/Mech8"), 1f);
        }
		public override bool PreAI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var proj in list) {
				if (projectile != proj && proj.friendly) {
					projectile.Kill();
				}
			}
			projectile.frameCounter++;
			if (projectile.frameCounter >= 4) {
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 2)
					projectile.frame = 0;
			}
			return true;
		}
		public override void AI()
		{
			Vector2 vector207 = new Vector2((float)projectile.width * 2, (float)projectile.height * 2) * projectile.scale * 0.85f;
			vector207 /= 2f;
			Vector2 value112 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * vector207;
			Vector2 position178 = projectile.Center + value112;
			int num1442 = Dust.NewDust(position178, 0, 0, DustID.Electric, 0f, 0f, 0, default, .5f);
			Main.dust[num1442].position = projectile.Center + value112;
			Main.dust[num1442].velocity = Vector2.Zero;
			Main.dust[num1442].noGravity = true;

			int range = 650;   //How many tiles away the projectile targets NPCs
							   //int targetingMax = 20; //how many frames allowed to target nearest instead of shooting
							   //float shootVelocity = 16f; //magnitude of the shoot vector (speed of arrows shot)

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			foreach (Player player in Main.player) {
				//if npc is a valid target (active, not friendly, and not a critter)
				if (player.active) {
					//if npc is within 50 blocks
					float dist = projectile.Distance(player.Center);
					if (dist / 16 < range) {
						//if npc is closer than closest found npc
						if (dist < lowestDist) {
							lowestDist = dist;

							//target this npc
							projectile.ai[1] = player.whoAmI;
						}
					}
				}
			}

			Player target = (Main.player[(int)projectile.ai[1]] ?? new Player());
			if (target.active && projectile.Distance(target.Center) / 16 < range && projectile.timeLeft < 945) {
				if (projectile.Center.X >= target.Center.X && moveSpeed >= -30) // flies to players x position
				{
					moveSpeed--;
				}

				if (projectile.Center.X <= target.Center.X && moveSpeed <= 30) {
					moveSpeed++;
				}

				projectile.velocity.X = moveSpeed * 0.08f;
				projectile.velocity.Y = 1f;
			}
		}
	}
}