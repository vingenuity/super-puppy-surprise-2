﻿#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.Cameras;
using SpaceHaste.Grids;
using SpaceHaste.GameObjects;
using SpaceHaste.Controls;
using SpaceHaste.Maps;
using SpaceHaste.Huds;
using SpaceHaste.Primitives;
#endregion

namespace SpaceHaste.Graphics
{
    /// <summary>
    /// Sample showing how to implement non-photorealistic rendering techniques,
    /// providing a cartoon shader, edge detection, and pencil sketch rendering effect.
    /// </summary>
    public class DrawSkyDomeManager : DrawableGameComponent
    {

        public static GraphicsDeviceManager graphics;

        public static ContentManager Content;
        public static Model SkyDome;
        public DrawSkyDomeManager(Game game, GraphicsDeviceManager _graphics)
            : base(game)
        {
            
            graphics = _graphics;
            Content = new ContentManager(game.Services);
            Content.RootDirectory = "Content";
            DrawOrder = -3;
        }
        static void InitTestCube()
        {
            SkyDome = Content.Load<Model>("models/skydome");
        }

        protected override void LoadContent()
        {
            InitTestCube();
        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // GraphicsShader.UnloadContent();
        }



        
        void DrawSkyDome()
        {
            DrawModel(GraphicsManager.SkyDome,
                  Matrix.CreateScale(100f) * Matrix.CreateTranslation(Vector3.Zero),
                  ControlManager.View, ControlManager.Projection);
        }
       private void DrawModel(Model model, Matrix world, Matrix View, Matrix Projections)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;

                    // Use the matrices provided by the chase camera
                    effect.View = View;
                    effect.Projection = Projections;
                }
                mesh.Draw();
            }
        }
       
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            DrawSkyDome();

            base.Draw(gameTime);
        }

    }
}
