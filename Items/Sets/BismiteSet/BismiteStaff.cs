using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismiteStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Staff");
			Tooltip.SetDefault("Shoots a bolt of energy\nOccasionally causes foes to receive 'Festering Wounds,' which deal more damage to foes under half health");
		}


		public override void SetDefaults()
		{
			item.damage = 10;
			item.magic = true;
			item.mana = 6;
			item.width = 34;
			item.height = 34;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 0;
			item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<BismiteShard>();
			item.shootSpeed = 8f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}