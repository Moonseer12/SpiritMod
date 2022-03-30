using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Returning;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismiteChakra : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Cutter");
			Tooltip.SetDefault("Occasionally causes foes to receive 'Festering Wounds,' which deal more damage to enemies under half health");
		}

		public override void SetDefaults()
		{
			item.damage = 9;
			item.melee = true;
			item.width = 30;
			item.height = 28;
			item.useTime = 28;
			item.useAnimation = 25;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 2;
			item.value = Terraria.Item.sellPrice(0, 0, 12, 0);
			item.rare = ItemRarityID.Blue;
			item.shootSpeed = 11f;
			item.shoot = ModContent.ProjectileType<BismiteCutter>();
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
		}

		public override bool CanUseItem(Player player)       //this make that you can shoot only 1 boomerang at once
		{
			for (int i = 0; i < Main.maxProjectiles; ++i) {
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
					return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}