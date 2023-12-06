using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public enum GameStep
{
    None,
    Step1,
    Step2,
    Step3,
}
[System.Serializable]
public class GameData
{
    public string kiosk_id;
    public string play_date;
    public int play_stage;
    public int play_time;
    public int is_success;
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

    [SerializeField] private GameObject _successPanel;
    [SerializeField] private GameObject _FailPanel;

    private GameStep gameStep;
    private int playTime;
    private Stopwatch sw;
    private GameData _gameData;
    private string sceneNameType;
    private bool _saveData;

    //랜덤 게임
    private string[] step2 = { "바코드없는 상품", "종량제·장바구니" };
    private string[] step3 = { "쿠폰을 한 개이상", "포인트 적립" };
    private int random;

    private void Awake()
    {
        Instance = this; //GameManager 할당\
        _gameData = new GameData();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameData.play_date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        string sceneName = SceneManager.GetActiveScene().name;
        sceneNameType = sceneName.Substring(5,5); //연습UI면 None 반환

        _gameData.kiosk_id = sceneName.Substring(0,4);
        UnityEngine.Debug.Log(sceneName.Substring(9, 1));
        _gameData.play_stage = int.Parse(sceneName.Substring(9, 1));

        if (sceneNameType.StartsWith("Prac"))
        {
            gameStep = GameStep.None;
        }
        gameStep = (GameStep)char.GetNumericValue(sceneNameType[sceneNameType.Length - 1]);
        Scan.UpdateItem();

        //시간 측정
        sw = new Stopwatch();
        sw.Start();

        //랜덤 진행 (2단계와 3단계 2개씩 랜덤 존재)
        if(randomTxt != null)
        {
            random = UnityEngine.Random.Range(0, 2);
            if(gameStep == GameStep.Step2)
            {
                randomTxt.text = step2[random] + "을(를) 한 개 이상 담고 결제해주세요." + System.Environment.NewLine + "(결제 수단이나 포인트는 적용해도 됩니다.)";
            }
            else if(gameStep == GameStep.Step3)
            {
                randomTxt.text = step3[random] + "하고 결제해주세요." + System.Environment.NewLine + "(결제 수단이나 포인트는 적용해도 됩니다.)";
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
                case GameStep.Step1:
                    isSuccess = Scan.IsItem();
                    
                    break;
                case GameStep.Step2:
                    Step2();
                    break;
                case GameStep.Step3:
                    Step3();
                    break;
            }

            if(sceneNameType.StartsWith("Test"))
            {
                if (isSuccess)
                {
                    _successPanel.SetActive(true);
                }
                else
                {
                    _FailPanel.SetActive(true);
                }
                _gameData.is_success = Convert.ToInt32(isSuccess);
                if (!_saveData)
                {
                    SaveData(); //끝나면 정보 보내기

                }

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
            _gameData.play_time = playTime;
        }
    }

    public void SetQuit()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
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

    private void SaveData()
    {
        _saveData = true;
        string jsonData = JsonUtility.ToJson(_gameData);

        string url = "https://003operation.shop/kiosk/insertData";

        StartCoroutine(SendDataToWeb(jsonData, url));
    }

    private IEnumerator SendDataToWeb(string jsonData, string url)
    {
        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = UnityWebRequest.PostWwwForm(url, "POST");
        www.uploadHandler = new UploadHandlerRaw(dataBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError("Failed to send data to the web server: " + www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Data sent successfully!");
            SetQuit();
        }
    }

}
