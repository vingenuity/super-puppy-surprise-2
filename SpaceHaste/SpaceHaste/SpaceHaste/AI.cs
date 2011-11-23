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
        private Random rand;

        public AI(Map battlefield) { map = battlefield; rand = new Random(); }

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
        public Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode> TakeTurn(List<GameObject> ships)
        {
            //Pop the active ship off of the list.
            GameObject myShip = ships[0];
            GameObject enemy = ClosestEnemy(myShip, ships);
            if (EnemiesLeft(ships) == 0)
                return new Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode>(myShip.GridLocation, ShipSelectionMode.Wait, ShipAttackSelectionMode.Laser);
            if (Map.map.IsObjectInRange(myShip, enemy) && myShip.MissileCount > 0)
                return new Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode>(enemy.GridLocation, ShipSelectionMode.Attack, ShipAttackSelectionMode.Missile);
            if (Map.map.IsObjectInRange(myShip, enemy) && myShip.AttackEnergyCost < myShip.energy[0])
            {
                return new Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode>(enemy.GridLocation, ShipSelectionMode.Attack, ShipAttackSelectionMode.Laser);
            }
            List<GridCube> path = GetMovePath(myShip.GridLocation, enemy.GridLocation);
            if (myShip.energy[0] > myShip.MovementEnergyCost)
            {
                GridCube selection = null;
                for (int i = path.Count() - 1; i > 0; i--)
                {
                    if (i * myShip.MovementEnergyCost < myShip.energy[0] && !path[i].BlocksMovement())
                    {
                        selection = path[i];
                        break;
                    }
                }
                if(selection != null)
                    return new Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode>(selection, ShipSelectionMode.Movement, ShipAttackSelectionMode.Laser);

            }
            return new Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode>(myShip.GridLocation, ShipSelectionMode.Wait, ShipAttackSelectionMode.Laser);
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

        /// <summary>
        /// This function uses a version of A* to compute the best movement path to a cube.
        /// </summary>
        /// <param name="start">The cube in which we start moving.</param>
        /// <param name="finish">The cube that we want to reach.</param>
        /// <returns>List of GridCubes that is the best path to the finish.</returns>
        private List<GridCube> GetMovePath(GridCube start, GridCube finish)
        {
            List<GridCube> path = new List<GridCube>();
            List<GridCube> closedSet = new List<GridCube>();
            List<GridCube> openSet = new List<GridCube>();
            openSet.Add(start);
            bool guess_is_better = false;
            clearPath();

            start.g_score = 0f;
            start.h_score = Vector3.Distance(start.Position, finish.Position);
            start.f_score = start.g_score + start.h_score;

            while (openSet.Count() != 0)
            {
                openSet.Sort(new Comparison<GridCube>((x, y) => x.f_score.CompareTo(y.f_score)));
                GridCube currentCube = openSet[0];
                if (currentCube == finish)
                    return ReconstructPath(path, finish);
                openSet.RemoveAt(0);
                closedSet.Add(currentCube);
                foreach (GridCube neighbor in currentCube.ConnectedGridSquares)
                {
                    if (closedSet.Find(x => x == neighbor) != null || (neighbor.BlocksMovement() && neighbor != finish))
                        continue;
                    float g_guess = currentCube.g_score + Vector3.Distance(currentCube.Position, neighbor.Position);

                    if (openSet.Find(x => x == neighbor) == null)
                    {
                        openSet.Add(neighbor);
                        guess_is_better = true;
                    }
                    else if (g_guess < neighbor.g_score)
                        guess_is_better = true;
                    else
                        guess_is_better = false;
                    if (guess_is_better == true)
                    {
                        neighbor.came_from = currentCube;
                        neighbor.g_score = g_guess;
                        neighbor.h_score = Vector3.Distance(neighbor.Position, finish.Position);
                        neighbor.f_score = neighbor.g_score + neighbor.h_score;
                    }
                }
            }
            return null;
        }

        void clearPath()
        {
            foreach (GridCube cube in Map.map.MapGridCubes)
            {
                cube.came_from = null;
            }
        }
        List<GridCube> ReconstructPath(List<GridCube> path, GridCube current)
        {
            if (current.came_from != null)
            {
                List<GridCube> p = ReconstructPath(path, current.came_from);
                p.Add(current);
                return p;
            }
            else
                path.Add(current);
                return path;
        }
        #endregion

        public static bool IsSameAction(Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode> action1, Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode> action2)
        {
            if (action1.Item1 == action2.Item1 && action1.Item2 == action2.Item2 && action1.Item3 == action2.Item3)
                return true;
            else return false;
        }
    }
}
