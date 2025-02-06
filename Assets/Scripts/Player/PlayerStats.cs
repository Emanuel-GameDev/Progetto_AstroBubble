using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("HEALTH")]
    [SerializeField] private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float healthLossRate = 1f;
    
    [Header("DAMAGE")]
    [SerializeField, Tooltip("Player is invincible for a certain amount of time after being hit")]
    private float invincibilityTime = 2f;
    private bool _invincible;
    
    [Header("OXYGEN")]
    [SerializeField] private float oxygen = 100f;
    public float Oxygen => oxygen;
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float oxygenLossRate = 1f;
    [SerializeField] private float oxygenGainRate = 2f;
    
    
    private bool _isCarryingBubble;
    private bool _isInPause;

    private void Start()
    {
//     BarsUI.instance.SetMaxHealth(_maxHealth, GetComponent<PlayerInput>().playerIndex);
//     BarsUI.instance.SetMaxOxygen(_maxOxygen, GetComponent<PlayerInput>().playerIndex);
//     BarsUI.instance.SetExp((float)0, GetComponent<PlayerInput>().playerIndex);
    }
    

    void Update()
    {
        if(_isInPause)
            return;
        
        // Oxygen variation over time
        if(!_isCarryingBubble && oxygen > 0)
        {
            oxygen -= oxygenLossRate * Time.deltaTime;
        }
        else if(_isCarryingBubble && oxygen < maxOxygen)
        {
            oxygen += oxygenGainRate * Time.deltaTime;
        }
        
        // Health decrease over time
        if(oxygen <= 0)
        {
            health -= healthLossRate * Time.deltaTime;
            //BarsUI.instance.SetHealth(_health, GetComponent<PlayerInput>().playerIndex);
        }

        if (health <= 0)
        {
            //GameHUDmanager.instance.Die();
        }

        //BarsUI.instance.SetOxygen(_oxygen, GetComponent<PlayerInput>().playerIndex);
    }

    public void SetCarryingBubble(bool value)
    {
        _isCarryingBubble = value;
    }

    public void TakeDamage(float damage)
    {
        if(_invincible)
            return;

        health -= damage;

        //BarsUI.instance.SetHealth(_health, GetComponent<PlayerInput>().playerIndex);
        
        if (health <= 0)
        {
            //GameHUDmanager.instance.Die();
        }
        else
        {
            _invincible = true;
            InvincibilityTimer().Forget();
        }
    }

    private async UniTask InvincibilityTimer()
    {
        await UniTask.WaitForSeconds(invincibilityTime, true);
        _invincible = false;
    }
    
    public void TakeOxygen(float damage)
    {
        oxygen -= damage;
        if(oxygen < 0)
        {
            oxygen = 0;
        }
    }

    public void RegenerateHealth(float amount)
    {
        health += amount;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void RiseOxygenGainRate(int amount)
    {
        oxygenGainRate += oxygenGainRate * amount / 100f;
    }
}
