using UnityEngine;

public class TimeTravel : MonoBehaviour {
    public GameObject Player;
    public Vector3 TeleportOffset;
    bool isOffset = false;
    CharacterController cc; 

    void Start() {
        cc = Player.GetComponent<CharacterController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Vector3 newPos = Player.transform.position + isOffset ? -TeleportOffset : TeleportOffset;

            cc.enabled = false;
            Player.transform.position = newPos;
            cc.enabled = true;
            
            isOffset = !isOffset;
        }
    }
}
