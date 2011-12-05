using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using EasyStorage;
using SpaceHaste;

namespace AvatarElementalBash.SaveLoad
{
    public class LoadSaveManager
    {
        static IAsyncSaveDevice saveDevice;
        public static int LevelNumber = 1;
        public static void Init()
        {
            // we can set our supported languages explicitly or we can allow the
            // game to support all the languages. the first language given will
            // be the default if the current language is not one of the supported
            // languages. this only affects the text found in message boxes shown
            // by EasyStorage and does not have any affect on the rest of the game.
            EasyStorageSettings.SetSupportedLanguages(Language.French, Language.Spanish);

            // on Windows Phone we use a save device that uses IsolatedStorage

            // create and add our SaveDevice
            SharedSaveDevice sharedSaveDevice = new SharedSaveDevice();
            Game1.game.Components.Add(sharedSaveDevice);

            // make sure we hold on to the device
            saveDevice = sharedSaveDevice;

            // hook two event handlers to force the user to choose a new device if they cancel the
            // device selector or if they disconnect the storage device after selecting it
            sharedSaveDevice.DeviceSelectorCanceled += (s, e) => e.Response = SaveDeviceEventResponse.Force;
            sharedSaveDevice.DeviceDisconnected += (s, e) => e.Response = SaveDeviceEventResponse.Force;

            // prompt for a device on the first Update we can
            sharedSaveDevice.PromptForDevice();

        
        }

        public static void Load(String SaveName)
        {
            if (saveDevice.FileExists("TestContainer", SaveName + ".txt"))
            {
                saveDevice.Load(
                     "TestContainer",
                         SaveName + ".txt",
                     stream =>
                     {
                         using (StreamReader reader = new StreamReader(stream))
                         {
                             LevelNumber = int.Parse(reader.ReadLine());
                         }
                     });
            }
            else
            {
                Save(SaveName);
                Load(SaveName);
            }
        }
  
        public static void Save(String SaveName)
        {
            // make sure the device is ready
            if (saveDevice.IsReady)
            {
                // save a file asynchronously. this will trigger IsBusy to return true
                // for the duration of the save process.
                saveDevice.SaveAsync(
                    "TestContainer",
                    SaveName + ".txt",
                    stream =>
                    {

                        using (StreamWriter writer = new StreamWriter(stream))
                            writer.WriteLine(""+2);

                    });
            }
        }
    }
}