using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class task1Result
{
    public float time;
    public float position;
    public float rotation;
    public int errors;
}

public class task2Result
{
    public float time;
    public int errors;
}

public class ExperimentResults : MonoBehaviour
{

    private static ExperimentResults instance;

    private int currentRound = 0;
    private string filename;

    public int numberParticipant;
    [SerializeField] ExperimentManager experimentManager;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Se désabonner lorsque l'objet est détruit
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentRound = 0;  // Réinitialise le round à chaque changement de scène
    }

    private void Start()
    {
        currentRound = 0;
        filename = Application.dataPath + "/Experiment/Results/results_" + numberParticipant + ".csv";
        CreateResultsFile();
}

    public void AddResultTask1(float time, float position, float rotation, int errors)
    {
        task1Result result = new task1Result();
        result.time = time;
        result.position = position;
        result.rotation = rotation;
        result.errors = errors;

        WriteTask1Results(result);
    }

    public void AddResultTask2(float time, int errors)
    {
        task2Result result = new task2Result();
        result.time = time;
        result.errors = errors;

        WriteTask2Results(result);
    }

    private void CreateResultsFile()
    {
        bool fileExists = File.Exists(filename);

        try
        {
            using (TextWriter tw = new StreamWriter(filename, true))  // Auto-close
            {
                if (!fileExists)
                {
                    tw.WriteLine($"ID : {numberParticipant}; Task ; Screens; Round; Result");
                }
            }

#if UNITY_EDITOR
            AssetDatabase.Refresh();  // Force l'apparition dans Unity
#endif

            Debug.Log("Fichier créé à : " + filename);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erreur création CSV : " + e.Message);
        }
    }

    private void WriteTask1Results(task1Result res)
    {
        using (StreamWriter writer = new StreamWriter(filename, true))  // true = append mode
        {

            float timeInSeconds = res.time;

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

            if (currentRound == 0)
            {
                writer.WriteLine();
                currentRound++;
                writer.WriteLine($" ; {experimentManager.scenesCombo[experimentManager.currentScene] + 1} ; ; {currentRound}; {formattedTime}");
            }
            else
            {
                currentRound++;
                writer.WriteLine($" ; ; ;{currentRound}; {formattedTime}");
            }

            string firstColumns = " ; ; ; ;";

            writer.WriteLine(firstColumns + res.position.ToString("F3"));
            writer.WriteLine(firstColumns + res.rotation.ToString("F3"));
            writer.WriteLine(firstColumns + res.errors);

#if UNITY_EDITOR
            // Force Unity à recharger les assets
            AssetDatabase.Refresh();  // Rafraîchissement de la base de données d'assets
            AssetDatabase.ImportAsset(filename);  // Assurer l'importation immédiate du fichier
#endif
        }
    }

    private void WriteTask2Results(task2Result res)
    {
        using (StreamWriter writer = new StreamWriter(filename, true))  // true = append mode
        {

            float timeInSeconds = res.time;

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

            if (currentRound == 0)
            {
                writer.WriteLine();
                currentRound++;
                writer.WriteLine($" ; {experimentManager.scenesCombo[experimentManager.currentScene] + 1} ; ; {currentRound}; {formattedTime}");
            }
            else
            {
                currentRound++;
                writer.WriteLine($" ; ; ;{currentRound}; {formattedTime}");
            }

            string firstColumns = " ; ; ; ;";

            writer.WriteLine(firstColumns + res.errors);

#if UNITY_EDITOR
            // Force Unity à recharger les assets
            AssetDatabase.Refresh();  // Rafraîchissement de la base de données d'assets
            AssetDatabase.ImportAsset(filename);  // Assurer l'importation immédiate du fichier
#endif
        }
    }

    public void WriteTotalScreenTime()
    {
        using (StreamWriter writer = new StreamWriter(filename, true))  // true = append mode
        {
            if (experimentManager.startWithScreens)
            {
                ChooseView chooseView = FindFirstObjectByType<ChooseView>();
                writer.WriteLine($" ; ; Screen : {chooseView.GetScreen()} {chooseView.GetTime()}");
            }
            else writer.WriteLine($" ; ; Screen : None");
        }

    }
}
