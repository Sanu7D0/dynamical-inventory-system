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
        [SerializeField] private RectTransform itemHolders;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject itemHolderPrefab;
        [SerializeField] private DragAndDropHolder dragAndDropHolder;

        private struct ItemHolder
        {
            public ItemHolder(RectTransform transform, int r, int c, int rotation)
            {
                this.transform = transform;
                this.r = r;
                this.c = c;
                this.rotation = rotation;
            }

            public RectTransform transform { get; }
            public int r, c;
            public int rotation;
        }
        private Dictionary<int, ItemHolder> itemHolderMap;

        public delegate void ContainerChangeHandler();
        public event ContainerChangeHandler OnContainerChanged;

        public int CELL_SIZE = 100;

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
            itemHolderMap = new Dictionary<int, ItemHolder>();

            int col = containerItem.colSize;
            int row = containerItem.rowSize;
            containerGrid.sizeDelta = CELL_SIZE * new Vector2(col, row);

            GridLayoutGroup grid = containerGrid.GetComponent<GridLayoutGroup>();
            grid.constraintCount = col;

            for (int i = 0; i < row * col; i++)
            {
                GameObject cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity);
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

        public Item PullItem(int id)
        {
            // TODO
            Item item = containerItem.PullItem(0, 0, 0);
            if (item != null)
            {

                OnContainerChanged?.Invoke();
            }
            return item;
        }

        public void UpdateContainer()
        {
            RectTransform cell;
            foreach (ItemHolder itemHolder in itemHolderMap.Values)
            {
                cell = containerGrid.GetChild(
                    itemHolder.r * containerItem.colSize + itemHolder.c) as RectTransform;

                itemHolder.transform.position = cell.position;
                if (itemHolder.rotation == 1)
                {
                    itemHolder.transform.Rotate(new Vector3(0, 0, -90));
                    itemHolder.transform.position += Vector3.down * CELL_SIZE;
                }
            }
        }

        private RectTransform CreateItemHolder(Item item)
        {
            RectTransform newItemHolder =
                GameObject.Instantiate(itemHolderPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<RectTransform>();
            newItemHolder.SetParent(itemHolders);
            newItemHolder.sizeDelta = CELL_SIZE * new Vector2(item.colLength, item.rowLength);

            newItemHolder.GetComponent<ItemHolderBehaviour>()
                .Init(item, this, dragAndDropHolder);

            return newItemHolder;
        }

        private void CheckItemHolder(Item item, int r, int c, int rotation)
        {
            int id = item.GetInstanceID();
            if (itemHolderMap.ContainsKey(id))
            {
                ItemHolder itemHolder = itemHolderMap[id];
                itemHolder.r = r;
                itemHolder.c = c;
                itemHolder.rotation = rotation;
            }
            else
            {
                RectTransform newItemHolder = CreateItemHolder(item);
                itemHolderMap.Add(id, new ItemHolder(newItemHolder, r, c, rotation));
            }
        }
    }
}
