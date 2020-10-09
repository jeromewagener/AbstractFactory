using UnityEngine;

public class CarFactory : VehicleFactory
{
    private const string SlowCarPrefabName = "SlowCar";
    private const string FastCarPrefabName = "FastCar";
    
    public override void CreateSlowVehicle()
    {
        var factoryTransformPosition = factoryBuildingTransform.transform.position;
        var slowCarGameObject = Resources.Load(SlowCarPrefabName) as GameObject;
        if (slowCarGameObject != null) 
        {
            var slowCar = Instantiate(slowCarGameObject.transform, new Vector3(factoryTransformPosition.x, factoryTransformPosition.y - 2 + 0.4f, factoryTransformPosition.z), Quaternion.identity);
            slowCar.gameObject.GetComponent<Car>().containerShip = containerShipTransform;
        }
        else
        {
            throw new System.ArgumentException(SlowCarPrefabName + "could not be found inside or loaded from Resources folder");
        }
    }

    public override void CreateFastVehicle()
    {
        var factoryTransformPosition = factoryBuildingTransform.transform.position;
        var fastCarGameObject = Resources.Load(FastCarPrefabName) as GameObject;
        if (fastCarGameObject != null) 
        {
            var fastCar = Instantiate(fastCarGameObject.transform, new Vector3(factoryTransformPosition.x, factoryTransformPosition.y - 2 + 0.4f, factoryTransformPosition.z), Quaternion.identity);
            fastCar.gameObject.GetComponent<Car>().containerShip = containerShipTransform;
        }
        else
        {
            throw new System.ArgumentException(FastCarPrefabName + "could not be found inside or loaded from Resources folder");
        }
    }
}
