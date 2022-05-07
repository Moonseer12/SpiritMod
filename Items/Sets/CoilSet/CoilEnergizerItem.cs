using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CoilSet
{
	public class CoilEnergizerItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Energizer");
			Tooltip.SetDefault("Creates an electric field that energizes nearby players, greatly increasing movement speed and melee speed");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 22;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.maxStack = 99;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;
			item.rare = ItemRarityID.Green;
			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<CoilEnergizerTile>();
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TechDrive>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}