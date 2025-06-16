using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ExperimentVariable
{
    Combo1234,
    Combo4123,
    Combo3412,
    Combo2341
}

public class ExperimentManager : MonoBehaviour
{
    private static ExperimentManager instance;

    public bool startWithScreens;
    public ExperimentVariable variable = ExperimentVariable.Combo1234;
    public bool secondScene;

    public int[] scenesCombo;
    public int currentScene = 0;

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
        switch (variable)
        {
            case ExperimentVariable.Combo1234:
                scenesCombo = new int[] { 0, 1, 2, 3 };
                break;

            case ExperimentVariable.Combo4123:
                scenesCombo = new int[] {3, 0, 1, 2 };
                break;

            case ExperimentVariable.Combo3412:
                scenesCombo = new int[] { 2, 3, 0, 1 };
                break;

            case ExperimentVariable.Combo2341:
                scenesCombo = new int[] { 1, 2, 3, 0 };
                break;

            default:
                scenesCombo = new int[] { 0, 1, 2, 3 }; // Valeur par défaut
                break;
        }

        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene(scenesCombo[0]);
    }

    public void NextTask()
    {
        startWithScreens = !startWithScreens;
        secondScene = !secondScene;
        if (secondScene) SceneManager.LoadScene(scenesCombo[currentScene]);
        else
        {
            if (currentScene++ < scenesCombo.Length) SceneManager.LoadScene(scenesCombo[currentScene]);
        }
    }
}
