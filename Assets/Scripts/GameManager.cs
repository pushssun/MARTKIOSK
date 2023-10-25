using System.Collections;
using System.Collections.Generic;
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
    public bool isSuccess; //성공 여부
    public Time gameTime; //게임 진행 시간

    private GameStep gameStep;
    

    private void Awake()
    {
        Instance = this; //GameManager 할당
    }

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        gameStep = (GameStep)char.GetNumericValue(sceneName[sceneName.Length - 1]);
        Scan.UpdateItem();
    }

    private void Update()
    {
        if (finishUI.activeSelf == true)
        {
            switch (gameStep)
            {
                case GameStep.One:
                    isSuccess = Scan.IsItem();
                    break;
            }
        }
    }
}
