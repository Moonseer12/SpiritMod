using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritBiomeDrops
{
	public class StarCutter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Cutter");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 26;
			item.height = 26;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.ranged = true;
			item.channel = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<StarCutterProj>();
			item.useAnimation = 25;
			item.consumable = true;
			item.maxStack = 999;
			item.useTime = 25;
			item.shootSpeed = 14f;
			item.damage = 39;
			item.knockBack = 3f;
			item.value = Item.sellPrice(0, 0, 4, 50);
			item.rare = ItemRarityID.Pink;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
		}
	}
}
