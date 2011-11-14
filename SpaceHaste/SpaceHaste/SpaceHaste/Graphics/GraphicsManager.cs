#region File Description
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
#endregion

namespace SpaceHaste.Graphics
{
    /// <summary>
    /// Sample showing how to implement non-photorealistic rendering techniques,
    /// providing a cartoon shader, edge detection, and pencil sketch rendering effect.
    /// </summary>
    public class GraphicsManager : DrawableGameComponent
    {
        public static List<GameObject> GraphicsGameObjects;
        public static List<SuperTerrain> GraphicsSuperTerrain;
       
        public static GraphicsDeviceManager graphics;

        public static ContentManager Content;

      //  GraphicsShaders GraphicsShader;
        public static Model TestCube;
        // Change the model to use our custom cartoon shading effect.
        //static Effect cartoonEffect;

        public GraphicsManager(Game game, GraphicsDeviceManager _graphics)
            : base(game)
        {
            graphics = _graphics;
            Content = new ContentManager(game.Services);
            Content.RootDirectory = "Content";
           // GraphicsShader = new GraphicsShaders(graphics);
            GraphicsGameObjects = new List<GameObject>();
           
        }
        static void InitTestCube()
        {
            TestCube = Content.Load<Model>("Ship");
           // GraphicsShaders.ChangeEffectUsedByModel(TestCube, cartoonEffect);
        }

        public static void AddGameObject(GameObject gameObject)
        {
            GraphicsGameObjects.Add(gameObject);
          //  GraphicsShaders.ChangeEffectUsedByModel(gameObject.Model, cartoonEffect);
            //GraphicsShader.CreateEffectsForModel(gameObject);
        }
        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            //GraphicsShader.LoadContent();
            //cartoonEffect = GraphicsManager.Content.Load<Effect>("CartoonEffect");
            //for(int i = 0; i<GraphicsGameObjects.Count; i++)
              //  GraphicsShaders.ChangeEffectUsedByModel(GraphicsGameObjects[i].Model, cartoonEffect);
            InitTestCube();
        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
           // GraphicsShader.UnloadContent();
        }



        /// <summary>
        /// Allows the game to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
         //   GraphicsShader.Update(gameTime);
            UpdateWorldMatricies(gameTime);
        }
        void DrawAllModels()
        {
           
            for (int i = 0; i < GraphicsManager.GraphicsGameObjects.Count; i++)
                DrawModel(GraphicsManager.GraphicsGameObjects[i].Model,
                    GraphicsManager.GraphicsGameObjects[i].World,
                    ControlManager.View, ControlManager.Projection);
            for (int i = 0; i < Maps.Map.map.EnvMapObjects.Count; i++)
            {
                DrawModel(GraphicsManager.TestCube,
                    Matrix.CreateScale(.1f) * Matrix.CreateTranslation(Maps.Map.map.EnvMapObjects[i].Center),
                    ControlManager.View, ControlManager.Projection);
            }
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
        private void UpdateWorldMatricies(GameTime gameTime)
        {
            for (int i = 0; i < GraphicsGameObjects.Count; i++)
            {
                if(GraphicsGameObjects[i].team == 0)
                    GraphicsGameObjects[i].World = Matrix.CreateScale(GraphicsGameObjects[i].Scale) * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateTranslation(GraphicsGameObjects[i].DrawPosition);
                else
                     GraphicsGameObjects[i].World = Matrix.CreateScale(GraphicsGameObjects[i].Scale) * Matrix.CreateTranslation(GraphicsGameObjects[i].DrawPosition) ;
            }
        }


       
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            device.Clear(Color.CornflowerBlue);

            graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            DrawAllModels();
            //GraphicsShader.Draw(gameTime);

            base.Draw(gameTime);
        }

    }
}
