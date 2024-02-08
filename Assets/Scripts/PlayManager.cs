using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    MeshGeneration meshGeneration;

    Button generateButton;

    [SerializeField]
    TextMeshProUGUI seedInput;

    int currentSeed;

    void Start()
    {
        meshGeneration = FindAnyObjectByType<MeshGeneration>();
        generateButton = FindFirstObjectByType<Button>();

        generateButton.onClick.AddListener(GenerateWorld);
    }

    void GenerateWorld()
    {
        //Removes the ascii character 8203 to check for null
        //string seed = seedInput.text.Replace("\u200b", "");
        string seed = seedInput.text;//.Replace("\u200b", "");

        if (string.IsNullOrWhiteSpace(seed))
        {
            int worldSeed = UnityEngine.Random.Range(0, 2147483646);
            currentSeed = worldSeed;

            meshGeneration.SetWorldSeed(currentSeed);
        }
        else
        {
            int inputSeed;
            try 
            {
                inputSeed = int.Parse(seed);
                seedInput.text = "";
                seedInput.color = Color.black;

                meshGeneration.SetWorldSeed(inputSeed);
            }
            catch
            {
                if (seedInput.color == Color.red)
                {
                    seedInput.text = "";
                    seedInput.color = Color.black;
                }
                else
                {
                    seedInput.color = Color.red;
                }
            }
        }
        
    }
}
