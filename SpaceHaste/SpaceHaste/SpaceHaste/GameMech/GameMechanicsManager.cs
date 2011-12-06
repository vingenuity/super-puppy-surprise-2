using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.GameObjects;
using SpaceHaste.Maps;
using SpaceHaste.Primitives;
using SpaceHaste.Huds;
using SpaceHaste.Sounds;
using GameStateManagement;
using SpaceHaste.GameMech.BattleMechanicsManagers;
using SpaceHaste.GameMech.LevelManagers;

namespace SpaceHaste.GameMech
{
    public class GameMechanicsManager : GameComponent
    {
        public static List<GameObject> GameObjectList;
        public static List<SuperTerrain> SuperTerrainList;

        public static GameState gamestate;
        public BattleMechanicsManager BattleManager;

        //For controls, we need a singleton
        public static GameMechanicsManager MechMan;
        public static LevelManager LevelManager;

        public GameMechanicsManager(Game g)
            : base(g)
        {
            MechMan = this;
            GameObjectList = new List<GameObject>();
            SuperTerrainList = new List<SuperTerrain>();
            BattleManager = new BattleMechanicsManager();
            LevelManager = new LevelManager();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            BattleManager.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
