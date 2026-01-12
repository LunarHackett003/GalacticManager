using System;
using UnityEngine;

public class BuildController : MonoBehaviour
{

    public static BuildController Instance;

    public BuildingData currentBuildingToPlace;

    public Transform buildingPreview;
    public GameObject validPlaceMesh, invalidPlaceMesh;
    public bool setUpCorrectly;

    public Vector3 currentBuildPos;

    public bool buildModeActive;
    public bool canBuildHere;

    private void Start()
    {
        if (InputManager.Instance != null)
        {
            Debug.Log("correctly set up the game!");
            setUpCorrectly = true;

            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                return;
            }

                InputManager.OnInteractPerform += BuildInteracted;

            if (currentBuildingToPlace)
            {
                //for debug purposes, if we start with a building wanting to be placed, we'll try to place it.
                BeginPlacement(currentBuildingToPlace);
            }
        }
    }
    private void OnDestroy()
    {
        InputManager.OnInteractPerform -= BuildInteracted;
    }

    private void BuildInteracted(bool secondary, RaycastHit hit, Interactable interactableHit)
    {
        //We can probably ignore the hit interactable until we have destruction and stuff.
        Debug.Log("Tried to build!");
        if (setUpCorrectly && canBuildHere && buildModeActive && !secondary)
        {
            GameObject newBuilding = Instantiate(currentBuildingToPlace.buildingObject.gameObject, currentBuildPos, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (!setUpCorrectly)
            return;

        if(currentBuildingToPlace && buildModeActive)
        {
            DefaultPlaceBehaviour();
        }
    }

    public void BeginPlacement(BuildingData newBuilding)
    {
        currentBuildingToPlace = newBuilding;
        buildingPreview.localScale = new(currentBuildingToPlace.buildingSize.x, 1, currentBuildingToPlace.buildingSize.y);
        buildModeActive = true;
    }

    public void EndPlacement()
    {
        currentBuildingToPlace = null;
        buildingPreview.gameObject.SetActive(false);
        buildModeActive = false;
    }

    public void DefaultPlaceBehaviour()
    {
        if (currentBuildingToPlace.useCustomPlacementBehaviour)
        {
            currentBuildingToPlace.DoCustomPlacementBehaviour(InputManager.Instance.hitPos);
            return;
        }

        canBuildHere = InputManager.Instance.currentTargeted == null && InputManager.Instance.currentHit.normal == Vector3.up;

        //If DidHit is not equal to our current preview state, ensure it is. duh.
        if((InputManager.Instance.didHit && canBuildHere) != buildingPreview.gameObject.activeInHierarchy)
            buildingPreview.gameObject.SetActive(InputManager.Instance.didHit && canBuildHere);

        if (canBuildHere)
        {
            if (currentBuildingToPlace.allowSubGridBuilding)
            {
                if(currentBuildingToPlace.useSubGridSnap)
                {
                    currentBuildPos = Snapping.Snap(InputManager.Instance.hitPos, currentBuildingToPlace.subGridSnapSize, SnapAxis.X | SnapAxis.Z);
                }
                else
                {
                    currentBuildPos = InputManager.Instance.hitPos;
                }
            }
            else
            {
                
                currentBuildPos =  Snapping.Snap(InputManager.Instance.hitPos, Vector3.one, SnapAxis.X | SnapAxis.Z);
            }
        }

        buildingPreview.position = currentBuildPos;

    }
}
