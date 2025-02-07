using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public WeaponDatabase database;

    private void Start()
    {
        //TEMP
        //Lista player
        List<PlayerInputHandler> players = PlayerConfigurationManager.Instance.PlayerInputHandlers;
        PlayerWeaponHandler weaponHandler = players[0].gameObject.GetComponent<PlayerWeaponHandler>();
        BaseWeapon weapon = database.weaponDatabase[0].GetComponent<BaseWeapon>();
        weaponHandler.EquipWeapon(weapon);
    }
}
