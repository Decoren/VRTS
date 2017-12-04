using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    //애니메이션, 이동
    private Transform enemyTr;
    private Animator anim;
    private NavMeshAgent nvAgent;
    public Transform firePos;
    private GameObject temp;


    struct Phase
    {
        public const int idle = 0;
        public const int move = 1;
        public const int found = 2;
        public const int fire = 3;
    }
    int phase = 0;
    
    public float collRate;
    
    //Fire Effect
    [SerializeField]private LineRenderer line;
    private RaycastHit hit;

    //거점을 중심으로 돌아다니게 하는 것 1
    public GameObject GP;
    private int dest = 0;
    private Transform[] GuardPost;
    public float guardTime = 2.0f;

    //몇초 기다리고 발사할지
    [SerializeField] private float waitTime = 3.0f;

    // Use this for initialization
    void Start () {
        enemyTr = this.gameObject.GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        anim = this.gameObject.GetComponent<Animator>();
        temp = new GameObject();
        GuardPost = GP.GetComponentsInChildren<Transform>();

        //Fire Effect
        line.useWorldSpace = false;
        line.enabled = false;
        line.startWidth = 0.1f;
        line.endWidth =0.01f;
        StartCoroutine(_Start());
	}

    IEnumerator _Start()
    {
        StartCoroutine(Patrol());
        bool wasRun = false;
        while (true)
        {
            switch (phase)
            {
                case Phase.idle:
                    anim.SetBool("IsRun", false);
                    nvAgent.updateRotation = false;
                    nvAgent.isStopped = true;
                    break;
                case Phase.move:
                    anim.SetBool("IsRun", true);
                    nvAgent.updateRotation = true;
                    nvAgent.isStopped = false;
                    break;
                case Phase.found:
                    nvAgent.isStopped = true;
                    wasRun = anim.GetBool("IsRun");;
                    anim.SetBool("IsRun", false);
                    yield return new WaitForSeconds(waitTime);
                    phase = Phase.fire;
                    break;
                case Phase.fire:
                    StartCoroutine(Fire(enemyTr.forward));
                    yield return new WaitForSeconds(2.0f);
                    if (wasRun == true)
                        phase = Phase.move;
                    else
                        phase = Phase.idle;
                    break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    //적인공지능이 해야할 것은 그냥 돌아다니다가, 시야앞에 있는 사람을 보면 쏜다.
    
    //거점 중심 정찰 1
    IEnumerator Patrol()
    {
        while (true)//!isDie)
        {
            if (phase == Phase.found || phase == Phase.fire)
            {    //적을 발견했거나 사격중이라면 정찰 멈춤.
                //nvAgent.isStopped = true;         // 간략화.
                yield return null;
                continue;
            }
            //0.2 초마다 실행
            yield return new WaitForSeconds(0.2f);
            
            nvAgent.SetDestination(GuardPost[(dest % (GuardPost.Length - 1)) + 1].position);
            phase = Phase.move;

            if (Vector3.Distance(enemyTr.position, GuardPost[(dest % (GuardPost.Length - 1)) + 1].position) < 0.2)
            {
                phase = Phase.idle;
                dest++;
                //transform.rotation = GuardPost[(dest % (GuardPost.Length - 1)) + 1].transform.rotation;  //it worked
                StartCoroutine(Turn(GuardPost[(dest % (GuardPost.Length - 1))].transform.rotation));
                yield return new WaitForSeconds(guardTime);
            }
        }
    }

    IEnumerator Fire(Vector3 vector3)
    {//0.2초 간격으로 5발 격발.
        for (int i = 0; i < 5; i++)
        {
            Vector3 ran = (Vector3.forward * Random.Range(-collRate, collRate))
                           + (Vector3.right * Random.Range(-collRate, collRate))
                           + (Vector3.up * Random.Range(-collRate, collRate));
            RaycastHit hit;
            Ray ray = new Ray(firePos.position, (firePos.forward + ran) * 10.0f);

            //Debug.DrawRay(firePos.position, (firePos.forward + ran) * 10.0f, Color.red);

            //이펙트 그리기
            line.SetPosition(0, firePos.InverseTransformPoint(ray.origin));

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                line.SetPosition(1, firePos.InverseTransformPoint(hit.point));
            }
            else
            {
                line.SetPosition(1, firePos.InverseTransformPoint(ray.GetPoint(100.0f)));
            }
            FireEffect();
            anim.SetTrigger("IsShoot");
            
            if (Physics.Raycast(firePos.position, firePos.forward + ran, out hit, 10.0f))
            {
                //맞을시 로직
                if (hit.collider.tag == "Player")
                {
                    //ray 에 맞은 객체에 sendMessege 를 통해 전달할 파라미터
                    object[] _params = new object[2];
                    _params[0] = hit.point;//맞은위치
                    _params[1] = 5;//줄 데미지
                    hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    //어찌됬건 코루틴 돌려서 사운드, 효과를 넘겨준 후, 여기서는 데미지 등 게임 로직을 만들어 준다. 위에꺼 설명이다.

    private void OnFound(Transform target)
    {
        //Quaternion rotvalue = Quaternion.FromToRotation(Vector3.up,vector3);
        //Debug.Log(rotvalue.ToString());
        //Turn(rotvalue);
        //transform.LookAt(Vector3.Lerp(transform.forward,vector3,);
        //transform.LookAt(vector3);
        StartCoroutine(Turn(target));
        phase = Phase.found;
    }


    private void FireEffect()
    {//혈흔효과 추가예정.

        StartCoroutine(ShowLaserBeam());
    }
    IEnumerator ShowLaserBeam()
    {
        line.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.02f, 0.04f));
        line.enabled = false;
    }

    IEnumerator Turn(Transform target)
    {
        //발견시 바로 쏘는 소스

        yield return null;

        /*
        //Debug.Log(target.transform.position.ToString());
        float inc = 0.01f;
        float smooth = 0.01f;
        //Debug.Log(transform.localToWorldMatrix.ToString());
        temp.transform.position = Vector3.zero;
        //temp.transform.position = transform.position;
        while ((phase == Phase.found))
        {
            temp.transform.LookAt(target);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, temp.transform.rotation, smooth);
            smooth += inc;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;*/
    }
    IEnumerator Turn(Quaternion quaternion)
    {
        float inc = 0.01f;
        float smooth = 0.01f;
        while (smooth <= 1)
        {

            transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, smooth);
            if(smooth != 1) smooth += inc;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
    
    public void OnEnd()
    {
        StopAllCoroutines();
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))//이후 VR 에서 발사.. 가 아니고 디버깅용으로 쓰고 발견시 발사로 변경.
        {
            phase = Phase.found;
        }
	}
}
