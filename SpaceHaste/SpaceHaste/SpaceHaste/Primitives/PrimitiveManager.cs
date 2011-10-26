using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceHaste.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.Primitives;

namespace SpaceHaste.Grids
{
    public class PrimitiveManager : DrawableGameComponent
    {
        //Matrix worldMatrix;

        BasicEffect basicEffect;
        VertexDeclaration vertexDeclaration;
        VertexPositionColor[] pointList ;
        VertexBuffer vertexBuffer;

        

        int points = 8;
        short[] lineListIndices;
        short[] lineStripIndices;
        //short[] triangleListIndices;
        //short[] triangleStripIndices;

        //GraphicsDevice GraphicsDevice;

        enum PrimType
        {
            LineList,
            LineStrip,
            TriangleList,
            TriangleStrip
        };
        //PrimType typeToDraw = PrimType.LineList;

        RasterizerState rasterizerState;

        //GamePadState currentGamePadState;
        //GamePadState lastGamePadState;

        //KeyboardState currentKeyboardState;
        //KeyboardState lastKeyboardState;

        GraphicsDeviceManager graphics;

        public PrimitiveManager(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            this.graphics = graphics;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
           
            InitializeEffect();
            InitializePoints();
            InitializeLineList();
            InitializeLineStrip();


            rasterizerState = new RasterizerState();
            rasterizerState.FillMode = FillMode.WireFrame;
            rasterizerState.CullMode = CullMode.None;


        }


        

        /// <summary>
        /// Initializes the effect (loading, parameter setting, and technique selection)
        /// used by the game.
        /// </summary>
        private void InitializeEffect()
        {

            vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                }
            );

            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;

            //worldMatrix = Matrix.CreateTranslation(graphics.GraphicsDevice.Viewport.Width / 2f - 150,
             //   graphics.GraphicsDevice.Viewport.Height / 2f - 50, 0);
           // basicEffect.World = worldMatrix;
           // basicEffect.View = CameraManager.View;
           // basicEffect.Projection = CameraManager.Projection;
        }
        public override void Update(GameTime gameTime)
        {
            basicEffect.World = Matrix.Identity;
            basicEffect.View = ControlManager.View;
            basicEffect.Projection = ControlManager.Projection;
            base.Update(gameTime);
        }
        /// <summary>
        /// Initializes the point list.
        /// </summary>
        private void InitializePoints()
        {
            pointList = new VertexPositionColor[points];

            for (int x = 0; x < points / 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    pointList[(x * 2) + y] = new VertexPositionColor(
                        new Vector3(x * 100, y * 100, 0), Color.DarkMagenta);
                }
            }

            // Initialize the vertex buffer, allocating memory for each vertex.
            vertexBuffer = new VertexBuffer(graphics.GraphicsDevice, vertexDeclaration,
                points, BufferUsage.None);

            // Set the vertex buffer data to the array of vertices.
            vertexBuffer.SetData<VertexPositionColor>(pointList);
        }

        /// <summary>
        /// Initializes the line list.
        /// </summary>
        private void InitializeLineList()
        {
            // Initialize an array of indices of type short.
            lineListIndices = new short[2] { 0, 1};
        }

        /// <summary>
        /// Initializes the line strip.
        /// </summary>
        private void InitializeLineStrip()
        {
            // Initialize an array of indices of type short.
            lineStripIndices = new short[points];

            // Populate the array with references to indices in the vertex buffer.
            for (int i = 0; i < points; i++)
            {
                lineStripIndices[i] = (short)(i);
            }
        }

       
       

      

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {

            basicEffect.View = ControlManager.View;
            basicEffect.Projection = ControlManager.Projection;

            //GraphicsDevice.Clear(Color.SteelBlue);

            // The effect is a compiled effect created and compiled elsewhere
            // in the application.

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();


                  DrawLineList();


               // DrawLineStrip();



            }
            base.Draw(gameTime);
        }

        void DrawAllLines()
        {
            for (int i = 0; i < LineManager.Lines.Count; i++)
            {
                graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList, pointList, 0, 2, lineListIndices, 0, 2);
            }
        }
        /// <summary>
        /// Draws the line list.
        /// </summary>
        private void DrawLineList()
        {
            graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.LineList,
                pointList,
                0,  // vertex buffer offset to add to each element of the index buffer
                2,  // number of vertices in pointList
                lineListIndices,  // the index buffer
                0,  // first index element to read
                1   // number of primitives to draw
            );
        }

        /// <summary>
        /// Draws the line strip.
        /// </summary>
        private void DrawLineStrip()
        {
            for (int i = 0; i < pointList.Length; i++)
                pointList[i].Color = Color.Red;

            graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.LineStrip,
                pointList,
                0,   // vertex buffer offset to add to each element of the index buffer
                8,   // number of vertices to draw
                lineStripIndices,
                0,   // first index element to read
                7    // number of primitives to draw
            );
            Color c = Color.DarkMagenta;
            for (int i = 0; i < pointList.Length; i++)
                pointList[i].Color = c;

        }
    }
}
