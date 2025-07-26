using Pickups;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(Collider2D))]
    public class PickupManager : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Pickup"))
            {
                Pickup(other.GetComponent<IPickupable>());
            }
        }

        private void Pickup(IPickupable pickupable)
        {
            pickupable.OnPickedUp();
        }
    }
}
