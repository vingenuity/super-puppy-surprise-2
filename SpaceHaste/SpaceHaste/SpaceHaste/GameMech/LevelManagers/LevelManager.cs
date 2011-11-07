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
        public LevelManager()
        {
            LoadLevel1();
        }
        void LoadLevel1()
        {
            if (loadCutScene)
            {
                GameMechanicsManager.gamestate = GameState.CutScene;
                new CutScene("TestCutScene1");
            }
            else
                GameMechanicsManager.gamestate = GameState.StartBattle;
        }
        public void Update(GameTime gameTime)
        {
        }
    }
}
