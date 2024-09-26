using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Stage2EventHandler : MonoBehaviour
{
    private GameObject[] spinners;
    [SerializeField] private GameObject target;

    void Start()
    {
        spinners = PrefabUtility.FindAllInstancesOfPrefab(target);
        foreach (GameObject s in spinners)
        {
            print("Cool: "+ s);
        }
        print("yippe");

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
