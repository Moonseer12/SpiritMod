using SpiritMod.Items.Sets.StarplateDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class BlueMoonSpawn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Turquoise Lens");
			Tooltip.SetDefault("Use at nighttime to summon the Mystic Moon");
		}

		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.Pink;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;

			item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player)
		{
			if (Main.dayTime) {
				Main.NewText("The moon isn't powerful in daylight.", 80, 80, 150, true);
				return false;
			}
			if (MyWorld.BlueMoon)
                return false;
			return true;
		}

		public override bool UseItem(Player player)
		{
			Main.NewText("The Mystic Moon is Rising...", 0, 90, 220, true);
			Main.PlaySound(SoundID.Roar, (int)player.position.X, (int)player.position.Y, 0);
			if (!Main.dayTime)
				MyWorld.BlueMoon = true;
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrystalShard, 6);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>(), 30);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
