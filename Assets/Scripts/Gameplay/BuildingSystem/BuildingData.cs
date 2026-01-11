using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable Objects/BuildingData")]
public class BuildingData : ScriptableObject
{
    /// <summary>
    /// The display name of this building
    /// </summary>
    public string buildingDisplayName;
    public Building buildingObject;
    /// <summary>
    /// How many grid tiles this building occupies. Must be an integer.
    /// </summary>
    public Vector2 buildingSize;
    /// <summary>
    /// If true, this building will not be checked for collisions when trying to build it.
    /// </summary>
    public bool ignoreCollision;
    /// <summary>
    /// If true, multiple of this object can be build in a single grid space.
    /// </summary>
    public bool allowSubGridBuilding;
    /// <summary>
    /// a range between 0.01f and 1f, determines the snap increment. If set below 0.01f, it will not snap. The Y axis is ignored.
    /// </summary>
    public Vector3 subGridSnapSize;
    /// <summary>
    /// If false, this building will ignore snapping.
    /// </summary>
    public bool useSubGridSnap;
    /// <summary>
    /// Should this object snap to a building's defined SnapPoint by a specific tag, when moved close enough?
    /// </summary>
    public bool snapByTag;
    /// <summary>
    /// Which objects, by tag, to snap this object to
    /// </summary>
    public string[] snapTags;
    /// <summary>
    /// How much it costs to build this building
    /// </summary>
    public int buildCost;
    /// <summary>
    /// How much it costs to destroy this building. Set to negative to reimburse funds.
    /// </summary>
    public int destroyCost;
    /// <summary>
    /// How long, in seconds, it takes to build this structure.
    /// </summary>
    public int buildTime;
    /// <summary>
    /// The prefab to spawn whilst this is in progress.
    /// </summary>
    public GameObject buildProgressPrefab;

    /// <summary>
    /// For some objects, such as power cables, you may need custom placement behaviour. If true, the building you're trying to place will use the CustomPlacementBehaviour.
    /// <br></br>Only works on CHILD CLASSES of <see cref="BuildingData"/>
    /// </summary>
    public bool useCustomPlacementBehaviour;


    public virtual void InitialiseCustomPlacement(Building thisBuilding)
    {

    }

    public virtual void DoCustomPlacementBehaviour(Vector3 cursorWorldPos)
    {

    }

}
