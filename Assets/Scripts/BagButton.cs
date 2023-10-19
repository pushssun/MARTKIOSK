using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagButton : MonoBehaviour
{
    public Button minusButton; //장바구니 갯수 카운트
    public Button plusButton; //장바구니 갯수 카운트
    public TextMeshProUGUI count;
    
    private Bag bag;

    private void Start()
    {
        bag = GetComponent<Bag>();

        //버튼 리스너 할당
        minusButton.onClick.AddListener(bag.MinusCount);
        plusButton.onClick.AddListener(bag.Pluscount);
    }
    private void Update()
    {
        //text 변환
        if(count != null)
        {
            count.text = bag.count.ToString();  
        }
    }
}
