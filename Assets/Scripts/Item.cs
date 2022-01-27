using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ItemType
{
    Weapon,
    Medical,
    Loot,
    Provision,
    Container
}

[CreateAssetMenu(fileName = "Item", menuName = "Dynamic Inventory/Item")]
public abstract class Item : ScriptableObject
{
    #region Item attributes
    [SerializeField]
    private string _name;
    public new string name { get { return name; } }

    [SerializeField]
    private string _description;
    public string description { get { return _description; } }

    [SerializeField]
    private Sprite _sprite;
    public Sprite sprite { get { return _sprite; } }

    [SerializeField]
    private ItemType _itemType;
    public ItemType itemType { get { return _itemType; } }

    [SerializeField]
    private bool _stackable;
    public bool stackable { get { return _stackable; } }

    [SerializeField]
    private int _rowLength, _colLength;
    public int rowLength { get { return _rowLength; } }
    public int colLength { get { return _colLength; } }

    [SerializeField]
    private float _weight;
    public float weight { get { return _weight; } }
    #endregion

    public Event onClicked;
    public Event onDoubleClicked;
    public Event onRightClicked;

    public abstract void Use();
}