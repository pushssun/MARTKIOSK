using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NoBarcode Item Database", menuName = "NoBarcode Database")]//Asset���� �ٷ� create ����
public class NoBarcodeDatabase : ScriptableObject //GameObject���� �����ϴ� ���� �ƴ϶� Asset���� ����
{
    //data ������ setting�� ����

    public NoBarcodeItem[] items; //��Ʈ�� �ִ� ���ڵ尡 ���� ��ǰ


}