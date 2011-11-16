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
    public enum GameState
    {
        Other,
        StartBattle,
        SelectShipAction,
        EnterShipAction,
        MovingShipAnimation,
        AttackingAnimation,
        CutScene,
    };
    public class Enumerations
    {
    }
}
