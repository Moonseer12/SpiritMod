using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class TrueHallowedStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Hallowed Staff");
			Tooltip.SetDefault("Shoots out multiple swords with different effects.");
		}


		public override void SetDefaults()
		{
			item.damage = 64;
			item.magic = true;
			item.mana = 11;
			item.width = 70;
			item.height = 70;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = 120000;
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<TrueHallowedStaffProj>();
			item.shootSpeed = 16f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddIngredient(ModContent.ItemType<HallowedStaff>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (Main.myPlayer == player.whoAmI) {
				Vector2 mouse = Main.MouseWorld;

				for (int i = 0; i < 3; ++i) {
					int p = Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), mouse.Y - 50 + Main.rand.Next(-10, 10), 0, Main.rand.Next(2, 4), type, damage, knockBack, player.whoAmI);
				}

			}
			return false;
		}

	}
}
