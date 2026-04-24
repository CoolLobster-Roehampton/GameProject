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

    public void SetTeleportAllowed(bool state) {
        teleportAllowed = state;
    }

    bool CanTeleport(Vector3 targetPosition) {
        Vector3 worldCenter =
            targetPosition +
            Player.transform.rotation *
            Vector3.Scale(capsule.center, Player.transform.lossyScale);

        float halfHeight = Mathf.Max(
            capsuleHeight / 2f - capsuleRadius,
            0.01f
        );

        Vector3 bottom = worldCenter + Vector3.down * halfHeight;
        Vector3 top    = worldCenter + Vector3.up   * halfHeight;

        Collider[] hits = Physics.OverlapCapsule(
            bottom,
            top,
            capsuleRadius,
            teleportBlockerMask,
            QueryTriggerInteraction.Collide//.Ignore
        );

        foreach (var hit in hits)
        {
            if (hit == capsule) continue; // ignore self
            return false;
        }

        return true;
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            Vector3 newPos = Player.transform.position + (isOffset ? -TeleportOffset : TeleportOffset);

            if (CanTeleport(newPos) && teleportAllowed) {
                capsule.enabled = false;
                Player.transform.position = newPos;
                capsule.enabled = true;

                isOffset = !isOffset;
            }


        }
    }
}
