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
        TargetWeapon,
        TargetEngine,
    };
    public enum GameState
    {
        Other,
        StartBattle,
        SelectShipAction,
        SelectShipAttackAction,
        EnterShipAction,
        AITurnAnimation,
        MovingShipAnimation,
        AttackingLaserAnimation,
        AttackingMissileAnimation,
        AttackingDisableWeaponAnimation,
        AttackingDisableEngineAnimation,
        CutScene,
        Exiting,
    };
    public class Enumerations
    {
    }
}
