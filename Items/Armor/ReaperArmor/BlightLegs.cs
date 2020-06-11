using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReaperArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class BlightLegs : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Reaper's Greaves");
            Tooltip.SetDefault("25% increased movement speed\nIncreases max life by 30");
        }
        public override void SetDefaults() {
            item.width = 22;
            item.height = 16;
            item.value = Item.sellPrice(gold: 2);
            item.rare = 8;
            item.defense = 18;
        }
        public override void UpdateEquip(Player player) {

            player.moveSpeed += 0.25f;
            player.maxRunSpeed += 1f;
            player.statLifeMax2 += 30;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CursedFire>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
