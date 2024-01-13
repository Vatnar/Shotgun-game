using UnityEngine;

public class followPLayer : MonoBehaviour {
    [SerializeField] private Transform camTransform;
    private Vector3 position;
    private void Awake() {
        position = camTransform.position;
    }
    void Start() {
        transform.position = new Vector3(position.x, position.y, 0);
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(position.x, position.y, 0);

    }
}
