using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Journal : MonoBehaviour
{
    public float cubeSpeed = 1.0f;
    public float cubeSpeedOverTime = 1.02f;
    public float vanishSpeed = 0.007f;
    public bool AnimateCubes = false;

    //private GameObject[] cubes;
    private List<SketchCube> cubes = new List<SketchCube>();
    private float timeAccum = 0.0f;

    private class SketchCube
    {
        public SketchCube(GameObject g, float delay)
        {
            G = g;
            Delay = delay;
            Material = g.GetComponent<Renderer>().material;
            StartingPosition = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
        }

        public GameObject G;
        public float Delay;
        public bool ReadyToAnimate;
        public Material Material;
        public Vector3 StartingPosition;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var cubeArr = gameObject.GetComponentsInChildren<Transform>();

        int i = 0;
        foreach(var c in cubeArr)
        {
            // skip parent transform
            if (i > 0)
            {
                var cube = new SketchCube(c.gameObject, UnityEngine.Random.Range(0.0f, 1.0f));
                cubes.Add(cube);
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AnimateCubes)
        {
            foreach(var c in cubes)
            {
                if (timeAccum > c.Delay)
                {
                    var horizontalMovement = Time.deltaTime * cubeSpeed * -1;
                    c.G.transform.position = c.G.transform.position + new Vector3(horizontalMovement, 0, 0);
                    c.Material.color = c.Material.color - new Color(0, 0, 0, vanishSpeed);
                }
            }
            timeAccum += Time.deltaTime;
            cubeSpeed *= cubeSpeedOverTime;
        }
    }

    //IEnumerator MoveCubeRight(float delay, SketchCube cube)
    //{
    //    yield return new WaitForSeconds(delay);
    //    cube.ReadyToAnimate = true;
    //}
}
