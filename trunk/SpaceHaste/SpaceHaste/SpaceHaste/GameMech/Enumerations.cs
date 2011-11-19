using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceHaste.GameMech
{

    public enum ShipSelectionMode
    {
        Movement,
        Attack,
        Wait,
    };

    public enum ShipAttackSelectionMode
    {
        Laser,
        Missile,
    };
    public enum GameState
    {
        Other,
        StartBattle,
        SelectShipAction,
        SelectShipAttackAction,
        EnterShipAction,
        MovingShipAnimation,
        AttackingAnimation,
        CutScene,
    };
    public class Enumerations
    {
    }
}
