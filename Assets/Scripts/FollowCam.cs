using UnityEngine;

public class FollowCam : MonoBehaviour {
    [SerializeField] private Transform playerTransform;

    private void FixedUpdate() {
        Transform camTransform = transform;
        Vector3 position = playerTransform.position;
        camTransform.position = new Vector3(position.x, position.y, camTransform.position.z);
    }
}