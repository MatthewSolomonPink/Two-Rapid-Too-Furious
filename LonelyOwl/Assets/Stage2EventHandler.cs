using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Stage2EventHandler : MonoBehaviour
{
    private GameObject[] spinners;

    void Start()
    {
        spinners = GameObject.FindGameObjectsWithTag("spin");

    }
    void Update()
    {
        
    }

    private void startSpinning()
    {
        foreach (GameObject s in spinners)
        {
            Spin spinner = s.GetComponent<Spin>();  
            spinner.SetIsSpinning(true);
        }
    }

}
