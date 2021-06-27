using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet
{
	public class MarbleBident : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Bident");
			Tooltip.SetDefault("Occasionally inflicts 'Midas', causing enemies to drop more gold\nHitting an enemy that has 'Midas' may release a fountain of gold");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 50;
			item.height = 50;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.noMelee = true;
			item.useAnimation = 28;
			item.useTime = 28;
			item.shootSpeed = 4f;
			item.knockBack = 8f;
			item.damage = 18;
			item.value = Item.sellPrice(0, 0, 60, 0);
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<MarbleBidentProj>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
