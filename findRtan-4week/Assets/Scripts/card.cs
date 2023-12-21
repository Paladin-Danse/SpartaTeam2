using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    Animator anim;
    GameObject front;
    GameObject back;
    AudioSource audioSource;
    [SerializeField] AudioClip flip;

    private void Start()
    {
        anim = GetComponent<Animator>();
        front = transform.Find("front").gameObject;
        back = transform.Find("back").gameObject;
        audioSource = GetComponent<AudioSource>();
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
    void closeCardInvoke()
    {
        anim.SetBool("isOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }
}
