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
    public string member_id;
    public string kiosk_category_id;
    public string play_date;
    public int play_stage;
    public int play_time;
    public int is_success;
    public int is_game;
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
    public GameObject finishUI; //finishUI�� �����鼭 �������� Ȯ��
    public GameObject usePointUI; //point ��� ���� Ȯ��
    public TextMeshProUGUI playTimeTxt; //���� ���� �ð�
    public TextMeshProUGUI randomTxt; //���� ���� ���� text
    public bool isSuccess; //���� ����

    [SerializeField] private GameObject _successPanel;
    [SerializeField] private GameObject _FailPanel;

    private GameStep _gameStep;
    private int playTime;
    private Stopwatch sw;
    private GameData _gameData;
    private string _sceneNameType;
    private bool _saveData;

    //���� ����
    private string[] step2 = { "바코드없는 상품", "종량제·장바구니" };
    private string[] step3 = { "쿠폰을 한 개이상", "포인트 적립" };
    private int random;

    private void Awake()
    {
        Instance = this; //GameManager �Ҵ�\
        _gameData = new GameData();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.ExternalCall("unityFunction", _gameData.member_id);
        
        _gameData.play_date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        string sceneName = SceneManager.GetActiveScene().name;
        _sceneNameType = sceneName.Substring(5,5); //����UI�� None ��ȯ

        _gameData.kiosk_category_id = sceneName.Substring(0,4);
        UnityEngine.Debug.Log(sceneName.Substring(9, 1));
        _gameData.play_stage = int.Parse(sceneName.Substring(9, 1));

        if (_sceneNameType.StartsWith("Prac"))
        {
            _gameData.is_game = 0;
        }
        else if (_sceneNameType.StartsWith("Test"))
        {
            _gameData.is_game = 1;
        }
        _gameStep = (GameStep)char.GetNumericValue(_sceneNameType[_sceneNameType.Length - 1]);
        Scan.UpdateItem();

        //�ð� ����
        sw = new Stopwatch();
        sw.Start();

        //���� ���� (2�ܰ�� 3�ܰ� 2���� ���� ����)
        if(randomTxt != null)
        {
            random = UnityEngine.Random.Range(0, 2);
            if (_gameStep == GameStep.Step2)
            {
                randomTxt.text = step2[random] + "을(를) 한 개 이상 담고 결제해주세요." + System.Environment.NewLine + "(결제 수단이나 포인트는 적용해도 됩니다.)";
            }
            else if (_gameStep == GameStep.Step3)
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
            switch (_gameStep)
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
                SaveData(); //������ ���� ������

            }

            
        }

        //�ð� ���
        if(playTimeTxt != null)
        {
            playTime = (int)sw.ElapsedMilliseconds / 1000;
            int minutes;
            int seconds;

            minutes = playTime / 60;
            seconds = playTime % 60;

            if (minutes > 0)
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

    public void ReceiveData(string message)
    {
        _gameData.member_id = message;
        UnityEngine.Debug.Log("Received message from JavaScript: " + message);
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
        if (_sceneNameType.StartsWith("Test"))
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
        else if (_sceneNameType.StartsWith("Prac"))
        {
            isSuccess = Scan.IsNobarcodeItem() || IsBagItem(); 
        }
        
        
    }

    private void Step3()
    {
        if (_sceneNameType.StartsWith("Test"))
        {
            if (random == 0)
            {
                if (cuponCount > 0)
                {
                    isSuccess = true;
                }
            }
            else
            {
                if (usePointUI.activeSelf == true)
                {
                    isSuccess = true;
                }
            }
        }
        else if (_sceneNameType.StartsWith("Prac"))
        {
            if (cuponCount > 0 || usePointUI.activeSelf == true)
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
        www.SetRequestHeader("withCredentials", "true");

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
