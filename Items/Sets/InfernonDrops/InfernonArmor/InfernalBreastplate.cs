using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.InfernonDrops.InfernonArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class InfernalBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pain Monger's Breasplate");
			Tooltip.SetDefault("Increases maximum mana by 60");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.rare = ItemRarityID.Pink;
			item.value = 62000;
			item.defense = 10;
		}

		public override void UpdateEquip(Player player) => player.statManaMax2 += 60;

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<InfernalAppendage>(), 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
}
