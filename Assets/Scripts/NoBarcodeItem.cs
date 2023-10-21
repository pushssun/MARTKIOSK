using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldType { Fruit, Vegetable, MealKit };
[Serializable]
public class NoBarcodeItem : Item
{
    public FieldType field;
    public Sprite img;
}
