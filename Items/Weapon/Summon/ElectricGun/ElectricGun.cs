using SpiritMod.Projectiles.Summon;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.ElectricGun
{
	public class ElectricGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcbolter");
			Tooltip.SetDefault("4 summon tag damage\nYour summons will focus struck enemies\nHit enemies may create static links between each other when struck by minions, dealing additional damage");

            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Summon/ElectricGun/ElectricGun_Glow");
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .4f, .28f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Summon/ElectricGun/ElectricGun_Glow"),
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
        }////

        public override void SetDefaults()
		{
			item.damage = 12;
			item.summon = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item12;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<ElectricGunProjectile>();
			item.shootSpeed = 12f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 1)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
			Vector2 velocity = new Vector2(speedX, speedY); //convert speedx and speedy into one vector, and rotate it upwards
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10)); //then rotate it randomly, with a maximum spread of 30 degrees
            Projectile proj = Projectile.NewProjectileDirect(position, velocity, type, item.damage, knockBack, item.owner, 0, 0);
			proj.netUpdate = true; //sync velocity
            for (int index1 = 0; index1 < 5; ++index1)
            {
                int index2 = Dust.NewDust(new Vector2(position.X, position.Y), item.width - 60, item.height - 28, DustID.Electric, speedX, speedY, (int)byte.MaxValue, new Color(), (float)Main.rand.Next(6, 10) * 0.1f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].velocity *= 0.5f;
                Main.dust[index2].scale *= .6f;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }
    }
}
