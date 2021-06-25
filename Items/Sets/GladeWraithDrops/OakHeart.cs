using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GladeWraithDrops
{
	public class OakHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oak Heart");
			Tooltip.SetDefault("Hitting foes may cause poisonous spores to rain down\nPoisons hit foes");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 9;
			item.height = 15;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.maxStack = 1;
			item.shoot = ModContent.ProjectileType<Projectiles.Thrown.OakHeart>();
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 5f;
			item.damage = 12;
			item.knockBack = 1.5f; ;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.consumable = false;
		}
	}
}