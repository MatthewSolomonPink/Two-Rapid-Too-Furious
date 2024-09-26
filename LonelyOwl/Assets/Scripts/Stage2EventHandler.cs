using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Stage2EventHandler : MonoBehaviour
{
    private GameObject[] spinners;
    public bool playerInteracted = true;   
    private bool firstTime = false;

    private float spinSpeed = 10.0f;

    void Start()
    {
        spinners = GameObject.FindGameObjectsWithTag("spin");

    }
    void Update()
    {
        if (playerInteracted && !firstTime)
        {
            startSpinning();
            firstTime = true;
        }
        if (playerInteracted)
        {
            spinSpeed = spinSpeed + (Time.deltaTime * 15);
            UpdateSpeed();
        }
    }

    private void startSpinning()
    {
        foreach (GameObject s in spinners)
        {
            Spin spinner = s.GetComponent<Spin>();  
            spinner.SetIsSpinning(true);
            spinner.SetSpeed(spinSpeed);
        }
    }

    private void UpdateSpeed()
    {
        foreach (GameObject s in spinners)
        {
            Spin spinner = s.GetComponent<Spin>();
            spinner.SetSpeed(spinSpeed);
        }
    }


}
