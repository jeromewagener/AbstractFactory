using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    private const float ContainerShipPickupLocationOnXAxis = 33.5f;
    
    // Each vehicle implementation specifies its own movement speed
    protected float MovementSpeed = 0f;
    
    // To be able to change the factory on-the-fly, the vehicle need to know which container-ship it belongs too during runtime
    public Transform containerShip;
    
    // Just a flag to know if the vehicle is on the container ship, ready to be "delivered" ;-) ...
    private bool _isOnContainerShip;
    
    public enum VehicleType
    {
        Car,
        Truck
    }

    public abstract VehicleType GetVehicleType();
    
    // If a vehicle is not on a container ship, and if a vehicle is not yet waiting at the beach, (pickup location)
    // we let it move towards the beach (pickup location). Once it has reached the pickup location, we move the vehicle
    // onto the container ship
    void Update()
    {
        if (!_isOnContainerShip)
        {
            if (transform.position.x <= ContainerShipPickupLocationOnXAxis)
            {
                // Move to beach (pickup location)
                transform.Translate(Vector3.right * (MovementSpeed * Time.deltaTime));   
            }
            else
            {
                // Wait if the ship is full
                if (containerShip.GetComponent<ContainerShip>().isDocked && !containerShip.GetComponent<ContainerShip>().IsFull())
                {
                    // Park on ship
                    Vector3 loadPosition = containerShip.GetComponent<ContainerShip>().LoadVehicle(transform);
                    transform.position = loadPosition;
                    transform.Rotate(0f,90f,0f,Space.Self);
                    _isOnContainerShip = true;
                }
            }
        }
    }
}