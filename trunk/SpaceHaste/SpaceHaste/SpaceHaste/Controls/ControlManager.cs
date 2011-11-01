﻿using System;
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
            KeyMap.Add(Keys.Enter, new GameAction(GameMechanicsManager.MechMan.Selection));
            KeyMap.Add(Keys.Escape, new GameAction(GameMechanicsManager.MechMan.Back));
            KeyMap.Add(Keys.I,     new GameAction(GameMechanicsManager.MechMan.MoveSelectionUp));
            KeyMap.Add(Keys.K,     new GameAction(GameMechanicsManager.MechMan.MoveSelectionDown));
            KeyMap.Add(Keys.J,     new GameAction(GameMechanicsManager.MechMan.MoveSelectionLeft));
            KeyMap.Add(Keys.L,     new GameAction(GameMechanicsManager.MechMan.MoveSelectionRight));
            KeyMap.Add(Keys.O,     new GameAction(GameMechanicsManager.MechMan.MoveSelectionHigher));
            KeyMap.Add(Keys.U,     new GameAction(GameMechanicsManager.MechMan.MoveSelectionLower));

            //Add GamePad Buttons
            PadMap.Add(Buttons.A, new GameAction(GameMechanicsManager.MechMan.Selection));
            PadMap.Add(Buttons.B, new GameAction(GameMechanicsManager.MechMan.Back));
            PadMap.Add(Buttons.LeftThumbstickUp, new GameAction(GameMechanicsManager.MechMan.MoveSelectionUp));
            PadMap.Add(Buttons.LeftThumbstickDown, new GameAction(GameMechanicsManager.MechMan.MoveSelectionDown));
            PadMap.Add(Buttons.LeftThumbstickLeft, new GameAction(GameMechanicsManager.MechMan.MoveSelectionLeft));
            PadMap.Add(Buttons.LeftThumbstickRight, new GameAction(GameMechanicsManager.MechMan.MoveSelectionRight));
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

        //These functions dynamically remap the controls based upon the game situation.
        private void RemapStandard()
        {
            Remap(Buttons.LeftThumbstickUp, GameMechanicsManager.MechMan.MoveSelectionUp);
            Remap(Buttons.LeftThumbstickDown, GameMechanicsManager.MechMan.MoveSelectionDown);
            Remap(Buttons.LeftThumbstickLeft, GameMechanicsManager.MechMan.MoveSelectionLeft);
            Remap(Buttons.LeftThumbstickRight, GameMechanicsManager.MechMan.MoveSelectionRight);
            Remap(Keys.I, GameMechanicsManager.MechMan.MoveSelectionUp);
            Remap(Keys.K, GameMechanicsManager.MechMan.MoveSelectionDown);
            Remap(Keys.J, GameMechanicsManager.MechMan.MoveSelectionLeft);
            Remap(Keys.L, GameMechanicsManager.MechMan.MoveSelectionRight);
        }
        private void RemapToCameraPersp()
        {
            if (camera.getHorizontalAngle() < .785 || camera.getHorizontalAngle() > 5.497)
            {
                Remap(Buttons.LeftThumbstickUp, GameMechanicsManager.MechMan.MoveSelectionLeft);
                Remap(Buttons.LeftThumbstickDown, GameMechanicsManager.MechMan.MoveSelectionRight);
                Remap(Buttons.LeftThumbstickLeft, GameMechanicsManager.MechMan.MoveSelectionDown);
                Remap(Buttons.LeftThumbstickRight, GameMechanicsManager.MechMan.MoveSelectionUp);
                Remap(Keys.I, GameMechanicsManager.MechMan.MoveSelectionLeft);
                Remap(Keys.K, GameMechanicsManager.MechMan.MoveSelectionRight);
                Remap(Keys.J, GameMechanicsManager.MechMan.MoveSelectionDown);
                Remap(Keys.L, GameMechanicsManager.MechMan.MoveSelectionUp);
            }
            else if (camera.getHorizontalAngle() > .785 && camera.getHorizontalAngle() < 2.356)
            {
                Remap(Buttons.LeftThumbstickUp, GameMechanicsManager.MechMan.MoveSelectionDown);
                Remap(Buttons.LeftThumbstickDown, GameMechanicsManager.MechMan.MoveSelectionUp);
                Remap(Buttons.LeftThumbstickLeft, GameMechanicsManager.MechMan.MoveSelectionRight);
                Remap(Buttons.LeftThumbstickRight, GameMechanicsManager.MechMan.MoveSelectionLeft);
                Remap(Keys.I, GameMechanicsManager.MechMan.MoveSelectionDown);
                Remap(Keys.K, GameMechanicsManager.MechMan.MoveSelectionUp);
                Remap(Keys.J, GameMechanicsManager.MechMan.MoveSelectionRight);
                Remap(Keys.L, GameMechanicsManager.MechMan.MoveSelectionLeft);
            }
            else if (camera.getHorizontalAngle() > 2.356 && camera.getHorizontalAngle() < 3.926)
            {
                Remap(Buttons.LeftThumbstickUp, GameMechanicsManager.MechMan.MoveSelectionRight);
                Remap(Buttons.LeftThumbstickDown, GameMechanicsManager.MechMan.MoveSelectionLeft);
                Remap(Buttons.LeftThumbstickLeft, GameMechanicsManager.MechMan.MoveSelectionUp);
                Remap(Buttons.LeftThumbstickRight, GameMechanicsManager.MechMan.MoveSelectionDown);
                Remap(Keys.I, GameMechanicsManager.MechMan.MoveSelectionRight);
                Remap(Keys.K, GameMechanicsManager.MechMan.MoveSelectionLeft);
                Remap(Keys.J, GameMechanicsManager.MechMan.MoveSelectionUp);
                Remap(Keys.L, GameMechanicsManager.MechMan.MoveSelectionDown);
            }
            else
            {
                Remap(Buttons.LeftThumbstickUp, GameMechanicsManager.MechMan.MoveSelectionUp);
                Remap(Buttons.LeftThumbstickDown, GameMechanicsManager.MechMan.MoveSelectionDown);
                Remap(Buttons.LeftThumbstickLeft, GameMechanicsManager.MechMan.MoveSelectionLeft);
                Remap(Buttons.LeftThumbstickRight, GameMechanicsManager.MechMan.MoveSelectionRight);
                Remap(Keys.I, GameMechanicsManager.MechMan.MoveSelectionUp);
                Remap(Keys.K, GameMechanicsManager.MechMan.MoveSelectionDown);
                Remap(Keys.J, GameMechanicsManager.MechMan.MoveSelectionLeft);
                Remap(Keys.L, GameMechanicsManager.MechMan.MoveSelectionRight);
            }
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
            //Remap Controls to standard if in menus, else remap dynamically to the camera perspective.
            if (GameMechanicsManager.MechMan.gamestate == GameMechanicsManager.GameState.SelectShipAction)
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
