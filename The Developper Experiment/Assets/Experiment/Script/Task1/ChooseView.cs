using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChooseView : MonoBehaviour
{
    public List<Material> screenMaterial;

    public int currentScreen = 0;

    public InputActionReference inputActionNext;
    public InputActionReference inputActionPrevious;
    public InputActionReference inputActionFinish;
    public PlacementChecker Manager;
    public GameObject text;

    void Start()
    {
        ChangeMaterial(screenMaterial[currentScreen]);
        inputActionNext.action.performed += OnNextView;
        inputActionPrevious.action.performed += OnPreviousView;
        inputActionFinish.action.performed += FinishSelection;
    }

    private void OnNextView(InputAction.CallbackContext context)
    {
        currentScreen++;

        if (currentScreen == screenMaterial.Count)
        {
            currentScreen = 0;
        }

        ChangeMaterial(screenMaterial[currentScreen]);
    }

    private void OnPreviousView(InputAction.CallbackContext context)
    {
        currentScreen--;

        if (currentScreen == -1)
        {
            currentScreen = screenMaterial.Count - 1;
        }

        ChangeMaterial(screenMaterial[currentScreen]);
    }

    private void FinishSelection(InputAction.CallbackContext callbackContext)
    {
        inputActionNext.action.performed -= OnNextView;
        inputActionPrevious.action.performed -= OnPreviousView;
        inputActionFinish.action.performed -= FinishSelection;

        text.SetActive(false);

        StartCoroutine(Manager.MiniGame());
    }

    private void ChangeMaterial(Material material)
    {
        GetComponent<MeshRenderer>().material = material;
    }
}
