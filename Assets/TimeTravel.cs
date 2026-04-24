using UnityEngine;

public class TimeTravel : MonoBehaviour {
    public GameObject Player;
    public Vector3 TeleportOffset;
    public LayerMask teleportBlockerMask;
    
    bool isOffset = false;
    bool teleportAllowed = false;

    CharacterController capsule;
    float capsuleHeight, capsuleRadius;
    Vector3 capsuleCenter;

    void Start() {
        capsule = Player.GetComponent<CharacterController>();
        capsuleHeight = capsule.height * Player.transform.lossyScale.y;
        capsuleRadius = capsule.radius * Mathf.Max(Player.transform.lossyScale.x, Player.transform.lossyScale.z);
        capsuleCenter = capsule.center;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            //Vector3 newPos = Player.transform.position + isOffset ? -TeleportOffset : TeleportOffset;

            //cc.enabled = false;
            //Player.transform.position = newPos;
            //cc.enabled = true;
            
            isOffset = !isOffset;
        }

    }

    public void SetTeleportAllowed(bool state) {
        teleportAllowed = state;
    }
}
