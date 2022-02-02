using UnityEngine;

namespace DynamicInventory
{
    public static class GlobalData
    {
        public static readonly int cellSize = 100;

        public static class ItemHolderColors
        {
            public static readonly Color32 idle = new Color32(0, 0, 0, 100);
            public static readonly Color32 focused = new Color32(100, 100, 100, 100);
            public static readonly Color32 red = new Color32(200, 0, 0, 150);
            public static readonly Color32 green = new Color32(0, 200, 0, 150);

            public static Color32 ItemColor(ItemType type)
            {
                switch (type)
                {
                    case ItemType.Weapon:
                        return idle;
                    default:
                        return idle;
                }
            }
        }
    }
}
