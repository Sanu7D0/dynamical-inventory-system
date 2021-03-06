using UnityEngine;
using UnityEngine.UI;

namespace DynamicInventory
{
    public class DragAndDropHolder : MonoBehaviour
    {
        [SerializeField] private Image image;
        private RectTransform rectTransform;
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
            rectTransform.rotation = holderTransform.rotation;
            image.sprite = itemHolder.item.sprite;
            originItemHolder = itemHolder;
        }

        public void Clear()
        {
            originItemHolder = null;
            image.sprite = null;
        }
    }
}
