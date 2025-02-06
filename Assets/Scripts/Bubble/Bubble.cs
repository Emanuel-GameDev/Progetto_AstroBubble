using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public bool isGrabbed;
    public bool isGrabbable = true;

    [SerializeField] private float speed = 10f;
    [SerializeField] private int time = 2;

    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private AnimationCurve throwCurve;
    
    public CancellationTokenSource BubbleCancellationTokenSource;

    public async UniTask ThrowTask(Vector2 direction, CancellationTokenSource token)
    {
        if (!isGrabbed) return;
        
        //PubSub.Publish<GameObject, object>("BubbleThrown", player, null);
        
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
                Debug.Log("UniTask interrotta!");
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

        PubSub.Publish<object, object>("BubbleDestroyed", null, null);
        Destroy(gameObject);
    }


}
