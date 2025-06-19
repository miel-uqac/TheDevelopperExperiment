using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

[System.Serializable]
public class MaterialPair
{
    public Material original;
    public Material replacement;
}

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_SpawnPoints;
    [SerializeField] private GameObject m_CubePrefab;
    [SerializeField] private GameObject m_RectanglePrefab;
    [SerializeField] private List<Material> m_Colours;

    public int rounds;

    [SerializeField] private List<MaterialPair> m_ColourSecondTask;

    private List<Transform> m_Targets = new List<Transform>();
    private List<GameObject> currentObjects = new List<GameObject>();

    private ExperimentManager experimentManager;
    private ExperimentResults results;
    public int errors;
    private float timer;
    private bool gameStarted;

    private void Start()
    {
        Initiate();
        experimentManager = FindFirstObjectByType<ExperimentManager>();
        results = FindFirstObjectByType<ExperimentResults>();
        if (!experimentManager.startWithScreens)
        {
            switch (experimentManager.scenesCombo[experimentManager.currentScene])
            {
                default:
                    break;
                case 1:
                    FirstTask();
                    break;
                case 2:
                    SecondTask();
                    break;
                case 3:
                    ThirdTask();
                    break;
            }
        }
    }

    private void Update()
    {
       if (gameStarted) timer += Time.deltaTime;
    }

    private void Initiate()
    {
        foreach (GameObject obj in m_SpawnPoints)
        {
            foreach (Transform child in obj.transform)
            {
                
                m_Targets.Add(child);
            }
        }
    }

    public void FirstTask()
    {
        gameStarted = true;
        EraseAllObjects();
        int index = Random.Range(0, m_Targets.Count);
        int firstcolour = Random.Range(0, m_Colours.Count);
        List<Transform> targets = new List<Transform>(m_Targets);

        GameObject rectangle = Instantiate(m_RectanglePrefab, targets[index].position, Random.rotation);
        rectangle.GetComponent<MeshRenderer>().material = m_Colours[firstcolour];
        targets.Remove(targets[index]);
        currentObjects.Add(rectangle);

        foreach (Transform child in targets)
        {
            int colour = Random.Range(0, m_Colours.Count);
            GameObject cube = Instantiate(m_CubePrefab, child.position, Random.rotation);
            cube.GetComponent<MeshRenderer>().material = m_Colours[colour];
            currentObjects.Add(cube);
        }
    }

    private void EraseAllObjects()
    {
        foreach(GameObject obj in currentObjects)
        {
            Destroy(obj);
        }
        currentObjects.Clear();
        errors = 0;
    }

    public void SecondTask()
    {
        gameStarted = true;
        EraseAllObjects();

        int index = Random.Range(0, m_Targets.Count);
        int colours = Random.Range(0, m_ColourSecondTask.Count);
        List<Transform> targets = new List<Transform>(m_Targets);

        GameObject cubeReplacement = Instantiate(m_CubePrefab, targets[index].position, Random.rotation);
        cubeReplacement.GetComponent<MeshRenderer>().material = m_ColourSecondTask[colours].replacement;
        cubeReplacement.GetComponentInChildren<ObjectManager>().isFake = true;
        targets.Remove(targets[index]);
        currentObjects.Add(cubeReplacement);

        foreach (Transform child in targets)
        {
            GameObject cube = Instantiate(m_CubePrefab, child.position, Random.rotation);
            cube.GetComponent<MeshRenderer>().material = m_ColourSecondTask[colours].original;
            currentObjects.Add(cube);
        }
    }

    public void ThirdTask()
    {
        gameStarted = true;
        EraseAllObjects();

        int index = Random.Range(0, m_Targets.Count);
        int firstcolour = Random.Range(0, m_Colours.Count);
        List<Transform> targets = new List<Transform>(m_Targets);

        GameObject movingCube = Instantiate(m_CubePrefab, targets[index].position, Random.rotation);
        movingCube.GetComponent<MeshRenderer>().material = m_Colours[firstcolour];
        movingCube.GetComponent<Animator>().enabled = false;
        movingCube.GetComponentInChildren<ObjectManager>().isMoving = true;
        targets.Remove(targets[index]);
        currentObjects.Add(movingCube);

        foreach (Transform child in targets)
        {
            int colour = Random.Range(0, m_Colours.Count);
            GameObject cube = Instantiate(m_CubePrefab, child.position, Random.rotation);
            cube.GetComponent<MeshRenderer>().material = m_Colours[colour];
            currentObjects.Add(cube);
        }
    }

    public void EndAllTask()
    {
        EraseAllObjects();

        ExperimentManager experimentManager = FindFirstObjectByType<ExperimentManager>();
        experimentManager.NextTask();
    }

    public void WriteResults()
    {
        results.AddResultTask2(timer, errors);
        errors = 0;
        timer = 0;
    }
}
