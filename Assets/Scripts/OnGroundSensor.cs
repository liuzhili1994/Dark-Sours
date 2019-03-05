using Custom.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;
    private PlayerCtrl pc;

    private Vector3 point1;
    private Vector3 point2;
    private float radius;
    public Collider[] tempColliders;
    public float offset = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        pc = capcol.GetComponent<PlayerCtrl>();
        radius = capcol.radius - offset * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        point1 = transform.position + transform.up * (radius - offset);
        point2 = transform.position + transform.up * (capcol.height - offset) - transform.up * radius;
        tempColliders = Physics.OverlapCapsule(point1,point2,radius, 1 << LayerMask.NameToLayer("Ground"));

        if (tempColliders.Length != 0)
            pc.isGround = true;
        else
            pc.isGround = false;
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);
    }
}
