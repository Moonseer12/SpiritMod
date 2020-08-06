using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace SpiritMod.Tiles.Block
{
	public class NeonBlockPurple : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
		//	Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			//Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(53, 59, 74));
			dustType = -1;
            soundType = SoundID.Tink;
			drop = ModContent.ItemType<NeonBlockPurpleItem>();
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = .154f*3;
            g = .077f*3;
            b = .255f*3;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.slope() == 0 && !tile.halfBrick()) {
				{
                    Color colour = Color.White * MathHelper.Lerp(0.35f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTime * 1.3f) + 1f) * 0.5f));

                    Texture2D glow = ModContent.GetTexture("SpiritMod/Tiles/Block/NeonBlockPurple_Glow");
                    Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

                    spriteBatch.Draw(glow, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.frameX, tile.frameY, 16, 16), colour);
                }
            }
		}
	}
}

