using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicInventory
{
    sealed public class InventoryManager : Singleton<InventoryManager>
    {
        public ContainerItem test_container;
        public WeaponItem test_axe;

        public RectTransform test_containerGrid;
        public GameObject itemCellPrefab;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            // TODO: 100 -> cell size
            int col = test_container.colSize;
            int row = test_container.rowSize;
            test_containerGrid.sizeDelta = 100 * new Vector2(col, row);

            GridLayoutGroup grid = test_containerGrid.GetComponent<GridLayoutGroup>();
            grid.constraintCount = col;

            for (int i = 0; i < row * col; i++)
            {
                GameObject cell = Instantiate(itemCellPrefab, Vector3.zero, Quaternion.identity);
                cell.transform.SetParent(grid.transform);
            }

            test_container = test_container.Init() as ContainerItem;
            test_axe = test_axe.Init() as WeaponItem;

            test_axe.Rotate(1);
            Debug.Log(test_container.TryPushItem(test_axe));
        }
    }
}
