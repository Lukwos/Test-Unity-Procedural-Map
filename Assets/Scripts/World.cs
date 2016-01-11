using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{

    Chunk[,] worldChunks;
    public int worldSize = 20;
    public int chunkSize = 10;
    public int chunkHeight = 20;

    public float surfaceCrossValue = 0;
    public float noiseScaleFactor = 20;

    public Vector3 startPosition = new Vector3(0,0,0);

    public GameObject player;

    public Material material;

	// Use this for initialization
	void Start () {

        Vector3 cornerPosition = startPosition - new Vector3(worldSize*chunkSize / 2, 0, worldSize*chunkSize / 2);

        worldChunks = new Chunk[worldSize+1, worldSize+1];

        for(int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)
            {
                worldChunks[i, j] = new Chunk(cornerPosition.x + i * chunkSize, 0, cornerPosition.z + j * chunkSize, chunkSize, chunkHeight, surfaceCrossValue, noiseScaleFactor, material);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (player.transform.position.x > startPosition.x + chunkSize)
        {
            startPosition += new Vector3(chunkSize, 0, 0);
            Vector3 cornerPosition = startPosition - new Vector3(worldSize * chunkSize / 2, 0, worldSize * chunkSize / 2);
            for (int i = 0; i < worldSize; i++)
            {
                for (int j = 0; j < worldSize; j++)
                {
                    if (i == 0)
                    {
                        worldChunks[i, j].Dispose();
                    }
                    else if (i != worldSize - 1)
                    {
                        worldChunks[i, j] = worldChunks[i - 1, j];
                    }
                    else
                    {
                        worldChunks[i, j] = new Chunk(cornerPosition.x + i * chunkSize, 0, cornerPosition.z + j * chunkSize, chunkSize, chunkHeight, surfaceCrossValue, noiseScaleFactor, material);
                    }
                }
            }
        }
        if (player.transform.position.x < startPosition.x - chunkSize)
        {
            startPosition -= new Vector3(chunkSize, 0, 0);
            Vector3 cornerPosition = startPosition - new Vector3(worldSize * chunkSize / 2, 0, worldSize * chunkSize / 2);
            for (int i = worldSize - 1; i >= 0; i--)
            {
                for (int j = 0; j < worldSize; j++)
                {
                    if (i == worldSize - 1)
                    {
                        worldChunks[i, j].Dispose();
                    }
                    else if (i != 0)
                    {
                        worldChunks[i, j] = worldChunks[i + 1, j];
                    }
                    else
                    {
                        worldChunks[i, j] = new Chunk(cornerPosition.x + i * chunkSize, 0, cornerPosition.z + j * chunkSize, chunkSize, chunkHeight, surfaceCrossValue, noiseScaleFactor, material);
                    }
                }
            }
        }
        if (player.transform.position.z > startPosition.z + chunkSize)
        {
            startPosition += new Vector3(0, 0, chunkSize);
            Vector3 cornerPosition = startPosition - new Vector3(worldSize * chunkSize / 2, 0, worldSize * chunkSize / 2);
            for (int i = 0; i < worldSize; i++)
            {
                for (int j = 0; j < worldSize; j++)
                {
                    if (j == 0)
                    {
                        worldChunks[i, j].Dispose();
                    }
                    else if (j != worldSize - 1)
                    {
                        worldChunks[i, j] = worldChunks[i, j - 1];
                    }
                    else
                    {
                        worldChunks[i, j] = new Chunk(cornerPosition.x + i * chunkSize, 0, cornerPosition.z + j * chunkSize, chunkSize, chunkHeight, surfaceCrossValue, noiseScaleFactor, material);
                    }
                }
            }
        }
        if (player.transform.position.z < startPosition.z - chunkSize)
        {
            startPosition -= new Vector3(0, 0, chunkSize);
            Vector3 cornerPosition = startPosition - new Vector3(worldSize * chunkSize / 2, 0, worldSize * chunkSize / 2);
            for (int i = 0; i < worldSize; i++)
            {
                for (int j = worldSize - 1; j >= 0; j--) 
                {
                    if (j == worldSize - 1)
                    {
                        worldChunks[i, j].Dispose();
                    }
                    else if (j != 0)
                    {
                        worldChunks[i, j] = worldChunks[i, j + 1];
                    }
                    else
                    {
                        worldChunks[i, j] = new Chunk(cornerPosition.x + i * chunkSize, 0, cornerPosition.z + j * chunkSize, chunkSize, chunkHeight, surfaceCrossValue, noiseScaleFactor, material);
                    }
                }
            }
        }
    }
}
