using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.DuskingDrops
{
	public class DuskStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Shard");
			Tooltip.SetDefault("'The stone sparkles with twilight energies'");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/DuskingDrops/DuskStone_Glow");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.maxStack = 999;
			item.rare = ItemRarityID.Pink;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/DuskingDrops/DuskStone_Glow"),
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
	}
}
