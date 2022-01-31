using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DynamicInventory
{
    public class ItemHolderBehaviour : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        public Image image { get { return _image; } }
        [SerializeField] private Image _background;
        public Image background { get { return _background; } }

        public Item item { get; private set; }
        private ContainerBehaviour containerBehaviour;
        private DragAndDropHolder dragAndDropHolder;

        private bool isDragging;

        public void Init(Item item, ContainerBehaviour containerBehaviour, DragAndDropHolder dragAndDropHolder)
        {
            this.item = item;
            this.containerBehaviour = containerBehaviour;
            this.dragAndDropHolder = dragAndDropHolder;

            image.sprite = item.sprite;
            // TODO: width, height

            isDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (item == null)
                return;

            dragAndDropHolder.gameObject.SetActive(true);
            dragAndDropHolder.ImitateItemHolder(this);

            isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                dragAndDropHolder.transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // get r, c
            // if (containerItem.TryPutItem(r, c))

            dragAndDropHolder.gameObject.SetActive(false);
        }
    }
}
