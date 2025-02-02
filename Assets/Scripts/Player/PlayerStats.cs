using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStats : MonoBehaviour
{
    [Header("HEALTH")]
    [SerializeField] private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float healthLossRate = 1f;
    
    [Header("OXYGEN")]
    [SerializeField] private float oxygen = 100f;
    public float Oxygen => oxygen;
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float oxygenLossRate = 1f;
    [SerializeField] private float oxygenGainRate = 2f;
    
    [Header("BUBBLE")]
    [SerializeField] private bool carryBubble = false;
    public bool CarryBubble => carryBubble;
    [SerializeField] private float invincibilityTime = 2f;
    [SerializeField] private bool invincible = false;

    [SerializeField] GameObject schermataMortePrefab;
    private GameObject schermataMorte;

    private bool _isInPause = false;

    // void Awake()
    // {
    //     EventManager.OnBubbleGrabbed += SetBubbleCarring;
    //     EventManager.OnBubbleThrown += SetBubbleCarring;
    //
    //     BarsUI.instance.SetMaxHealth(_maxHealth, GetComponent<PlayerInput>().playerIndex);
    //     BarsUI.instance.SetMaxOxygen(_maxOxygen, GetComponent<PlayerInput>().playerIndex);
    //     BarsUI.instance.SetExp((float)0, GetComponent<PlayerInput>().playerIndex);
    // }

    void Update()
    {
        if(_isInPause)
            return;

        if(!carryBubble && oxygen > 0)
        {
            oxygen -= oxygenLossRate * Time.deltaTime;
        }
        else if(carryBubble && oxygen < maxOxygen)
        {
            oxygen += oxygenGainRate * Time.deltaTime;
        }

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

    public void SetCarryBubble(bool isCarringBubble)
    {
        carryBubble = isCarringBubble;
    }

    public void TakeDamage(float damage)
    {
        if(invincible)
            return;

        health -= damage;

        //BarsUI.instance.SetHealth(_health, GetComponent<PlayerInput>().playerIndex);
        
        if (health <= 0)
        {
            //GameHUDmanager.instance.Die();
        }
        else
        {
            invincible = true;
            InvincibilityTimer().Forget();
        }
    }

    private async UniTask InvincibilityTimer()
    {
        await UniTask.WaitForSeconds(invincibilityTime, true);
        invincible = false;
    }


    private void SetBubbleCarring(GameObject player)
    {
        // Debug.Log("Player: " + player.name);
        if(player == gameObject)
        {
            // Debug.Log("TRUE Player: " + player.name);
            if(carryBubble)
                carryBubble = false;
            else
                carryBubble = true;
        }
    }

    public void Pause()
    {
        _isInPause = true;
    }

    public void Unpause()
    {
        _isInPause = false;
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
