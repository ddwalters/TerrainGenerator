using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class MeshGeneration : MonoBehaviour
{
    Mesh mesh;

    int _worldSeed;
    public int WorldSeed
    {
        get 
        { 
            return _worldSeed; 
        }
        set 
        {
            if (_worldSeed == value) return;

            _worldSeed = value;
            GenerateWorld(_worldSeed);
        }
    }

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;


    [Header("Basic World")]
    float maxTerrainHeight;
    float minTerrainHeight;

    public int xSize = 20;
    public int zSize = 20;

    public float xperlin = .3f;
    public float zperlin = .3f;
    public float yperlin = 2f;

    public Gradient gradient;

    [Header("Biomes")]
    //Plains
    float maxPlainsHeight;
    float minPlainsHeight;

    int xPlainsSize;
    int zPlainsSize;

    float xPerlinPlains;
    float zPerlinPlains;
    float yPerlinPlains;

    Gradient plainsGradient;

    //Mountains
    float maxMountainsHeight;
    float minMountainsHeight;

    int xMountainsSize;
    int zMountainsSize;

    float xPerlinMountains;
    float zPerlinMountains;
    float yPerlinMountains;

    Gradient MountainsGradient;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * xperlin, z * zperlin) * yperlin;
                vertices[i] = new Vector3(x, y, z);

                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < zSize; y++)
            {
            triangles[tris + 0] = vert + 0;
            triangles[tris + 1] = vert + xSize + 1;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + xSize + 1;
            triangles[tris + 5] = vert + xSize + 2;

            vert++;
            tris += 6;
            }
        vert++;
        }

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
               
                i++;
            }
        }

        UpdateMesh();
    }

    void GenerateWorld(int worldSeed)
    {
        Debug.Log(WorldSeed);

        // find a way to make a blob
        //xPlainsSize = ;
        //zPlainsSize = ;

        xPerlinPlains = ((worldSeed % 1000000) / 1000000.0f);
        zPerlinPlains = ((worldSeed % 500000) / 500000.0f);
        yPerlinPlains = 1f + ((worldSeed % 1500000) / 1000000.0f);

        Debug.Log("x: " + xPerlinPlains);
        Debug.Log("y: " + yPerlinPlains);
        Debug.Log("z: " + zPerlinPlains);

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * xPerlinPlains, z * zPerlinPlains) * yperlin;
                vertices[i] = new Vector3(x, y, z);

                if (y > maxPlainsHeight)
                    maxPlainsHeight = y;
                if (y < minPlainsHeight)
                    minPlainsHeight = y;

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < zSize; y++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);

                i++;
            }
        }

        UpdateMesh();
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    public void SetWorldSeed(int newWorldSeed)
    {
        WorldSeed = newWorldSeed;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
