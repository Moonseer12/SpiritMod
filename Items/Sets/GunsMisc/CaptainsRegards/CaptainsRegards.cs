using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.CaptainsRegards
{
	public class CaptainsRegards : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Captain's Regards");
			Tooltip.SetDefault("'Pirate diplomacy at its finest'");
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.ranged = true;
			item.width = 65;
			item.height = 62;
			item.useTime = 24;
			item.useAnimation = 22;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 6;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item36;
			item.autoReuse = true;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 6.8f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.CannonballFriendly, damage, knockBack, player.whoAmI);
			Projectile newProj = Main.projectile[proj];
			newProj.friendly = true;
			newProj.hostile = false;
			Vector2 origVect = new Vector2(speedX, speedY);

			for (int X = 0; X < 3; X++)
			{
				Vector2 newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10) * (Main.rand.NextBool() ? -1 : 1));
				int proj2 = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, 45, knockBack, player.whoAmI);
				Projectile newProj2 = Main.projectile[proj2];
				newProj2.timeLeft = Main.rand.Next(13, 30);
			}
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}