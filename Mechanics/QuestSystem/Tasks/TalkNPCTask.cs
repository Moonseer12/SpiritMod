﻿using SpiritMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class TalkNPCTask : QuestTask
	{
		public override string ModCallName => "TalkNPC";

		private int _npcType;
		private Nullable<float> _spawnIncrease;
		private int _itemReceived;
		private string _objective;
		public readonly string NPCText = "Have a great day!";
		private bool hasTakenItems;

		public TalkNPCTask() { }

		public TalkNPCTask(int npcType, string text, string objective = null, Nullable<float> spawnIncrease = null, Nullable<int> itemReceived = null)
		{
			_npcType = npcType;
			NPCText = text;
			_objective = objective;
			_spawnIncrease = spawnIncrease;
			_itemReceived = itemReceived.GetValueOrDefault();
			hasTakenItems = false;
		}

		public override QuestTask Parse(object[] args)
		{
			// get the npc type
			int npcID = -1;
			if (!QuestUtils.TryUnbox(args[1], out npcID))
			{
				if (QuestUtils.TryUnbox(args[1], out short IDasShort, "NPC Type"))
				{
					npcID = IDasShort;
				}
				else
				{
					return null;
				}
			}

			// get the name override, if there is one
			string objective = null;
			if (args.Length > 2)
			{
				if (!QuestUtils.TryUnbox(args[2], out objective, "Talk NPC Objective"))
				{
					return null;
				}
			}

			return new TalkNPCTask(npcID, objective);
		}

		public override bool CheckCompletion()
		{
			if (Main.netMode == Terraria.ID.NetmodeID.SinglePlayer)
			{
				if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == _npcType)
				{
					Main.npcChatText = NPCText;
					if (!hasTakenItems)
					{
						Main.LocalPlayer.QuickSpawnItem(_itemReceived);
						hasTakenItems = true;
					}
					return Main.npc[Main.LocalPlayer.talkNPC].type == _npcType;
				}
			}
			else if (Main.netMode == Terraria.ID.NetmodeID.Server)
			{
				for (int i = 0; i < Main.player.Length; i++)
				{
					if (Main.player[i].active && Main.player[i].talkNPC >= 0 && Main.npc[Main.player[i].talkNPC].netID == _npcType)
					{
						Main.npcChatText = NPCText;
						if (!hasTakenItems)
						{
							Main.player[i].QuickSpawnItem((int)_itemReceived);
							hasTakenItems = true;
						}
						return true;
					}
				}
			}
			return false;
		}
		public override void Activate()
		{
			QuestGlobalNPC.OnEditSpawnPool += QuestGlobalNPC_OnEditSpawnPool;
		}

		public override void Deactivate()
		{
			QuestGlobalNPC.OnEditSpawnPool -= QuestGlobalNPC_OnEditSpawnPool;
		}

		private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
		    if (pool.ContainsKey(_npcType) && _spawnIncrease != null)
		    {
			    pool[_npcType] = (float)_spawnIncrease;
		    }
		}
		public override void AutogeneratedBookText(List<string> lines) => lines.Add(GetObjectives(false));

		public override void AutogeneratedHUDText(List<string> lines) => lines.Add(GetObjectives(true));

		public string GetObjectives(bool showProgress)
		{
			StringBuilder builder = new StringBuilder();

			if (_objective != null)
			{
				builder.Append(_objective);
				return builder.ToString();
			}

			builder.Append("Talk to the ").Append(Lang.GetNPCNameValue(_npcType));
			return builder.ToString();
		}
	}
}
