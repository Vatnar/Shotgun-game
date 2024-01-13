
using UnityEngine;

public class followPLayer : MonoBehaviour {
    [SerializeField] private Transform camTransform;
    // Start is called before the first frame update
    void Start() {
        transform.position = new Vector3(camTransform.position.x, camTransform.position.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camTransform.position.x, camTransform.position.y, 0);
        
    }
}
