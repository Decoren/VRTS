using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Detect : MonoBehaviour
{
    public Transform tr;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(tr.position, viewRadius, targetMask);//Collider 의 집합 물리 오버랩 스피어 해서 다 잡아옴(단 targetmask 인 player 안에서만))

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - tr.position).normalized;
            if (Vector3.Angle(tr.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(tr.position, target.position);

                if (!Physics.Raycast(tr.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    SendMessage("OnFound", visibleTargets[0], SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void Update()
    {

    }
}