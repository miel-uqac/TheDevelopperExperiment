using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEditor.Toolbars;
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
    public float timer;
    private bool isShowing;

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
        ExperimentManager experimentManager = FindFirstObjectByType<ExperimentManager>();
        if (!experimentManager.startWithScreens)
        {
            gameObject.SetActive(false);
            return;
        }

        ChangeMaterial(screenMaterial[currentScreen]);
        inputActionNext.action.performed += OnNextView;
        inputActionPrevious.action.performed += OnPreviousView;
        inputActionFinish.action.performed += FinishSelection;
    }

    private void Update()
    {
        if (isShowing) timer += Time.deltaTime;
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

        StartCoroutine(RunTaskCoroutine());
    }

    private IEnumerator RunTaskCoroutine()
    {
        switch (currentTask)
        {
            case 1:
                PlacementChecker placementChecker = Manager.GetComponent<PlacementChecker>();
                yield return StartCoroutine(placementChecker.MiniGame());
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
        isShowing = true;
    }

    private void HideScreen(InputAction.CallbackContext context)
    {
        ChangeMaterial(screenMaterialTransparent[currentScreen]);
        SetOpacity(0.2f);
        isShowing = false;
    }

    private void SetOpacity(float alpha)
    {
        Color color = myImage.color;
        color.a = Mathf.Clamp01(alpha);
        myImage.color = color;
    }

    public string GetScreen()
    {
        switch (currentScreen)
        {
            case 0:
                return "Back";
            case 1:
                return "Front";
            case 2:
                return "Left";
            case 3:
                return "Right";
            case 4:
                return "Up";
            default:
                return "None";
        }
    }

    public string GetTime()
    {
        float timeInSeconds = timer;

        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        float seconds = timeInSeconds % 60f;

        // Formate le temps en minutes et secondes
        string formattedTime;
        if (minutes > 0)
        {
            formattedTime = string.Format("{0} min {1:00.00} s", minutes, seconds);
        }
        else
        {
            formattedTime = string.Format("{0:0.00} s", seconds);
        }

        return formattedTime;
    }

}
