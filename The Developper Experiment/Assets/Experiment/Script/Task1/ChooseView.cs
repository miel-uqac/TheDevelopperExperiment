using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChooseView : MonoBehaviour
{
    public List<Material> screenMaterialTransparent;
    public List<Material> screenMaterial;
    public Image myImage; // Assigne l'image dans l'inspecteur

    private int currentScreen = 0;

    public InputActionReference inputActionNext;
    public InputActionReference inputActionPrevious;
    public InputActionReference inputActionFinish;
    public InputActionReference inputActionShowScreen;

    public int currentTask;

    public GameObject Manager;
    public GameObject text;
    public float transparencyStrength;

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

        SetOpacity(0.2f);
        ChangeMaterial(screenMaterialTransparent[currentScreen]);

        inputActionShowScreen.action.started += ShowScreen;
        inputActionShowScreen.action.canceled += HideScreen;

        switch (currentTask)
        {
            case 1:
                PlacementChecker placementChecker = Manager.GetComponent<PlacementChecker>();
                StartCoroutine(placementChecker.MiniGame());
                break;
            case 2:
                SpawnManager spawnManager = Manager.GetComponent<SpawnManager>();
                spawnManager.FirstTask();
                break;
            case 3:
                SpawnManager spawnManager2 = Manager.GetComponent<SpawnManager>();
                spawnManager2.SecondTask();
                break;
            case 4:
                SpawnManager spawnManager3 = Manager.GetComponent<SpawnManager>();
                spawnManager3.ThirdTask();
                break;
            default:
                break;
        }

        
    }

    private void OnDisable()
    {
        inputActionShowScreen.action.started -= ShowScreen;
        inputActionShowScreen.action.canceled -= HideScreen;

    }

    private void ChangeMaterial(Material material)
    {
        GetComponent<MeshRenderer>().material = material;
    }

    private void ShowScreen(InputAction.CallbackContext context)
    {
        ChangeMaterial(screenMaterial[currentScreen]);
        SetOpacity(0.8f);
    }

    private void HideScreen(InputAction.CallbackContext context)
    {
        ChangeMaterial(screenMaterialTransparent[currentScreen]);
        SetOpacity(0.2f);
    }

    private void SetOpacity(float alpha)
    {
        Color color = myImage.color;
        color.a = Mathf.Clamp01(alpha);
        myImage.color = color;
    }

}
