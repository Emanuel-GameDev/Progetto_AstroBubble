using Cysharp.Threading.Tasks;
using UnityEngine;

public class BubblePointer : MonoBehaviour
{
    private Transform _target;

    void Update()
    {
        if (!_target) return;
        if (!gameObject.activeSelf) return;

        Vector3 direction = _target.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.localRotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    public void SetTarget(Transform t)
    {
        _target = t;
    }
    
    public void TriggerPointerActivation(bool mode)
    {
        gameObject.SetActive(mode);
    }

    /// <summary>
    /// Activates the GO after a certain time
    /// </summary>
    /// <param name="time"></param>
    public async UniTaskVoid ActivatePointer(float time)
    {
        await UniTask.Delay((int)(time * 1000));
        gameObject.SetActive(true);
    }
}
