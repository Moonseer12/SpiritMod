using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Glyphs
{
	public class RadiantGlyph : GlyphBase, IGlowing
	{
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Radiant;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x28cacc };
		public override string Effect => "Radiance";
		public override string Addendum =>
			"Not attacking builds stacks of Divine Strike\n" +
			"The next hit is empowered by 11% per stack";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiant Glyph");
			Tooltip.SetDefault(
				"+4% crit chance\n" +
				"Not attacking builds stacks of Divine Strike\n" +
				"The next hit is empowered by 11% per stack");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.LightRed;

			item.maxStack = 999;
		}


		public static void DivineStrike(Player player, ref int damage)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			modPlayer.divineCounter = 0;
			int index = player.FindBuffIndex(SpiritMod.Instance.BuffType("DivineStrike"));
			if (index < 0)
				return;

			damage += (int)(.11f * modPlayer.divineStacks * damage);
			player.DelBuff(index);
			modPlayer.divineStacks = 1;
		}
	}
}