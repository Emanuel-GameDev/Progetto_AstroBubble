using Cysharp.Threading.Tasks;
using UnityEngine;

public class RayPistolProjectile : MonoBehaviour
{
    [SerializeField] private float aliveTime = 3f;
    public float BaseDmg { get; set; }
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 direction, float speed, Quaternion rotation)
    {
        if (!_rb) return;
        
        transform.rotation = rotation;
        _rb.linearVelocity = direction * speed;

        AliveTimer().Forget();
    }

    private async UniTaskVoid AliveTimer()
    {
        await UniTask.Delay((int)aliveTime * 1000);
        await UniTask.WaitForFixedUpdate(); 
        
        _rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);    
    }
}
