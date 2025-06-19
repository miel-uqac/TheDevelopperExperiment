using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ExperimentVariable
{
    Combo123,
    Combo213,
    Combo321
}

public class ExperimentManager : MonoBehaviour
{
    private static ExperimentManager instance;

    public bool startWithScreens;
    public ExperimentVariable variable = ExperimentVariable.Combo123;

    public int[] scenesCombo;
    public int currentScene = 0;
    public float waitingTime;

    private bool secondRun;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartScene") StartCoroutine(StartExperimentCoroutine());
    }

    private IEnumerator StartExperimentCoroutine()
    {
        Initiate();
        yield return new WaitForSeconds(waitingTime);
        SceneManager.LoadScene(scenesCombo[0]);
    }

    private void Initiate()
    {
        switch (variable)
        {
            case ExperimentVariable.Combo123:
                scenesCombo = new int[] { 0, 1, 2 };
                break;

            case ExperimentVariable.Combo213:
                scenesCombo = new int[] { 1, 0, 2 };
                break;

            case ExperimentVariable.Combo321:
                scenesCombo = new int[] { 2, 1, 0 };
                break;

            default:
                scenesCombo = new int[] { 0, 1, 2 }; // Valeur par défaut
                break;
        }
    }

    public void NextTask()
    {
        currentScene++;
        if (currentScene == scenesCombo.Length)
        {
            if (secondRun) return;
            secondRun = true;
            currentScene = 0;
            startWithScreens = !startWithScreens;
        }

        SceneManager.LoadScene(scenesCombo[currentScene]);
    }
}
