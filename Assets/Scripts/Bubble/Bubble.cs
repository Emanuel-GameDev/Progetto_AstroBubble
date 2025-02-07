using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public bool isGrabbed;
    public bool isGrabbable = true;
    public CancellationTokenSource BubbleCancellationTokenSource;

    [SerializeField] private float speed = 10f;
    [SerializeField] private int time = 2;

    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private AnimationCurve throwCurve;

    private BubbleDestroyedEvent _bubbleDestroyedEvent;

    private void Start()
    {
        _bubbleDestroyedEvent = new BubbleDestroyedEvent();
        health = maxHealth;
    }

    public async UniTask ThrowTask(Vector2 direction, CancellationTokenSource token)
    {
        if (!isGrabbed) return;
        
        if(direction == Vector2.zero)
            direction = Vector2.right;

        var rb = GetComponent<Rigidbody2D>();
        isGrabbable = false;

        float elapsedTime = 0f;
        Vector2 initialVelocity = direction * speed;
        
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float curveValue = throwCurve.Evaluate(elapsedTime / time);

            rb.linearVelocity = initialVelocity * curveValue;
            
            if (token.IsCancellationRequested)
            {
                rb.linearVelocity = Vector2.zero;
                isGrabbable = true;
                return;
            }

            await UniTask.Yield();
        }

        rb.linearVelocity = Vector2.zero;
        isGrabbable = true;
    }

    public void TakeDamage(float damage)
    {
        if(isGrabbed || !isGrabbable)
            return;
            
        health -= damage;
        
        if (!(health <= 0)) return;
        
        EventBus.Raise<BubbleDestroyedEvent>(_bubbleDestroyedEvent);
        Destroy(gameObject);
    }


}
