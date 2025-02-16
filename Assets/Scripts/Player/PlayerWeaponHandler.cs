using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private List<BaseWeapon> equippedWeapons;
    public List<BaseWeapon> EquippedWeapons => equippedWeapons;

    private Vector2 _shootingDirection;
    private Vector2 _lastShootingDirection;
    private List<Vector2> _activeDirections;
    private bool _canShoot;
    private GameObject _sight;

    private void Start()
    {
        _sight = GetComponentInParent<PlayerController>().SightObject;
        _activeDirections = new List<Vector2>();
    }

    public void EquipWeapon(BaseWeapon weapon)
    {
        equippedWeapons.Add(weapon);
        weapon.InitializeWeapon(this);
    }

    public void OnShoot(Vector2 direction)
    {
        if (equippedWeapons.Count <= 0) return;

        if (direction == Vector2.zero)
        {
            _canShoot = false;
        }
        else if (direction.x != 0 || direction.y != 0)
        {
            _shootingDirection = direction;
            _canShoot = true;
        }
    }

    private void Update()
    {
        if (!_canShoot) return;
        if (_shootingDirection == Vector2.zero) return;
        
        foreach (BaseWeapon weapon in equippedWeapons)
        {
            weapon.Shoot(_shootingDirection, _sight.transform);
        }
    }
    
    private bool IsValidDirection(Vector2 direction)
    {
        return (direction == Vector2.up || direction == Vector2.down || direction == Vector2.left || direction == Vector2.right);
    }
}
