using UnityEngine;
using System.Collections;

public class Chunk
{
    float[,,] data;

    int size;
    int height;

    float xOrigin;
    float yOrigin;
    float zOrigin;

    float surfaceCrossValue;
    float noiseScaleFactor;

    Mesh mesh;
    MeshFilter meshFilter;

    public GameObject meshGameObject;

    public Chunk(float x, float y, float z, int size, int height, float surfaceCrossValue, float noiseScaleFactor, Material material)
    {
        size ++;
        this.size = size;
        this.height = height;
        data = new float[size, height, size];
        this.xOrigin = x;
        this.yOrigin = y;
        this.zOrigin = z;
        this.surfaceCrossValue = surfaceCrossValue;
        this.noiseScaleFactor = noiseScaleFactor;

        meshGameObject = new GameObject("Chunk" + x + "," + y + "," + z, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        meshGameObject.transform.position = new Vector3(x, y, z);
        mesh = new Mesh();
        FillData();
        TerrainMeshGenerator.FillMesh(ref mesh, data, size, height, surfaceCrossValue);
        data = null;
        meshGameObject.GetComponent<MeshFilter>().mesh = mesh;
        meshGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        meshGameObject.GetComponent<MeshRenderer>().material = material;
    }

    void FillData()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    /*if (x == 0 || x == size - 1)
                    {
                        data[x, y, z] = -1;
                        continue;
                    }*/
                    if (y == 0 || y == height - 1)
                    {
                        data[x, y, z] = -1;
                        continue;
                    }
                    /*if (z == 0 || z == size - 1)
                    {
                        data[x, y, z] = -1;
                        continue;
                    }*/

                    float dataX = (xOrigin + x) / noiseScaleFactor;
                    float dataY = (yOrigin + y) / noiseScaleFactor;
                    float dataZ = (zOrigin + z) / noiseScaleFactor;

                    data[x, y, z] = Mathf.PerlinNoise(dataY, dataX + dataZ) - Mathf.PerlinNoise(dataX, dataZ);
                    //data[x, y, z] += 1 - 2 * y / (float)height; //big hill
                    data[x, y, z] += 1 - (2 * y / (float)height) * (Mathf.PerlinNoise(dataY, dataX + dataZ) + 1); //small hill
                }
            }
        }
    }

    public void Dispose()
    {
        meshGameObject = null;
    }
}
