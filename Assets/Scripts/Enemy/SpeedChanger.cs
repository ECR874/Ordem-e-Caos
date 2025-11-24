using UnityEngine;

public class SpeedChanger : MonoBehaviour
{
    public float ChangeSpeed(float speed)
    {
        speed *= 2f;
        
        return speed;
    }
}
