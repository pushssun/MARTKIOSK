using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectItemUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public NoBarcodeDatabase NoBarcodeDatabase;

    private void SelectRandomNoBarcodeItem()
    {
        int random = Random.Range(0, NoBarcodeDatabase.items.Length);
        text.text = NoBarcodeDatabase.items[random] + "를 선택해주세요.";
    }
}
