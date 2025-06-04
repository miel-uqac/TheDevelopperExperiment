using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class PlacementChecker : MonoBehaviour
{
    public GameObject CubeSpawnPoint;
    public List<GameObject> SpawnPoints;
    public GameObject PlacementPrefab;
    public GameObject CubePrefab;
    public int numberRounds = 3;
    public float positionPrecision = 0.01f;
    public float rotationPrecision = 5f;

    private GameObject Cube;
    private GameObject PlacementArea;
    private PlacementArea placementAreaScript;
    private CubeInteractable cubeScript;
    private bool cubeInArea = false;
    private bool CorrectlyPlaced = false;

    private void Start()
    {
        StartCoroutine(MiniGame());
    }

    private IEnumerator MiniGame()
    {
        for (int i = 0; i < numberRounds; i++)
        {
            NewWave();
            yield return new WaitUntil(() => CorrectlyPlaced);
        }
    }

    private void NewWave()
    {
        CorrectlyPlaced = false;
        Vector3 realcubeSpawnPoint = CubeSpawnPoint.transform.position + new Vector3(0f, 0.5f, 0f);
        Cube = Instantiate(CubePrefab, realcubeSpawnPoint, CubeSpawnPoint.transform.rotation);
        GameObject SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)];
        PlacementArea = Instantiate(PlacementPrefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation);

        placementAreaScript = PlacementArea.GetComponent<PlacementArea>();
        cubeScript = Cube.GetComponent<CubeInteractable>();

        placementAreaScript.OnCubeInAreaChanged += OnCubeInAreaChanged;
        cubeScript.OnReleased += OnCubeReleased;
    }

    private void OnCubeInAreaChanged(bool isInArea)
    {
        cubeInArea = isInArea;
    }

    private void OnCubeReleased()
    {
        if (cubeInArea)
        {
            StartCoroutine(CheckDistanceCouroutine());
        }
    }

    private IEnumerator CheckDistanceCouroutine()
    {
        yield return new WaitForSeconds(0.1f); // attendre que le cube se pose

        float distance = Vector3.Distance(PlacementArea.transform.position, Cube.transform.position);

        //Ce qui nous intéresse c'est la différence de rotation a partir de 45 degrés
        float yRotationDiff = Mathf.Abs(Cube.transform.eulerAngles.y - PlacementArea.transform.eulerAngles.y) % 45;

        Debug.Log("<color=green>[Event] Distance : " + distance.ToString("F3") + "</color>");
        Debug.Log("<color=blue>[Event] Rotation step diff : " + yRotationDiff + "</color>");

        if (distance <= positionPrecision && yRotationDiff <= rotationPrecision)
        {
            CorrectlyPlaced = true;
            Destroy(Cube);
            Destroy(PlacementArea);
        }
    }


}
