using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops
{
	public class MagicConch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shankya");
			Tooltip.SetDefault("Summons a whirlpool at the location of the cursor");
		}


		public override void SetDefaults()
		{
			item.damage = 19;
			item.magic = true;
			item.mana = 20;
			item.width = 40;
			item.height = 40;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 0f;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item21;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<MagicConchProj>();
			item.shootSpeed = 0f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
			Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 0f, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}
