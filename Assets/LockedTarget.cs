using UnityEngine;

public class LockedTarget : MonoBehaviour
{
    [Header("Lock Requirement")]
    [SerializeField] private string requiredItemId = "key_1";

    [Header("Unlock Action")]
    [SerializeField] private Animator animator;
    [SerializeField] private string unlockTrigger = "Unlock";
    [SerializeField] private GameObject disableOnUnlock; // if null, disables this GameObject

    [Header("Behaviour")]
    [SerializeField] private bool consumeKey = false; // usually false for keys; true for consumables

    /// <summary>
    /// Called by PlayerInteraction when the player looks at this and presses E.
    /// </summary>
    public void TryUnlock(PlayerInteraction player)
    {
        if (player == null) return;

        if (!player.HasItem(requiredItemId))
        {
            // Player doesn't have the required key/item
            return;
        }

        if (consumeKey)
            player.RemoveItem(requiredItemId);

        // Play animation if provided
        if (animator != null)
            animator.SetTrigger(unlockTrigger);

        // Disable barrier/object
        if (disableOnUnlock != null)
            disableOnUnlock.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}