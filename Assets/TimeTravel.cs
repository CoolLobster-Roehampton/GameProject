using UnityEngine;

public class TimeTravel : MonoBehaviour {
    public GameObject Player;
    public Vector3 TeleportOffset;
    bool isOffset = false;

    void Start() {
            
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (isOffset) {
                gameObject.transform.position = gameObject.transform.position + ( -1 * TeleportOffset);
            } else {
                gameObject.transform.position = gameObject.transform.position + TeleportOffset;
            }
            isOffset = !isOffset;
        }
    }
}
