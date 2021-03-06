﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceHaste.GameMech.CutScenes;
using SpaceHaste.Maps;
using GameStateManagement;
using AvatarElementalBash.SaveLoad;

namespace SpaceHaste.GameMech.LevelManagers
{
    public class LevelManager
    {

        bool loadCutScene = true;
        CutScene cutSceneStart = new CutScene();
        public CutScene cutSceneEnd = new CutScene();

        public static LevelManager Instance;

        public bool loaded;
        public static bool skipFirstLine;
        public LevelManager()
        {
            Instance = this;
            int a = LoadSaveManager.LevelNumber;
            MapManager.currentAct = (int)((a-1) / 3) +1;
            if (MapManager.currentAct > 3)
                MapManager.currentAct = 1;
            MapManager.currentScene = a % 3;
            if (MapManager.currentScene == 0)
                MapManager.currentScene = 3;
            loaded = false;
            LoadLevel();
           // timeInLevel = 0;
        }

        public void LoadLevel()
        {
            if (loadCutScene)
            {
                GameMechanicsManager.gamestate = GameState.CutScene;

                string cutSceneFile = "act" + MapManager.currentAct + "scene" + MapManager.currentScene;
                cutSceneStart = new CutScene(cutSceneFile);
                cutSceneStart.drawCutscene();
                cutSceneStart.Text.RemoveAt(0);
                try
                {
                    string cutSceneEndFile = cutSceneFile + "ending";
                    cutSceneEnd = new CutScene(cutSceneEndFile);
                    //cutSceneEnd.drawCutscene();
                    cutSceneEnd.Text.RemoveAt(0);
                }
                catch { }
            }
            else
            {
                GameMechanicsManager.gamestate = GameState.StartBattle;
                
            }
        }

        public CutScene getScene() {
            if (GameMechanicsManager.gamestate == GameState.CutSceneEnd)
            {
                return cutSceneEnd;
            }
            return cutSceneStart;
        }

        public void Update(GameTime gameTime)
        {
        }
        internal void NextTextStart()
        {
            
            if(!skipFirstLine)
            {
                if (GameMechanicsManager.gamestate == GameState.CutScene)
                {
                    if (cutSceneStart.Text.Count > 0)
                    {
                        cutSceneStart.currentLine = cutSceneStart.Text[0];
                        cutSceneStart.Text.RemoveAt(0);
                        cutSceneStart.drawCutscene();
                    }
                    else
                    {
                        cutSceneStart.currentLine = "";
                        cutSceneStart.destroyBox();
                        GameMechanicsManager.gamestate = GameState.StartBattle;
                    }
                }
            }
            skipFirstLine = false;
        }
        internal void NextTextEnd()
        {
            if (GameMechanicsManager.gamestate == GameState.CutSceneEnd)
            {
                if (cutSceneEnd.Text.Count > 0)
                {
                    cutSceneEnd.currentLine = cutSceneEnd.Text[0];
                    cutSceneEnd.Text.RemoveAt(0);
                    cutSceneEnd.drawCutscene();
                }
                else
                {
                    cutSceneEnd.currentLine = "";
                    cutSceneEnd.destroyBox();
                    Game1.game.ScreenManager.AddScreen(new VictoryDefeatScreen(BattleMechanicsManagers.BattleMechanicsManager.VictoryDefeatScreenText, ""), PlayerIndex.One);
                }
            }
        }

    }
}
