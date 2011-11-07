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
        public List<String> Text;
        public static String currentLine;
        public CutScene(String FileName)
        {
            Text = new List<String>();
            ReadFile(FileName);
        }
        public CutScene()
        {
        }
        void ReadFile(String FileName)
        {
            int counter = 0;
            string line;
            //String s = Environment.CurrentDirectory;
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader( "CutScenes\\"+FileName+".txt");
            while ((line = file.ReadLine()) != null)
            {
                Text.Add(line);
                counter++;
            }

            file.Close();

        }
       
    }
}
