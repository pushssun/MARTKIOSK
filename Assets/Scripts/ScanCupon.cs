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
        //enrollButton ���� �ؽ�Ʈ ����
        Color color = enrollButton.image.color;
        color.a = 1f;
        enrollButton.image.color = color;
        enrollButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "��� �Ϸ�";

        //random ����
        int randomPrice = Random.Range(1, 100)*100;
        GameManager.Instance.cuponPrice += randomPrice;
        cuponCount.text = string.Format("{0:#,0}�� ����", (++GameManager.Instance.cuponCount));
        cuponPrice.text = string.Format("{0:#,0}��", GameManager.Instance.cuponPrice);
    }

    private void EnrollCupon()
    {
        if(enrollButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text.Equals("��� �Ϸ�"))
        {
            maincuponCount.text = string.Format("���� ���({0:#,0}��)", GameManager.Instance.cuponCount);
            discount.text = string.Format("{0:#,0}��", GameManager.Instance.cuponPrice);

            gameObject.SetActive(false);
        }

        GameManager.Instance.Scan.UpdateItem();
    }
}
