using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelEntranceTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            TunnelManager.Instance.EnterTunnelStage();
    }
}
