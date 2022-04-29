using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Direction3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public Transform3D.DirectionAxis axis = Transform3D.DirectionAxis.NONE;

    [SerializeField] private List<Renderer> renderers = new List<Renderer>();
    [SerializeField] private Material highlightedAxisMaterial;
    
    private Transform3D transform3D;
    private bool isInitialized;
    private List<Material> cachedRendererMaterials = new List<Material>();

    public void Initialize(Transform3D _parent)
    {
        transform3D = _parent;
        isInitialized = true;
    }

    public void Initialize(Transform3D _parent, Transform3D.DirectionAxis _axis)
    {
        transform3D = _parent;
        axis = _axis;
        isInitialized = true;
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInitialized)
            return;
        
        SetHoverState(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInitialized)
            return;

        SetHoverState(false);
    }

    private void SetHoverState(bool hasPointerEntered)
    {
        if (hasPointerEntered)
        {
            Highlight();
        }
        else
        {
            UnHighlight();
        }
    }

    private void Highlight()
    {
        cachedRendererMaterials.Clear();

        // Enable highlight material / shader
        foreach (Renderer renderer in renderers)
        {
            cachedRendererMaterials.Add(renderer.material);
            renderer.material = highlightedAxisMaterial;
        }
    }

    private void UnHighlight()
    {
        // Disable highlight material / shader
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = cachedRendererMaterials[i];
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isInitialized)
            return;
        
        transform3D.OnPointerDown(eventData, this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isInitialized)
            return;
        
        transform3D.OnPointerUp(this);
    }
}
