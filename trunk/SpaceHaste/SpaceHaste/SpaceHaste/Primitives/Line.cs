using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceHaste.Primitives
{
    public class Line
    {
        Vector3 A;
        Vector3 B;
        short[] connections = new short[2] { 0, 1};
         VertexPositionColor[] pointList;
        public Line(Vector3 A, Vector3 B)
        {
            this.A = A;
            this.B = B;
            pointList = new VertexPositionColor[2];
            pointList[0] = new VertexPositionColor(
                        new Vector3(A.X, A.Y, A.Z), Color.DarkMagenta);
            pointList[1] = new VertexPositionColor(
                        new Vector3(B.X, B.Y, B.Z), Color.DarkMagenta);
        }
        public void DrawLine(GraphicsDeviceManager graphics)
        {

            graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                PrimitiveType.LineList,
                pointList,
                0,  // vertex buffer offset to add to each element of the index buffer
                2,  // number of vertices in pointList
                connections,  // the index buffer
                0,  // first index element to read
                1   // number of primitives to draw
            );
        }
    }
}
