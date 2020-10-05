using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Fargowiltas;
using Fargowiltas.Projectiles;

namespace Fargowiltas.Items.Misc
{
    public class SuperDummy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Dummy");
            Tooltip.SetDefault("Spawns a super dummy at your cursor" +
                               "\nSame as regular Target Dummy except minions and projectiles detect and home onto it" +
                               "\nOn hit effects get triggered as well" +
                               "\nRight click to remove all spawned super dummies");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.rare = ItemRarityID.Blue;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == ItemAlternativeFunctionID.ActivatedAndUsed)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<NPCs.SuperDummy>())
                    {
                        //Main.npc[i].active = false;
                        Main.npc[i].ai[0] = 1;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, i, 9999f, 10f, (float)(-(float)Main.npc[i].direction), 0, 0, 0);
                        }
                        Main.npc[i].StrikeNPCNoInteraction(9999, 10f, -Main.npc[i].direction, false, false, false);
                        //Main.npc[i].netUpdate = true;
                    }
                }
            }
            else if (player.whoAmI == Main.myPlayer)
            {
                Vector2 pos = new Vector2((int)Main.MouseWorld.X - 9, (int)Main.MouseWorld.Y - 20);
                Projectile.NewProjectile(pos, Vector2.Zero, ModContent.ProjectileType<SpawnProj>(), 0, 0, player.whoAmI, ModContent.NPCType<NPCs.SuperDummy>());

                //NPC.NewNPC((int)pos.X, (int)pos.Y, ModContent.NPCType<NPCs.SuperDummy>());
            }

            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TargetDummy);
            recipe.AddIngredient(ItemID.FallenStar);
            recipe.AddTile(TileID.CookingPots);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
