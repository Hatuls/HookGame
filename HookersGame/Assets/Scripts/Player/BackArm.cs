using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackArm : MonoBehaviour
{
    public Transform _grappleSource;
    internal Quaternion startRot;
    private void Start()
    {
        startRot = transform.rotation;
    }
}
