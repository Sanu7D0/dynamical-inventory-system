using UnityEngine;
using UnityEngine.InputSystem;

namespace DynamicInventory
{
    public class InventoryController : MonoBehaviour
    {
        PlayerInput playerInput;
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        public delegate void ActionHandler();
        public event ActionHandler OnRotate;

        private void OnEnable()
        {
            playerInput.onActionTriggered += HandleAction;
        }

        private void OnDisable()
        {
            playerInput.onActionTriggered -= HandleAction;
        }

        private void HandleAction(InputAction.CallbackContext context)
        {
            switch (context.action.name)
            {
                case "Rotate":
                    if (context.started)
                        OnRotate?.Invoke();
                    break;
            }
        }
    }
}
