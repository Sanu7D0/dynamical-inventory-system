using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicInventory
{
    sealed public class InventoryManager : Singleton<InventoryManager>
    {
        public Item[] test_items;

        public ContainerBehaviour container1;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
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
