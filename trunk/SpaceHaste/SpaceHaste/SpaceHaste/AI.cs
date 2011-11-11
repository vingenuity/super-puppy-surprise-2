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
        public Tuple<GridCube, ShipSelectionMode> TakeTurn(List<GameObject> ships)
        {
            //Pop the active ship off of the list.
            GameObject myShip = ships[0];
            GameObject enemy = ClosestEnemy(myShip, ships);
            if (EnemiesLeft(ships) == 0)
                return new Tuple<GridCube, ShipSelectionMode>(myShip.GridLocation, ShipSelectionMode.Wait);
            if (Map.map.IsObjectInRange(myShip, enemy) && myShip.energy[0] < myShip.AttackEnergyCost)
            {
                return new Tuple<GridCube, ShipSelectionMode>(enemy.GridLocation, ShipSelectionMode.Attack);
            }
            else if (myShip.MovementEnergyCost * DistanceBetween(myShip, enemy) < myShip.energy[0])
                return new Tuple<GridCube, ShipSelectionMode>(enemy.GridLocation, ShipSelectionMode.Movement);
            else
                return new Tuple<GridCube, ShipSelectionMode>(myShip.GridLocation, ShipSelectionMode.Wait);
        }

        #region AI Considerations
        private GameObject ClosestEnemy(GameObject self, List<GameObject> ships)
        {
            float closestDistance = float.PositiveInfinity;
            GameObject closestShip = null;
            foreach (GameObject ship in ships)
            {
                if (ship.team == GameObject.Team.Player && DistanceBetween(self, ship) < closestDistance)
                {
                    closestDistance = DistanceBetween(self, ship);
                    closestShip = ship;
                }
            }
            return closestShip;
        }

        private float DistanceBetween(GameObject obj1, GameObject obj2)
        {
            return Vector3.Distance(obj1.GridPosition, obj2.GridPosition);
        }

        private int EnemiesLeft(List<GameObject> ships)
        {
            int enemies = 0;
            foreach (GameObject ship in ships)
                if (ship.team == GameObject.Team.Player) enemies++;
            return enemies;
        }

        private GameObject MostDamagedEnemy(List<GameObject> ships)
        {
            double leastHealth = double.PositiveInfinity;
            GameObject mostDamaged = null;
            foreach (GameObject ship in ships)
            {
                if (ship.team == GameObject.Team.Player && ship.hull[0] < leastHealth)
                {
                    leastHealth = ship.hull[0];
                    mostDamaged = ship;
                }
            }
            return mostDamaged;
        }
        #endregion
    }
}
