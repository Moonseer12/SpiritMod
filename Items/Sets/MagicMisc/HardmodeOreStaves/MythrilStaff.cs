using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.HardmodeOreStaves
{
	public class MythrilStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Staff");
			Tooltip.SetDefault("Shooots homing bolts that occasionally strike enemies twice");
		}


		public override void SetDefaults()
		{
			item.damage = 37;
			item.magic = true;
			item.mana = 9;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item101;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<MythrilStaffProj>();
			item.shootSpeed = 8f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int amountOfProjectiles = 2;
			for (int i = 0; i < amountOfProjectiles; ++i) {
				float sX = speedX;
				float sY = speedY;
				sX += (float)Main.rand.Next(-60, 61) * 0.05f;
				sY += (float)Main.rand.Next(-60, 61) * 0.05f;
				Projectile.NewProjectile(position.X, position.Y, sX, sY, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MythrilBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
