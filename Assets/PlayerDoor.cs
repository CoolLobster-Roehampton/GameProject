using UnityEngine;
using System.Collections;

public class PlayerDoor : MonoBehaviour
{
    [Header("Hinge / Rotation")]
    [SerializeField] private Transform hinge;          // the object that rotates (set to this.transform if null)
    [SerializeField] private Vector3 openEuler = new Vector3(0f, 90f, 0f);
    [SerializeField] private float degreesPerSecond = 180f;

    [Header("State")]
    [SerializeField] private bool startsOpen = false;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private string requiredItemId;
    public bool IsOpen { get; private set; }

    private Quaternion closedRot;
    private Quaternion openRot;
    private Coroutine moveRoutine;

    void Start()
    {
        if (requiredItemId == null) { isLocked = false; }
        else { isLocked = true; }
    }

    void Awake()
    {
        if (hinge == null) hinge = transform;

        closedRot = hinge.localRotation;
        openRot = Quaternion.Euler(openEuler) * closedRot;

        IsOpen = startsOpen;
        hinge.localRotation = IsOpen ? openRot : closedRot;
    }

    // Called by PlayerInteraction when you press E looking at the door
    public void Toggle(PlayerInteraction player)
    {
        if (isLocked)
        {
            if (player == null) return;
            if (!player.HasItem(requiredItemId)) return;
            player.RemoveItem(requiredItemId);
            isLocked = false;
            SetOpen(!IsOpen);
        } else {
            SetOpen(!IsOpen);   
        }
    }

    public void SetOpen(bool open)
    {
        IsOpen = open;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(RotateTo(IsOpen ? openRot : closedRot));
    }

    private IEnumerator RotateTo(Quaternion target)
    {
        // RotateTowards is stable and reaches the target cleanly
        while (Quaternion.Angle(hinge.localRotation, target) > 0.1f)
        {
            hinge.localRotation = Quaternion.RotateTowards(
                hinge.localRotation, target, degreesPerSecond * Time.deltaTime);

            yield return null;
        }

        hinge.localRotation = target;
        moveRoutine = null;
    }
}
