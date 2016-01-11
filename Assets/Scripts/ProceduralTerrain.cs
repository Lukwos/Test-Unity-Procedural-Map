using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class ProceduralTerrain : MonoBehaviour
{
    float[,,] data;

    public int size = 25;
    public int height = 20;

    public float surfaceCrossValue = 0;
    public float noiseScaleFactor = 20;

    Mesh mesh;
    MeshFilter meshFilter;

	// Use this for initialization
	void Start ()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        data = new float[size, height, size];
        FillData(transform.position.x, transform.position.y, transform.position.z);
        ApplyDataToMesh();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void ApplyDataToMesh()
    {
        TerrainMeshGenerator.FillMesh(ref mesh, data, size, height, surfaceCrossValue);
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void FillData(float xOrigin, float yOrigin, float zOrigin)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    if (x == 0 || x == size - 1)
                    {
                        data[x, y, z] = -1;
                        continue;
                    }
                    if (y == 0 || y == height- 1)
                    {
                        data[x, y, z] = -1;
                        continue;
                    }
                    if (z == 0 || z == size - 1)
                    {
                        data[x, y, z] = -1;
                        continue;
                    }

                    float dataX = (xOrigin + x) / noiseScaleFactor;
                    float dataY = (yOrigin + y) / noiseScaleFactor;
                    float dataZ = (zOrigin + z) / noiseScaleFactor;

                    data[x, y, z] = Mathf.PerlinNoise(dataY, dataX + dataZ) - Mathf.PerlinNoise(dataX, dataZ);
                }
            }
        }
    }
}
