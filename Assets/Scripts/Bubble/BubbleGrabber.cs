using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BubbleGrabber : MonoBehaviour
{
    [SerializeField] private float stopOnThrowDuration = .2f;
    public float StopOnThrowDuration => stopOnThrowDuration;
    
    private Bubble _bubbleCarried;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out Bubble bubble))
        {
            if(bubble.isGrabbed
               || !bubble.isGrabbable) 
                return;

            _bubbleCarried = bubble;

            _bubbleCarried.isGrabbed = true;
            _bubbleCarried.isGrabbable = false;
            
            if (bubble.BubbleCancellationTokenSource != null)
            {
                _bubbleCarried.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                bubble.BubbleCancellationTokenSource.Cancel();
                bubble.BubbleCancellationTokenSource = null;
            }

            _bubbleCarried.gameObject.transform.SetParent(transform.parent, false);
            _bubbleCarried.gameObject.transform.localPosition = Vector3.zero;

            //PubSub.Publish<GameObject, object>("BubbleGrabbed", transform.parent.gameObject, null);
        }
    }

    public void ThrowBubble(Vector2 direction)
    {
        if(_bubbleCarried == null) return;
        
        _bubbleCarried.transform.parent = null;
        _bubbleCarried.BubbleCancellationTokenSource = new CancellationTokenSource();
        _bubbleCarried.ThrowTask(direction, _bubbleCarried.BubbleCancellationTokenSource).Forget();
        
        _bubbleCarried.isGrabbed = false;
        _bubbleCarried.isGrabbable = true;
        
        _bubbleCarried = null;
    }
}
