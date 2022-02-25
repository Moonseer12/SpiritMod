using SpiritMod.NPCs.Boss.Atlas;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class StoneSkin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Fist");
			Tooltip.SetDefault("Use anywhere to summon Atlas");
		}

		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.Cyan;
			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.consumable = false;
			item.autoReuse = false;

			item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(ModContent.NPCType<Atlas>()))
				return true;
			return false;
		}

		public override bool UseItem(Player player)
		{
			Main.PlaySound(SoundID.Roar, (int)player.Center.X, (int)player.Center.Y, 0);
			NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 600, ModContent.NPCType<Atlas>());

			Main.NewText("The earth is trembling!", 255, 60, 255);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell, 1);
			recipe.AddIngredient(ItemID.MartianConduitPlating, 20);
			recipe.AddIngredient(ItemID.StoneBlock, 100);
			recipe.AddIngredient(ItemID.Bone, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
