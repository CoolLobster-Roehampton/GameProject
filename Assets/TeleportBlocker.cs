using UnityEngine;

public class TeleportBlocker : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        TimeTravel tt = other.GetComponentInParent<TimeTravel>();
        if (tt != null) {
            tt.SetTeleportAllowed(false);
        }
    }

    private void OnTriggerExit(Collider other) {
        TimeTravel tt = other.GetComponentInParent<TimeTravel>();
        if (tt != null) {
            tt.SetTeleportAllowed(true);
        }
    }
}