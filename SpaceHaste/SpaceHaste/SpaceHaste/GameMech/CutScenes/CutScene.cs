using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace SpaceHaste.GameMech.CutScenes
{
    public class CutScene
    {
        List<String> Text;
        public static String currentLine;
       
        public CutScene(String FileName)
        {
            Text = new List<String>();
            ReadFile(FileName);
        }
        void ReadFile(String FileName)
        {
            int counter = 0;
            string line;
            //String s = Environment.CurrentDirectory;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader( "CutSceneText\\"+FileName+".txt");
            while ((line = file.ReadLine()) != null)
            {
                Text.Add(line);
                counter++;
            }

            file.Close();

        }
        internal void NextText()
        {
            if(GameMechanicsManager.gamestate == GameState.CutScene)
            {
                if (Text.Count > 0)
                {
                    currentLine = Text[0];
                    Text.RemoveAt(0);
                }
                else
                    GameMechanicsManager.gamestate = GameState.EnterShipAction;
            }
        }
    }
}
