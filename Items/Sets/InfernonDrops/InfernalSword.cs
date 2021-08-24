using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.InfernonDrops
{
	public class InfernalSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Char Fury");
			Tooltip.SetDefault("Shoots out two blazes that cause foes to combust, multiple hits causing the combustion to deal more damage");
		}


		public override void SetDefaults()
		{
			item.width = 52;
			item.height = 64;
			item.rare = ItemRarityID.Pink;
			item.damage = 43;
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 25;
			item.melee = true;
			item.shoot = ModContent.ProjectileType<CombustionBlaze>();
			item.shootSpeed = 3f;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(2) == 0)
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Fire);
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(ModContent.BuffType<StackingFireBuff>(), 300);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int I = 0; I < 2; I++) {
				Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(500, 900) / 100), speedY * (Main.rand.Next(500, 900) / 100), ModContent.ProjectileType<CombustionBlaze>(), item.damage / 6 * 5, item.knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
