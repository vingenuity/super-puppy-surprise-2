#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using SpaceHaste;
using AvatarElementalBash.SaveLoad;
using SpaceHaste.GameMech.LevelManagers;

#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class VictoryDefeatScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public VictoryDefeatScreen(string p, string p2)
            : base(p)
        {
        //    if (p.Contains("F"))
        //        Game1.SoundEngine.PlaySound(SoundEffects.Victory);
            // Create our menu entries.
            MenuEntry restartGameMenuEntry= new MenuEntry(p2);
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            // Hook up menu event handlers.
            restartGameMenuEntry.Selected += RestartEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(restartGameMenuEntry);
            //MenuEntries.Add(quitGameMenuEntry);

            //Stop the hovering
           // Game1.SoundEngine.StopHover();
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        void ContinueToNextLevelSelected(object sender, PlayerIndexEventArgs e)
        {
            //LoadSaveManager.Load("Save2");
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void RestartEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
            //                                                new MainMenuScreen());
            foreach (GameScreen screen in ScreenManager.GetScreens())
                ScreenManager.RemoveScreen(screen);
            ScreenManager.AddScreen(new BackgroundScreen(), null);
            ScreenManager.AddScreen(new MainMenuScreen(), null);
        //    Game1.game.ExitGameComponents();
            LoadSaveManager.Load("Save2");
            LevelManager.skipFirstLine = true;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
           
           
        }

        #endregion
    }
}