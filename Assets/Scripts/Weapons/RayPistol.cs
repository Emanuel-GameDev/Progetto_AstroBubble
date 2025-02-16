using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RayPistol : BaseWeapon
{
    #region Vars
    
    [Header("WeaponData")]
    
    [SerializeField, Min(0.1f)]
    protected float fireRate = 2f;
    
    #region ProjectileVars
    
    [Header("ProjectileData")]
    [SerializeField]
    private float projectileSpeed = 10f;

    [SerializeField]
    private int projectileDmg = 5;
    
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField, Tooltip("Every weapon that uses projectiles has it's own pool")]
    private int poolSize = 60;
    
    [SerializeField]
    private string poolName = "PistolPool";
    
    private GameObject _pistolProjectilePool;
    private List<GameObject> _projectilePool;
    private int _currentPoolIndex;
    
    #endregion
    
    #region UpgradeVars
    [Header("TIER 1 UPGRADE")]

    [SerializeField]
    private int tier1UpgradeDmg = 5;

    [SerializeField, Range(0, 100)]
    private float tier1UpgradeFireRate = 50;
    
    [Header("TIER 2 UPGRADE")]

    [SerializeField]
    private float timeBetweenShoots = 0.1f;
    
    #endregion
    
    private bool _inCooldown;
    
    #endregion
    
    public override void InitializeWeapon(PlayerWeaponHandler handler)
    {
        base.InitializeWeapon(handler);
        InitializePool();
        _inCooldown = false;
    }
    
    #region Projectile Functions
    private void InitializePool()
    {
        _pistolProjectilePool = new GameObject(poolName);
        _projectilePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            InstantiateProjectile();
        }
    }
    
    private void InstantiateProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, _pistolProjectilePool.transform);
        projectile.SetActive(false);
        _projectilePool.Add(projectile);
    }
    
    private GameObject GetPooledProjectile()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int index = (_currentPoolIndex + i) % poolSize;
            if (!_projectilePool[index].activeInHierarchy)
            {
                _currentPoolIndex = index;
                return _projectilePool[index];
            }
        }

        //Debug.Log("All projectile in the pool are active! increase poolSize!");
        return null;
    }
    
    #endregion

    public override void UpgradeTier()
    {
        base.UpgradeTier();

        if (tierCounter == 1)
        {
            projectileDmg += tier1UpgradeDmg;

            float percentage = tier1UpgradeFireRate / 100f;
            fireRate *= percentage;
        }
    }

    public override void Shoot(Vector2 direction, Transform sight)
    {
        base.Shoot(direction, sight);
        if (_inCooldown) return;

        GameObject projectile = GetPooledProjectile();
        if (!projectile) return;

        Fire(projectile, direction, sight.rotation);

        if (tierCounter == 2)
        {
            //StartCoroutine(CooldownGeneric(timeBetweenShoots));
        }
        
        _inCooldown = true;
        Cooldown(fireRate).Forget();
    }

    private void Fire(GameObject projectile, Vector2 direction, Quaternion rotation)
    {
        RayPistolProjectile pistolProjectile = projectile.GetComponent<RayPistolProjectile>();
        if (!pistolProjectile) return;
        
        pistolProjectile.BaseDmg = projectileDmg;
        projectile.transform.position = Handler.transform.position;
        projectile.SetActive(true);
        
        pistolProjectile.Fire(direction, projectileSpeed, rotation);
    }
    
    private async UniTaskVoid Cooldown(float cooldown)
    {
        await UniTask.Delay((int)cooldown * 1000);
        _inCooldown = false;
    }
}
