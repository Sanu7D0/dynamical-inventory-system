using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicInventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Dynamic Inventory/Item/Container")]
    public class ContainerItem : Item
    {
        [SerializeField] private int _rowSize, _colSize;
        public int rowSize
        {
            get { return _rowSize; }
        }
        public int colSize
        {
            get { return _colSize; }
        }

        public Item[,] container
        {
            get; private set;
        }

        public override Item Init()
        {
            ContainerItem clone = ScriptableObjectExt.Clone<ContainerItem>(this);

            clone.container = new Item[rowSize, colSize];

            return clone;
        }

        public override void Use() { }

        public bool TryPutItem(Item item, int targetR, int targetC, int rotation)
        {
            if (!CanPutItem(item, targetR, targetC, rotation))
            {
                return false;
            }

            // If item is rotated 90 degree, swap row and column length
            int itemRowLength = (rotation == 0) ? item.rowLength : item.colLength;
            int itemColLength = (rotation == 0) ? item.colLength : item.rowLength;

            // Put the item reference at the target positions
            for (int r = targetR; r < targetR + itemRowLength; r++)
            {
                for (int c = targetC; c < targetC + itemColLength; c++)
                {
                    container[r, c] = item;
                }
            }
            return true;
        }

        public bool CanPutItem(Item item, int targetR, int targetC, int rotation)
        {
            if (targetR < 0 || targetR >= rowSize || targetC < 0 || targetR >= colSize)
            {
                Debug.LogError("Index out of range");
                return false;
            }

            // If item is rotated 90 degree, swap row and column length
            int itemRowLength = (rotation == 0) ? item.rowLength : item.colLength;
            int itemColLength = (rotation == 0) ? item.colLength : item.rowLength;

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

            return true;
        }

        public bool TryPushItem(Item item, out int R, out int C, out int rotation)
        {
            for (int r = 0; r < rowSize; r++)
            {
                for (int c = 0; c < colSize; c++)
                {
                    if (container[r, c] == null)
                    {
                        R = r;
                        C = c;
                        // Try put item in both rotation
                        if (TryPutItem(item, r, c, 0))
                        {
                            rotation = 0;
                            return true;
                        }
                        else if (TryPutItem(item, r, c, 1))
                        {
                            rotation = 1;
                            return true;
                        }
                    }
                }
            }
            R = 0;
            C = 0;
            rotation = 0;
            return false;
        }

        public bool TryPullItem(Item item)
        {
            bool result = false;
            for (int i = 0; i < container.GetLength(0); i++)
            {
                for (int j = 0; j < container.GetLength(1); j++)
                {
                    if (container[i, j] == item)
                    {
                        result = true;
                        container[i, j] = null;
                    }
                }
            }
            return result;
        }
    }
}
