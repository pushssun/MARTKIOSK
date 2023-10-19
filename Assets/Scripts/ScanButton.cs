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
        //현재 스캔된 아이템을 알 수 있는 배열
        scanItems = new ScanItem[ItemDatabase.items.Length];

        //바코드를 찍을 수 없어 랜덤적으로 상품이 나오도록 함
        scanButton.onClick.AddListener(SpawnItem);
    }

    private void SpawnItem()
    {
        //버튼 클릭 시 랜덤적인 상품이 나오도록 
        int random = Random.Range(0, ItemDatabase.items.Length);
        Item randomItem = ItemDatabase.items[random];
        Debug.Log(randomItem.name);

        //같은 아이템이 없거나 처음 생성이면 스폰 아니면 갯수 증가
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
            //이름이 같으면 갯수가 늘어나도록
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
        //text 변경
        itemPf.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.name;
        itemPf.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = string.Format("{0:#,0}원", item.price);
        itemPf.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = scanItems[FindIndex(item.name)].count.ToString() + "개";
    }
}
