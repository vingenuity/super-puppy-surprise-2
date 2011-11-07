using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.Cameras;
using SpaceHaste.GameMech;
using SpaceHaste.GameMech.BattleMechanicsManagers;

namespace SpaceHaste.Controls
{
    public class ControlManager : GameComponent
    {
        //Camera
        public static RotationCamera camera;
        GraphicsDeviceManager graphMan;
        public static Matrix View;
        public static Matrix Projection;

        //Movement
        bool reverse;
        private static Dictionary<Keys, GameAction> KeyMap;
        private static Dictionary<Buttons, GameAction> PadMap;
        internal delegate void GameAction(); 
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
                case Keys.Home:
                case Keys.End:
                    return true;
                default:
                    return false;
            }
        }

        private void MapControls()
        {
            //Add Keyboard Keys
            KeyMap.Add(Keys.W,     new GameAction(camera.MoveCameraUp));
            KeyMap.Add(Keys.S,     new GameAction(camera.MoveCameraDown));
            KeyMap.Add(Keys.A,     new GameAction(camera.MoveCameraLeft));
            KeyMap.Add(Keys.D,     new GameAction(camera.MoveCameraRight));
            KeyMap.Add(Keys.E,     new GameAction(camera.ZoomIn));
            KeyMap.Add(Keys.Q,     new GameAction(camera.ZoomOut));
            KeyMap.Add(Keys.Enter, new GameAction(BattleMechanicsManager.Instance.Selection));
            KeyMap.Add(Keys.Escape, new GameAction(BattleMechanicsManager.Instance.Back));
            KeyMap.Add(Keys.I,     new GameAction(BattleMechanicsManager.Instance.MoveSelectionUp));
            KeyMap.Add(Keys.K,     new GameAction(BattleMechanicsManager.Instance.MoveSelectionDown));
            KeyMap.Add(Keys.J,     new GameAction(BattleMechanicsManager.Instance.MoveSelectionLeft));
            KeyMap.Add(Keys.L,     new GameAction(BattleMechanicsManager.Instance.MoveSelectionRight));
            KeyMap.Add(Keys.O,     new GameAction(BattleMechanicsManager.Instance.MoveSelectionHigher));
            KeyMap.Add(Keys.U,     new GameAction(BattleMechanicsManager.Instance.MoveSelectionLower));

            //alt keyboard
            KeyMap.Add(Keys.Home,     new GameAction(camera.ZoomIn));
            KeyMap.Add(Keys.End,      new GameAction(camera.ZoomOut));
            KeyMap.Add(Keys.Right,    new GameAction(BattleMechanicsManager.Instance.MoveSelectionUp));
            KeyMap.Add(Keys.Left,     new GameAction(BattleMechanicsManager.Instance.MoveSelectionDown));
            KeyMap.Add(Keys.Up,       new GameAction(BattleMechanicsManager.Instance.MoveSelectionLeft));
            KeyMap.Add(Keys.Down,     new GameAction(BattleMechanicsManager.Instance.MoveSelectionRight));
            KeyMap.Add(Keys.PageUp,   new GameAction(BattleMechanicsManager.Instance.MoveSelectionHigher));
            KeyMap.Add(Keys.PageDown, new GameAction(BattleMechanicsManager.Instance.MoveSelectionLower));

            //Add GamePad Buttons
            PadMap.Add(Buttons.A, new GameAction(BattleMechanicsManager.Instance.Selection));
            PadMap.Add(Buttons.B, new GameAction(BattleMechanicsManager.Instance.Back));
            PadMap.Add(Buttons.LeftThumbstickUp, new GameAction(BattleMechanicsManager.Instance.MoveSelectionUp));
            PadMap.Add(Buttons.LeftThumbstickDown, new GameAction(BattleMechanicsManager.Instance.MoveSelectionDown));
            PadMap.Add(Buttons.LeftThumbstickLeft, new GameAction(BattleMechanicsManager.Instance.MoveSelectionLeft));
            PadMap.Add(Buttons.LeftThumbstickRight, new GameAction(BattleMechanicsManager.Instance.MoveSelectionRight));
            PadMap.Add(Buttons.LeftStick, new GameAction(VertSelection));
            PadMap.Add(Buttons.RightStick, new GameAction(CameraZoom));
        }
        internal void Remap(Keys newKey, GameAction action)
        {
            if (KeyMap.ContainsKey(newKey))
                KeyMap[newKey] = action;
            else
                KeyMap.Add(newKey, action);

        }
        internal void Remap(Buttons newButton, GameAction action)
        {
            if (PadMap.ContainsKey(newButton))
                PadMap[newButton] = action;
            else
                PadMap.Add(newButton, action);
        }

        //These functions dynamically remap the controls for the game situation.
        private void RemapStandard()
        {
            Remap(Buttons.LeftThumbstickUp, BattleMechanicsManager.Instance.MoveSelectionUp);
            Remap(Buttons.LeftThumbstickDown, BattleMechanicsManager.Instance.MoveSelectionDown);
            Remap(Buttons.LeftThumbstickLeft, BattleMechanicsManager.Instance.MoveSelectionLeft);
            Remap(Buttons.LeftThumbstickRight, BattleMechanicsManager.Instance.MoveSelectionRight);
            Remap(Keys.I, BattleMechanicsManager.Instance.MoveSelectionUp);
            Remap(Keys.K, BattleMechanicsManager.Instance.MoveSelectionDown);
            Remap(Keys.J, BattleMechanicsManager.Instance.MoveSelectionLeft);
            Remap(Keys.L, BattleMechanicsManager.Instance.MoveSelectionRight);
        }
        private void RemapToCameraPersp()
        {
            if (camera.getHorizontalAngle() < .785 || camera.getHorizontalAngle() > 5.497)
            {
                Remap(Buttons.LeftThumbstickUp, BattleMechanicsManager.Instance.MoveSelectionLeft);
                Remap(Buttons.LeftThumbstickDown, BattleMechanicsManager.Instance.MoveSelectionRight);
                Remap(Buttons.LeftThumbstickLeft, BattleMechanicsManager.Instance.MoveSelectionDown);
                Remap(Buttons.LeftThumbstickRight, BattleMechanicsManager.Instance.MoveSelectionUp);
                Remap(Keys.I, BattleMechanicsManager.Instance.MoveSelectionLeft);
                Remap(Keys.K, BattleMechanicsManager.Instance.MoveSelectionRight);
                Remap(Keys.J, BattleMechanicsManager.Instance.MoveSelectionDown);
                Remap(Keys.L, BattleMechanicsManager.Instance.MoveSelectionUp);
            }
            else if (camera.getHorizontalAngle() > .785 && camera.getHorizontalAngle() < 2.356)
            {
                Remap(Buttons.LeftThumbstickUp, BattleMechanicsManager.Instance.MoveSelectionDown);
                Remap(Buttons.LeftThumbstickDown, BattleMechanicsManager.Instance.MoveSelectionUp);
                Remap(Buttons.LeftThumbstickLeft, BattleMechanicsManager.Instance.MoveSelectionRight);
                Remap(Buttons.LeftThumbstickRight, BattleMechanicsManager.Instance.MoveSelectionLeft);
                Remap(Keys.I, BattleMechanicsManager.Instance.MoveSelectionDown);
                Remap(Keys.K, BattleMechanicsManager.Instance.MoveSelectionUp);
                Remap(Keys.J, BattleMechanicsManager.Instance.MoveSelectionRight);
                Remap(Keys.L, BattleMechanicsManager.Instance.MoveSelectionLeft);
            }
            else if (camera.getHorizontalAngle() > 2.356 && camera.getHorizontalAngle() < 3.926)
            {
                Remap(Buttons.LeftThumbstickUp, BattleMechanicsManager.Instance.MoveSelectionRight);
                Remap(Buttons.LeftThumbstickDown, BattleMechanicsManager.Instance.MoveSelectionLeft);
                Remap(Buttons.LeftThumbstickLeft, BattleMechanicsManager.Instance.MoveSelectionUp);
                Remap(Buttons.LeftThumbstickRight, BattleMechanicsManager.Instance.MoveSelectionDown);
                Remap(Keys.I, BattleMechanicsManager.Instance.MoveSelectionRight);
                Remap(Keys.K, BattleMechanicsManager.Instance.MoveSelectionLeft);
                Remap(Keys.J, BattleMechanicsManager.Instance.MoveSelectionUp);
                Remap(Keys.L, BattleMechanicsManager.Instance.MoveSelectionDown);
            }
            else
            {
                Remap(Buttons.LeftThumbstickUp, BattleMechanicsManager.Instance.MoveSelectionUp);
                Remap(Buttons.LeftThumbstickDown, BattleMechanicsManager.Instance.MoveSelectionDown);
                Remap(Buttons.LeftThumbstickLeft, BattleMechanicsManager.Instance.MoveSelectionLeft);
                Remap(Buttons.LeftThumbstickRight, BattleMechanicsManager.Instance.MoveSelectionRight);
                Remap(Keys.I, BattleMechanicsManager.Instance.MoveSelectionUp);
                Remap(Keys.K, BattleMechanicsManager.Instance.MoveSelectionDown);
                Remap(Keys.J, BattleMechanicsManager.Instance.MoveSelectionLeft);
                Remap(Keys.L, BattleMechanicsManager.Instance.MoveSelectionRight);
            }
        }

        private void CameraZoom()
        {
            if (reverse) camera.ZoomOut();
            else camera.ZoomIn();
        }
        private void VertSelection()
        {
            if (reverse) BattleMechanicsManager.Instance.MoveSelectionLower();
            else BattleMechanicsManager.Instance.MoveSelectionHigher();
        }

        public override void Update(GameTime gameTime)
        {
            //Remap Controls to standard if in menus, else remap dynamically to the camera perspective.
            if (GameMechanicsManager.gamestate == GameState.SelectShipAction)
                RemapStandard();
            else
                RemapToCameraPersp();

            //Take Keyboard controls if the gamepad isn't connected; otherwise take gamepad controls
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
