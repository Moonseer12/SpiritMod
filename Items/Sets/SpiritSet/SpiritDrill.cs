using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class SpiritDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Drill");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/SpiritSet/SpiritDrill_Glow");
		}

		public override void SetDefaults()
		{
			item.width = 54;
			item.height = 22;
			item.value = 40000;
			item.pick = 180;
			item.damage = 39;
			item.knockBack = 0;
			item.shootSpeed = 40f;
			item.useTime = 7;
			item.useAnimation = 25;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item23;
			item.rare = ItemRarityID.Pink;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<SpiritDrillProjectile>();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Sets/SpiritSet/SpiritDrill_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 18);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
	}
}
