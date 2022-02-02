using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DynamicInventory
{
    public class ItemHolderBehaviour : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Image background;

        private RectTransform rectTransform;
        private ContainerBehaviour containerBehaviour;
        private DragAndDropHolder dragAndDropHolder;
        private bool isDragging;

        public Item item { get; private set; }
        public int rotation { get; private set; }
        public int deltaRotation { get; private set; }
        public int r { get; private set; }
        public int c { get; private set; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            dragAndDropHolder = InventoryManager.Instance.dragAndDropHolder;
        }

        public void Init(Item item, int r, int c, int rotation, ContainerBehaviour containerBehaviour)
        {
            this.item = item;
            this.r = r;
            this.c = c;
            this.rotation = rotation;
            this.containerBehaviour = containerBehaviour;

            rectTransform.sizeDelta = GlobalData.cellSize * new Vector2(item.colLength, item.rowLength);
            itemImage.sprite = item.sprite;

            isDragging = false;
            deltaRotation = 0;
        }

        public void Set(int r, int c, int rotation)
        {
            this.r = r;
            this.c = c;
            this.rotation = rotation;
        }

        public void SetContainer(ContainerBehaviour target)
        {
            // Remove data stored in origin
            containerBehaviour.itemHolderMap.Remove(item.GetInstanceID());

            containerBehaviour = target;
            rectTransform.SetParent(target.itemHolders);
        }

        public void PullSelf()
        {
            containerBehaviour.TryPullItem(item);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (item == null)
                return;

            isDragging = true;

            dragAndDropHolder.gameObject.SetActive(true);
            dragAndDropHolder.ImitateItemHolder(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging)
                return;

            // if shift
            // deltaRotation = ++deltaRotation % 2;

            dragAndDropHolder.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isDragging)
            {

            }

            isDragging = false;
            deltaRotation = 0;

            dragAndDropHolder.Clear();
            dragAndDropHolder.gameObject.SetActive(false);
        }
    }
}
