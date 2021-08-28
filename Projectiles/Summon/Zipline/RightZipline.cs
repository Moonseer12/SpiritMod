using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zipline
{
	public class RightZipline : ModProjectile
	{
		Vector2 direction9 = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Right Zipline");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 12;
			projectile.height = 12;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 600;
			projectile.alpha = 0;
			projectile.tileCollide = true;
		}

		bool chain = false;
		int leftValue;
		int distance = 9999;
		bool stuck = false;
		float alphaCounter;

		public override bool PreAI()
		{
			alphaCounter += 0.04f;
			if (!stuck) {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			}
			projectile.timeLeft = 50;
			leftValue = (int)projectile.ai[1];
			if (leftValue < (double)Main.projectile.Length && leftValue != 0) {
				Projectile other = Main.projectile[leftValue];
				if (other.active) {
					direction9 = other.Center - projectile.Center;
					distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
					chain = true;
				}
				else {
					chain = false;
				}
			}
			else {
				chain = false;
			}
			if (stuck) {
				projectile.velocity = Vector2.Zero;
			}
			return true;
		}
		public override void AI()
		{
			if (stuck)
				DoDustEffect(projectile.Center, 18f);
		}
		private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
		{
			float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
			Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

			int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.Electric);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale *= .26f;
			Main.dust[dust].velocity = vel;
			Main.dust[dust].customData = follow;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (chain && distance < 2000 && stuck) {
				Projectile other = Main.projectile[leftValue];
				direction9 = other.Center - projectile.Center;
				direction9.Normalize();
				//	direction9 *= 6;
				ProjectileExtras.DrawChain(projectile.whoAmI, other.Center,
				"SpiritMod/Projectiles/Summon/Zipline/Zipline_Chain", false, 0, true, direction9.X, direction9.Y);
			}

		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!stuck) {
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 52);
			}
			if (oldVelocity.X != projectile.velocity.X) //if its an X axis collision
				{
				if (projectile.velocity.X > 0) {
					projectile.rotation = 1.57f;
				}
				else {
					projectile.rotation = 4.71f;
				}
			}
			if (oldVelocity.Y != projectile.velocity.Y) //if its a Y axis collision
			{
				if (projectile.velocity.Y > 0) {
					projectile.rotation = 3.14f;
				}
				else {
					projectile.rotation = 0f;
				}
			}
			stuck = true;
			return false;
		}
	}
}
