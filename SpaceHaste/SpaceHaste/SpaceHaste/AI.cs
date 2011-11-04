using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceHaste.GameMech;
using SpaceHaste.GameObjects;
using SpaceHaste.Maps;

namespace SpaceHaste
{
    /// <summary>
    /// This class represents the enemy that the player is trying to defeat.
    /// </summary>
    public class AI
    {
        //Map information for strategic use.
        private Map map;

        public AI(Map battlefield) { map = battlefield; }

        /// <summary>
        /// This is the function that does all of the work for the AI. 
        /// When called, the AI will look through its possible actions and determine what the best move for a ship to make is.
        /// This is called by GameMechanicsManager.
        /// </summary>
        /// <param name="ships">List of active ships on the battlefield. The AI will use this to decide how to move.</param>
        /// <returns>
        /// A tuple of GridCube and ShipSelectionMode that represents the actions the AI wishes to take. 
        /// This is used by GameMechanics to perform the AI's actions.
        /// </returns>
        public Tuple<GridCube, GameMechanicsManager.ShipSelectionMode> TakeTurn(List<GameObject> ships)
        {
            GameObject myShip = ships[0];
            if(myShip.Energy == 0)
                return new Tuple<GridCube, GameMechanicsManager.ShipSelectionMode>(myShip.GridLocation, GameMechanicsManager.ShipSelectionMode.Wait);
            GridCube location = myShip.GridLocation;
            location.X += 1;
            return new Tuple<GridCube, GameMechanicsManager.ShipSelectionMode>(location, GameMechanicsManager.ShipSelectionMode.Movement);
        }
    }
}
