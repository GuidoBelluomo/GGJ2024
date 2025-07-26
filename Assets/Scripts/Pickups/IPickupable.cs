namespace Pickups
{
    public interface IPickupable
    {
        public void OnPickedUp();
        public bool AutomaticPickup();
    }
}
