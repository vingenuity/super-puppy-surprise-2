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
        bool reverse;
        private static Dictionary<Keys, GameAction> KeyMap;
        private static Dictionary<Buttons, GameAction> PadMap;
        private delegate void GameAction(); 
        KeyboardState lastKState;
        GamePadState lastGState;

        public ControlManager(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            graphMan = graphics;
            camera = new RotationCamera(graphMan);

            KeyMap = new Dictionary<Keys, GameAction>();
            PadMap = new Dictionary<Buttons, GameAction>();
            MapControls();
            lastKState = Keyboard.GetState();
            lastGState = GamePad.GetState(PlayerIndex.One);
        }

        private bool isCameraButton(Buttons b)
        {
            if (b == Buttons.RightStick) return true;
            return false;
        }
        private bool isCameraKey(Keys key)
        {
            switch (key)
            {
                case Keys.W:
                case Keys.A:
                case Keys.S:
                case Keys.D:
                case Keys.E:
                case Keys.Q:
                    return true;
                default:
                    return false;
            }
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

            //Add GamePad Buttons
            PadMap.Add(Buttons.A, new GameAction(GameMechanicsManager.MechMan.Selection));
            PadMap.Add(Buttons.LeftThumbstickUp, new GameAction(GameMechanicsManager.MechMan.MoveSelectionUp));
            PadMap.Add(Buttons.LeftThumbstickDown, new GameAction(GameMechanicsManager.MechMan.MoveSelectionDown));
            PadMap.Add(Buttons.LeftThumbstickLeft, new GameAction(GameMechanicsManager.MechMan.MoveSelectionLeft));
            PadMap.Add(Buttons.LeftThumbstickRight, new GameAction(GameMechanicsManager.MechMan.MoveSelectionRight));
            PadMap.Add(Buttons.LeftStick, new GameAction(VertSelection));
            PadMap.Add(Buttons.RightStick, new GameAction(CameraZoom));
        }

        private void CameraZoom()
        {
            if (reverse) camera.ZoomOut();
            else camera.ZoomIn();
        }
        private void VertSelection()
        {
            if (reverse) GameMechanicsManager.MechMan.MoveSelectionLower();
            else GameMechanicsManager.MechMan.MoveSelectionHigher();
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState Gstate = GamePad.GetState(PlayerIndex.One);
            if (!Gstate.IsConnected)
            {
                KeyboardState Kstate = Keyboard.GetState();
                foreach (Keys key in KeyMap.Keys)
                {
                    if (Kstate.IsKeyDown(key))
                    {
                        if(lastKState.IsKeyUp(key))
                            KeyMap[key]();
                        else if (isCameraKey(key))
                            KeyMap[key]();
                    }
                }
                lastKState = Kstate;
            }
            else
            {
                reverse = (Gstate.IsButtonDown(Buttons.RightTrigger)) ? true : false;
                camera.AnalogMove(Gstate.ThumbSticks.Right);
                foreach (Buttons button in PadMap.Keys)
                {
                    if (Gstate.IsButtonDown(button))
                    {
                        if (lastGState.IsButtonUp(button))
                            PadMap[button]();
                        else if (isCameraButton(button))
                            PadMap[button]();
                    }
                }
                lastGState  = Gstate;
            }
            camera.UpdateView(gameTime);
            camera.UpdateProjection(gameTime);
            base.Update(gameTime);
        }
    }
}
