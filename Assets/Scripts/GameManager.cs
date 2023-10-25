using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStep
{
    None,
    One,
    Two,
    Three,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Scan Scan;

    //============Item===============
    public int itemTotal;
    public int itemCount;

    //==========Bag==================
    public Bag[] bags;
    public int bagCount;
    public int bagTotal;

    //=========Cupon================
    public int cuponCount;
    public int cuponPrice;

    public int totalPrice;
    public int totalCount;
    public int pay;

    //=========Game================
    public GameObject finishUI; //finishUI가 켜지면서 성공여부 확인
    public GameObject usePointUI; //point 사용 여부 확인
    public TextMeshProUGUI playTimeTxt; //게임 진행 시간
    public TextMeshProUGUI randomTxt; //게임 랜덤 진행 text
    public bool isSuccess; //성공 여부

    private GameStep gameStep;
    private int playTime;
    private Stopwatch sw;

    //랜덤 게임
    private string[] step2 = { "바코드없는 상품", "종량제·장바구니" };
    private string[] step3 = { "쿠폰을 한 개이상", "포인트 적립" };
    private int random;

    private void Awake()
    {
        Instance = this; //GameManager 할당
    }

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name.Substring(5,5); //연습UI면 None 반환
        if (sceneName.StartsWith("Prac"))
        {
            gameStep = GameStep.None;
        }
        gameStep = (GameStep)char.GetNumericValue(sceneName[sceneName.Length - 1]);
        Scan.UpdateItem();

        //시간 측정
        sw = new Stopwatch();
        sw.Start();

        //랜덤 진행 (2단계와 3단계 2개씩 랜덤 존재)
        if(randomTxt != null)
        {
            random = Random.Range(0, 2);
            if(gameStep == GameStep.Two)
            {
                randomTxt.text = step2[random] + "을(를) 한 개 이상 담고 결제해주세요." + System.Environment.NewLine + "(결제 수단이나 포인트는 적용해도 됩니다.";
            }
            else if(gameStep == GameStep.Three)
            {
                randomTxt.text = step3[random] + "하고 결제해주세요." + System.Environment.NewLine + "(결제 수단이나 포인트는 적용해도 됩니다.";
            }
        }
    }

    private void Update()
    {
        if (finishUI.activeSelf == true)
        {
            sw.Stop();
            switch (gameStep)
            {
                case GameStep.One:
                    isSuccess = Scan.IsItem();
                    
                    break;
                case GameStep.Two:
                    Step2();
                    break;
                case GameStep.Three:
                    Step3();
                    break;
            }
        }

        //시간 계산
        if(playTimeTxt != null)
        {
            playTime = (int)sw.ElapsedMilliseconds / 1000;
            int minutes;
            int seconds;

            minutes = playTime / 60;
            seconds = playTime % 60;
            
            if(minutes > 0)
            {
                playTimeTxt.text = "소요 시간 : " + minutes.ToString() + "분 " + seconds.ToString() + "초";
            }
            else
            {
                playTimeTxt.text = "소요 시간 : " + seconds.ToString() + "초";
            }
        }
    }

    private void Step2()
    {
        if(random == 0)
        {
            isSuccess = Scan.IsNobarcodeItem();
        }
        else
        {
            isSuccess = IsBagItem();
        }
        
    }

    private void Step3()
    {
        if (random == 0)
        {
            if(cuponCount > 0)
            {
                isSuccess = true;
            }
        }
        else
        {
            if(usePointUI.activeSelf == true)
            {
                isSuccess = true;
            }
        }

    }

    private bool IsBagItem()
    {
        if(bagCount > 0)
        {
            return true;
        }
        return false;
    }
}
