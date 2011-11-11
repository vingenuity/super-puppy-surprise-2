using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceHaste.GameMech.CutScenes;

namespace SpaceHaste.GameMech.LevelManagers
{
    public class LevelManager
    {

        bool loadCutScene = true;
        CutScene cutScene = new CutScene();

        public static LevelManager Instance;

        public LevelManager()
        {
            Instance = this;
            LoadLevel1();
        }
        void LoadLevel1()
        {
            if (loadCutScene)
            {
                GameMechanicsManager.gamestate = GameState.CutScene;
                cutScene = new CutScene("TestCutScene1");
                cutScene.drawCutscene();
            }
            else
                GameMechanicsManager.gamestate = GameState.StartBattle;
        }

        public CutScene getScene() {
            return cutScene;
        }

        public void Update(GameTime gameTime)
        {
        }
        internal void NextText()
        {
            if (GameMechanicsManager.gamestate == GameState.CutScene)
            {
                if (cutScene.Text.Count > 0)
                {
                    cutScene.currentLine = cutScene.Text[0];
                    cutScene.Text.RemoveAt(0);
                    cutScene.drawCutscene();
                }
                else
                    GameMechanicsManager.gamestate = GameState.StartBattle;
            }
        }

    }
}
