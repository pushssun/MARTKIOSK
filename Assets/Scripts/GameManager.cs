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
    public GameObject finishUI; //finishUI�� �����鼭 �������� Ȯ��
    public GameObject usePointUI; //point ��� ���� Ȯ��
    public TextMeshProUGUI playTimeTxt; //���� ���� �ð�
    public TextMeshProUGUI randomTxt; //���� ���� ���� text
    public bool isSuccess; //���� ����

    private GameStep gameStep;
    private int playTime;
    private Stopwatch sw;

    //���� ����
    private string[] step2 = { "���ڵ���� ��ǰ", "����������ٱ���" };
    private string[] step3 = { "������ �� ���̻�", "����Ʈ ����" };
    private int random;

    private void Awake()
    {
        Instance = this; //GameManager �Ҵ�
    }

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name.Substring(5,5); //����UI�� None ��ȯ
        if (sceneName.StartsWith("Prac"))
        {
            gameStep = GameStep.None;
        }
        gameStep = (GameStep)char.GetNumericValue(sceneName[sceneName.Length - 1]);
        Scan.UpdateItem();

        //�ð� ����
        sw = new Stopwatch();
        sw.Start();

        //���� ���� (2�ܰ�� 3�ܰ� 2���� ���� ����)
        if(randomTxt != null)
        {
            random = Random.Range(0, 2);
            if(gameStep == GameStep.Two)
            {
                randomTxt.text = step2[random] + "��(��) �� �� �̻� ��� �������ּ���." + System.Environment.NewLine + "(���� �����̳� ����Ʈ�� �����ص� �˴ϴ�.";
            }
            else if(gameStep == GameStep.Three)
            {
                randomTxt.text = step3[random] + "�ϰ� �������ּ���." + System.Environment.NewLine + "(���� �����̳� ����Ʈ�� �����ص� �˴ϴ�.";
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

        //�ð� ���
        if(playTimeTxt != null)
        {
            playTime = (int)sw.ElapsedMilliseconds / 1000;
            int minutes;
            int seconds;

            minutes = playTime / 60;
            seconds = playTime % 60;
            
            if(minutes > 0)
            {
                playTimeTxt.text = "�ҿ� �ð� : " + minutes.ToString() + "�� " + seconds.ToString() + "��";
            }
            else
            {
                playTimeTxt.text = "�ҿ� �ð� : " + seconds.ToString() + "��";
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
