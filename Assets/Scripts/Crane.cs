using UnityEngine;

public class Crane : MonoBehaviour
{
    [SerializeField] private float speed;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(new Vector3 (0,0,1f), speed * Time.deltaTime);    
    }
}
