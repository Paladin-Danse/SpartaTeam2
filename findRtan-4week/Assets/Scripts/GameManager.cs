using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Text timeTxt;
    [SerializeField] Text endTxt;
    [SerializeField] GameObject card;
    [SerializeField] AudioClip match;
    AudioSource audioSource;
    GameObject firstCard = null;
    GameObject secondCard = null;
    GameObject cards;
    float time;
    int cardsLeft;//'������' �����ִ� ī��
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        cards = GameObject.Find("cards");
        audioSource = GetComponent<AudioSource>();
        InitGame();
    }

    void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");
        if (time >= 20.0f)
        {
            time20();
        }



        if (time >= 30.0f)
        {

            GameEnd();
        }
        
    }

    public void isMatched()
    {
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if(firstCardImage == secondCardImage)
        {
            audioSource.PlayOneShot(match);

            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();
            
            //ī�尡 ���������� 1���� �ð��� �ɸ���. ���� �� �常 ���Ҿ ������ ������ ������ �ƴϴ�.
            //�׷��Ƿ� ī�尡 �������� Ȯ���� �� ���� ī��� ������ �����ִ�.
            if(cardsLeft == 2)
            {
                Time.timeScale = 0;
                endTxt.gameObject.SetActive(true);
            }
            cardsLeft -= 2;//���� ī�� �� ���� �տ� �θ� ���� �����ִ� �� ���� �����⵵ ���� ������ ����������.            
        }
        else
        {
            Time.timeScale++;
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
        }
        firstCard = secondCard = null;
    }
    //firstCard�� ���� ����ִ°�? = ù��°ī�带 ���� ���³�?
    public bool checkFirst()
    {
        if (firstCard != null) return true;
        else return false;
    }
    //ù��°ī�带 ����
    public void setFirst(GameObject obj)
    {
        firstCard = obj;
    }
    //���� �����ϰ� �ι�°ī�� üũ
    public bool checkSecond()
    {
        if (secondCard != null) return true;
        else return false;
    }
    public void setSecond(GameObject obj)
    {
        secondCard = obj;
    }
    void GameEnd()
    {
        Time.timeScale = 0f;
        endTxt.gameObject.SetActive(true);
    }

    public void InitGame()
    {
        Time.timeScale = 1.0f;
        firstCard = secondCard = null;
        //�迭 ���� �����ϰ� ����. List������ ����� �� ���� �� ����.
        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = cards.transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);
            //newCard.transform.position = new Vector3(1.4f * (i % 4), -1.4f * (i / 4), 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite
                = Resources.Load<Sprite>(rtanName);
        }
        cardsLeft = cards.transform.childCount;
    }
    public void time20()
    {

        timeTxt.text = "<color=#FF4C33>" + timeTxt.text + "</color>";



    }
}
