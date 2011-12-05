using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceHaste.Primitives
{
    public class LineManager : GameComponent
    {
        public static LineManager Instance;
        public static List<Line> Lines;
        GraphicsDeviceManager graphics;
        public LineManager(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            Lines = new List<Line>();
            this.graphics = graphics;
            Instance = this;
        }
        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Lines.Count; i++)
                Lines[i].DrawLine(graphics);
        //    base.Draw(gameTime);
        }
        public static void AddLine(Line line)
        {
            Lines.Add(line);
        }
        public static void Clear()
        {
            Lines = new List<Line>();
        }
        public static void RemoveLine(Line line)
        {
            Lines.Remove(line);
        }
    }
}