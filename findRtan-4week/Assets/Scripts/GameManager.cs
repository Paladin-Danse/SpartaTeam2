using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public enum Diff
{
    Easy, Normal, Hard
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Text timeTxt;
    [SerializeField] Text endTxt;
    [SerializeField] GameObject card;
    [SerializeField] AudioClip start;
    [SerializeField] AudioClip match;
    [SerializeField] AudioClip missmatch;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip lose;
    [SerializeField] int cardCnt;
    [Header("# Notice")]
    [SerializeField] CardFlipNotice cardFlipNotice;

    [SerializeField] Diff diff;
    public Sprite[] sprites;    

    AudioSource audioSource;
    public AudioManager audioManager;
    GameObject firstCard = null;
    GameObject secondCard = null;
    GameObject cards;
    public GameObject tile;
    bool hasplayed = true;
    float time;
    int cardsLeft;//'������' �����ִ� ī��
    Dictionary<int, card> cardDictionary;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SetDifficult();
        cards = GameObject.Find("cards");
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(start, 0.5f);
        InitGame();
        Debug.Log(Difficulty.Diificulty);
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

    private void SetDifficult()
    {
        diff = (Diff)Difficulty.Diificulty;        
        switch (diff)
        {
            case Diff.Easy:
                Debug.Log("�������");
                cardCnt = 4;
                break;
            case Diff.Normal:
                Debug.Log("��ָ��");
                cardCnt = 16;
                break;
            case Diff.Hard:
                Debug.Log("�ϵ���");
                cardCnt = 36;
                break;
        }
    }

    public void isMatched()
    {
        bool isMatched = false;
        cardFlipNotice._tryCount++;

        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            isMatched = true;

            audioSource.PlayOneShot(match,0.5f);

            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();

            //ī�尡 ���������� 1���� �ð��� �ɸ���. ���� �� �常 ���Ҿ ������ ������ ������ �ƴϴ�.
            //�׷��Ƿ� ī�尡 �������� Ȯ���� �� ���� ī��� ������ �����ִ�.
            if (cardsLeft == 2)
            {
                Time.timeScale = 0;
                audioSource.PlayOneShot(win, 0.5f);
                audioManager.gameObject.SetActive(false);
                endTxt.gameObject.SetActive(true);

                cardFlipNotice.TryCountNotice();
            }
            cardsLeft -= 2;//���� ī�� �� ���� �տ� �θ� ���� �����ִ� �� ���� �����⵵ ���� ������ ����������.            
        }
        else
        {
            audioSource.PlayOneShot(missmatch, 0.5f);
            time += 0.5f; // �̺κ� �ٲٸ� ���� �г�Ƽ ����
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
        }
        cardFlipNotice.MatchCheck(isMatched, firstCard.GetComponent<card>().WhosCard);
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
        cardFlipNotice.ResetText();
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
        if (hasplayed)
            audioSource.PlayOneShot(lose, 0.25f);
        hasplayed = false;
        Time.timeScale = 0f;
        audioManager.gameObject.SetActive(false);
        endTxt.gameObject.SetActive(true);

        cardFlipNotice.TryCountNotice();
    }

    public void InitGame()
    {
        Time.timeScale = 1.0f;
        firstCard = secondCard = null;
        List<int> rtans = new List<int>();

        SpawnCards();

        /*
        //�迭 ���� �����ϰ� ����. List������ ����� �� ���� �� ����.
        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = cards.transform;
            newCard.GetComponent<card>().Setup(rtans[i]);

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);
            //newCard.transform.position = new Vector3(1.4f * (i % 4), -1.4f * (i / 4), 0);

            //string rtanName = "rtan" + rtans[i].ToString();
            //newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite
            //    = Resources.Load<Sprite>(rtanName);
        }
        */
        cardsLeft = cards.transform.childCount;
    }

    private void SpawnCards()
    {
        List<int> rtans = new List<int>();

        for (int i = 0; i < cardCnt / 2; i++)
        {
            int random = -1;
            while (true)
            {
                random = UnityEngine.Random.Range(0, sprites.Length);
                if (rtans.Any(x => x == random))
                    continue;

                break;
            }            

            rtans.Add(random);
            rtans.Add(random);
        }

        rtans = rtans.OrderBy(item => UnityEngine.Random.Range(-1.0f, 1.0f)).ToList();

        int raw = (int)Mathf.Sqrt(cardCnt);        
        for (int y = 0; y < raw; y++)
        {
            for (int x = 0; x < raw; x++)
            {
                GameObject newtile = Instantiate(tile);
                newtile.transform.parent = cards.transform;                
                card newCard = Instantiate(card).GetComponent<card>();
                newCard.transform.parent = newtile.transform;
                newCard.Setup(rtans[y * raw + x]);

                float offset = 1.5f;
                float startAnchor = offset * (raw / 2) - 0.75f;

                float posX = -startAnchor + offset * x;
                float posY = startAnchor - offset * y;

                newtile.transform.position = new Vector3(posX, posY, 0);
            }            
        }        
    }

    public void time20()
    {
        timeTxt.text = "<color=#FF4C33>" + timeTxt.text + "</color>";
    }
}