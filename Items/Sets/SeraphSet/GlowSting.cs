using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SeraphSet
{
	public class GlowSting : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Strike");
			Tooltip.SetDefault("Right-click to release a flurry of strikes");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/SeraphSet/GlowSting_Glow");
		}

		int currentHit;
		public override void SetDefaults()
		{
			item.damage = 47;
			item.melee = true;
			item.width = 34;
			item.height = 40;
			item.autoReuse = true;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<GlowStingSpear>();
			item.shootSpeed = 10f;
			this.currentHit = 0;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/SeraphSet/GlowSting_Glow"),
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
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 180);
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				item.noUseGraphic = true;
				item.shoot = ModContent.ProjectileType<GlowStingSpear>();
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.useTime = 7;
				item.useAnimation = 7;
				item.damage = 34;
				item.knockBack = 2;
				item.shootSpeed = 8f;
			}
			else {
				item.damage = 47;
				item.noUseGraphic = false;
				item.useTime = 25;
				item.useAnimation = 25;
				item.shoot = ProjectileID.None;
				item.knockBack = 5;
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.shootSpeed = 0f;
			}
			if (player.ownedProjectileCounts[item.shoot] > 0)
				return false;
			return true;
		}
		public override void UseStyle(Player player)
		{
			if (player.altFunctionUse != 2) {
				for (int projFinder = 0; projFinder < 300; ++projFinder) {
					if (Main.projectile[projFinder].type == ModContent.ProjectileType<GlowStingSpear>()) {
						Main.projectile[projFinder].Kill();
						Main.projectile[projFinder].timeLeft = 2;
					}
				}
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2) {
				Vector2 origVect = new Vector2(speedX, speedY);
				Vector2 newVect;
				if (Main.rand.Next(2) == 1) {
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				}
				else {
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				}
				speedX = newVect.X;
				speedY = newVect.Y;
				this.currentHit++;
				return true;
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MoonStone>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}