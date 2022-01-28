using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicInventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Dynamic Inventory/Item/Container")]
    public class ContainerItem : Item
    {
        [SerializeField]
        private int _rowSize, _colSize;
        public int rowSize
        {
            get { return _rowSize; }
        }
        public int colSize
        {
            get { return _colSize; }
        }

        public Item[,] container;

        public override Item Init()
        {
            ContainerItem clone = ScriptableObjectExt.Clone<ContainerItem>(this);

            clone.container = new Item[rowSize, colSize];

            return clone;
        }

        public override void Use() { }

        public bool TryPutItem(Item item, int targetR, int targetC, int deltaRotation = 0)
        {
            if (targetR < 0 || targetR >= rowSize || targetC < 0 || targetR >= colSize)
            {
                Debug.LogError("TryPutItem: Index out of range");
                return false;
            }

            // If item is rotated 90 degree, swap row and column length
            int itemRowLength = (item.rotation + deltaRotation == 0) ? item.rowLength : item.colLength;
            int itemColLength = (item.rotation + deltaRotation == 0) ? item.colLength : item.rowLength;

            // Check if item is out of container size
            if (targetR + itemRowLength > rowSize || targetC + itemColLength > colSize)
            {
                return false;
            }

            // Check if the cells are already occupied
            for (int r = targetR; r < targetR + itemRowLength; r++)
            {
                for (int c = targetC; c < targetC + itemColLength; c++)
                {
                    if (container[r, c] != null)
                    {
                        return false;
                    }
                }
            }

            // Put the item reference at the target position
            for (int r = targetR; r < targetR + itemRowLength; r++)
            {
                for (int c = targetC; c < targetC + itemColLength; c++)
                {
                    container[r, c] = item;
                }
            }
            return true;
        }

        public bool TryPushItem(Item item)
        {
            for (int r = 0; r < rowSize; r++)
            {
                for (int c = 0; c < colSize; c++)
                {
                    if (TryPutItem(item, r, c))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
