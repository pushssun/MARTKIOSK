using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagButton : MonoBehaviour
{
    public Button minusButton; //��ٱ��� ���� ī��Ʈ
    public Button plusButton; //��ٱ��� ���� ī��Ʈ
    public TextMeshProUGUI count;
    
    private Bag bag;

    private void Start()
    {
        bag = GetComponent<Bag>();

        //��ư ������ �Ҵ�
        minusButton.onClick.AddListener(bag.MinusCount);
        plusButton.onClick.AddListener(bag.Pluscount);
    }
    private void Update()
    {
        //text ��ȯ
        if(count != null)
        {
            count.text = bag.count.ToString();  
        }
    }
}
