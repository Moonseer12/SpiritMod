using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.PolymorphGun
{
	public class PolymorphGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polymorph Gun");
			Tooltip.SetDefault("Turns enemies into harmless bunnies!\nOnly works on enemies below half health");
		}


		private Vector2 newVect;
		public override void SetDefaults()
		{
			item.magic = true;
			item.damage = 35;
			item.mana = 18;
			item.width = 54;
			item.height = 26;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = Terraria.Item.buyPrice(0, 60, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.DD2_SonicBoomBladeSlash;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = ModContent.ProjectileType<Polyshot>();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}