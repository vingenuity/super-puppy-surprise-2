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

        //Strategic variables that are used every turn for evaluation in TakeTurn.
        GameObject myShip;
        GameObject enemy;
        GridCube bestFiringLocation;
        List<GridCube> path;
        Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode> lastAction;
        int frustration;

        public AI(Map battlefield) { map = battlefield; frustration = 0; }

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
            myShip = ships[0];
            Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode> action = Tuple.Create(myShip.GridLocation, ShipSelectionMode.Wait, ShipAttackSelectionMode.Laser);
            //If there aren't any enemies, we need to wait and do nothing or the game will freeze.
            if (EnemiesLeft(ships) == 0)
                return action;
            
            enemy = ClosestEnemy(myShip, ships);
            //If we have missiles, attempt to stay at missile range and pummel the enemy from there.
            if (myShip.MissileCount > 0)
            {
                if (myShip.MissileCount * myShip.dmg[1] > enemy.hull[0] && DistanceBetween(myShip, enemy) <= myShip.MissileRange)
                    action = Tuple.Create(enemy.GridLocation, ShipSelectionMode.Attack, ShipAttackSelectionMode.Missile);
                else if (DistanceBetween(myShip, enemy) != myShip.MissileRange)
                {
                    bestFiringLocation = ClosestCubeAtDistanceFrom(myShip, enemy, myShip.MissileRange);
                    path = GetMovePath(myShip.GridLocation, bestFiringLocation);
                    GridCube selection = MoveMaxAlongPath(myShip, path);
                    if (selection != null)
                        action = Tuple.Create(selection, ShipSelectionMode.Movement, ShipAttackSelectionMode.Missile);
                    else
                        action = Tuple.Create(myShip.GridLocation, ShipSelectionMode.Wait, ShipAttackSelectionMode.Missile);
                }
                else
                    action = Tuple.Create(enemy.GridLocation, ShipSelectionMode.Attack, ShipAttackSelectionMode.Missile);
            }
            //If not, if we can kill the enemy with lasers without a return volley, close in and fire; otherwise take a quick shot and evasive action.
            else
            {
                if (MaxDamageThisTurn(myShip, enemy) > enemy.hull[0] && Map.map.IsObjectInRange(myShip, enemy))
                    action = Tuple.Create(enemy.GridLocation, ShipSelectionMode.Attack, ShipAttackSelectionMode.Laser);
                else
                {
                    path = GetMovePath(myShip.GridLocation, enemy.GridLocation);
                    bestFiringLocation = FindKillCube(myShip, path);
                    if (bestFiringLocation != null)
                        action = Tuple.Create(bestFiringLocation, ShipSelectionMode.Movement, ShipAttackSelectionMode.Laser);
                    else
                    {
                        if(lastAction != Tuple.Create(enemy.GridLocation, ShipSelectionMode.Attack, ShipAttackSelectionMode.Laser))
                            action = Tuple.Create(enemy.GridLocation, ShipSelectionMode.Attack, ShipAttackSelectionMode.Laser);
                        int enemyHighDamageRadius = (int)Math.Floor(100 / enemy.MovementEnergyCost);
                        if (DistanceBetween(myShip, enemy) <= enemyHighDamageRadius)
                        {
                            path = FindEvasionPath(myShip, enemy);
                            if (myShip.energy[0] > myShip.MovementEnergyCost)
                            {
                                GridCube selection = MoveMaxAlongPath(myShip, path);
                                if (selection != null)
                                    action = Tuple.Create(selection, ShipSelectionMode.Movement, ShipAttackSelectionMode.Laser);
                            }
                            else
                                action = Tuple.Create(myShip.GridLocation, ShipSelectionMode.Wait, ShipAttackSelectionMode.Laser);
                        }
                        else
                        {
                            GridCube bestLocation = ClosestCubeAtDistanceFrom(myShip, enemy, enemyHighDamageRadius + 1);
                            path = GetMovePath(myShip.GridLocation, bestLocation);
                            if (myShip.energy[0] > myShip.MovementEnergyCost)
                            {
                                GridCube selection = MoveMaxAlongPath(myShip, path);
                                if (selection != null)
                                    action = Tuple.Create(selection, ShipSelectionMode.Movement, ShipAttackSelectionMode.Laser);
                            }
                            else
                                action = Tuple.Create(myShip.GridLocation, ShipSelectionMode.Wait, ShipAttackSelectionMode.Laser);
                        }
                    }
                }
            }

            //If we perform the same action 5 times in a row, we must be performing an illegal action and the game is stopping us, so wait instead.
            if (lastAction != action)
            {
                lastAction = action;
                frustration = 0;
            }
            else if (frustration < 5) frustration++;
            else action = Tuple.Create(myShip.GridLocation, ShipSelectionMode.Wait, ShipAttackSelectionMode.Laser);
            return action;
        }

        #region AI Helper Functions
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

        /// <summary>
        /// This function finds the closest cube at a set distance from a target.
        /// This is used by the AI to find the best cubes to fire from maximum missile range.
        /// </summary>
        /// <param name="self">The playing ship</param>
        /// <param name="target">The Game Object we want to stay at range from.</param>
        /// <param name="distance">The distance to stay at.</param>
        /// <returns>The best gridcube meeting the qualifications.</returns>
        private GridCube ClosestCubeAtDistanceFrom(GameObject self, GameObject target, int distance)
        {
            Vector3 center = target.GridPosition;
            List<Vector3> possibles = new List<Vector3>();
            for (int i = 0; i <= distance; i++)
            {
                for (int j = 0; j <= distance; j++)
                {
                    if (i + j > distance)
                        continue;
                    for (int k = 0; k <= distance; k++)
                    {
                        int sum = i + j + k;
                        if (sum == distance)
                            possibles.Add(new Vector3(center.X + i, center.Y + j, center.Z + k));
                        else if (sum > distance)
                            continue;
                    }
                }
            }
            float closestDistance = 100f;
            Vector3 closestLoc = new Vector3();
            foreach (Vector3 loc in possibles)
            {
                if (Vector3.Distance(loc, self.GridPosition) < closestDistance)
                {
                    closestDistance = Vector3.Distance(loc, self.GridPosition);
                    closestLoc = loc;
                }
            }
            return Map.map.GetCubeAt(closestLoc);
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

        GridCube MoveMaxAlongPath(GameObject ship, List<GridCube> path)
        {
            GridCube selection = null;
            for (int i = path.Count() - 1; i > 0; i--)
            {
                if (i * ship.MovementEnergyCost < ship.energy[0] && !path[i].BlocksMovement())
                {
                    selection = path[i];
                    break;
                }
            }
            return selection;
        }

        //This function attempts to find a cube to fire upon that will kill the player before they can return fire.
        //If no such path is found, it returns null.
        GridCube FindKillCube(GameObject ship, List<GridCube> path)
        {
            double originalEnergy = ship.energy[0];
            Vector3 originalPosition = ship.GridPosition;
            GridCube selection = null;
            GameObject target = path[path.Count - 1].GetObject();
            for (int i = path.Count() - 1; i > 0; i--)
            {
                ship.GridPosition = path[i].Position;
                ship.energy[0] = originalEnergy - i * ship.MovementEnergyCost;
                if (!path[i].BlocksMovement() && MaxDamageThisTurn(ship, target) > target.hull[0])
                {
                    selection = path[i];
                    break;
                }
            }
            ship.GridPosition = originalPosition;
            ship.energy[0] = originalEnergy;
            return selection;
        }

        //This function finds the maximum damage we can possibly do from our square to an enemy using lasers.
        int MaxDamageThisTurn(GameObject self, GameObject target)
        {
            return (int) Math.Floor(self.energy[0] / self.AttackEnergyCost) * self.GetLaserDamage(target);
        }

        /// <summary>
        /// This function finds the best path to take to achieve maximum distance from an enemy.
        /// It is used by the AI to run away from battles that it doesn't think it can win.
        /// </summary>
        /// <param name="self">The playing Game Object.</param>
        /// <param name="enemy">The Game Object to run from.</param>
        /// <returns>A list of GridCubes representing the optimal running path.</returns>
        List<GridCube> FindEvasionPath(GameObject self, GameObject enemy)
        {
            Vector3 awayVector = 2 * (self.GridPosition - enemy.GridPosition);
            if (awayVector.X < 0) awayVector.X = 0;
            if (awayVector.Y < 0) awayVector.Y = 0;
            if (awayVector.Z < 0) awayVector.Z = 0;
            if (awayVector.X > Map.map.Size.X) awayVector.X = Map.map.Size.X;
            if (awayVector.Y > Map.map.Size.Y) awayVector.Y = Map.map.Size.Y;
            if (awayVector.Z > Map.map.Size.Z) awayVector.Z = Map.map.Size.Z;
            return GetMovePath(self.GridLocation, Map.map.GetCubeAt(awayVector));
        }
        #endregion
    }
}
