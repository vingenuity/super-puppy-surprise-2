using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpaceHaste.GameMech.CutScenes
{
    public class CutScene
    {
        List<String> Text;
        String currentLine;
        public CutScene(String FileName)
        {
            Text = new List<String>();
            ReadFile(FileName);
        }
        void ReadFile(String FileName)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(FileName);
            while ((line = file.ReadLine()) != null)
            {
                Text.Add(line);
                counter++;
            }

            file.Close();

        }
        internal void NextText()
        {
            //if(GameMechanicsManager.MechMan.)
            //{
            //}
        }

    }
}
