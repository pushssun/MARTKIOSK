using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour //장바구니 클래스 생성 //이름과 가격 등을 관리자가 수정할 수 있도록 public으로 함
{
    public string objectName;
    public int price;
    public Sprite img;
    public int count { get; private set; }

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Image image;

    public Button minusButton; //장바구니 갯수 카운트
    public Button plusButton; //장바구니 갯수 카운트
    public TextMeshProUGUI countUI;

    private void Start()
    {
        nameText.text = objectName;
        priceText.text = string.Format("{0:#,0}원",price);
        image.sprite = img;

        //버튼 리스너 할당
        minusButton.onClick.AddListener(MinusCount);
        plusButton.onClick.AddListener(PlusCount);
    }
    public void MinusCount()
    {
        count--;
        if (count < 0)
        {
            count = 0;
        }

        countUI.text = count.ToString();
    }

    public void PlusCount()
    {
        count++;
        countUI.text = count.ToString();  
    }

}

