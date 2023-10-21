using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyBag : MonoBehaviour
{
    public TextMeshProUGUI[] mainCount;

    private Button buyButton; 

    private void Start()
    {
        buyButton = GetComponent<Button>();
        buyButton.onClick.AddListener(OnBuyBag);
    }

    private void OnBuyBag()
    {
        int bagCount = 0;
        int bagPrice = 0;
        for(int i=0; i<mainCount.Length; i++)
        {
            bagCount += GameManager.Instance.bags[i].count;
            bagPrice += GameManager.Instance.bags[i].price * GameManager.Instance.bags[i].count;

            mainCount[i].text = GameManager.Instance.bags[i].count.ToString();  
        }

        GameManager.Instance.bagCount = bagCount;
        GameManager.Instance.bagTotal = bagPrice;
        GameManager.Instance.Scan.UpdateItem();
    }
}
