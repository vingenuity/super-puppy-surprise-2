using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste.GameMech;
using SpaceHaste.GameMech.BattleMechanicsManagers;
using SpaceHaste.GameMech.CutScenes;
using SpaceHaste.GameMech.LevelManagers;

namespace SpaceHaste.Huds
{
    public class DisplayCutScenes
    {
        public bool DisplayCommands;

        CutScene scene;

        private bool ShowCutSceneText = true;


        public DisplayCutScenes()
        {
            scene = new CutScene();
        }

        /*public void Load()
        {
            texture1 = Hud.Content.Load<Texture2D>("UI_backPane_blue");
        }*/

        public void Draw()
        {

            scene = LevelManager.Instance.getScene();
            if (!ShowCutSceneText)
                return;

            scene.drawCutscene();
            
        }
        public void Update(GameTime gameTime)
        {
            if (GameMechanicsManager.gamestate == GameState.CutScene)
                ShowCutSceneText = true;
            else
                ShowCutSceneText = false;
           
        }
        public void Attack()
        {
        }
    }
}
