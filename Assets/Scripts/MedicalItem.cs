using UnityEngine;

namespace DynamicInventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Dynamic Inventory/Item/Medical")]
    public class MedicalItem : Item
    {
        public override Item Init()
        {
            return ScriptableObjectExt.Clone<MedicalItem>(this);
        }

        public override void Use()
        {
            Debug.Log($"{name} used");
        }
    }
}
