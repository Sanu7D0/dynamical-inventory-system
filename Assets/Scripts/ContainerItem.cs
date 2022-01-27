using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Dynamic Inventory/Item/Container")]
public class ContainerItem : Item
{
    [SerializeField]
    protected int rowSize, colSize;

    public override void Use() { }
}
