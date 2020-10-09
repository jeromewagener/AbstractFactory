using System.Collections.Generic;
using UnityEngine;

public class ContainerShip : MonoBehaviour
{
    private const int MaxCapacity = 6;
    
    const float VehicleDumpLocationOnXAxis = 60;
    
    // References to all the vehicles already loaded onto the ship. We need the references as we want to destroy
    // the vehicles once the ship is out of the view :-)
    private readonly List<Transform> _loadedVehicles = new List<Transform>();
    
    public bool isDocked;
    
    // Some logic to load the ship from the back to front using modulus and division
    public Vector3 LoadVehicle(Transform vehicle)
    {
        Vector3 position;

        var shipPosition = transform.position;
        position.x = shipPosition.x - (_loadedVehicles.Count % 2  == 0 ? -1 : + 1);
        position.y = vehicle.GetComponent<Vehicle>().GetVehicleType().Equals(Vehicle.VehicleType.Truck) ? shipPosition.y + 0.95f : shipPosition.y + 0.65f;
        position.z = shipPosition.z + 1 - (((int)(_loadedVehicles.Count / 2)) * 2.3f);

        _loadedVehicles.Add(vehicle);

        return position;
    }

    public bool IsFull()
    {
        return MaxCapacity == _loadedVehicles.Count;
    }

    // The container ship moves towards the beach if it is not yet docked and not full. Once the beach is close, the "docked" flag gets set to stop the ship
    // If the container ship is full, it will move away from the beach to dump the vehicles.. :-) Once dumped, its no longer "full" and returns to the beach
    private void Update()
    {
        // Move to beach or stay at beach if not docked / full yet...
        if (!isDocked && !IsFull())
        {
            if (transform.position.x <= 40)
            {
                isDocked = true;
            } 
            else 
            {
                transform.Translate(Vector3.back * 5 * Time.deltaTime);
            }
        }
        
        // Go dump (destroy) the vehicles
        if (IsFull())
        {
            // Move out of sight to dump the loaded vehicles
            isDocked = false;
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            foreach (var loadedVehicle in _loadedVehicles)
            {
                loadedVehicle.Translate(Vector3.forward * 5 * Time.deltaTime);
            }
            
            // Dump all vehicles once we reach the dump coordinates
            if (transform.position.x >= VehicleDumpLocationOnXAxis)
            {
                foreach (var loadedVehicle in _loadedVehicles)
                {
                    Destroy(loadedVehicle.gameObject);
                }
                _loadedVehicles.Clear();
            }
        }
    }
}