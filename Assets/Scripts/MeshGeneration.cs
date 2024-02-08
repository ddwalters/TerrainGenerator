using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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

    Vector3[] verticies;
    int[] triangles;
    Color[] colors;


    //[Header("Basic World")]
    //float maxTerrainHeight;
    //float minTerrainHeight;
    //
    //public int xSize = 20;
    //public int zSize = 20;
    //
    //public float xperlin = .3f;
    //public float zperlin = .3f;
    //public float yperlin = 2f;
    //

    public Gradient gradient;

    [Header("Biomes")]
    //Plains
    float maxPlainsHeight;
    float minPlainsHeight;

    int xPlainsSize;
    int zPlainsSize;

    float xPerlinPlains;
    float zPerlinPlains;
    float yAmplitude;

    Gradient plainsGradient;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void GenerateWorld(int worldSeed)
    {
        Debug.Log(WorldSeed);

        /// PLAINS NOISE
        xPlainsSize = 20;
        zPlainsSize = 20;

        xPerlinPlains = ((worldSeed % 1000000) / 1000000.0f);
        zPerlinPlains = ((worldSeed % 500000) / 500000.0f);
        yAmplitude = 1f + ((worldSeed % 1500000) / 1000000.0f);

        verticies = new Vector3[(xPlainsSize + 1) * (zPlainsSize + 1)];

        for (int i = 0, z = 0; z <= zPlainsSize; z++)
        {
            for (int x = 0; x <= xPlainsSize; x++)
            {
                float y = Mathf.PerlinNoise(x * xPerlinPlains, z * zPerlinPlains) * yAmplitude;
                verticies[i] = new Vector3(x, y, z);

                if (y > maxPlainsHeight)
                    maxPlainsHeight = y;
                if (y < minPlainsHeight)
                    minPlainsHeight = y;

                i++;
            }
        }

        triangles = new int[(xPlainsSize * zPlainsSize * 6)];

        // how do I add the mountains
        int vert = 0;
        int tris = 0;
        for (int x = 0; x < xPlainsSize; x++)
        {
            for (int y = 0; y < zPlainsSize; y++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xPlainsSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xPlainsSize + 1;
                triangles[tris + 5] = vert + xPlainsSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        colors = new Color[verticies.Length];

        for (int i = 0, z = 0; z <= zPlainsSize; z++)
        {
            for (int x = 0; x <= xPlainsSize; x++)
            {
                float height = Mathf.InverseLerp(minPlainsHeight, maxPlainsHeight, verticies[i].y);
                colors[i] = gradient.Evaluate(height);

                i++;
            }
        }

        UpdateMesh();
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verticies;
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
        if (verticies == null)
        {
            return;
        }

        for (int i = 0; i < verticies.Length; i++)
        {
            Gizmos.DrawSphere(verticies[i], .1f);
        }
    }
}
