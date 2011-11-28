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
        MovingShipAnimation,
        AttackingLaserAnimation,
        AttackingMissileAnimation,
        AttackingDisableWeaponAnimation,
        AttackingDisableEngineAnimation,
        CutScene,
    };
    public class Enumerations
    {
    }
}
