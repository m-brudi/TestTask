using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Controller : SingletonMonoBehaviour<Controller>
{
    public List<Vector3> positions = new List<Vector3>();
    public Pooler pooler;
    public bool gameOn;
    public static Action gameOverEvent;
    [SerializeField] UIManager uiManager;
    [SerializeField] Obj objPrefab;
    private int numOfAliveObjs;
    private float sizeX;
    private float sizeY;
    private float gridSize;

    void Start()
    {
        uiManager.SetupMenuPanel();
        Vector3 screen = Camera.main.ScreenToWorldPoint(Vector3.zero);
        sizeX = Mathf.FloorToInt(Mathf.Abs(screen.x));
        sizeY = Mathf.FloorToInt(Mathf.Abs(screen.y));
        SetupGridSize(500);
        numOfAliveObjs = 0;
    }
    
    void SetupGridSize(int num) {
        //setup size of grid that can fit atleast max amount of objects (500)
        gridSize = .8f;
        float gridX = Mathf.FloorToInt((2 * sizeX - gridSize) / gridSize);
        float gridY = Mathf.FloorToInt((2 * sizeY - gridSize) / gridSize);
        while (gridX * gridY < num) {
            gridSize -= 0.1f;
            gridX = Mathf.FloorToInt((2 * sizeX - gridSize) / gridSize);
            gridY = Mathf.FloorToInt((2 * sizeY - gridSize) / gridSize);
        }
    }

    int GetObjColorIndex(int num) {
        switch (num) {
            case 50:
                return 0;
            case 100:
                return 1;
            case 250:
                return 2;
            default:
                return 3;
        }
    }

    public void StartGame(int numOfObjects) {
        int objColorIndex = GetObjColorIndex(numOfObjects);

        //generate grid
        for (float x = -sizeX + gridSize; x < sizeX - gridSize; x+= gridSize) {
            for (float y = -sizeY + gridSize; y < sizeY - gridSize; y+= gridSize) {
                positions.Add(new(x, y, 0));
            }
        }
        //spawn objects
        for (int i = 0; i < numOfObjects; i++) {
            Obj o = pooler.GetObj();
            o.Setup(GetNewPosition(), gridSize, objColorIndex);
            numOfAliveObjs++;
        }
        gameOn = true;
    }

    public Vector3 GetNewPosition() {
        Vector3 pos = positions[UnityEngine.Random.Range(0, positions.Count)];
        positions.Remove(pos);
        return pos;
    }
    
    public void ObjDied() {
        numOfAliveObjs--;
        if (numOfAliveObjs == 1 && gameOn) StartCoroutine(DelayGameOver());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && gameOn) GameOver();
    }

    IEnumerator DelayGameOver() {
        gameOn = false;
        yield return new WaitForSeconds(1);
        GameOver();
    }

    public void GameOver() {
        numOfAliveObjs = 0;
        positions.Clear();
        gameOverEvent?.Invoke();
        gameOn = false;
        uiManager.SetupGameOverPanel();
    }
}
