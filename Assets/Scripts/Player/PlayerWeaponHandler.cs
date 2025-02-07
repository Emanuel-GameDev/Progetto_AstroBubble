using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private List<BaseWeapon> equippedWeapons;
    public List<BaseWeapon> EquippedWeapons => equippedWeapons;

    public void EquipWeapon(BaseWeapon weapon)
    {
        equippedWeapons.Add(weapon);
    }

    public void OnShoot(Vector2 direction)
    {
        foreach (BaseWeapon weapon in equippedWeapons)
        {
            weapon.Shoot(direction);   
        }
    }
}
