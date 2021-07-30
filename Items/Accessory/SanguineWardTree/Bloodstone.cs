
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SanguineWardTree
{
	public class Bloodstone : ModItem
	{
		public override bool Autoload(ref string name) => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodstone");
			Tooltip.SetDefault("A bloody ward surrounds you, inflicting Blood Corruption to nearby enemies\nKilling enemies within the aura restores some life\nHearts are more likely to drop from enemies");

		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().vitaStone = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BloodWard>(), 1);
			recipe.AddIngredient(ModContent.ItemType<VitalityStone>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
