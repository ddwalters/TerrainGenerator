using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    MeshGeneration meshGeneration;

    TextMeshProUGUI SeedInput;
    Button GenerateButton;

    int currentSeed; 

    void Start()
    {
        meshGeneration = FindAnyObjectByType<MeshGeneration>();

        SeedInput = FindAnyObjectByType<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
        GenerateButton = FindAnyObjectByType<Canvas>().GetComponentInChildren<Button>();

        GenerateButton.onClick.AddListener(Int32.TryParse(SeedInput.text, out currentSeed) ? GeneratePreviousWorld : GenerateNewWorld);
    }

    void GenerateNewWorld()
    {
        int worldSeed = UnityEngine.Random.Range(100000000, 999999999);
        currentSeed = worldSeed;

        meshGeneration.SetWorldSeed(currentSeed);
    }

    void GeneratePreviousWorld()
    {
        if(Int32.TryParse(SeedInput.text, out currentSeed))
        {
            Tuple<int, bool> worldSeed = new Tuple<int, bool>(Int32.Parse(SeedInput.text), true);
            SeedInput.color = Color.white;

            meshGeneration.SetWorldSeed(currentSeed);
        }
        else
        {
            SeedInput.color = Color.red;
        }
    }
}
