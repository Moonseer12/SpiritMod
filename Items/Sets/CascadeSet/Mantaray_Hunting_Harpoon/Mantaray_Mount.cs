﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Mantaray_Hunting_Harpoon
{
    public class Mantaray_Mount : ModMountData
    {
		protected int circularGlide = 0;

        public override void SetDefaults()
        {
			Player player = Main.player[Main.myPlayer];
			mountData.spawnDust = 103;
			mountData.buff = mod.BuffType("Mantaray_Buff");
			mountData.heightBoost = 14;
			mountData.flightTimeMax = 99999;
			mountData.fatigueMax = 99999;
			mountData.fallDamage = 0.0f;
			mountData.usesHover = true;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 3f;
			mountData.acceleration = 0.35f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 3f;
			mountData.swimSpeed = 95f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 7;
			int[] numArray13 = new int[mountData.totalFrames];
			for (int index = 0; index < numArray13.Length; ++index)
				numArray13[index] = 12;
			mountData.playerYOffsets = numArray13;
			mountData.xOffset = -10;
			mountData.bodyFrame = 3;
			mountData.yOffset = 20;
			mountData.playerHeadOffset = 31;
			mountData.standingFrameCount = 7;
			mountData.standingFrameDelay = 4;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 4;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 7;
			mountData.flyingFrameDelay = 4;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 7;
			mountData.inAirFrameDelay = 4;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = 7;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;
			if (Main.netMode != NetmodeID.Server)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
        }
 
        public override void UpdateEffects(Player player)
		{
			float velocity = Math.Abs(player.velocity.X);

			float num1 = (float) player.velocity.X / mountData.dashSpeed;
			int outofWaterTimer = 0;
			if ((double) num1 > 0.95)
			  num1 = .95f;
			if ((double) num1 < -0.95)
			  num1 = -.95f;
			float num2 = (float) (0.785398185253143 * (double) num1 / 2.0);

			if (!player.wet)
			{
				mountData.usesHover = false;
				mountData.acceleration = 0.05f;
				mountData.dashSpeed = 0f;
				mountData.runSpeed = 0.05f;
				mountData.jumpHeight = 0;

				if ((player.velocity.Y != 0 || player.oldVelocity.Y != 0))
				{
					int direction = (velocity == 0) ? 0 :
						(player.direction == Math.Sign(player.velocity.X)) ? 1 : -1;
					player.fullRotation = player.velocity.Y * 0.05f * player.direction * direction * mountData.jumpHeight / 14f;
					player.fullRotationOrigin = (player.Hitbox.Size() + new Vector2(0, 42)) / 2;
				}

			}
			else
			{
				mountData.jumpHeight = 10;
				mountData.acceleration = 0.35f;
				mountData.dashSpeed = 8f;
				mountData.runSpeed = 8f;
				outofWaterTimer = 0;
				player.fullRotation = 0F;
				mountData.usesHover = true;

			}

			player.gills = true;
        }
    }
}