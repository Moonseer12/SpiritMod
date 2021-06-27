using SpiritMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritBiomeDrops
{
	public class GhostJellyBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghost Jelly Bomb");
		}


		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Shuriken);
			item.width = 37;
			item.height = 26;
			item.shoot = ModContent.ProjectileType<GhostJellyBombProj>();
			item.useAnimation = 27;
			item.useTime = 27;
			item.shootSpeed = 11f;
			item.damage = 35;
			item.knockBack = 1.0f;
			item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
			item.crit = 6;
			item.rare = ItemRarityID.Pink;
			item.ranged = true;
			item.autoReuse = false;
		}


	}
}
