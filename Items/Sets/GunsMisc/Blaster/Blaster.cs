using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Sets.GunsMisc.Blaster
{
	public class Blaster : ModItem
	{
		public static string[] RandNames = { "Luminous", "Ecliptic", "Aphelaic", "Cosmic", "Perihelaic", "Ionized", "Axial" };
		protected ushort nameIndex;
		//protected int counter;

		public string WeaponName => RandNames[nameIndex % RandNames.Length] + " Blaster";
		public int elementSecondary;
		public int elementPrimary;

		public override bool CloneNewInstances => true;
		public int fireType = 1;
		//Stats
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blaster");
		}
		public override void SetDefaults()
		{
			item.ranged = true;
			item.width = 60;
			item.height = 32;
			item.damage = 13;
			item.useTime = 27;
			item.useAnimation = 27;
			item.knockBack = 1f;
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.scale = .85f;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.useTurn = false;
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 9f;
			item.useAmmo = AmmoID.Bullet;

			Generate();
		}
		public override ModItem Clone(Item itemClone)
		{
			var myClone = (Blaster)base.Clone(itemClone);

			myClone.nameIndex = nameIndex;
			myClone.elementSecondary = elementSecondary;
			myClone.elementPrimary = elementPrimary;

			myClone.ApplyStats();

			return myClone;
		}
		int dustType;
		//Behavior
		public override bool CanRightClick()
		{
			return true;
		}
		public override bool ConsumeItem(Player player)
		{
			return false;
		}
		public override void HoldItem(Player player)
		{
			if (elementPrimary <= 1 && fireType == 1) {
				SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/GunsMisc/Blaster/Blaster_FireGlow");
				dustType = 6;
			}
			if (elementPrimary >= 2 && fireType == 1) {
				SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/GunsMisc/Blaster/Blaster_CorrosiveGlow");
				dustType = 163;
			}
			if (elementSecondary <= 4 && fireType == 2) {
				SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/GunsMisc/Blaster/Blaster_ShockGlow");
				dustType = 226;
			}
			if (elementSecondary >= 5 && fireType == 2) {
				SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/GunsMisc/Blaster/Blaster_FreezeGlow");
				dustType = 180;
			}
		}
		public override void RightClick(Player player)
		{
			if (elementPrimary <= 1) {
				elementalType = "Fire";
			}
			if (elementPrimary >= 2) {
				elementalType = "Poison";
			}
			if (elementSecondary <= 4) {
				elementalType2 = "Shock";
			}
			if (elementSecondary >= 5) {
				elementalType2 = "Freeze";
			}
			{
				if (fireType == 1) {
					item.useAnimation = elementPrimary;
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 10, player.width, player.height), new Color(255, 255, 255, 100),
				   elementalType2);
					fireType++;
				}
				else if (fireType == 2) {
					item.useAnimation = item.useTime;
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 10, player.width, player.height), new Color(255, 255, 255, 100),
				   elementalType);
					fireType--;
				}
			}
		}
		int magazine = 1;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MaliwanShot1"));
			{
				Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 1)) * 38f;
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
					position += muzzleOffset;
				}
				float spread = MathHelper.ToRadians(4f); //45 degrees converted to radians
				float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
				double baseAngle = Math.Atan2(speedX, speedY);
				double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
				speedX = baseSpeed * (float)Math.Sin(randomAngle);
				speedY = baseSpeed * (float)Math.Cos(randomAngle);
			}
			if (elementPrimary <= 1 && fireType == 1) {
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanFireCommon = true;
			}
			if (elementPrimary >= 2 && fireType == 1) {
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanAcidCommon = true;
			}
			if (elementSecondary <= 4 && fireType == 2) {
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanShockCommon = true;
			}
			if (elementSecondary >= 5 && fireType == 2) {
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanFreezeCommon = true;
			}
			for (int index1 = 0; index1 < 5; ++index1) {
				int index2 = Dust.NewDust(new Vector2(position.X, position.Y), item.width - 64, item.height - 16, dustType, speedX, speedY, (int)byte.MaxValue, new Color(), (float)SpiritMod.instance.spiritRNG.Next(10, 17) * 0.1f);
				Main.dust[index2].noLight = true;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.5f;
				Main.dust[index2].scale *= .6f;
			}
			return false;
		}
		//Rendering
		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override TagCompound Save() => new TagCompound {
			{ nameof(nameIndex),nameIndex },
			{ nameof(elementPrimary),elementPrimary },
			{ nameof(elementSecondary),elementSecondary }
		};
		public override void Load(TagCompound tag)
		{
			if (!tag.ContainsKey(nameof(nameIndex))) {
				return;
			}

			nameIndex = tag.Get<ushort>(nameof(nameIndex));
			elementPrimary = tag.Get<int>(nameof(elementPrimary));
			elementSecondary = tag.Get<int>(nameof(elementSecondary));

			ApplyStats();
		}

		//Net
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(nameIndex);
			writer.Write(elementPrimary);
			writer.Write(elementSecondary);
		}
		public override void NetRecieve(BinaryReader reader)
		{
			nameIndex = reader.ReadUInt16();
			elementPrimary = reader.ReadInt32();
			elementSecondary = reader.ReadInt32();

			ApplyStats();
		}
		public void Generate()
		{
			nameIndex = (ushort)SpiritMod.instance.spiritRNG.Next(RandNames.Length); ;
			elementPrimary = SpiritMod.instance.spiritRNG.Next(0, 3);
			elementSecondary = SpiritMod.instance.spiritRNG.Next(3, 6);

			ApplyStats();
		}
		public void ApplyStats()
		{
			item.SetNameOverride(WeaponName);
		}
		public string elementalType;
		public string elementalType2;
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (elementPrimary <= 1) {
				elementalType = "Fire";
			}
			if (elementPrimary >= 2) {
				elementalType = "Poison";
			}
			if (elementSecondary <= 4) {
				elementalType2 = "Shock";
			}
			if (elementSecondary >= 5) {
				elementalType2 = "Freeze";
			}
			var line = new TooltipLine(mod, "", "Right-click in inventory to toggle between " + elementalType + " & " + elementalType2);
			tooltips.Add(line);
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			scale *= .85f;
		}
	}
}