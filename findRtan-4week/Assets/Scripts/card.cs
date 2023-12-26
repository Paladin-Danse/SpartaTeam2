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
    Animator anim;
    GameObject front;
    GameObject back;
    AudioSource audioSource;

    SpriteRenderer spriteRenderer;    

    [SerializeField] AudioClip flip;
    [SerializeField] AudioClip reFlip;
    bool isOpened = false;
    [SerializeField] Color notOpenColor = Color.white;
    [SerializeField] Color OpenedColor;

    public WhosCard WhosCard { get; private set; }

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
        spriteRenderer.sprite = GameManager.Instance.sprites[index];

        switch (index)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                WhosCard = WhosCard.Seungjun;
                break;
            case 4:
            case 5:
            case 6:
            case 7:
                WhosCard = WhosCard.Geon_o;
                break;
            case 8:
            case 9:
            case 10:
            case 11:
                WhosCard = WhosCard.Geonhyeong;
                break;
            case 12:
            case 13:
            case 14:
                WhosCard = WhosCard.Jiyoon;
                break;
            case 15:
            case 16:
            case 17:
                WhosCard = WhosCard.Ingyu;
                break;
        }
    }

    public void openCard()
    {
        audioSource.PlayOneShot(flip);

        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

        //checkFirst()�� firstCard�� ������ true�� ��ȯ.
        //�ݴ�� null���� ������ false
        //���� GameManager�� ���� firstCard�� ���ٸ�
        if(!GameManager.Instance.checkFirst())
        {
            GameManager.Instance.setFirst(gameObject);
        }
        //�ݴ�� firstCard�� �ִٸ�
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
    public void closeCard1()
    {
        closeCardInvoke();
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
        audioSource.PlayOneShot(reFlip, 0.5f);
    }
}
