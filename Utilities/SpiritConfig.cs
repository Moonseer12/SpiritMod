using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SpiritMod.Utilities
{
	[Label("Client Config")]
	class SpiritClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Screen Distortion")]
        [Tooltip("Enables screen distortion while in the Spirit Biome or when fighting the Starplate Voyager")]
        [DefaultValue(true)]
        public bool DistortionConfig { get; set; }

		[Label("Extra Particles")]
		[Tooltip("Enables extra particles in the foreground under certain conditions")]
		[DefaultValue(true)]
		public bool Particles { get; set; }

		[Label("Quick Sell Feature")]
		[Tooltip("Enables the quick-sell feature, which allows for easily selling all unwanted items to NPCs")]
		[DefaultValue(true)]
		public bool QuickSell { get; set; }

		[Label("Auto-reuse Tooltip")]
		[Tooltip("Enables an extra line in weapon tooltips displaying if an item is auto-reuse/autoswing")]
		[DefaultValue(true)]
		public bool AutoReuse { get; set; }

        [Label("Ambient Sounds")]
        [Tooltip("Enables ambient sound effects for some biomes")]
        [DefaultValue(true)]
        public bool AmbientSounds { get; set; }

		[Label("Quest Book Button")]
		[Tooltip("Change where the quest book's button appears in your inventory.")]
		[DefaultValue(QuestUtils.QuestInvLocation.Minimap)]
		[DrawTicks]
		public QuestUtils.QuestInvLocation QuestBookLocation { get; set; }
	}
}