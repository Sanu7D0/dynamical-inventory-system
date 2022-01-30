using UnityEngine;

namespace DynamicInventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Dynamic Inventory/Item/Weapon")]
    public class WeaponItem : Item
    {
        public override Item Init()
        {
            return ScriptableObjectExt.Clone<WeaponItem>(this);
        }

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}