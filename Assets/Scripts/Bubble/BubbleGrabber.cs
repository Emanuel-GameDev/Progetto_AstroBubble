using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BubbleGrabber : MonoBehaviour
{
    [SerializeField] private float stopOnThrowDuration = .2f;
    public float StopOnThrowDuration => stopOnThrowDuration;
    
    private Bubble _bubbleCarried;
    
    private BubbleGrabbedEvent _bubbleGrabbedEvent;

    private void Start()
    {
        _bubbleGrabbedEvent = new BubbleGrabbedEvent();
    }

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
                bubble.BubbleCancellationTokenSource.Cancel();
                bubble.BubbleCancellationTokenSource = null;
            }

            _bubbleCarried.gameObject.transform.SetParent(transform.parent, false);
            _bubbleCarried.gameObject.transform.localPosition = Vector3.zero;

            _bubbleGrabbedEvent.Parent = transform.parent.gameObject;
            GetComponentInParent<PlayerStats>().SetCarryingBubble(true);
        }
    }

    public void ThrowBubble(Vector2 direction)
    {
        if(_bubbleCarried == null) return;
        
        _bubbleCarried.transform.parent = null;
        GetComponentInParent<PlayerStats>().SetCarryingBubble(false);
        
        _bubbleCarried.BubbleCancellationTokenSource = new CancellationTokenSource();
        _bubbleCarried.ThrowTask(direction, _bubbleCarried.transform.parent?.gameObject, _bubbleCarried.BubbleCancellationTokenSource).Forget();
        
        _bubbleCarried.isGrabbed = false;
        _bubbleCarried.isGrabbable = true;
        
        _bubbleCarried = null;
    }
}
