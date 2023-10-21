using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ScanNobarcodeItem : MonoBehaviour
{
    public NoBarcodeDatabase NoBarcodeDatabase;

    private Button itemButton;
    private string itemName;
    private int price;

    // Start is called before the first frame update
    void Start()
    {
        itemButton = GetComponent<Button>();
        itemButton.onClick.AddListener(ScanItem);

        itemName = gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text;
    }

    private void ScanItem()
    {
        GameManager.Instance.itemCount++;

        price = FindItemPrice(itemName);
        if (price != -1)
        {
            GameManager.Instance.itemTotal += price; 
        }
        GameManager.Instance.Scan.SpawnNobarcodeItem(itemName, price);
    }

    private int FindItemPrice(string name)
    {
        for(int i=0;i<NoBarcodeDatabase.items.Length;i++)
        {
            if(NoBarcodeDatabase.items[i].name.Equals(name)){
                return NoBarcodeDatabase.items[i].price;
            }

        }
        return -1;
    }
}
