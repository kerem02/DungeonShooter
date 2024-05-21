using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    public Transform target;
    private Vector3 vell = Vector3.zero;
    

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(target != null){
            Vector3 targetPosition = target.position + offset;
            targetPosition.z = transform.position.z;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition,ref vell, damping);
        }
    }
}
