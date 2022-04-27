using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Transform3D : MonoBehaviour
{
    public enum DirectionAxis
    {
        NONE,
        X,
        Y,
        Z
    }

    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask cubeInputLayer;
    [SerializeField] private Direction3D x;
    [SerializeField] private Direction3D y;
    [SerializeField] private Direction3D z;
    [SerializeField] private Direction3D currentAxis;

    private bool isInitialized;

    [SerializeField] private bool isHolding;
    private PointerEventData cachedPointerData;
    private GameObject inputCube;
    private Vector3 startingPosition;

    private void Start()
    {
        isInitialized = Setup();
    }

    /// <summary>
    /// Returns true if we have references to all 3 Direction3D components.
    /// </summary>
    /// <returns></returns>
    private bool Setup()
    {
        if (x != null && y != null && z != null)
        {
            x.Initialize(this, DirectionAxis.X);
            y.Initialize(this, DirectionAxis.Y);
            z.Initialize(this, DirectionAxis.Z);
            
            return true;
        }
        
        Debug.LogWarning("Unable to setup this Transform3D successfully." +
                         "One or more directions are missing.", gameObject);
        return false;
    }

    
    private void Update()
    {
        if (!isInitialized)
            return;

        if (isHolding)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(cachedPointerData.position);

            if (Physics.Raycast(ray, out hit, camera.farClipPlane, cubeInputLayer))
            {
                if (currentAxis == z)
                {
                    inputCube.transform.position = new Vector3(startingPosition.x, startingPosition.y, hit.point.z);
                }
                else if (currentAxis == x)
                {
                    inputCube.transform.position = new Vector3(hit.point.x, startingPosition.y, startingPosition.z);
                }
                else if (currentAxis == y)
                {
                    inputCube.transform.position = new Vector3(startingPosition.x, hit.point.y, startingPosition.z);
                }

                transform.position = inputCube.transform.position;

                foreach (Selection selection in SelectionManager.instance.GetSelections())
                {
                    selection.transform.position = inputCube.transform.position;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData pointerData, Direction3D _direction3D)
    {
        if (!isInitialized)
            return;

        Debug.Log("Axis changed! " + _direction3D.axis);
        currentAxis = _direction3D;
        startingPosition = transform.position;
        cachedPointerData = pointerData;

        GeneratePlane(_direction3D);

        isHolding = true;
    }

    private void GeneratePlane(Direction3D direction3D)
    {
        inputCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        inputCube.GetComponent<Renderer>().enabled = false;
        inputCube.transform.localScale = new Vector3(100, 100, 0.01f);
        inputCube.transform.position = direction3D.transform.position;
        inputCube.layer = LayerMask.NameToLayer("CubeInput");

        // Rotate plane to match movement direction
        if (direction3D == z)
        {
            inputCube.transform.eulerAngles = new Vector3(-90, 0, 0);
        }
    }

    public void OnPointerUp(Direction3D _direction3D)
    {
        if (!isInitialized)
            return;
        
        isHolding = false;
        Destroy(inputCube);
    }
}
