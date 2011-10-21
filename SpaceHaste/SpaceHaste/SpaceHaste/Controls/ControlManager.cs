using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.Cameras;
using SpaceHaste.GameMech;

namespace SpaceHaste.Controls
{
    public class ControlManager : GameComponent
    {
        //Camera
        RotationCamera camera;
        GraphicsDeviceManager graphMan;
        public static Matrix View;
        public static Matrix Projection;

        //Movement
        private static Dictionary<Keys, GameAction> KeyMap;
        private delegate void GameAction(); 
        KeyboardState lastKState;
        int LastPacket;

        public ControlManager(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            graphMan = graphics;
            camera = new RotationCamera(graphMan);

            KeyMap = new Dictionary<Keys, GameAction>();
            MapControls();
            lastKState = Keyboard.GetState();
            LastPacket = 0;
        }

        private void MapControls()
        {
            //Add Keyboard Keys
            KeyMap.Add(Keys.W, new GameAction(camera.MoveCameraUp));
            KeyMap.Add(Keys.S, new GameAction(camera.MoveCameraDown));
            KeyMap.Add(Keys.A, new GameAction(camera.MoveCameraLeft));
            KeyMap.Add(Keys.D, new GameAction(camera.MoveCameraRight));
            KeyMap.Add(Keys.E, new GameAction(camera.ZoomIn));
            KeyMap.Add(Keys.Q, new GameAction(camera.ZoomOut));
            KeyMap.Add(Keys.Enter, new GameAction(GameMechanicsManager.MechMan.Selection));
            KeyMap.Add(Keys.I, new GameAction(GameMechanicsManager.MechMan.MoveSelectionUp));
            KeyMap.Add(Keys.K, new GameAction(GameMechanicsManager.MechMan.MoveSelectionDown));
            KeyMap.Add(Keys.J, new GameAction(GameMechanicsManager.MechMan.MoveSelectionLeft));
            KeyMap.Add(Keys.L, new GameAction(GameMechanicsManager.MechMan.MoveSelectionRight));
            KeyMap.Add(Keys.O, new GameAction(GameMechanicsManager.MechMan.MoveSelectionHigher));
            KeyMap.Add(Keys.U, new GameAction(GameMechanicsManager.MechMan.MoveSelectionLower));
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState Gstate = GamePad.GetState(PlayerIndex.One);
            if (!Gstate.IsConnected)
            {
                KeyboardState Kstate = Keyboard.GetState();
                foreach (Microsoft.Xna.Framework.Input.Keys key in KeyMap.Keys)
                    if (Kstate.IsKeyDown(key) && lastKState.IsKeyUp(key))
                        KeyMap[key]();
                lastKState = Kstate;
            }
            else
            {
                if (Gstate.PacketNumber == LastPacket)
                    return;
                LastPacket = Gstate.PacketNumber;
                if (Gstate.ThumbSticks.Right.X < -0.5) camera.MoveCameraLeft();
                if (Gstate.ThumbSticks.Right.X > 0.5) camera.MoveCameraRight();
                if (Gstate.ThumbSticks.Right.Y > 0.5) camera.MoveCameraUp();
                if (Gstate.ThumbSticks.Right.Y < -0.5) camera.MoveCameraDown();
                if (Gstate.IsButtonDown(Buttons.RightStick)) camera.ZoomIn();
                if (Gstate.IsButtonDown(Buttons.RightStick) && Gstate.IsButtonDown(Buttons.LeftTrigger)) camera.ZoomOut();
                if (Gstate.IsButtonDown(Buttons.A)) GameMechanicsManager.MechMan.Selection();
                if (Gstate.ThumbSticks.Right.X < -0.5) GameMechanicsManager.MechMan.MoveSelectionLeft();
                if (Gstate.ThumbSticks.Right.X > 0.5) GameMechanicsManager.MechMan.MoveSelectionRight();
                if (Gstate.ThumbSticks.Right.Y > 0.5) GameMechanicsManager.MechMan.MoveSelectionUp();
                if (Gstate.ThumbSticks.Right.Y < -0.5) GameMechanicsManager.MechMan.MoveSelectionDown();
                if (Gstate.IsButtonDown(Buttons.RightStick)) GameMechanicsManager.MechMan.MoveSelectionLower();
                if (Gstate.IsButtonDown(Buttons.RightStick) && Gstate.IsButtonDown(Buttons.LeftTrigger)) GameMechanicsManager.MechMan.MoveSelectionHigher();
            }
            camera.UpdateView(gameTime);
            camera.UpdateProjection(gameTime);
            base.Update(gameTime);
        }
    }
}
