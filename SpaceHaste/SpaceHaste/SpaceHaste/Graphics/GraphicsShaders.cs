using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceHaste.Cameras;

namespace SpaceHaste.Graphics
{
    public class GraphicsShaders
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        Random random = new Random();


        // Effect used to apply the edge detection and pencil sketch postprocessing.
        Effect postprocessEffect;


        // Overlay texture containing the pencil sketch stroke pattern.
        Texture2D sketchTexture;


        // Randomly offsets the sketch pattern to create a hand-drawn animation effect.
        Vector2 sketchJitter;
        TimeSpan timeToNextJitter;


        // Custom rendertargets.
        RenderTarget2D sceneRenderTarget;
        RenderTarget2D normalDepthRenderTarget;


        // Choose what display settings to use.
        NonPhotoRealisticSettings Settings
        {
            get { return NonPhotoRealisticSettings.PresetSettings[settingsIndex]; }
        }

        int settingsIndex = 5;


        public GraphicsShaders()
        {
            
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        public void LoadContent()
        {
            postprocessEffect = GraphicsManager.Content.Load<Effect>("PostprocessEffect");
            sketchTexture = GraphicsManager.Content.Load<Texture2D>("SketchTexture");

            Effect cartoonEffect = GraphicsManager.Content.Load<Effect>("CartoonEffect");
           
           //GraphicsShaders.ChangeEffectUsedByModel(Model2, cartoonEffect);

            // Create two custom rendertargets.
            PresentationParameters pp = GraphicsManager.graphics.GraphicsDevice.PresentationParameters;

            sceneRenderTarget = new RenderTarget2D(GraphicsManager.graphics.GraphicsDevice,
                                                   pp.BackBufferWidth, pp.BackBufferHeight, false,
                                                   pp.BackBufferFormat, pp.DepthStencilFormat);

            normalDepthRenderTarget = new RenderTarget2D(GraphicsManager.graphics.GraphicsDevice,
                                                         pp.BackBufferWidth, pp.BackBufferHeight, false,
                                                         pp.BackBufferFormat, pp.DepthStencilFormat);


        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        public void UnloadContent()
        {
            if (sceneRenderTarget != null)
            {
                sceneRenderTarget.Dispose();
                sceneRenderTarget = null;
            }

            if (normalDepthRenderTarget != null)
            {
                normalDepthRenderTarget.Dispose();
                normalDepthRenderTarget = null;
            }
        }


        /// <summary>
        /// Alters a model so it will draw using a custom effect, while preserving
        /// whatever textures were set on it as part of the original effects.
        /// </summary>
        public static void ChangeEffectUsedByModel(Model model, Effect replacementEffect)
        {
            try
            {
                // Table mapping the original effects to our replacement versions.
                Dictionary<Effect, Effect> effectMapping = new Dictionary<Effect, Effect>();

                foreach (ModelMesh mesh in model.Meshes)
                {
                    // Scan over all the effects currently on the mesh.
                    foreach (BasicEffect oldEffect in mesh.Effects)
                    {
                        // If we haven't already seen this effect...
                        if (!effectMapping.ContainsKey(oldEffect))
                        {
                            // Make a clone of our replacement effect. We can't just use
                            // it directly, because the same effect might need to be
                            // applied several times to different parts of the model using
                            // a different texture each time, so we need a fresh copy each
                            // time we want to set a different texture into it.
                            Effect newEffect = replacementEffect.Clone();

                            // Copy across the texture from the original effect.
                            newEffect.Parameters["Texture"].SetValue(oldEffect.Texture);
                            newEffect.Parameters["TextureEnabled"].SetValue(oldEffect.TextureEnabled);

                            effectMapping.Add(oldEffect, newEffect);
                        }
                    }

                    // Now that we've found all the effects in use on this mesh,
                    // update it to use our new replacement versions.
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = effectMapping[meshPart.Effect];
                    }
                }
            }
            catch { }
        }


        /// <summary>
        /// Allows the game to run logic.
        /// </summary>
        public void Update(GameTime gameTime)
        {

            // Update the sketch overlay texture jitter animation.
            if (Settings.SketchJitterSpeed > 0)
            {
                timeToNextJitter -= gameTime.ElapsedGameTime;

                if (timeToNextJitter <= TimeSpan.Zero)
                {
                    sketchJitter.X = (float)random.NextDouble();
                    sketchJitter.Y = (float)random.NextDouble();

                    timeToNextJitter += TimeSpan.FromSeconds(Settings.SketchJitterSpeed);
                }
            }

            
        }

        void DrawAllModels(string effectTechniqueName)
        {
            for(int i = 0; i< GraphicsManager.GraphicsGameObjects.Count; i++)
                DrawModel(GraphicsManager.GraphicsGameObjects[i].Model,
                    GraphicsManager.GraphicsGameObjects[i].World, 
                    CameraManager.View, CameraManager.Projection, effectTechniqueName);
        }

        
           /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            GraphicsDevice device = GraphicsManager.graphics.GraphicsDevice;

            // Calculate the camera matrices.
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            
            Matrix rotation = Matrix.CreateRotationY(time * 0.5f);

            // If we are doing edge detection, first off we need to render the
            // normals and depth of our model into a special rendertarget.
            
            if (Settings.EnableEdgeDetect)
            {
                device.SetRenderTarget(normalDepthRenderTarget);

                device.Clear(Color.Black);

                //DrawAllModels("NormalDepth");
                //DrawModel(rotation, CameraManager.View, CameraManager.Projection, "NormalDepth");
            }

            // If we are doing edge detection and/or pencil sketch processing, we
            // need to draw the model into a special rendertarget which can then be
            // fed into the postprocessing shader. Otherwise can just draw it
            // directly onto the backbuffer.
            if (Settings.EnableEdgeDetect || Settings.EnableSketch)
                device.SetRenderTarget(sceneRenderTarget);
            else
                device.SetRenderTarget(null);

            device.Clear(Color.Black);

            // Draw the model, using either the cartoon or lambert shading technique.
            string effectTechniqueName;

            if (Settings.EnableToonShading)
                effectTechniqueName = "Toon";
            else
                effectTechniqueName = "Lambert";


            DrawAllModels(effectTechniqueName);

            //DrawModel(rotation, CameraManager.View, CameraManager.Projection, effectTechniqueName);

            // Run the postprocessing filter over the scene that we just rendered.
            if (Settings.EnableEdgeDetect || Settings.EnableSketch)
            {
                device.SetRenderTarget(null);

                ApplyPostprocess();
            }

        }
        
        /// <summary>
        /// Helper for drawing the spinning model using the specified effect technique.
        /// </summary>
        void DrawModel(Model model, Matrix world, Matrix view, Matrix projection,
                       string effectTechniqueName)
        {
            GraphicsDevice GraphicsDevice = GraphicsManager.graphics.GraphicsDevice;
            // Set suitable renderstates for drawing a 3D model.
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Look up the bone transform matrices.
            Matrix[] transforms = new Matrix[model.Bones.Count];

            model.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model.
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    // Specify which effect technique to use.
                    effect.CurrentTechnique = effect.Techniques[effectTechniqueName];

                    Matrix localWorld = transforms[mesh.ParentBone.Index] * world;

                    effect.Parameters["World"].SetValue(localWorld);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                }

                mesh.Draw();
            }
        }
        /*
        void DrawModel(Matrix world, Matrix view, Matrix projection,
                       string effectTechniqueName)
        {
            GraphicsDevice GraphicsDevice = GraphicsManager.graphics.GraphicsDevice;
            // Set suitable renderstates for drawing a 3D model.
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Look up the bone transform matrices.
            Matrix[] transforms = new Matrix[Model2.Bones.Count];

            Model2.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model.
            foreach (ModelMesh mesh in Model2.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    // Specify which effect technique to use.
                    effect.CurrentTechnique = effect.Techniques[effectTechniqueName];

                    Matrix localWorld = transforms[mesh.ParentBone.Index] * world;

                    effect.Parameters["World"].SetValue(localWorld);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                }

                mesh.Draw();
            }
        }
        */
        /// <summary>
        /// Helper applies the edge detection and pencil sketch postprocess effect.
        /// </summary>
        void ApplyPostprocess()
        {
            EffectParameterCollection parameters = postprocessEffect.Parameters;
            string effectTechniqueName;

            // Set effect parameters controlling the pencil sketch effect.
            if (Settings.EnableSketch)
            {
                parameters["SketchThreshold"].SetValue(Settings.SketchThreshold);
                parameters["SketchBrightness"].SetValue(Settings.SketchBrightness);
                parameters["SketchJitter"].SetValue(sketchJitter);
                parameters["SketchTexture"].SetValue(sketchTexture);
            }

            // Set effect parameters controlling the edge detection effect.
            if (Settings.EnableEdgeDetect)
            {
                Vector2 resolution = new Vector2(sceneRenderTarget.Width,
                                                 sceneRenderTarget.Height);

                Texture2D normalDepthTexture = normalDepthRenderTarget;

                parameters["EdgeWidth"].SetValue(Settings.EdgeWidth);
                parameters["EdgeIntensity"].SetValue(Settings.EdgeIntensity);
                parameters["ScreenResolution"].SetValue(resolution);
                parameters["NormalDepthTexture"].SetValue(normalDepthTexture);

                // Choose which effect technique to use.
                if (Settings.EnableSketch)
                {
                    if (Settings.SketchInColor)
                        effectTechniqueName = "EdgeDetectColorSketch";
                    else
                        effectTechniqueName = "EdgeDetectMonoSketch";
                }
                else
                    effectTechniqueName = "EdgeDetect";
            }
            else
            {
                // If edge detection is off, just pick one of the sketch techniques.
                if (Settings.SketchInColor)
                    effectTechniqueName = "ColorSketch";
                else
                    effectTechniqueName = "MonoSketch";
            }

            // Activate the appropriate effect technique.
            postprocessEffect.CurrentTechnique = postprocessEffect.Techniques[effectTechniqueName];

            // Draw a fullscreen sprite to apply the postprocessing effect.
            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, postprocessEffect);
            spriteBatch.Draw(sceneRenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}

