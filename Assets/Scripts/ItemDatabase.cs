using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Item Database")]//Asset에서 바로 create 가능
public class ItemDatabase : ScriptableObject //GameObject에서 관리하는 것이 아니라 Asset으로 관리
{
    //data 관리나 setting에 적합

    public Item[] items; //마트에 있는 모든 아이템


}