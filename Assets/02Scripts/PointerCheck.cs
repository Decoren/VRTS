using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Sprite ExitImage;
    public Sprite OverImage;

    Image m_Image;
    //public Image LoadingImage;
    private bool buttonFlag = false;
    public float timer = 0.0f;

    // Use this for initialization
    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    void Update()
    {
        if (buttonFlag)
        {
            timer += 1.0f / 3 * Time.deltaTime;
            if (timer >= 1)
                StageGo();
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

    public void StageOneSelect()
    {
        //LoadingImage.fillAmount += 1.0f / 3 * Time.deltaTime;
        //if (LoadingImage.fillAmount == 1)
        //Invoke("AMOLRANG", 2);
        StageGo();
    }
    void StageGo()
    {
        Application.LoadLevel("Main");
    }
    /*
    public void PointerOn()
    {
        m_Image.sprite = OverImage;
        m_Image.SetNativeSize();
    }
    public void PointerExit()
    {
        m_Image.sprite = ExitImage;
        m_Image.SetNativeSize();
    }*/

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Image.sprite = OverImage;
        m_Image.SetNativeSize();
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Image.sprite = ExitImage;
        m_Image.SetNativeSize();
    }
}
