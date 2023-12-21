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
    int cardsLeft;//'실제로' 남아있는 카드
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
            
            //카드가 사라지기까지 1초의 시간이 걸린다. 또한 두 장만 남았어도 뒤집기 전까진 끝난게 아니다.
            //그러므로 카드가 뒤집어져 확정이 난 순간 카드는 두장이 남아있다.
            if(cardsLeft == 2)
            {
                Time.timeScale = 0;
                endTxt.gameObject.SetActive(true);
            }
            cardsLeft -= 2;//남은 카드 장 수를 앞에 두면 아직 남아있는 두 장을 뒤집기도 전에 게임이 끝나버린다.            
        }
        else
        {
            Time.timeScale++;
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
        }
        firstCard = secondCard = null;
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
        Time.timeScale = 0f;
        endTxt.gameObject.SetActive(true);
    }

    public void InitGame()
    {
        Time.timeScale = 1.0f;
        firstCard = secondCard = null;
        //배열 내용 랜덤하게 섞기. List에서도 써먹을 수 있을 거 같다.
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
