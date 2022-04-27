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
    [SerializeField] private Direction3D x;
    [SerializeField] private Direction3D y;
    [SerializeField] private Direction3D z;
    [SerializeField] private Direction3D currentAxis;

    private bool isInitialized;
    private PointerEventData cachedPointerData;
    private Vector3 startingTransformPosition;

    [SerializeField] private bool isHolding;
    private Vector3 startPosition;
    private Vector3 endPosition;

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
            endPosition = cachedPointerData.position;
            
            // Screenspace input line
            Debug.DrawLine(startPosition, endPosition, Color.black);

            Color currentAxisColor = Color.white;

            if (currentAxis == x)
            {
                endPosition = new Vector3(endPosition.x, startPosition.y, startPosition.z);
                currentAxisColor = Color.red;
            }else if (currentAxis == y)
            {
                endPosition = new Vector3(startPosition.x, endPosition.y, startPosition.z);
                currentAxisColor = Color.green;
            }
            else if (currentAxis == z)
            {
                endPosition = new Vector3(startPosition.x, startPosition.y, endPosition.z);
                currentAxisColor = Color.blue;
            }

            Vector3 distance = (endPosition - startPosition) * 0.01f;
            transform.position = startingTransformPosition + distance;

            // Axis input line
            Debug.DrawLine(startPosition, endPosition, currentAxisColor);

            SelectionManager.instance.GetLastSelection().transform.position = transform.position;
        }
    }

    public void OnPointerDown(PointerEventData eventData, Direction3D _direction3D)
    {
        if (!isInitialized)
            return;

        Debug.Log("Axis changed! " + _direction3D.axis);
        currentAxis = _direction3D;

        startingTransformPosition = transform.position;
        cachedPointerData = eventData;
        isHolding = true;
        startPosition = eventData.position;
    }
    
    public void OnPointerUp(PointerEventData eventData, Direction3D _direction3D)
    {
        if (!isInitialized)
            return;
        
        endPosition = eventData.position;
        isHolding = false;
    }
}
