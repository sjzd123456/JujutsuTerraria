﻿using System; using JujutsuTerraria.Buffs;
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
using JujutsuTerraria.Ancients;
using JujutsuTerraria.Items.Shadows;
using JujutsuTerraria.Items.Techniques;

using Terraria.GameContent;
using ReLogic.Content;
using JujutsuTerraria.Tiles;
namespace JujutsuTerraria.Projectiles
{
    public class Shred : ModProjectile
    {
        int rspeed;
        int yspeed;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Shred");
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = false;




        }
        private int lockedin;
        public sealed override void SetDefaults()
        {
            xspeed = 6.5f * CountryHammer.positive;
            Projectile.spriteDirection = CountryHammer.positive;
            lockedin = CountryHammer.positive;
            Projectile.width = 240;
            //projectile.aiStyle = 54;
            //aiType = NPCID.Raven;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            //projectile.velocity.X = -rspeed;
            //   Projectile.velocity.X = yspeed;
            Projectile.DamageType = ModContent.GetInstance<CursedDamage>();

            Projectile.height = 138;
            // Makes the minion go through tiles freely
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            // These below are needed for a minion weapon
            // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.friendly = true;
            // Only determines the damage type
            //	projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
           Projectile.penetrate = -1;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
          //  Projectile.timeLeft = 120; 
            Projectile.Opacity = .4f;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet; // Act exactly like default Bullet
        }

        public override bool? CanCutTiles()
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.type == NPCID.SkeletronHand || target.type == NPCID.QueenBee) { 
            }
            else
            {
                target.AddBuff(BuffID.ShadowFlame, 60 + 60 + 30);
            }
            Player player = Main.player[Projectile.owner];
            if (crit)
            {
                SoundEngine.PlaySound(SoundID.NPCHit53, target.position);
                int pos;
                int dustType;
                damage *= player.GetModPlayer<MP>().ZoneDamage;

                CombatText.clearAll();

                for (int i = 0; i < 30; i++)
                {
                    if (Main.rand.Next(1, 3) == 2)
                    {
                        pos = 1;
                    }
                    else
                    {
                        pos = -1;
                    }
                    if (Main.rand.Next(1, 4) == 2)
                    {
                        dustType = ModContent.DustType<CustomDust>();
                    }
                    else
                    {
                        if (Main.rand.Next(1, 3) == 2)
                        {
                            dustType = ModContent.DustType<CustomDust2>();
                        }
                        else
                        {
                            dustType = ModContent.DustType<CustomDust3>();

                        }
                    }
                    var dust = Dust.NewDustDirect(target.position, target.width, target.height, dustType);

                    dust.velocity.X += Main.rand.NextFloat(.5f, 1f) * pos;
                    dust.velocity.Y += Main.rand.NextFloat(.5f, 1f) * pos;

                    dust.scale *= 1f + Main.rand.NextFloat(-0.05f, 0.05f);
                }
                player.AddBuff(ModContent.BuffType<ZoneBuff>(), 60 * player.GetModPlayer<MP>().ZoneDuration);

                CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height), Color.DarkRed, damage * 2, true, false);
            }
            else
            {
                for (int i = 0; i < Main.rand.Next(1, 4); i++)
                {
                    int dustType = DustID.Silver;
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType);

                    dust.velocity.X += Main.rand.NextFloat(-0.3f, 0.3f);
                    dust.velocity.Y += Main.rand.NextFloat(-0.3f, 0.3f);

                    dust.scale *= 1f + Main.rand.NextFloat(-0.03f, 0.03f);
                }
            }


        }
        float xspeed;
       private int timer = 0;
        private bool once;
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Turquoise;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(projectileTexture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        
        
            return true;
        }
       
        public override void AI()
        {
            Lighting.AddLight(Projectile.position, Color.Turquoise.ToVector3() * 1f);

            Projectile.Opacity = .4f;

            if (once == false)
            {
                once = true;
                Projectile.velocity.X *= 3;
                Projectile.velocity.Y *= 3  ;

            }
            timer++;
                if(timer > 60)
            {

                Projectile.active = false;
            }


        
        }

    }
}