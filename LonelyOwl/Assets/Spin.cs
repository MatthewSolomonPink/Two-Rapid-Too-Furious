using UnityEngine;

public class Spin : MonoBehaviour
{
    public float rotateSpeed  = 0.5f;
    public bool rotateRight = false;
    
    void Update()
    {
        int rotateDir = rotateRight == true ? 1 : -1;
        this.transform.Rotate(0, (Time.deltaTime*(rotateDir * rotateSpeed)), 0);
    }
}
