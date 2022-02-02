using UnityEngine;
using UnityEngine.EventSystems;

namespace DynamicInventory
{
    public class ContainerCellBehaviour : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            ContainerBehaviour containerBehaviour = GetComponentInParent<ContainerBehaviour>();
            containerBehaviour.DropItem(transform.GetSiblingIndex());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Debug.Log("Pointer entered");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Debug.Log("Pointer exited");
        }
    }
}