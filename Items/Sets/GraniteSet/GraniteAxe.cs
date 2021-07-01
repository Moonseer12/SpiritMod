using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GraniteSet
{
	public class GraniteAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Axe");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 40;
			item.value = 8000;
			item.rare = ItemRarityID.Green;

			item.axe = 12;

			item.damage = 16;
			item.knockBack = 6;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 28;
			item.useAnimation = 28;

			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;

			item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
