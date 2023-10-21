using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class NobarcodeUI : MonoBehaviour
{
    public GameObject barcodeItemPf;
    public Transform[] itemPosition;
    public FieldType fieldType;
    public NoBarcodeDatabase NoBarcodeDatabase;
    public Button[] fieldButton;

    private void Start()
    {
        for(int i = 0; i < fieldButton.Length; i++)
        {
            int index = i; //i 증가로 인해 지역변수 ndex 할당
            fieldButton[i].onClick.AddListener(()=>OnClickField(index)); //각각 버튼에 field에 맞는 Listener 할당
            fieldType = (FieldType)i;//fieldType 할당
            SpawnItem();//fieldType별 spawn
        }

        //초기 설정
        fieldType = FieldType.Fruit;
        fieldButton[(int)FieldType.Fruit].Select();
    }

    private void OnClickField(int field)
    {
        switch ((FieldType)field)
        {
            case FieldType.Fruit:
                itemPosition[0].gameObject.SetActive(true);
                itemPosition[1].gameObject.SetActive(false);
                itemPosition[2].gameObject.SetActive(false);
                break;
            case FieldType.Vegetable:
                fieldType = FieldType.Vegetable;
                itemPosition[0].gameObject.SetActive(false);
                itemPosition[1].gameObject.SetActive(true);
                itemPosition[2].gameObject.SetActive(false);
                break;
            case FieldType.MealKit:
                itemPosition[0].gameObject.SetActive(false);
                itemPosition[1].gameObject.SetActive(false);
                itemPosition[2].gameObject.SetActive(true);
                break;
        }
    }

    private void SpawnItem()
    {
        for(int i = 0; i < NoBarcodeDatabase.items.Length; i++)
        {
            if(fieldType == NoBarcodeDatabase.items[i].field)
            {
                barcodeItemPf.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = NoBarcodeDatabase.items[i].name;
                if (NoBarcodeDatabase.items[i].img != null)
                { 
                    barcodeItemPf.transform.Find("Image").GetComponent<Image>().sprite = NoBarcodeDatabase.items[i].img;
                }

                Instantiate(barcodeItemPf, itemPosition[(int)fieldType]);
            }
        }
    }
}
