using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceHaste.GameMech.CutScenes;
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

        public LevelManager()
        {
            Instance = this;
            int a = LoadSaveManager.LevelNumber;
            if(a== 2)
                LoadLevel1();
            else
                LoadLevel1();
        }
        void LoadLevel1()
        {
            if (loadCutScene)
            {
                GameMechanicsManager.gamestate = GameState.CutScene;
                cutSceneStart = new CutScene("TestCutScene1");
                cutSceneStart.drawCutscene();
                cutSceneStart.Text.RemoveAt(0);
                try {
                    cutSceneEnd = new CutScene("TestCutScene1");
                    //cutSceneEnd.drawCutscene();
                    cutSceneEnd.Text.RemoveAt(0);
                }
                catch { }
            }
            else
                GameMechanicsManager.gamestate = GameState.StartBattle;
        }

        public CutScene getScene() {
            if (GameMechanicsManager.gamestate == GameState.CutSceneEnd)
                return cutSceneEnd;
            return cutSceneStart;
        }

        public void Update(GameTime gameTime)
        {
        }
        internal void NextTextStart()
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
