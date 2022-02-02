using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DynamicInventory
{
    public class ContainerCellBehaviour : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image background;
        private ContainerBehaviour containerBehaviour;

        public void Set(ContainerBehaviour containerBehaviour)
        {
            this.containerBehaviour = containerBehaviour;
        }

        public void OnDrop(PointerEventData eventData)
        {
            containerBehaviour.DropItem(transform.GetSiblingIndex());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // background.color
            // Debug.Log("Pointer entered");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Debug.Log("Pointer exited");
        }
    }
}