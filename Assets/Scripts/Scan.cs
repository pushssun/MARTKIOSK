using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Scan : MonoBehaviour
{
    public GameObject itemPf;
    public Button scanButton;
    public Transform itemPosition;
    public ItemDatabase ItemDatabase;
    public TextMeshProUGUI total;
    public TextMeshProUGUI count;
    public TextMeshProUGUI pay;
    public TextMeshProUGUI calTotal;
    public TextMeshProUGUI calCount;
    public TextMeshProUGUI calPay;
    public TextMeshProUGUI calDiscount;
    public TextMeshProUGUI payTotal;

    //==========nobarcode===========
    public NoBarcodeDatabase NoBarcodeDatabase;

    private int index;
    private ScanItem[] scanItems;

    // Start is called before the first frame update
    void Start()
    {   
        //���� ��ĵ�� �������� �� �� �ִ� �迭
        scanItems = new ScanItem[ItemDatabase.items.Length + NoBarcodeDatabase.items.Length];

        //���ڵ带 ���� �� ���� ���������� ��ǰ�� �������� ��
        scanButton.onClick.AddListener(SpawnItem);
    }

    private void SpawnItem()
    {
        //��ư Ŭ�� �� �������� ��ǰ�� �������� 
        int random = UnityEngine.Random.Range(0, ItemDatabase.items.Length);
        Item randomItem = ItemDatabase.items[random];

        //GameManager�� item �հ� ����
        GameManager.Instance.itemCount++;
        GameManager.Instance.itemTotal += randomItem.price;

        //���� �������� ���ų� ó�� �����̸� ���� �ƴϸ� ���� ����
        if (index == 0 || !FindItem(randomItem.name))
        {
            scanItems[index] = new ScanItem();
            scanItems[index].ItemToScanItem(randomItem);
            scanItems[index].count++;
            scanItems[index].totalPrice = scanItems[index].price * scanItems[index].count;

            TransText(itemPf, scanItems[index]);
            scanItems[index].itemPf = Instantiate(itemPf, itemPosition);
            index++;
        }
        else
        {
            //�̸��� ������ ������ �þ����
            for (int i= 0; i < index; i++)
            {
                if (randomItem.name.Equals(scanItems[i].name))
                {
                    scanItems[i].count++;
                    scanItems[i].totalPrice = scanItems[i].price * scanItems[i].count;
                    TransText(scanItems[i].itemPf, scanItems[i]);
                    break;
                }
            }
             
        }

        UpdateItem();
    }

    public void SpawnNobarcodeItem(string itemName, int price)
    {
        if (index == 0 || !FindItem(itemName))
        {
            scanItems[index] = new ScanItem();
            scanItems[index].NobarcodeItemToScanItem(itemName, price);
            scanItems[index].count++;
            scanItems[index].totalPrice = scanItems[index].price * scanItems[index].count;

            TransText(itemPf, scanItems[index]);
            scanItems[index].itemPf = Instantiate(itemPf, itemPosition);
            index++;
        }
        else
        {
            //�̸��� ������ ������ �þ����
            for (int i = 0; i < index; i++)
            {
                if (itemName.Equals(scanItems[i].name))
                {
                    scanItems[i].count++;
                    scanItems[i].totalPrice = scanItems[i].price * scanItems[i].count;
                    TransText(scanItems[i].itemPf, scanItems[i]);
                    break;
                }
            }

        }

        UpdateItem();
    }

    private bool FindItem(string name)
    {
        for (int i = 0; i < index; i++)
        {
            if (scanItems[i].name.Equals(name))
            {
                return true;
            }
        }
        return false;
    }

    private int FindIndex(string name)
    {
        for(int i = 0; i < scanItems.Length; i++)
        {
            if (scanItems[i].name.Equals(name))
            {
                return i;
            }
        }
        return -1;
    }
    
    private void TransText(GameObject itemPf, ScanItem item)
    {
        //text ����
        itemPf.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.name;
        itemPf.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = string.Format("{0:#,0}��", item.totalPrice); ;
        itemPf.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = scanItems[FindIndex(item.name)].count.ToString() + "��";
    }

    public void UpdateItem()
    {
        GameManager.Instance.totalPrice = GameManager.Instance.itemTotal + GameManager.Instance.bagTotal;
        GameManager.Instance.totalCount = GameManager.Instance.itemCount + GameManager.Instance.bagCount;
        GameManager.Instance.pay = GameManager.Instance.totalPrice - GameManager.Instance.cuponPrice;
        if(GameManager.Instance.pay < 0)
        {
            GameManager.Instance.pay = 0;
        }
        
        //total UI
        total.text = string.Format("{0:#,0}��", GameManager.Instance.totalPrice);
        count.text = string.Format("{0:#,0}", GameManager.Instance.totalCount);
        pay.text = string.Format("{0:#,0}��", GameManager.Instance.pay);

        //PayUI
        payTotal.text = string.Format("{0:#,0}����", GameManager.Instance.pay);

        //ReceiptUI
        calTotal.text = string.Format("{0:#,0}��", GameManager.Instance.totalPrice);
        calCount.text = string.Format("{0:#,0}��", GameManager.Instance.totalCount);
        calPay.text = string.Format("{0:#,0}��", GameManager.Instance.pay);
        calDiscount.text = string.Format("{0:#,0}��", GameManager.Instance.cuponPrice);
    }
}
