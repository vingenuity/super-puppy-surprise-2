using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using SpaceHaste.Huds;

namespace SpaceHaste.GameMech.CutScenes
{
    public class CutScene
    {
        public List<String> Text;
        public String currentLine;
        TextBox box;
        public Boolean end = false;


        public CutScene(String FileName)
        {
            box = new TextBox();
            Text = new List<String>();
            ReadFile(FileName);
            currentLine = Text[0];
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
        public void drawCutscene() {
            if (currentLine == null)
                box = null;
            else
            {
                string[] strings = currentLine.Split('|');
                box.DrawSet(strings);
            }
        }

        public void destroyBox() {
            box.endBox();
        }


    }
}
