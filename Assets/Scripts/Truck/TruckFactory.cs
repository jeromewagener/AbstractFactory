using UnityEngine;

public class TruckFactory : VehicleFactory
{
    private const string SlowTruckPrefabName = "SlowTruck";
    private const string FastTruckPrefabName = "FastTruck";
    
    public override void CreateSlowVehicle()
    {
        var factoryTransformPosition = factoryBuildingTransform.transform.position;
        var slowTruckGameObject = Resources.Load(SlowTruckPrefabName) as GameObject;
        if (slowTruckGameObject != null) 
        {
            var slowTruck = Instantiate(slowTruckGameObject.transform, new Vector3(factoryTransformPosition.x, factoryTransformPosition.y - 2 + 0.66f, factoryTransformPosition.z), Quaternion.identity);
            slowTruck.gameObject.GetComponent<Truck>().containerShip = containerShipTransform;
        }
        else
        {
            throw new System.ArgumentException(SlowTruckPrefabName + "could not be found inside or loaded from Resources folder");
        }
    }

    public override void CreateFastVehicle()
    {
        var factoryTransformPosition = factoryBuildingTransform.transform.position;
        var fastTruckGameObject = Resources.Load(FastTruckPrefabName) as GameObject;
        if (fastTruckGameObject != null) 
        {
            var fastTruck = Instantiate(fastTruckGameObject.transform, new Vector3(factoryTransformPosition.x, factoryTransformPosition.y - 2 + 0.66f , factoryTransformPosition.z), Quaternion.identity);
            fastTruck.gameObject.GetComponent<Truck>().containerShip = containerShipTransform;
        }
        else
        {
            throw new System.ArgumentException(FastTruckPrefabName + "could not be found inside or loaded from Resources folder");
        }
    }
}
