using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Item Database")]//Asset���� �ٷ� create ����
public class ItemDatabase : ScriptableObject //GameObject���� �����ϴ� ���� �ƴ϶� Asset���� ����
{
    //data ������ setting�� ����

    public Item[] items; //��Ʈ�� �ִ� ��� ������


}