using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public enum Diff
{
    Easy, Normal, Hard
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Text timeTxt;
    [SerializeField] Text endTxt;
    [SerializeField] Text easyBestTimeTxt;
    [SerializeField] Text normalBestTimeTxt;
    [SerializeField] Text hardBestTimeTxt;
    [SerializeField] GameObject card;
    [SerializeField] AudioClip start;
    [SerializeField] AudioClip match;
    [SerializeField] AudioClip missmatch;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip lose;
    [SerializeField] int cardCnt;
    [Header("# Notice")]
    [SerializeField] CardFlipNotice cardFlipNotice;
    float time1 = 0;
    [SerializeField] Diff diff;
    public Sprite[] sprites;
    public GameObject endPanel;
    AudioSource audioSource;
    public AudioManager audioManager;
    GameObject firstCard = null;
    GameObject secondCard = null;
    GameObject cards;
    public GameObject tile;
    public Text ScoreTxt;
    bool hasplayed = true;
    float time;
    float bestscore = 0;

    [SerializeField] GameObject Easy;
    [SerializeField] GameObject Normal;
    [SerializeField] GameObject Hard;

    //float bestTime = 30f;
    //float easyBestTime = 30f;
    //float normalBestTime = 30f;
    //float hardBestTime = 30f;

    int cardsLeft;//'실제로' 남아있는 카드
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
        switch (diff)
        {
            case Diff.Easy:
                if(PlayerPrefs.HasKey("easyBestTime") == true)
                    easyBestTimeTxt.text = "최단 시간 : " + PlayerPrefs.GetFloat("easyBestTime").ToString("N2") + "초";
                else
                    easyBestTimeTxt.text = "없음";
                break;
            case Diff.Normal:
                if (PlayerPrefs.HasKey("normalBestTime") == true)
                    normalBestTimeTxt.text = "최단 시간 : " + PlayerPrefs.GetFloat("normalBestTime").ToString("N2") + "초";
                else
                    normalBestTimeTxt.text = "없음";
                break;
            case Diff.Hard:
                if (PlayerPrefs.HasKey("hardBestTime") == true)
                    hardBestTimeTxt.text = "최단 시간 : " + PlayerPrefs.GetFloat("hardBestTime").ToString("N2") + "초";
                else
                    hardBestTimeTxt.text = "없음";
                break;
        }
        //bestTimeTxt.text = "최단 시간 : " + PlayerPrefs.GetFloat("bestTime").ToString("N2") + "초";

        time += Time.deltaTime;
        if (firstCard != null)

        {
            time1 += Time.deltaTime;
            if (time1 > 5)
            {
                firstCard.GetComponent<card>().closeCard1();
                firstCard = null;
                time1 = 0;
            }


        }
        bestscore -= Time.deltaTime;

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
                Debug.Log("이지모드");
                cardCnt = 4;
                break;
            case Diff.Normal:
                Debug.Log("노멀모드");
                cardCnt = 16;
                break;
            case Diff.Hard:
                Debug.Log("하드모드");
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
            bestscore += 7;

            //카드가 사라지기까지 1초의 시간이 걸린다. 또한 두 장만 남았어도 뒤집기 전까진 끝난게 아니다.
            //그러므로 카드가 뒤집어져 확정이 난 순간 카드는 두장이 남아있다.
            if (cardsLeft == 2)
            {
                if (diff == Diff.Easy) PlayerPrefs.SetInt("easyClear", 1);
                else if (diff == Diff.Normal) PlayerPrefs.SetInt("normalClear", 1);

                Time.timeScale = 0;
                audioSource.PlayOneShot(win, 0.5f);
                audioManager.gameObject.SetActive(false);
                endTxt.gameObject.SetActive(true);
                endPanel.SetActive(true);
                cardFlipNotice.TryCountNotice();
                ScoreTxt.text = bestscore.ToString("N2");
                /*if (PlayerPrefs.GetFloat("bestTime") > time)
                {
                    PlayerPrefs.SetFloat("bestTime", time);
                }*/              
                switch (diff)
                {
                    case Diff.Easy:
                        Debug.Log("xxx");
                        if (PlayerPrefs.HasKey("easyBestTime") == false)
                        {
                            PlayerPrefs.SetFloat("easyBestTime", time);
                        }
                        else
                        {
                            if (PlayerPrefs.GetFloat("easyBestTime") > time)
                                PlayerPrefs.SetFloat("easyBestTime", time);
                        }
                        break;
                    case Diff.Normal:
                        if (PlayerPrefs.HasKey("normalBestTime") == false)
                        {
                            PlayerPrefs.SetFloat("normalBestTime", time);
                        }
                        else
                        {
                            if (PlayerPrefs.GetFloat("normalBestTime") > time)
                                PlayerPrefs.SetFloat("normalBestTime", time);
                        }
                        break;
                    case Diff.Hard:
                        if (PlayerPrefs.HasKey("hardBestTime") == false)
                        {
                            PlayerPrefs.SetFloat("hardBestTime", time);
                        }
                        else
                        {
                            if (PlayerPrefs.GetFloat("hardBestTime") > time)
                                PlayerPrefs.SetFloat("hardBestTime", time);
                        }
                        break;
                }
            }
            cardsLeft -= 2;//남은 카드 장 수를 앞에 두면 아직 남아있는 두 장을 뒤집기도 전에 게임이 끝나버린다.            
        }
        else
        {
            audioSource.PlayOneShot(missmatch, 0.5f);
            time += 0.5f; // 이부분 바꾸면 실패 패널티 조절
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
            bestscore -= 5;
        }
        cardFlipNotice.MatchCheck(isMatched, firstCard.GetComponent<card>().WhosCard);
        firstCard = secondCard = null;
        time1 = 0;
    }
    //firstCard에 값이 들어있는가? = 첫번째카드를 집은 상태냐?
    public bool checkFirst()
    {
        if (firstCard != null) return true;
        else return false;
    }
    //첫번째카드를 지정
    public void setFirst(GameObject obj)
    {
        firstCard = obj;
        cardFlipNotice.ResetText();
    }
    //위와 동일하게 두번째카드 체크
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
        endPanel.SetActive(true);
        cardFlipNotice.TryCountNotice();
        ScoreTxt.text = bestscore.ToString("N2");
    }

    public void InitGame()
    {
        Time.timeScale = 1.0f;
        firstCard = secondCard = null;
        List<int> rtans = new List<int>();

        SpawnCards();

        /*
        //배열 내용 랜덤하게 섞기. List에서도 써먹을 수 있을 거 같다.
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