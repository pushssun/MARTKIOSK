using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScanCupon : MonoBehaviour
{
    public TextMeshProUGUI cuponCount;
    public TextMeshProUGUI cuponPrice;
    public Button enrollButton;
    public Button scanButton;

    public TextMeshProUGUI maincuponCount;
    public TextMeshProUGUI discount;

    // Start is called before the first frame update
    void Start()
    {
        scanButton.onClick.AddListener(ScanBarcode);
        enrollButton.onClick.AddListener(EnrollCupon);
    }

    private void ScanBarcode()
    {
        //enrollButton 색과 텍스트 변경
        Color color = enrollButton.image.color;
        color.a = 1f;
        enrollButton.image.color = color;
        enrollButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "등록 완료";

        //random 쿠폰
        int randomPrice = Random.Range(1, 100)*100;
        GameManager.Instance.cuponPrice += randomPrice;
        cuponCount.text = string.Format("{0:#,0}개 쿠폰", (++GameManager.Instance.cuponCount));
        cuponPrice.text = string.Format("{0:#,0}원", GameManager.Instance.cuponPrice);
    }

    private void EnrollCupon()
    {
        if(enrollButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text.Equals("등록 완료"))
        {
            maincuponCount.text = string.Format("쿠폰 등록({0:#,0}개)", GameManager.Instance.cuponCount);
            discount.text = string.Format("{0:#,0}원", GameManager.Instance.cuponPrice);

            gameObject.SetActive(false);
        }

        GameManager.Instance.Scan.UpdateItem();
    }
}
