using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet
{
	public class MarbleStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Tome");
			Tooltip.SetDefault("Rains down gilded stalactites from the sky\nThese stalactites stick to enemies and slow them down");
		}


		public override void SetDefaults()
		{
			item.damage = 21;
			item.magic = true;
			item.mana = 8;
			item.width = 50;
			item.height = 50;
			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 1;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<MarbleStalactite>();
			item.shootSpeed = 20f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 13);
			recipe.AddIngredient(ItemID.Book, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			{
				if (Main.myPlayer == player.whoAmI) {
					Vector2 mouse = Main.MouseWorld;
					Projectile.NewProjectile(mouse.X, player.Center.Y - 700 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), ModContent.ProjectileType<MarbleStalactite>(), damage, knockBack, player.whoAmI);
				}
			}
			return false;
		}
	}
}
