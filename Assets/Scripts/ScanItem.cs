using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScanItem
{
    public string name;
    public int price;
    public int totalPrice;
    public int count;
    public GameObject itemPf;

    public void ItemToScanItem(Item item)
    {
        this.name = item.name;
        this.price = item.price;
        this.count = 0;
    }
    public void NobarcodeItemToScanItem(string name, int price)
    {
        this.name = name;
        this.price = price;
        this.count = 0;
    }
}
