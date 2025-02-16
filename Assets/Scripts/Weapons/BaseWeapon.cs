using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    #region vars
    
    [Header("Generics")]
    public string weaponName;
    public string description;
    public string tier1Description;
    public string tier2Description;
    public Sprite weaponSprite;
    public int tierCounter;
    
    
    [Header("Audio")]
    [SerializeField] protected AudioClip shootClip;
    
    protected PlayerWeaponHandler Handler;
    
    #endregion


    public virtual void Shoot(Vector2 direction, Transform sight) {}

    public virtual void InitializeWeapon(PlayerWeaponHandler handler)
    {
        Handler = handler;
    }

    public virtual void UpgradeTier()
    {
        if (tierCounter < 3)
        {
            tierCounter++;
            
            //TODO: if max tier remove from database
        }
    }
}
