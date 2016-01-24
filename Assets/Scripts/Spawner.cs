using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    const int Block = 1;
    const int Spikes = 2;

    public GameObject[] blocks1;
    public GameObject[] blocks2;
    public GameObject[] blocks3;
    public GameObject[] spikes1;
    public GameObject[] spikes2;
    public GameObject[] spikes3;

    public float wallProbability = 70;
    public float spikesProbability = 30;

    public int tilesWide = 5;

    private int nextLeftPosition = 0;
    private int nextRightPosition = 0;

    private int lastBlockTypeLeft = 0;
    private int lastBlockTypeRight = 0;

    private int lastBlockSizeLeft = 0;
    private int lastBlockSizeRight = 0;

    private float scale;
    private float tileWidth;

    private Vector2 offset;

    private int spawnPosition;

    void CalculateScale() {
        float cameraHeight = Camera.main.orthographicSize * 2.0f;
        float cameraWidth = Camera.main.aspect * cameraHeight;

        offset = new Vector2(-cameraWidth / 2.0f, -cameraHeight / 2.0f);

        Debug.Log("Camera Height: " + cameraHeight);
        Debug.Log("Camera Width: " + cameraWidth);

        float desiredWidth = cameraWidth / tilesWide;
        float actualWidth = blocks1[0].GetComponent<SpriteRenderer>().bounds.size.x;
        scale = desiredWidth / actualWidth;
        tileWidth = desiredWidth;

        Debug.Log("Desired Width: " + desiredWidth);
        Debug.Log("Actual Width: " + actualWidth);
        Debug.Log("Scale: " + scale);

        spawnPosition = (int)(transform.position.y / tileWidth);

        Debug.Log("Spawn Position: " + spawnPosition);
    }

    Vector3 getWorldPosition(int x, int y, int size) {
        Vector3 position = new Vector3(
            offset.x + x * tileWidth + tileWidth / 2,
            offset.y + y * tileWidth + tileWidth / 2 + (tileWidth / 2) * (size - 1)
        );

        return position;
    }

    int SpawnRandomWall(int side) {
        GameObject toSpawn = null;
        int height = 0;

        int r = Random.Range(0, 3);
        Debug.Log(r);
        switch (r) {
            case 0:
                toSpawn = blocks1[Random.Range(0, blocks1.Length)];
                height = 1;
                break;
            case 1:
                toSpawn = blocks2[Random.Range(0, blocks2.Length)];
                height = 2;
                break;
            case 2:
                toSpawn = blocks3[Random.Range(0, blocks3.Length)];
                height = 3;
                break;
        }

        if (toSpawn == null) {
            Debug.Log("NULL");
            return 0;
        }

        int x = side == 1 ? 0 : tilesWide - 1;
        int y = side == 1 ? nextLeftPosition : nextRightPosition;

        GameObject obj = (GameObject)Instantiate(toSpawn, Vector3.zero, Quaternion.identity);
        obj.transform.localScale = new Vector3(scale * side, scale, 1);
        obj.transform.position = getWorldPosition(x, y, height);

        return height; 
    }

    int SpawnSpikes(int side, int blockSize)
    {
        GameObject toSpawn = null;

        switch (blockSize) {
            case 0:
                toSpawn = spikes1[Random.Range(0, spikes1.Length)];
                break;
            case 1:
                toSpawn = spikes2[Random.Range(0, spikes2.Length)];
                break;
            case 2:
                toSpawn = spikes3[Random.Range(0, spikes3.Length)];
                break;
        }

        blockSize++;

        if (toSpawn == null) {
            Debug.Log("NULL");
            return 0;
        }

        int x = side == 1 ? 0 : tilesWide - 1;
        int y = side == 1 ? nextLeftPosition : nextRightPosition;

        GameObject obj = (GameObject)Instantiate(toSpawn, Vector3.zero, Quaternion.identity);
        obj.transform.localScale = new Vector3(scale * side, scale, 1);
        obj.transform.position = getWorldPosition(x, y, blockSize);

        return blockSize;
    }

    void SpawnBlockLeft() {
        int random = Random.Range(0, 100);
        if (random <= wallProbability)
        {
            lastBlockSizeLeft = SpawnRandomWall(1);
            nextLeftPosition += lastBlockSizeLeft;
            lastBlockTypeLeft = Block;
        }
        else if (random < (wallProbability + spikesProbability))
        {
            if (lastBlockTypeLeft == Spikes)
            {
                lastBlockSizeLeft = SpawnRandomWall(1);
                nextLeftPosition += lastBlockSizeLeft;
                lastBlockTypeLeft = Block;
            }
            else
            {
                if (lastBlockTypeRight == Spikes && lastBlockSizeRight == 3) {
                    lastBlockSizeLeft = SpawnSpikes(1, 0);
                }
                else {
                    lastBlockSizeLeft = SpawnSpikes(1, Random.Range(0, 3));
                }
                nextLeftPosition += lastBlockSizeLeft;
                lastBlockTypeLeft = Spikes;
            }
        }
    }

    void SpawnBlockRight()
    {
        int random = Random.Range(0, 100);
        if (random < wallProbability)
        {
            lastBlockSizeRight = SpawnRandomWall(-1);
            nextRightPosition += lastBlockSizeRight;
            lastBlockTypeRight = Block;
        }
        else if (random < (wallProbability + spikesProbability))
        {
            if (lastBlockTypeRight == Spikes)
            {
                lastBlockSizeRight = SpawnRandomWall(-1);
                nextRightPosition += lastBlockSizeRight;
                lastBlockTypeRight = Block;
            }
            else
            {
                if (lastBlockTypeLeft == Spikes && lastBlockSizeLeft == 3)
                {
                    lastBlockSizeRight = SpawnSpikes(-1, 1);
                }
                else
                {
                    lastBlockSizeRight = SpawnSpikes(-1, Random.Range(0, 3));
                }
                nextRightPosition += lastBlockSizeRight;
                lastBlockTypeRight = Spikes;
            }
        }
    }
    void Spawn() {
        while (nextLeftPosition <= spawnPosition && nextRightPosition <= spawnPosition) {
            SpawnBlockLeft();
            SpawnBlockRight();
        }
        while (nextLeftPosition <= spawnPosition)
        {
            SpawnBlockLeft();
        }
        while (nextRightPosition <= spawnPosition)
        {
            SpawnBlockRight();
        }

    }

    void Start () {
        CalculateScale();
        Spawn();
    }
	
	void Update () {
        spawnPosition = (int) (transform.position.y / tileWidth);
        Spawn();
    }

}
