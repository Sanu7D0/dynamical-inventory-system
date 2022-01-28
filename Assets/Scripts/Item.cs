using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicInventory
{
    public enum ItemType
    {
        Weapon,
        Medical,
        Loot,
        Container,
        Provision,
        Ammunition,
        Currency
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

        public int rotation { get; private set; } = 0;
        #endregion

        public abstract Item Init();

        public abstract void Use();

        // Rotate the item 90 degree to clock-wise in inventory
        // 0, 90 = 0, 1
        public void Rotate(int deltaRot)
        {
            // Cannot rotate square-shaped item
            if (rowLength == colLength)
            {
                return;
            }

            rotation = Mathf.Abs(rotation + deltaRot) % 2;
        }
    }
}