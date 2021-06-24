using SpiritMod.Items.Sets.DuskingDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.DuskingDrops.DuskArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class DuskPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Plate");
			Tooltip.SetDefault("Increases ranged damage by 10%\n25% Chance to not consume ammo");

		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 30;
			item.value = 50000;
			item.rare = ItemRarityID.Pink;
			item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.rangedDamage = 1.10f;
			player.ammoCost75 = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DuskStone>(), 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}