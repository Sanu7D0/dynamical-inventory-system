using UnityEngine;
using UnityEngine.UI;

namespace DynamicInventory
{
    public class DragAndDropHolder : MonoBehaviour
    {
        private RectTransform rectTransform;
        public Image image;
        public Item originItem;

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
            rectTransform.rotation = holderTransform.rotation;
            image.sprite = itemHolder.item.sprite;
        }
    }
}
