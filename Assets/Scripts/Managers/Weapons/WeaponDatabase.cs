using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewWeaponDatabase", menuName = "ScriptableObjects/Weapon Database", order = 1)]
public class WeaponDatabase : ScriptableObject
{
    public List<GameObject> weaponDatabase;
}