using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Client : MonoBehaviour
{
    private const int MaxNumberOfFactoryBuildings = 3;
    private const string FactoryBuildingPrefabName = "Factory";
    private const string ContainerShipPrefabName = "ContainerShip";
    
    // The client class keeps references to the factory building transforms. We do this to be able to keep the same
    // object/building in the scene but change the underlying vehicle factory implementation on-the-fly
    // Each time we instantiate a new vehicle factory, the corresponding factory building transform reference
    // needs to be assigned.
    private readonly Transform[] _factoryBuildings = new Transform[MaxNumberOfFactoryBuildings];

    // We also keep references to all container ships as the vehicle factory needs to know where to send the newly
    // produced vehicles. Each time we instantiate a new vehicle factory, the corresponding container ship reference
    // needs to be assigned.
    private readonly Transform[] _containerShips = new Transform[MaxNumberOfFactoryBuildings];

    public TMP_Dropdown buildingDropdown;
    public TMP_Dropdown factoryTypeDropdown;

    private void Start()
    {
        InvokeRepeating(nameof(ProduceVehicles), 5, 3);
    }

    public void ProduceVehicles()
    {
        for (var i = 0; i < MaxNumberOfFactoryBuildings; i++)
        {
            // Skip if the no factory
            if (_factoryBuildings[i] == null || _factoryBuildings[i].GetComponent<VehicleFactory>() == null) continue;

            // Skip if no ship is docked or if ship is full
            if (!_containerShips[i].GetComponent<ContainerShip>().isDocked ||
                _containerShips[i].GetComponent<ContainerShip>().IsFull()) continue;
            
            // Produce at random slow or fast vehicles
            if (Random.Range(0, 2) == 0)
            {
                _factoryBuildings[i].GetComponent<VehicleFactory>().CreateFastVehicle();
            }
            else
            {
                _factoryBuildings[i].GetComponent<VehicleFactory>().CreateSlowVehicle();
            }
        }
    }
    
    // This method is called whenever the "Assign" button is clicked in the user interface.
    // The method checks which building is selected and which factory implementation is needed.
    // If a building already exists, the existing building transform is reused. Otherwise it is created.
    // The same goes for the container ship
    public void AssignFactoryToBuilding()
    {
        Transform factoryBuildingTransform = null;
        Transform containerShipTransform = null;

        switch (buildingDropdown.captionText.text)
        {
            case "Building A":
                CreateFactoryBuildingAndShip(0, 48, out factoryBuildingTransform, out containerShipTransform);
                break;
            
            case "Building B":
                CreateFactoryBuildingAndShip(1, 34, out factoryBuildingTransform, out containerShipTransform);
                break;
            
            case "Building C":
                CreateFactoryBuildingAndShip(2, 20, out factoryBuildingTransform, out containerShipTransform);
                break;
        }

        if (factoryBuildingTransform == null)
        {
            Debug.Log("Could not initialize or load factory transform");
            return;
        }

        // Before attaching a new vehicle factory implementation (script) to the factory building, we need to deactivate the transform
        // This is necessary as the transform might otherwise access the factory implementation (script) while we are trying to 
        // change it which would cause all sorts of issues...
        factoryBuildingTransform.gameObject.SetActive(false);

        // Out of laziness, we simply destroy any existing / attached vehicle factory implementation even if the same one is assigned again :-)
        VehicleFactory vehicleFactory = factoryBuildingTransform.gameObject.GetComponent<VehicleFactory>();
        Destroy(vehicleFactory);

        // Depending on the selected vehicle factory implementation type, we then assign a new vehicle factory implementation (script) to the transform
        switch (factoryTypeDropdown.captionText.text)
        {
            case "CarFactory":
                CarFactory carFactory = factoryBuildingTransform.gameObject.AddComponent<CarFactory>();
                carFactory.containerShipTransform = containerShipTransform;
                carFactory.factoryBuildingTransform = factoryBuildingTransform;
                break;
            case "TruckFactory":
                TruckFactory truckFactory = factoryBuildingTransform.gameObject.AddComponent<TruckFactory>();
                truckFactory.containerShipTransform = containerShipTransform;
                truckFactory.factoryBuildingTransform = factoryBuildingTransform;
                break;
        }

        // Finally we reactivate the transform and let the newly attached vehicle factory implementation do its magic...
        factoryBuildingTransform.gameObject.SetActive(true);
    }

    private Transform CreateContainerShip(Transform factoryBuilding)
    {
        var factoryTransformPosition = factoryBuilding.position;
        var containerShip = Resources.Load(ContainerShipPrefabName) as GameObject;
        Transform containerShipTransform;

        if (containerShip != null)
        {
            containerShipTransform = Instantiate(containerShip.transform, new Vector3(70, factoryTransformPosition.y - 2f, factoryTransformPosition.z), Quaternion.identity);
        }
        else
        {
            throw new System.ArgumentException(ContainerShipPrefabName + "could not be found inside or loaded from Resources folder");
        }

        containerShipTransform.Rotate(0f, 90f, 0f, Space.Self);

        return containerShipTransform;
    }

    private Transform CreateFactoryBuilding(int zAxisPosition)
    {
        var factory = Resources.Load(FactoryBuildingPrefabName) as GameObject;
        if (factory == null) throw new System.ArgumentException(FactoryBuildingPrefabName + "could not be found inside or loaded from Resources folder");
        Transform newFactory = Instantiate(factory.transform, new Vector3(16.80779f, 14.1f, zAxisPosition), Quaternion.identity);
        newFactory.transform.Rotate(0f, 90f, 0f, Space.Self);
        return newFactory;
    }

    private void CreateFactoryBuildingAndShip(int arrayPosition, int zAxisPosition, out Transform factoryBuildingTransform, out Transform containerShipTransform)
    {
        if (_factoryBuildings[arrayPosition] == null)
        {
            // Create transforms for factory building and ship
            _factoryBuildings[arrayPosition] = CreateFactoryBuilding(zAxisPosition);
            _containerShips[arrayPosition] = CreateContainerShip(_factoryBuildings[arrayPosition]);
            factoryBuildingTransform = _factoryBuildings[arrayPosition];
            containerShipTransform = _containerShips[arrayPosition];
        }
        else
        {
            // Reuse existing transforms
            factoryBuildingTransform = _factoryBuildings[arrayPosition];
            containerShipTransform = _containerShips[arrayPosition];
        }
    }
}