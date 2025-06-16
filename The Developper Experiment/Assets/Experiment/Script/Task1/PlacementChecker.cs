using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class PlacementChecker : MonoBehaviour
{
    public GameObject CubeSpawnPointRight;
    public GameObject CubeSpawnPointLeft;
    public List<GameObject> SpawnPoints;
    public GameObject PlacementPrefab;
    public GameObject CubePrefab;
    public int numberRounds = 3;
    public float positionPrecision = 0.01f;
    public float rotationPrecision = 5f;

    private GameObject CubeRight;
    private GameObject CubeLeft;
    private GameObject PlacementArea;
    private PlacementArea placementAreaScript;
    private CubeInteractable cubeScriptRight;
    private CubeInteractable cubeScriptLeft;
    private bool cubeInArea = false;
    private bool CorrectlyPlaced = false;

    public IEnumerator MiniGame()
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
        //Right Side
        Vector3 realcubeSpawnPointRight = CubeSpawnPointRight.transform.position + new Vector3(0f, 0.5f, 0f);
        CubeRight = Instantiate(CubePrefab, realcubeSpawnPointRight, CubeSpawnPointRight.transform.rotation);
        cubeScriptRight = CubeRight.GetComponent<CubeInteractable>();
        cubeScriptRight.OnReleased += OnCubeReleased;

        //Left Side
        Vector3 realcubeSpawnPointLeft = CubeSpawnPointLeft.transform.position + new Vector3(0f, 0.5f, 0f);
        CubeLeft = Instantiate(CubePrefab, realcubeSpawnPointLeft, CubeSpawnPointLeft.transform.rotation);
        cubeScriptLeft = CubeLeft.GetComponent<CubeInteractable>();
        cubeScriptLeft.OnReleased += OnCubeReleased;

        //Placement zone
        GameObject SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)];
        PlacementArea = Instantiate(PlacementPrefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        placementAreaScript = PlacementArea.GetComponent<PlacementArea>();
        placementAreaScript.OnCubeInAreaChanged += OnCubeInAreaChanged;
        
    }

    public void RestartWave()
    {
        Destroy(CubeRight);
        Destroy(CubeLeft);
        Destroy(PlacementArea);
        NewWave();
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

        float distanceRight = Vector3.Distance(PlacementArea.transform.position, CubeRight.transform.position);
        float yRotationDiffRight = Mathf.Abs(Mathf.DeltaAngle(CubeRight.transform.eulerAngles.y, PlacementArea.transform.eulerAngles.y));

        float distanceLeft = Vector3.Distance(PlacementArea.transform.position, CubeLeft.transform.position);
        float yRotationDiffLeft = Mathf.Abs(Mathf.DeltaAngle(CubeLeft.transform.eulerAngles.y, PlacementArea.transform.eulerAngles.y));

        Debug.Log("<color=green>[Event] Distance : " + distanceRight.ToString("F3") + "</color>");
        Debug.Log("<color=blue>[Event] Rotation step diff : " + yRotationDiffRight + "</color>");

        if ((distanceRight <= positionPrecision && yRotationDiffRight <= rotationPrecision) || (distanceLeft <= positionPrecision && yRotationDiffLeft <= rotationPrecision))
        {
            CorrectlyPlaced = true;
            Destroy(CubeRight);
            Destroy (CubeLeft);
            Destroy(PlacementArea);
        }
    }

}
