using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicInventory
{
    public class ContainerBehaviour : MonoBehaviour
    {
        [SerializeField] private ContainerItem _containerItem;
        public ContainerItem containerItem
        {
            get { return _containerItem; }
            private set { _containerItem = value; }
        }
        [SerializeField] private RectTransform containerGrid;
        [SerializeField] private RectTransform _itemHolders;
        public RectTransform itemHolders { get { return _itemHolders; } }

        public Dictionary<int, ItemHolderBehaviour> itemHolderMap
        {
            get;
            private set;
        }

        public delegate void ContainerChangeHandler();
        public event ContainerChangeHandler OnContainerChanged;

        public int focusingIndex;

        private void Awake()
        {
            containerItem = containerItem.Init() as ContainerItem;
        }

        private void OnEnable()
        {
            OnContainerChanged += UpdateContainer;
            focusingIndex = -1;
        }

        private void Start()
        {
            itemHolderMap = new Dictionary<int, ItemHolderBehaviour>();

            int col = containerItem.colSize;
            int row = containerItem.rowSize;
            containerGrid.sizeDelta = GlobalData.cellSize * new Vector2(col, row);

            GridLayoutGroup grid = containerGrid.GetComponent<GridLayoutGroup>();
            grid.constraintCount = col;

            for (int i = 0; i < row * col; i++)
            {
                GameObject cell =
                    Instantiate(InventoryManager.Instance.containerCellPrefab, Vector3.zero, Quaternion.identity);
                cell.transform.SetParent(grid.transform);
                cell.GetComponent<ContainerCellBehaviour>().Set(this);
            }
        }

        private void OnDisable()
        {
            OnContainerChanged -= UpdateContainer;
        }

        public bool TryPutItem(Item item, int r, int c, int rotation)
        {
            if (containerItem.TryPutItem(item, r, c, rotation))
            {
                // Create or update item holder
                CheckItemHolder(item, r, c, rotation);

                OnContainerChanged?.Invoke();
                return true;
            }
            return false;
        }

        public bool TryPushItem(Item item)
        {
            if (containerItem.TryPushItem(item, out int r, out int c, out int rotation))
            {
                // Create or update item holder
                CheckItemHolder(item, r, c, rotation);

                OnContainerChanged?.Invoke();
                return true;
            }
            return false;
        }

        public bool TryPullItem(Item item)
        {
            if (containerItem.TryPullItem(item))
            {
                itemHolderMap.Remove(item.GetInstanceID());

                OnContainerChanged?.Invoke();
                return true;
            }
            return false;
        }

        public void UpdateContainer()
        {
            RectTransform cell;
            foreach (ItemHolderBehaviour itemHolder in itemHolderMap.Values)
            {
                cell = containerGrid.GetChild(
                    itemHolder.r * containerItem.colSize + itemHolder.c) as RectTransform;

                itemHolder.transform.position = cell.position;
                // Pivot compensation
                if (itemHolder.rotation == 1)
                {
                    itemHolder.transform.position += Vector3.down * GlobalData.cellSize;
                }
            }
        }

        private ItemHolderBehaviour CreateItemHolder(Item item, int r, int c, int rotation)
        {
            ItemHolderBehaviour newItemHolder =
                GameObject.Instantiate(InventoryManager.Instance.itemHolderPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<ItemHolderBehaviour>();

            newItemHolder.GetComponent<RectTransform>().SetParent(itemHolders);
            newItemHolder.Init(item, r, c, rotation, this);

            return newItemHolder;
        }

        private void CheckItemHolder(Item item, int r, int c, int rotation)
        {
            int id = item.GetInstanceID();
            if (itemHolderMap.ContainsKey(id))
            {
                itemHolderMap[id].Set(r, c, rotation);
            }
            else
            {
                itemHolderMap.Add(id, CreateItemHolder(item, r, c, rotation));
            }
        }

        public bool TryDropItem(int idx)
        {
            GetDropTarget(idx, out Item item, out int r, out int c, out int rotation);
            ItemHolderBehaviour itemHolder =
                InventoryManager.Instance.dragAndDropHolder.originItemHolder;

            if (containerItem.TryPutItem(item, r, c, rotation))
            {
                // Succeed to put item
                itemHolder.SetContainer(this);
                itemHolder.Set(r, c, rotation);

                itemHolderMap.Add(itemHolder.item.GetInstanceID(), itemHolder);

                OnContainerChanged?.Invoke();

                return true;
            }
            else
            {
                // Failed to put item -> back to original position
                containerItem.TryPutItem(itemHolder.item, itemHolder.r, itemHolder.c, itemHolder.rotation);
                return false;
            }
        }

        public bool CanDropItem(int idx)
        {
            GetDropTarget(idx, out Item item, out int r, out int c, out int rotation);

            return containerItem.CanPutItem(item, r, c, rotation);
        }

        public void SetColorIndicator(bool flush = false)
        {
            if (focusingIndex == -1 || flush)
            {
                for (int r = 0; r < containerItem.rowSize; r++)
                {
                    for (int c = 0; c < containerItem.colSize; c++)
                    {
                        containerGrid.GetChild(r * containerItem.colSize + c)
                        .GetComponent<ContainerCellBehaviour>().SetColor(GlobalData.ItemHolderColors.idle);
                    }
                }

                if (focusingIndex == -1)
                    return;
            }

            GetDropTarget(focusingIndex, out Item item, out int R, out int C, out int rotation);

            Color32 indicator =
                CanDropItem(focusingIndex) ? GlobalData.ItemHolderColors.green : GlobalData.ItemHolderColors.red;

            // If item is rotated 90 degree, swap row and column length
            int itemRowLength = (rotation == 0) ? item.rowLength : item.colLength;
            int itemColLength = (rotation == 0) ? item.colLength : item.rowLength;

            // Put the item reference at the target positions
            for (int r = R; r < Mathf.Min(R + itemRowLength, containerItem.rowSize); r++)
            {
                for (int c = C; c < Mathf.Min(C + itemColLength, containerItem.colSize); c++)
                {
                    containerGrid.GetChild(r * containerItem.colSize + c)
                        .GetComponent<ContainerCellBehaviour>().SetColor(indicator);
                }
            }
        }

        private void GetDropTarget(int idx, out Item item, out int r, out int c, out int rotation)
        {
            DragAndDropHolder dragDropHolder = InventoryManager.Instance.dragAndDropHolder;
            ItemHolderBehaviour itemHolder = dragDropHolder.originItemHolder;

            item = itemHolder.item;
            r = idx / containerItem.colSize;
            c = idx % containerItem.colSize;
            rotation = (itemHolder.rotation + itemHolder.deltaRotation) % 2;
        }
    }
}
