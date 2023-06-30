﻿using Mono.Cecil;
using TenShadows.Buffs;
using TenShadows.Items.Materials;
using TenShadows.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;
using TenShadows.Ancients;
namespace TenShadows.Armor
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Head)]
    public class MakiHead : ModItem
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Zenin Maki");
            Tooltip.SetDefault("6% increased cursed damage");
        }


        public override void SetDefaults()
        {
            Item.width = 28; // Width of the item
            Item.height = 26; // Height of the item
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
            Item.rare = ItemRarityID.Blue; // The rarity of the item
            Item.defense = 1; // The amount of defense the item will give when equipped
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<UniformBody>() && legs.type == ModContent.ItemType<UniformLegs>();
        }

        // UpdateArmorSet allows you to give set bonuses to the armor.
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increased speed!"; // This is the setbonus tooltip
            player.moveSpeed *= 2f;
            player.maxRunSpeed *= 2;
            player.accRunSpeed *= 2;
            player.runAcceleration *= 2;

        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<CursedDamage>() += (6 / 100);

        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

    }
}