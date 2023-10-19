using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScanItem
{
    public string name;
    public int price;
    public int count;
    public GameObject itemPf;

    public void ItemToScanItem(Item item)
    {
        this.name = item.name;
        this.price = item.price;
        this.count = 1;
    }
}
