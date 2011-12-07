#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using SpaceHaste;
using AvatarElementalBash.SaveLoad;

#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Space Haste")
        {
            // Create our menu entries.
            //Game1.SoundEngine.TurnSoundOn(ConstantSounds.MenuBackground);
            MenuEntry newGameMenuEntry = new MenuEntry("New Game");
            MenuEntry continueGameMenuEntry = new MenuEntry("Continue Game");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry controlsMenuEntry = new MenuEntry("Controls");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            newGameMenuEntry.Selected += NewGameMenuEntrySelected;
            continueGameMenuEntry.Selected += ContinueGameMenuEntrySelected;
            controlsMenuEntry.Selected += ControlsMenuEntrySelected;
            creditsMenuEntry.Selected += CreditsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(continueGameMenuEntry);
            MenuEntries.Add(newGameMenuEntry);
            MenuEntries.Add(controlsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
            //Game1.game.PauseGameComponents();
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the New Game menu entry is selected.
        /// </summary>
        void NewGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
           // Game1.SoundEngine.TurnSoundOff(ConstantSounds.MenuBackground);
            LoadSaveManager.NewSave("Save2");
            LoadSaveManager.Load("Save2");
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }

        /// <summary>
        /// Event handler for when the Continue Game menu entry is selected.
        /// </summary>
        void ContinueGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadSaveManager.Load("Save2");
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void ControlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
           ScreenManager.AddScreen(new CreditsScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you wish to exit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
