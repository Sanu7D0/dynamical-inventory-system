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

        private void Awake()
        {
            containerItem = containerItem.Init() as ContainerItem;
        }

        private void OnEnable()
        {
            OnContainerChanged += UpdateContainer;
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

        public bool TryPullItem(Item item, bool clearMap = false)
        {
            if (containerItem.TryPullItem(item))
            {
                if (clearMap)
                {
                    itemHolderMap.Remove(item.GetInstanceID());
                }

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
                if (itemHolder.rotation == 1)
                {
                    itemHolder.transform.Rotate(new Vector3(0, 0, -90));
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

        public void DropItem(int index)
        {
            Debug.Log($"Received item drop on {index}");
            DragAndDropHolder dragDropHolder = InventoryManager.Instance.dragAndDropHolder;
            ItemHolderBehaviour itemHolder = dragDropHolder.originItemHolder;

            // Pull the item before put it
            bool isTransferred = false;
            if (!containerItem.TryPullItem(itemHolder.item))
            {
                // Item holder is from another container -> transfer later
                isTransferred = true;
            }

            int targetR = index / containerItem.colSize,
                targetC = index % containerItem.colSize,
                targetRotaion = itemHolder.rotation + itemHolder.deltaRotation;
            if (containerItem.TryPutItem(itemHolder.item, targetR, targetC, targetRotaion))
            {
                // Succeed to put item
                if (isTransferred)
                {
                    // If transfereed, pull from the origin container and change container
                    itemHolder.PullSelf();

                    itemHolder.SetContainer(this);
                    itemHolderMap.Add(itemHolder.item.GetInstanceID(), itemHolder);
                }
                itemHolder.Set(targetR, targetC, targetRotaion);

                OnContainerChanged?.Invoke();
            }
            else
            {
                // Failed to put item -> back to original position
                if (!isTransferred)
                {
                    containerItem.TryPutItem(itemHolder.item, itemHolder.r, itemHolder.c, itemHolder.rotation);
                }
            }
        }
    }
}
