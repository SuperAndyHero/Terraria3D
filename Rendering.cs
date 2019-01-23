﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.UI.Chat;
using ReLogic.Graphics;

namespace Terraria3D
{
    public static class Rendering
    {
        public static void UpdateState()
        {
            if (Main.magmaBGFrameCounter >= 8)
            {
                Main.magmaBGFrameCounter = 0;
                Main.magmaBGFrame++;
                if (Main.magmaBGFrame >= 3)
                {
                    Main.magmaBGFrame = 0;
                }
            }
            if (!Main.gamePaused)
            {
                Main.essScale += (float)Main.essDir * 0.01f;
                if (Main.essScale > 1f)
                {
                    Main.essDir = -1;
                    Main.essScale = 1f;
                }
                if ((double)Main.essScale < 0.7)
                {
                    Main.essDir = 1;
                    Main.essScale = 0.7f;
                }
            }
        }

        public static void DrawBackgroundWater()
        {
            if (Main.drawToScreen)
                Reflection.DrawWaters(true, -1, true);
            else
            {
                Main.spriteBatch.Draw(Main.instance.backWaterTarget, Main.sceneBackgroundPos - Main.screenPosition, Microsoft.Xna.Framework.Color.White);
                TimeLogger.DetailedDrawTime(11);
            }
        }

        public static void DrawSceneBackground()
        {
            float x = (Main.sceneBackgroundPos.X - Main.screenPosition.X + (float)Main.offScreenRange) * Main.caveParallax - (float)Main.offScreenRange;
            if (Main.drawToScreen)
            {
                Main.tileBatch.Begin();
                Reflection.DrawBackground();
                Main.tileBatch.End();
            }
            else
                Main.spriteBatch.Draw(Main.instance.backgroundTarget, new Vector2(x, Main.sceneBackgroundPos.Y - Main.screenPosition.Y), Color.White);
        }

        public static void DrawSandstorm() => Sandstorm.DrawGrains(Main.spriteBatch);

        public static void CacheDraws()
        {
            Reflection.CacheNPCDraws();
            Reflection.CacheProjDraws();
        }

        public static void DrawMoonMoon()
            => Reflection.DrawCachedNPCs(Main.instance.DrawCacheNPCsMoonMoon, true);

        public static void DrawBlack()
        {
            if (Main.drawToScreen)
                Reflection.DrawBlack();
            else
                Main.spriteBatch.Draw(Main.instance.blackTarget, Main.sceneTilePos - Main.screenPosition, Color.White);
        }

        public static void DrawWalls()
        {
            if (Main.drawToScreen)
            {
                Main.tileBatch.Begin();
                Reflection.DrawWalls();
                Main.tileBatch.End();
            }
            else
                Main.spriteBatch.Draw(Main.instance.wallTarget, Main.sceneWallPos - Main.screenPosition, Color.White);
        }

        public static void DrawWallOfFlesh() => Reflection.DrawWoF();

        public static void DrawBackGore()
        {
            if (Main.drawBackGore)
                Reflection.DrawGoreBehind();
        }

        public static void DrawMoonLordDeath()
        {
            MoonlordDeathDrama.DrawPieces(Main.spriteBatch);
            MoonlordDeathDrama.DrawExplosions(Main.spriteBatch);
        }

        public static void DrawNPCsBehinNonSoldTiles()
            => Reflection.DrawCachedNPCs(Main.instance.DrawCacheNPCsBehindNonSolidTiles, true);

        public static void DrawNonSolidTiles()
        {
            if (Main.drawToScreen)
                Reflection.DrawTiles(false);
            else
                Main.spriteBatch.Draw(Main.instance.tile2Target, Main.sceneTile2Pos - Main.screenPosition, Color.White);
        }
        public static void DrawWaterFalls()
        {
            Main.instance.waterfallManager.Draw(Main.spriteBatch);
        }


        public static void DrawProjsBehindNPCsAndTiles() =>
            Reflection.DrawCachedProjs(Main.instance.DrawCacheProjsBehindNPCsAndTiles, true);

        public static void DrawNPCsBehindTiles() => Reflection.DrawNPCs(true);

        public static void DrawSolidTiles()
        {
            if (Main.drawToScreen)
                Reflection.DrawTiles(true);
            else
                Main.spriteBatch.Draw(Main.instance.tileTarget, Main.sceneTile2Pos - Main.screenPosition, Color.White);
        }

        public static void DrawHitTileAnimation() => Main.player[Main.myPlayer].hitTile.DrawFreshAnimations(Main.spriteBatch);

        public static void DrawNPCsInfrontOfTiles() => Reflection.DrawNPCs(false);

        public static void DrawNPCPRojectiles() => Reflection.DrawCachedNPCs(Main.instance.DrawCacheNPCProjectiles, false);

        public static void SortDrawCacheWorm() => Reflection.SortDrawCashWorms();

        public static void DrawProjsBehindProjectiles() => Reflection.DrawCachedProjs(Main.instance.DrawCacheProjsBehindProjectiles, true);

        public static void DrawProjectiles() => Reflection.DrawProjectiles();

        public static void DrawPlayers() => Reflection.DrawPlayers();

        public static void DrawNPCsOverPlayer() => Reflection.DrawCachedNPCs(Main.instance.DrawCacheNPCsOverPlayers, false);

        public static void DrawItems() => Main.instance.DrawItems();

        public static void DrawRain() => Reflection.DrawRain();

        public static void DrawGore() => Reflection.DrawGore();

        public static void DrawDust() => Reflection.DrawDust();


        public static void DrawForegroundWater()
        {
            if (Main.drawToScreen)
                Reflection.DrawWaters(false, -1, true);
            else
                Main.spriteBatch.Draw(Main.waterTarget, Main.sceneWaterPos - Main.screenPosition, Color.White);
        }

        public static void DrawWires()
        {
            if (WiresUI.Settings.DrawWires)
                Reflection.DrawWires();
        }

        public static void DrawProjsOverWireUI() => Reflection.DrawCachedProjs(Main.instance.DrawCacheProjsOverWiresUI, false);

        public static void DrawInfernoRings() => Main.instance.DrawInfernoRings();

        public static void DrawMoonlordDeathFront() => MoonlordDeathDrama.DrawWhite(Main.spriteBatch);

        public static void DrawScreenObstructions() => ScreenObstruction.Draw(Main.spriteBatch);

        public static void DrawChatOverPlayerHeads()
        {
            for (int m = 0; m < 255; m++)
            {
                if (Main.player[m].active && Main.player[m].chatOverhead.timeLeft > 0 && !Main.player[m].dead)
                {
                    Vector2 messageSize = Main.player[m].chatOverhead.messageSize;
                    Vector2 vector5;
                    vector5.X = Main.player[m].position.X + (float)(Main.player[m].width / 2) - messageSize.X / 2f;
                    vector5.Y = Main.player[m].position.Y - messageSize.Y - 2f;
                    vector5.Y += Main.player[m].gfxOffY;
                    vector5 = vector5.Floor();
                    if (Main.player[Main.myPlayer].gravDir == -1f)
                    {
                        vector5.Y -= Main.screenPosition.Y;
                        vector5.Y = Main.screenPosition.Y + (float)Main.screenHeight - vector5.Y;
                    }
                    int num66 = 0;
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, Main.player[m].chatOverhead.snippets, vector5 - Main.screenPosition, 0f, Vector2.Zero, Vector2.One, out num66, -1f, 2f);
                }
            }
        }

        public static void DrawCombatText()
        {
            float scale = CombatText.TargetScale;
            for (int n = 0; n < 100; n++)
            {
                if (Main.combatText[n].active)
                {
                    int num68 = 0;
                    if (Main.combatText[n].crit)
                    {
                        num68 = 1;
                    }
                    Vector2 vector6 = Main.fontCombatText[num68].MeasureString(Main.combatText[n].text);
                    Vector2 origin = new Vector2(vector6.X * 0.5f, vector6.Y * 0.5f);
                    float num69 = Main.combatText[n].scale / scale;
                    float num70 = (float)Main.combatText[n].color.R;
                    float num71 = (float)Main.combatText[n].color.G;
                    float num72 = (float)Main.combatText[n].color.B;
                    float num73 = (float)Main.combatText[n].color.A;
                    num70 *= num69 * Main.combatText[n].alpha * 0.3f;
                    num72 *= num69 * Main.combatText[n].alpha * 0.3f;
                    num71 *= num69 * Main.combatText[n].alpha * 0.3f;
                    num73 *= num69 * Main.combatText[n].alpha;
                    Microsoft.Xna.Framework.Color color9 = new Microsoft.Xna.Framework.Color((int)num70, (int)num71, (int)num72, (int)num73);
                    for (int num74 = 0; num74 < 5; num74++)
                    {
                        float num75 = 0f;
                        float num76 = 0f;
                        if (num74 == 0)
                        {
                            num75 -= scale;
                        }
                        else if (num74 == 1)
                        {
                            num75 += scale;
                        }
                        else if (num74 == 2)
                        {
                            num76 -= scale;
                        }
                        else if (num74 == 3)
                        {
                            num76 += scale;
                        }
                        else
                        {
                            num70 = (float)Main.combatText[n].color.R * num69 * Main.combatText[n].alpha;
                            num72 = (float)Main.combatText[n].color.B * num69 * Main.combatText[n].alpha;
                            num71 = (float)Main.combatText[n].color.G * num69 * Main.combatText[n].alpha;
                            num73 = (float)Main.combatText[n].color.A * num69 * Main.combatText[n].alpha;
                            color9 = new Microsoft.Xna.Framework.Color((int)num70, (int)num71, (int)num72, (int)num73);
                        }
                        if (Main.player[Main.myPlayer].gravDir == -1f)
                        {
                            float num77 = Main.combatText[n].position.Y - Main.screenPosition.Y;
                            num77 = (float)Main.screenHeight - num77;
                            Main.spriteBatch.DrawString(Main.fontCombatText[num68], Main.combatText[n].text, new Vector2(Main.combatText[n].position.X - Main.screenPosition.X + num75 + origin.X, num77 + num76 + origin.Y), color9, Main.combatText[n].rotation, origin, Main.combatText[n].scale, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            Main.spriteBatch.DrawString(Main.fontCombatText[num68], Main.combatText[n].text, new Vector2(Main.combatText[n].position.X - Main.screenPosition.X + num75 + origin.X, Main.combatText[n].position.Y - Main.screenPosition.Y + num76 + origin.Y), color9, Main.combatText[n].rotation, origin, Main.combatText[n].scale, SpriteEffects.None, 0f);
                        }
                    }
                }
            }
        }

        public static void DrawItemText()
        {
            var scale = ItemText.TargetScale;
            if (scale == 0f)
                scale = 1f;
            for (int num78 = 0; num78 < 20; num78++)
            {
                if (Main.itemText[num78].active)
                {
                    string text = Main.itemText[num78].name;
                    if (Main.itemText[num78].stack > 1)
                    {
                        text = string.Concat(new object[]
                            {
                                        text,
                                        " (",
                                        Main.itemText[num78].stack,
                                        ")"
                            });
                    }
                    Vector2 vector7 = Main.fontMouseText.MeasureString(text);
                    Vector2 origin2 = new Vector2(vector7.X * 0.5f, vector7.Y * 0.5f);
                    float num79 = Main.itemText[num78].scale / scale;
                    float num80 = (float)Main.itemText[num78].color.R;
                    float num81 = (float)Main.itemText[num78].color.G;
                    float num82 = (float)Main.itemText[num78].color.B;
                    float num83 = (float)Main.itemText[num78].color.A;
                    num80 *= num79 * Main.itemText[num78].alpha * 0.3f;
                    num82 *= num79 * Main.itemText[num78].alpha * 0.3f;
                    num81 *= num79 * Main.itemText[num78].alpha * 0.3f;
                    num83 *= num79 * Main.itemText[num78].alpha;
                    Microsoft.Xna.Framework.Color color10 = new Microsoft.Xna.Framework.Color((int)num80, (int)num81, (int)num82, (int)num83);
                    for (int num84 = 0; num84 < 5; num84++)
                    {
                        float num85 = 0f;
                        float num86 = 0f;
                        if (num84 == 0)
                        {
                            num85 -= scale * 2f;
                        }
                        else if (num84 == 1)
                        {
                            num85 += scale * 2f;
                        }
                        else if (num84 == 2)
                        {
                            num86 -= scale * 2f;
                        }
                        else if (num84 == 3)
                        {
                            num86 += scale * 2f;
                        }
                        else
                        {
                            num80 = (float)Main.itemText[num78].color.R * num79 * Main.itemText[num78].alpha;
                            num82 = (float)Main.itemText[num78].color.B * num79 * Main.itemText[num78].alpha;
                            num81 = (float)Main.itemText[num78].color.G * num79 * Main.itemText[num78].alpha;
                            num83 = (float)Main.itemText[num78].color.A * num79 * Main.itemText[num78].alpha;
                            color10 = new Microsoft.Xna.Framework.Color((int)num80, (int)num81, (int)num82, (int)num83);
                        }
                        if (num84 < 4)
                        {
                            num83 = (float)Main.itemText[num78].color.A * num79 * Main.itemText[num78].alpha;
                            color10 = new Microsoft.Xna.Framework.Color(0, 0, 0, (int)num83);
                        }
                        float num87 = Main.itemText[num78].position.Y - Main.screenPosition.Y + num86;
                        if (Main.player[Main.myPlayer].gravDir == -1f)
                        {
                            num87 = (float)Main.screenHeight - num87;
                        }
                        Main.spriteBatch.DrawString(Main.fontMouseText, text, new Vector2(Main.itemText[num78].position.X - Main.screenPosition.X + num85 + origin2.X, num87 + origin2.Y), color10, Main.itemText[num78].rotation, origin2, Main.itemText[num78].scale, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
