using UnityEngine;
using UnityEngine.UI;

namespace DynamicInventory
{
    public class DragAndDropHolder : MonoBehaviour
    {
        [SerializeField] private Image image;
        public RectTransform rectTransform { get; private set; }
        public ItemHolderBehaviour originItemHolder { get; private set; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ImitateItemHolder(ItemHolderBehaviour itemHolder)
        {
            RectTransform holderTransform = itemHolder.GetComponent<RectTransform>();
            rectTransform.sizeDelta = holderTransform.rect.size;
            image.sprite = itemHolder.item.sprite;
            originItemHolder = itemHolder;
            Rotate(itemHolder.rotation);
        }

        public void Rotate(int rotation)
        {
            rotation %= 2;
            rectTransform.rotation = (rotation == 1) ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
        }

        public void Clear()
        {
            originItemHolder = null;
            image.sprite = null;
        }
    }
}
