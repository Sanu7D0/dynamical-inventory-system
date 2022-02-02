using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicInventory
{
    sealed public class InventoryManager : Singleton<InventoryManager>
    {
        [SerializeField] private DragAndDropHolder _dragAndDropHolder;

        public GameObject containerCellPrefab;
        public GameObject itemHolderPrefab;
        public DragAndDropHolder dragAndDropHolder { get { return _dragAndDropHolder; } }
        public InventoryController inventoryController { get; private set; }
        public bool isDragging;

        public Item[] test_items;
        public ContainerBehaviour container1;

        protected override void Awake()
        {
            base.Awake();

            inventoryController = GetComponent<InventoryController>();
        }

        private void Start()
        {
            isDragging = false;

            // Test
            for (int i = 0; i < test_items.Length; i++)
            {
                test_items[i] = test_items[i].Init();
            }

            foreach (Item item in test_items)
            {
                Debug.Log(container1.TryPushItem(item));
            }

            StartCoroutine(testRoutine());
        }

        IEnumerator testRoutine()
        {
            yield return new WaitForEndOfFrame();
            container1.UpdateContainer();
        }
    }
}
