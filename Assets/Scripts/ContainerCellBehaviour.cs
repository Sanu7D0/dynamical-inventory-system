using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DynamicInventory
{
    public class ContainerCellBehaviour : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image background;
        private ContainerBehaviour containerBehaviour;
        private bool isDragging { get { return InventoryManager.Instance.isDragging; } }
        public int index { get { return transform.GetSiblingIndex(); } }

        private void Start()
        {
            background.color = GlobalData.ItemHolderColors.idle;
        }

        public void Set(ContainerBehaviour containerBehaviour)
        {
            this.containerBehaviour = containerBehaviour;
        }

        public void SetColor(Color32 color)
        {
            background.color = color;
        }

        public void OnDrop(PointerEventData eventData)
        {
            containerBehaviour.TryDropItem(index);

            containerBehaviour.focusingIndex = -1;
            containerBehaviour.SetColorIndicator();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isDragging)
                return;

            containerBehaviour.focusingIndex = index;
            containerBehaviour.SetColorIndicator();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isDragging)
                return;

            containerBehaviour.focusingIndex = -1;
            containerBehaviour.SetColorIndicator();
        }
    }
}