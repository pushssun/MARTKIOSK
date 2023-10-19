using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScanButton : MonoBehaviour
{
    public GameObject itemPf;
    public Button scanButton;
    public Transform itemPosition;
    public ItemDatabase ItemDatabase;

    private int index;
    private ScanItem[] scanItems;

    // Start is called before the first frame update
    void Start()
    {
        //���� ��ĵ�� �������� �� �� �ִ� �迭
        scanItems = new ScanItem[ItemDatabase.items.Length];

        //���ڵ带 ���� �� ���� ���������� ��ǰ�� �������� ��
        scanButton.onClick.AddListener(SpawnItem);
    }

    private void SpawnItem()
    {
        //��ư Ŭ�� �� �������� ��ǰ�� �������� 
        int random = Random.Range(0, ItemDatabase.items.Length);
        Item randomItem = ItemDatabase.items[random];
        Debug.Log(randomItem.name);

        //���� �������� ���ų� ó�� �����̸� ���� �ƴϸ� ���� ����
        if (index == 0 || !FindItem(randomItem.name))
        {
            scanItems[index] = new ScanItem();
            scanItems[index].ItemToScanItem(randomItem);

            TransText(itemPf, randomItem);
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
                    TransText(scanItems[i].itemPf, randomItem);
                    break;
                }
            }
             
        }

       
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
    
    private void TransText(GameObject itemPf, Item item)
    {
        //text ����
        itemPf.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.name;
        itemPf.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = string.Format("{0:#,0}��", item.price);
        itemPf.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = scanItems[FindIndex(item.name)].count.ToString() + "��";
    }
}
