using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WhosCard
{
    Seungjun,
    Geon_o,
    Geonhyeong,
    Jiyoon,
    Ingyu
}

public class card : MonoBehaviour
{
    public WhosCard WhosCard { get; private set; }

    Animator anim;
    GameObject front;
    GameObject back;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    [SerializeField] AudioClip flip;
    [SerializeField] Sprite[] sprites;
    bool isOpened = false;
    [SerializeField] Color notOpenColor = Color.white;
    [SerializeField] Color OpenedColor;

    private void Start()
    {
        anim = GetComponent<Animator>();
        front = transform.Find("front").gameObject;
        back = transform.Find("back").gameObject;
        back.GetComponent<SpriteRenderer>().color = notOpenColor;
        audioSource = GetComponent<AudioSource>();        

        isOpened = false;
    }

    public void Setup(int index)
    {
        spriteRenderer = transform.Find("front").GetComponent<SpriteRenderer>();
        switch (index)
        {
            case 0:
            case 1:
                WhosCard = WhosCard.Seungjun;                
                break;
            case 2:
            case 3:
                WhosCard = WhosCard.Geon_o;
                break;
            case 4:
            case 5:
                WhosCard = WhosCard.Geonhyeong;
                break;
            case 6:
                WhosCard = WhosCard.Jiyoon;
                break;
            case 7:
                WhosCard = WhosCard.Ingyu;
                break;
        }
        spriteRenderer.sprite = sprites[index];

        Debug.Log(index);
    }

    public void openCard()
    {
        audioSource.PlayOneShot(flip);

        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

        //checkFirst()는 firstCard가 있을때 true로 반환.
        //반대로 null값을 가지면 false
        //만약 GameManager가 가진 firstCard가 없다면
        if(!GameManager.Instance.checkFirst())
        {
            GameManager.Instance.setFirst(gameObject);
        }
        //반대로 firstCard가 있다면
        else
        {
            GameManager.Instance.setSecond(gameObject);
            GameManager.Instance.isMatched();
        }
    }
    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 1.0f);
    }

    private void destroyCardInvoke()
    {
        Destroy(gameObject);
    }
    public void closeCard()
    {
        Invoke("closeCardInvoke", 1.0f);
    }
    void closeCardInvoke()
    {
        if (isOpened == false)
        {
            back.GetComponent<SpriteRenderer>().color = OpenedColor;
            isOpened = true;
        }

        anim.SetBool("isOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }
}
