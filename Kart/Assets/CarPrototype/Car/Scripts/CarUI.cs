using UnityEngine;
using TMPro;

public class CarUI : MonoBehaviour
{
    public Car car;
    public TMP_Text speedText;
    
    void Update()
    {
        float speed = car.GetSpeed();
        speedText.text = $"Speed: {speed}";
    }
}
