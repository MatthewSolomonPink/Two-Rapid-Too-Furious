using UnityEngine;
using System.Collections.Generic;
using System;

public class PunkOwlFaceManager : MonoBehaviour
{

    public List<Texture2D> faces;
    public Material faceMaterial;


    private List<Tuple<float, int>> faceTimes = new List<Tuple<float, int>>();

    


    private float currentTime = 0f;
    private int currentIndex = 0;
    public bool isAngry = false;
    private int openEyes = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        faceTimes.Add(new Tuple<float, int>(0.0f, openEyes));
        faceTimes.Add(new Tuple<float, int>(2.0f, 1));
        faceTimes.Add(new Tuple<float, int>(2.2f, openEyes));
    }

    // Update is called once per frame
    void Update()
    {
        openEyes = isAngry ? 2 : 0;
        //Debug.Log(openEyes);
        if (currentTime > 2.2f)
        {
            currentTime = 0f;
            currentIndex = 0;
        }

        if(currentTime >= faceTimes[currentIndex + 1].Item1)
        {
            currentIndex++;
        }
        // Make more elegant if have time
        if (faceTimes[currentIndex].Item2 == 0 || faceTimes[currentIndex].Item2 == 2)
        {
            faceTimes[currentIndex] = new Tuple<float, int>(faceTimes[currentIndex].Item1, openEyes);
        }
        faceMaterial.mainTexture = faces[faceTimes[currentIndex].Item2];
        currentTime += Time.deltaTime;
    }
}
