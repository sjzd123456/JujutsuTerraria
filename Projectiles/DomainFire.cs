﻿using System; 
using JujutsuTerraria.Buffs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//using static Terraria.ModLoader.ModContent;

using JujutsuTerraria.Ancients;
using JujutsuTerraria.Items.Shadows;
using JujutsuTerraria.Items.Techniques;
using Terraria.GameContent;
using ReLogic.Content;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;
using JujutsuTerraria.Items.Techniques.Domains;
using JujutsuTerraria.Items.Materials;

namespace JujutsuTerraria.Projectiles
{
    public class DomainFire : ModProjectile
    {
        int rspeed;
        int yspeed;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Volanic Domain");
            Main.projFrames[Projectile.type] = 1;
           Main.projPet[Projectile.type] = false;




        }
        private int lockedin;
        public sealed override void SetDefaults()
        {
            Projectile.width = 522;
  
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 12;
            Projectile.DamageType = ModContent.GetInstance<CursedDamage>();

            Projectile.height = 522;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;

            Projectile.friendly = true;
            Projectile.hostile = false;

           Projectile.penetrate = -1 ;
            Projectile.Opacity = 0f;
            Projectile.scale = 1;
            Projectile.hide = true;
        }
     

        public override bool? CanCutTiles()
        {
            return false;
        }

        public int TargetWhoAmI
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
 
        public override bool? CanHitNPC(NPC target)
        {
            if (target.friendly == true || target.immune[Projectile.owner] == 5) { return false; }
            else
            {
                return true;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float radius = 522/2;
            return Projectile.Center.DistanceSQ(targetHitbox.ClosestPointInRect(Projectile.Center)) < radius * Projectile.scale * radius * Projectile.scale;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.immune[Projectile.owner] = 5;

        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            			target.immune[Projectile.owner] = 5;

            target.AddBuff(BuffID.OnFire3, 60 * 10);
            target.AddBuff(BuffID.ShadowFlame, 60 * 10);
            TargetWhoAmI = target.whoAmI;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Inferno, 60 * 10);        
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White * Wacko, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;   
        }
 
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            Main.instance.DrawCacheProjsBehindNPCs.Add(index);
        }


        public float Wacko;
        bool once = false;
        public int counter;
        public override void AI()
        {
            Projectile.Size = new Vector2(522, 522) * Projectile.scale; 
           if (once == false)
            {
                Wacko = 0;
                once = true;
                Projectile.scale = 0;
                Projectile.Opacity = 0;


            }
            if (Main.rand.Next(1, 8) == 2)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ash);
                if (Main.rand.Next(1, 3) == 2)
                {
                    dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava);
                }
                else
                {
                    dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);




                    dust.velocity.X += Main.rand.NextFloat(-0.3f, 0.3f);
                    dust.velocity.Y += Main.rand.NextFloat(-0.3f, 0.3f);

                    dust.scale *= 1f + Main.rand.NextFloat(-0.03f, 0.03f);
                }
            }


            Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3() * 0.78f);

            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<MPArmors>().DomainActive = true;
            Player Local = Main.LocalPlayer;


            // If the player channels the weapon, do something. This check only works if item.channel is true for the weapon.
            if (player.channel)
            {
                counter++;
                if (counter >= 60)
                {
                    counter = 0;
                    bool once = false;
                    for (int i = 0; i < Main.InventorySlotsTotal; i++)
                    {
                        if (player.inventory[i].type == ModContent.ItemType<CursedEnergy>() && once == false)
                        {
                            if (player.HasBuff(ModContent.BuffType<SixEyesBuff>()))
                            {
                                player.inventory[DEFire.InventoryNumber].stack -= DEFire.Cost - DEFire.Reduction;
                                once = true;


                            }
                            else if (player.HasBuff(ModContent.BuffType<TwinEyesBuff>()))
                            {
                                player.inventory[DEFire.InventoryNumber].stack -= DEFire.Cost - DEFire.Reduction;
                                once = true;


                            }
                            else if (player.HasBuff(ModContent.BuffType<NueEyeBuff>()))
                            {
                                player.inventory[DEFire.InventoryNumber].stack -= DEFire.Cost - DEFire.Reduction;
                                once = true;


                            }
                            else
                            {
                                player.inventory[DEFire.InventoryNumber].stack -= DEFire.Cost - DEFire.Reduction;
                                once = true;

                            }
                        }
                    }
                }

                if (player.inventory[DEFire.InventoryNumber].stack < DEFire.Cost - DEFire.Reduction)
                {
                    Projectile.Center = player.Center;

                    Projectile.scale -= .1f;
                    Wacko -= .045f;
                    if (Projectile.scale <= 0)
                    {
                        Projectile.active = false;
                    }
                }
                else
                {

                    Vector2 center = new Vector2((int)player.position.X, (int)player.position.Y);
                    const float range = 16 * 16;  // 20 tiles
                    if (Local.DistanceSQ(center) <= range * range)
                    {
                        Local.AddBuff(BuffID.Inferno, 60 * 10);
                    }
                    if (Wacko <= .85)
                    {
                        Wacko += .0096f;
                    }
                    if (Projectile.scale <= 1)
                    {
                        Projectile.scale += .012f;
                    }

                    Projectile.Center = player.Center;
                }
            }
            else
            {

                Projectile.Center = player.Center;

                Projectile.scale -= .1f;
                Wacko -= .045f;
                if (Projectile.scale <= 0)
                {
                    Projectile.active = false;
                }

            }
        }

    }
}