using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIselector : MonoBehaviour {

    public Vector3 screenCenter;
    public Image LoadingImage;
    private bool buttonFlag = false;
    public float timer = 0.0f;

    // Use this for initialization
    void Start () {
        //screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
	}

    // Update is called once per frame
    /*
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 300f))
        {
            if (hit.collider.CompareTag("UIEnd"))
            {
                LoadingImage.fillAmount += 1.0f / 3 * Time.deltaTime;
                if (LoadingImage.fillAmount == 1)
                    Application.LoadLevel("Menu");
            }
        }
        else
            LoadingImage.fillAmount = 0;
	}
    */
    void Update()
    {
        if (buttonFlag)
        {
            timer += 1.0f / 3 * Time.deltaTime;
            if (timer >= 1)
                Application.LoadLevel("Menu");
        }
        else
        {
            timer = 0.0f;
        }
    }

    // Pointer Exit에 추가
    public void OnTriggerExit()
    {
        buttonFlag = false;
    }

    public void OnTriggerEnter()
    {
        buttonFlag = true;
    }

    // Pointer Down에 추가
    public void OnTriggerDown()
    {
        buttonFlag = true;
    }

    // Pointer Up에 추가
    public void OnTriggerUp()
    {
        buttonFlag = true;
    }
}
