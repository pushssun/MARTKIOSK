using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour //��ٱ��� Ŭ���� ���� //�̸��� ���� ���� �����ڰ� ������ �� �ֵ��� public���� ��
{
    public string objectName;
    public int price;
    public Sprite img;
    public int count { get; private set; }

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Image image;

    public Button minusButton; //��ٱ��� ���� ī��Ʈ
    public Button plusButton; //��ٱ��� ���� ī��Ʈ
    public TextMeshProUGUI countUI;

    private void Start()
    {
        nameText.text = objectName;
        priceText.text = string.Format("{0:#,0}��",price);
        image.sprite = img;

        //��ư ������ �Ҵ�
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

