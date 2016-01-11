using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainMeshGenerator
{
    public static void FillMesh(ref Mesh meshToUpdate, float[,,] data, int size, int height, float surfaceCrossValue)
    {

        Vector3[] interpolatedValues = new Vector3[12];
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        int nTriangle = 0;

        for (int x = 0; x < size-1; x++)
        {
            for (int y = 0; y < height-1; y++)
            {
                for (int z = 0; z < size-1; z++)
                {
                    Vector3 basePoint = new Vector3(x, y, z);

                    // Recuperer les 8 points du cube
                    
                    float p0 = data[x, y, z];
                    float p1 = data[x+1, y, z];
                    float p2 = data[x, y+1, z];
                    float p3 = data[x+1, y+1, z];
                    float p4 = data[x, y, z+1];
                    float p5 = data[x+1, y, z+1];
                    float p6 = data[x, y+1, z+1];
                    float p7 = data[x+1, y+1, z+1];

                    /* effet cubique
                    if (data[x, y, z] >= 0)
                        p0 = 1;
                    else
                        p0 = -1;

                    if (data[x+1, y, z] >= 0)
                        p1 = 1;
                    else
                        p1 = -1;

                    if (data[x, y+1, z] >= 0)
                        p2 = 1;
                    else
                        p2 = -1;

                    if (data[x+1, y+1, z] >= 0)
                        p3 = 1;
                    else
                        p3 = -1;

                    if (data[x, y, z+1] >= 0)
                        p4 = 1;
                    else
                        p4 = -1;

                    if (data[x+1, y, z+1] >= 0)
                        p5 = 1;
                    else
                        p5 = -1;

                    if (data[x, y+1, z+1] >= 0)
                        p6 = 1;
                    else
                        p6 = -1;

                    if (data[x+1, y+1, z+1] >= 0)
                        p7 = 1;
                    else
                        p7 = -1;
                    */

                    int crossBitMap = 0;

                    if (p0 < surfaceCrossValue) crossBitMap |= 1;
                    if (p1 < surfaceCrossValue) crossBitMap |= 2;

                    if (p2 < surfaceCrossValue) crossBitMap |= 8;
                    if (p3 < surfaceCrossValue) crossBitMap |= 4;

                    if (p4 < surfaceCrossValue) crossBitMap |= 16;
                    if (p5 < surfaceCrossValue) crossBitMap |= 32;

                    if (p6 < surfaceCrossValue) crossBitMap |= 128;
                    if (p7 < surfaceCrossValue) crossBitMap |= 64;

                    int edgeBits = Contouring3D.EdgeLookupTable[crossBitMap];

                    if (edgeBits == 0)
                    {
                        continue;
                    }

                    float interpolatedCrossingPoint = 0f;

                    if ((edgeBits & 1) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p0) / (p1 - p0);
                        interpolatedValues[0] = Vector3.Lerp(new Vector3(x, y, z), new Vector3(x + 1, y, z), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 2) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p1) / (p3 - p1);
                        interpolatedValues[1] = Vector3.Lerp(new Vector3(x + 1, y, z), new Vector3(x+1, y+1, z), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 4) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p2) / (p3 - p2);
                        interpolatedValues[2] = Vector3.Lerp(new Vector3(x, y+1, z), new Vector3(x+1, y+1, z), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 8) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p0) / (p2 - p0);
                        interpolatedValues[3] = Vector3.Lerp(new Vector3(x, y, z), new Vector3(x, y+1, z), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 16) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p4) / (p5 - p4);
                        interpolatedValues[4] = Vector3.Lerp(new Vector3(x, y, z+1), new Vector3(x + 1, y, z+1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 32) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p5) / (p7 - p5);
                        interpolatedValues[5] = Vector3.Lerp(new Vector3(x+1, y, z+1), new Vector3(x + 1, y+1, z+1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 64) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p6) / (p7 - p6);
                        interpolatedValues[6] = Vector3.Lerp(new Vector3(x, y+1, z+1), new Vector3(x + 1, y+1, z+1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 128) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p4) / (p6 - p4);
                        interpolatedValues[7] = Vector3.Lerp(new Vector3(x, y, z+1), new Vector3(x, y+1, z+1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 256) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p0) / (p4 - p0);
                        interpolatedValues[8] = Vector3.Lerp(new Vector3(x, y, z), new Vector3(x, y, z+1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 512) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p1) / (p5 - p1);
                        interpolatedValues[9] = Vector3.Lerp(new Vector3(x+1, y, z), new Vector3(x + 1, y, z+1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 1024) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p3) / (p7 - p3);
                        interpolatedValues[10] = Vector3.Lerp(new Vector3(x+1, y+1, z), new Vector3(x + 1, y+1, z+1), interpolatedCrossingPoint);
                    }
                    if ((edgeBits & 2048) > 0)
                    {
                        interpolatedCrossingPoint = (surfaceCrossValue - p2) / (p6 - p2);
                        interpolatedValues[11] = Vector3.Lerp(new Vector3(x, y+1, z), new Vector3(x, y+1, z+1), interpolatedCrossingPoint);
                    }

                    for (int i = 0; Contouring3D.TriangleLookupTable[crossBitMap, i] != -1; i += 3)
                    {
                        int index1 = Contouring3D.TriangleLookupTable[crossBitMap, i];
                        int index2 = Contouring3D.TriangleLookupTable[crossBitMap, i+1];
                        int index3 = Contouring3D.TriangleLookupTable[crossBitMap, i+2];

                        vertices.Add(new Vector3(interpolatedValues[index1].x, interpolatedValues[index1].y, interpolatedValues[index1].z));
                        vertices.Add(new Vector3(interpolatedValues[index2].x, interpolatedValues[index2].y, interpolatedValues[index2].z));
                        vertices.Add(new Vector3(interpolatedValues[index3].x, interpolatedValues[index3].y, interpolatedValues[index3].z));
                        triangles.Add(nTriangle++);
                        triangles.Add(nTriangle++);
                        triangles.Add(nTriangle++);
                    }
                }
            }
        }

        meshToUpdate.vertices = vertices.ToArray();
        meshToUpdate.triangles = triangles.ToArray();
        meshToUpdate.RecalculateNormals();
        meshToUpdate.RecalculateBounds();
    }

}
