﻿using System; using JujutsuTerraria.Buffs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.Audio;

using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using JujutsuTerraria.Projectiles;
using JujutsuTerraria.Items.Materials;
using JujutsuTerraria.Ancients;

namespace JujutsuTerraria.Buffs
{
    public class FlowingRedStackBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flowing Red Scale: Stack");
            // Description.SetDefault("Substantially increased cursed damage in exchange for a substantial decrease life regen");
            //  Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = false; // The time remaining won't display on this buff
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<CursedDamage>() += 0.25f;

            player.lifeRegen -= 8   ;

        }

    }
}
