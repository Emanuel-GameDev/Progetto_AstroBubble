using UnityEngine;
using UnityEngine.Serialization;

public class PlayerLevelHandler : MonoBehaviour
{
    [SerializeField] private int level = 1;
    public int Level => level;
    [SerializeField] private float actualExp = 0f;
    public float ActualExp => actualExp;
    [SerializeField] private float grabExpRange = 1f;

    
    private float TrueLevelUpThreshold => levelUpThreshold * level * thresholdMultiplayer;
    [SerializeField] private float levelUpThreshold = 300f;
    [SerializeField] private float thresholdMultiplayer = 1f;

    private CircleCollider2D _collider;
    
    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        // EventManager.OnPlayerLevelUp += (level, manager) => { BarsUI.instance.SetExp((float)0, GetComponentInParent<PlayerInput>().playerIndex);
        //                                                       BarsUI.instance.SetMaxExp((float)_trueLevelUpThreshold, GetComponentInParent<PlayerInput>().playerIndex);
        // };
        // BarsUI.instance.SetMaxExp((float)_trueLevelUpThreshold, GetComponentInParent<PlayerInput>().playerIndex);
    }

    public void AddExp(float expToAdd)
    {
        actualExp += expToAdd;

        //BarsUI.instance.SetExp(_actualExp, GetComponentInParent<PlayerInput>().playerIndex);


        if (actualExp >= TrueLevelUpThreshold)
        {
            level++;
            actualExp = 0f;

            //if (EventManager.OnPlayerLevelUp != null)
            //{
            //    Debug.Log($"Registered methods: {EventManager.OnPlayerLevelUp.GetInvocationList().Length}");
            //}

            //EventBus.Raise<PlayerLevelUpEvent>();

            //SI lo so che � sbagliato perch� c'� l'evento sopra
            //GameHUDmanager.instance.UpdateLvlText(this, _level);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if(other.gameObject.TryGetComponent(out ExpItem expItem))
        // {
        //     AddExp(expItem.EXPValue);
        //     Destroy(expItem.gameObject);
        // }
    }

    public void ChangeGrabRange()
    {
        _collider.radius = grabExpRange;
    }
}
