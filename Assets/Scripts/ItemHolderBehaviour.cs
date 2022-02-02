using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DynamicInventory
{
    public class ItemHolderBehaviour : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Image background;

        private RectTransform rectTransform;
        private ContainerBehaviour containerBehaviour;
        private DragAndDropHolder dragAndDropHolder;
        private bool isDragging
        {
            get { return InventoryManager.Instance.isDragging; }
            set { InventoryManager.Instance.isDragging = value; }
        }
        private Color32 idleColor;

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
            idleColor = GlobalData.ItemHolderColors.ItemColor(item.itemType);
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
            SetColor(GlobalData.ItemHolderColors.idle);

            deltaRotation = 0;
        }

        public void Set(int r, int c, int rotation)
        {
            this.r = r;
            this.c = c;
            this.rotation = rotation % 2;

            rectTransform.rotation = (rotation == 1) ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
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

        private void HandleRotate()
        {
            if (!isDragging)
                return;

            if (item.rowLength == item.colLength)
                return;

            deltaRotation = (deltaRotation + 1) % 2;
            dragAndDropHolder.Rotate(rotation + deltaRotation);

            containerBehaviour.SetColorIndicator(true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;

            background.raycastTarget = false;
            dragAndDropHolder.gameObject.SetActive(true);
            dragAndDropHolder.ImitateItemHolder(this);

            PullSelf();

            InventoryManager.Instance.inventoryController.OnRotate += HandleRotate;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging)
                return;

            dragAndDropHolder.rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDragging)
                return;

            isDragging = false;
            background.raycastTarget = true;

            deltaRotation = 0;
            dragAndDropHolder.Clear();
            dragAndDropHolder.gameObject.SetActive(false);

            InventoryManager.Instance.inventoryController.OnRotate -= HandleRotate;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isDragging)
                return;

            SetColor(GlobalData.ItemHolderColors.focused);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isDragging)
                return;

            SetColor(GlobalData.ItemHolderColors.idle);
        }

        private void SetColor(Color32 color)
        {
            background.color = color;
        }
    }
}
