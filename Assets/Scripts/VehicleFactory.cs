using UnityEngine;

public abstract class VehicleFactory : MonoBehaviour
{
    // A reference to the factory building transform (object) which produces the vehicles (i.e. they roll out of it)
    // Initialized when creating instantiating the factory implementation 
    public Transform factoryBuildingTransform;

    // A reference to the container ship transform (object) which picks up the vehicles from the factory.
    // Initialized when creating instantiating the factory implementation 
    public Transform containerShipTransform;
    
    // Create a slow car or truck or ... depending on the concrete factory implementation
    public abstract void CreateSlowVehicle();
    
    // Create a fast car or truck or ... depending on the concrete factory implementation
    public abstract void CreateFastVehicle();
}
