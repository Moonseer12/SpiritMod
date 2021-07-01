using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class BloodStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vessel Drainer");
			Tooltip.SetDefault("Summons a blood tentacle that splits into life-stealing blood clots");
		}


		public override void SetDefaults()
		{
			item.damage = 36;
			item.magic = true;
			item.mana = 11;
			item.width = 52;
			item.height = 52;
			item.useTime = 44;
			item.useAnimation = 44;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = Terraria.Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.LightRed;
			item.crit += 10;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<BloodVessel>();
			item.shootSpeed = 16f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 offset = Vector2.UnitX.RotatedBy(new Vector2(speedX, speedY).ToRotation()) * item.width;
			if (Collision.CanHit(player.Center, 0, 0, player.Center + offset, 0, 0))
				position += offset;

			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<CrimsonStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<JungleStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<DungeonStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<HellStaff>(), 1);
			modRecipe.AddTile(TileID.DemonAltar);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}

	}
}
