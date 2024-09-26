using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 0.5f;
    [SerializeField] private bool rotateRight = false;
    [SerializeField] private bool isSpinning = false;

    void Update()
    {
        if (isSpinning)
        {
            int rotateDir = rotateRight == true ? 1 : -1;
            this.transform.Rotate(0, (Time.deltaTime * (rotateDir * rotateSpeed)), 0);
        }
    }

    public void SetIsSpinning(bool s)
    {
        isSpinning = s;
    }
    
    public void SetSpeed(float speed)
    {
        this.rotateSpeed = speed;
    }
}
