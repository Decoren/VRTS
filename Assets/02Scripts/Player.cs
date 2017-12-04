using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float speed = 20;
    public float rotSpeed = 50;
    //private Transform tr;
    [SerializeField] private int hp=100;
    private int score = 100;

    public Text HPtext;
    public GameObject DeadUI;
    public Text DeadHP;

    float timer = 0;

    // Use this for initialization
    void Start () {
        DeadUI.active = false;
        HPtext.text = "HP : " + hp.ToString();
        //tr = this.gameObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
        HPtext.text = "HP : "+hp.ToString();
    }
    void FixedUpdate()
    {
        if(hp <= 0)
        {
            timer += Time.deltaTime;
            if(timer > 5.0)
            {
                Application.LoadLevel("Main");
            }
        }
    }

    void OnDamage(object[] _params)
    {
        if(hp > 0)
            hp -= (int)_params[1];
        if (hp <= 0)
        {
            PlayerDie();
        }
        //animator.SetTrigger("IsHit");
    }

    /*
    void OnEnd()
    {
        //점수 계산
        score = hp;
        
        //이후 ui 양식 받아서 만든다 생각 -> 내일 모레까지잖아 시붱.
    }
    */
    void PlayerDie()
    {
        DeadHP.text = DeadHP.text + "\nSCORE : " + hp.ToString();
        DeadUI.active = true;
    }
}
//세이프존 first