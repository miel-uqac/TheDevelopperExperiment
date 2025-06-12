using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System;

public class CubeInteractable : MonoBehaviour
{
    public event Action OnReleased;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(_ => OnReleased?.Invoke());
    }
}
