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

    private void Start()
    {
        nameText.text = objectName;
        priceText.text = string.Format("{0:#,0}��",price);
        image.sprite = img;
    }
    public void MinusCount()
    {
        count--;
        if (count < 0)
        {
            count = 0;
        }
    }

    public void Pluscount()
    {
        count++;
    }
}

