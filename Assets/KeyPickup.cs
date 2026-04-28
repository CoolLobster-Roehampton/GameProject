using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private string itemId = "key_1";
    [SerializeField] private string displayName = "Key";
    [SerializeField] private bool finale = false;

    [Header("Pickup Behaviour")]
    [SerializeField] private bool disableOnPickup = true;

    /// <summary>
    /// Called by PlayerInteraction when the player looks at this and presses E.
    /// </summary>
    public void TryPickup(PlayerInteraction player)
    {
        if (player == null) return;

        bool added = player.AddItem(itemId, displayName);
        if (!added)
        {
            // Duplicate: already has the key, do nothing
            return;
        }

        if (disableOnPickup)
            gameObject.SetActive(false);
    }

    public bool isFinale() { return finale; }
}
