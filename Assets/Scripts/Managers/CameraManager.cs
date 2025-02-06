using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineTargetGroup targetGroup;
    
    private void Start()
    {
        if (targetGroup == null) return;
        
        List<PlayerInputHandler> inputs = PlayerConfigurationManager.Instance.PlayerInputHandlers; 

        for (int i = 0; i < inputs.Count; i++)
        {
            targetGroup.AddMember(inputs[i].gameObject.transform, 1f, 1f);
        }
    }
}
