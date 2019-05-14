using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpecs 
{
    public enum ShootMode { AUTO, SINGLE };
    public enum WeaponType { PISTOL, MELEE, SHOTGUN, MACHINE_GUN, ASSAULT_RIFLE, LONG_RANGE_RIFLE, SPEAR, GRENADE, SPECIAL}
    public enum WeaponAmmoType { BULLET, THROWABLE, NONE }
    public enum AimMode { NONE, SELF_AIM, AIM }
}
