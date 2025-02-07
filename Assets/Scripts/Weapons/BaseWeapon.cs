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
    public int tierCounter = 0;
    
    
    [Header("Audio")]
    [SerializeField] protected AudioClip shootClip;
    
    #endregion


    public virtual void Shoot()
    {
        //Debug.Log("shoot from BaseWeapon");
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
