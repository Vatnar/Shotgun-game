using UnityEngine;

public class followPLayer : MonoBehaviour {
    [SerializeField] private Transform camTransform;
    private Vector3 position;
    
    void Start() {
        transform.position = new Vector3(camTransform.position.x, camTransform.position.y, 0);
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(camTransform.position.x, camTransform.position.y, 0);

    }
}
