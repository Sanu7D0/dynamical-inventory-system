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
        [SerializeField] private GameObject containerCellPrefab;
        [SerializeField] private GameObject itemObjectPrefab;

        private void Awake()
        {
            containerItem = containerItem.Init() as ContainerItem;
        }

        private void OnEnable()
        {
            containerItem.OnContainerChanged += UpdateContainer;
        }

        private void Start()
        {
            // TODO: 100 -> cell size
            int col = containerItem.colSize;
            int row = containerItem.rowSize;
            containerGrid.sizeDelta = 100 * new Vector2(col, row);

            GridLayoutGroup grid = containerGrid.GetComponent<GridLayoutGroup>();
            grid.constraintCount = col;

            for (int i = 0; i < row * col; i++)
            {
                GameObject cell = Instantiate(containerCellPrefab, Vector3.zero, Quaternion.identity);
                cell.transform.SetParent(grid.transform);
            }
        }

        private void OnDisable()
        {
            containerItem.OnContainerChanged -= UpdateContainer;
        }

        public void UpdateContainer()
        {
            Item[,] container = containerItem.container;
            for (int r = 0; r < containerItem.rowSize; r++)
            {
                for (int c = 0; c < containerItem.colSize; c++)
                {
                    Item item = container[r, c];
                    if (item != null)
                    {
                        RectTransform cell =
                            containerGrid.GetChild(r * (c / containerItem.rowSize) + c) as RectTransform;
                        cell.GetComponentInChildren<Image>().sprite = item.sprite;
                    }
                }
            }
        }
    }
}
