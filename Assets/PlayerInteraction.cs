using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Look + Interact")]
    [SerializeField] public GameObject menu;
    private MenuManager menuManager;
    [SerializeField] private bool paused;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float distance = 1.5f;
    [SerializeField] private LayerMask interactMask = ~0;

    [Header("UI (single TMP used as list + animated indicator)")]
    [SerializeField] private TextMeshProUGUI indicatorText;

    [Header("Indicator Animation")]
    [SerializeField] private float stayTime = 2f;
    [SerializeField] private float moveDuration = 0.75f;
    [SerializeField] private Vector2 centreAnchor = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 topRightAnchor = new Vector2(0.95f, 0.95f);

    [Header("Behaviour")]
    [SerializeField] private bool animateOnInventoryChange = true; // if false, list updates silently

    private Coroutine indicatorRoutine;

    // Inventory data: itemId -> display name (dupe-safe)
    private readonly Dictionary<string, string> inventory = new Dictionary<string, string>();

    // Keeps a stable display order (IDs in acquisition order)
    private readonly List<string> inventoryOrder = new List<string>();

    void Start()
    {
        menuManager = menu.GetComponent<MenuManager>();
    }

    void Reset()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        // Optional debug reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearInventory();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (paused)
            {
                menuManager.HidePauseMenu();
                paused = false;
            } else
            {
                menuManager.ShowPauseMenu();
                paused = true;
            }
            
        }
    }

    private void TryInteract()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, distance, interactMask)) return;

        // Check collider object first, then parents (more robust)
        KeyPickup pickup = hit.collider.GetComponent<KeyPickup>() ?? hit.collider.GetComponentInParent<KeyPickup>();
        if (pickup != null)
        {
            pickup.TryPickup(this);
            return;
        }

        LockedTarget locked = hit.collider.GetComponent<LockedTarget>() ?? hit.collider.GetComponentInParent<LockedTarget>();
        if (locked != null)
        {
            locked.TryUnlock(this);
            return;
        }

        PlayerDoor door = hit.collider.GetComponent<PlayerDoor>() ?? hit.collider.GetComponentInParent<PlayerDoor>();
        if (door != null)
        {
            door.Toggle();
            return;
        }
    }

    // ===== Inventory API (used by KeyPickup / LockedTarget) =====

    /// <summary>
    /// Adds an item if not already owned. Returns true if newly added.
    /// Any add/remove automatically rebuilds the list text.
    /// </summary>
    public bool AddItem(string itemId, string displayNameForUI)
    {
        if (string.IsNullOrWhiteSpace(itemId)) return false;

        if (inventory.ContainsKey(itemId))
            return false; // dupe check

        inventory[itemId] = string.IsNullOrWhiteSpace(displayNameForUI) ? itemId : displayNameForUI;
        inventoryOrder.Add(itemId);

        RebuildItemListUI(triggerAnimation: animateOnInventoryChange);
        return true;
    }

    public bool HasItem(string itemId)
    {
        return !string.IsNullOrWhiteSpace(itemId) && inventory.ContainsKey(itemId);
    }

    /// <summary>
    /// Removes an item if owned. Returns true if removed.
    /// Any add/remove automatically rebuilds the list text.
    /// </summary>
    public bool RemoveItem(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId)) return false;

        if (!inventory.Remove(itemId))
            return false;

        inventoryOrder.Remove(itemId);

        RebuildItemListUI(triggerAnimation: animateOnInventoryChange);
        return true;
    }

    public void ClearInventory()
    {
        inventory.Clear();
        inventoryOrder.Clear();
        RebuildItemListUI(triggerAnimation: false);
    }

    // ===== UI list rebuild =====

    private void RebuildItemListUI(bool triggerAnimation)
    {
        if (indicatorText == null) return;

        // Build newline list: item1 + '\n' + item2 ...
        if (inventoryOrder.Count == 0)
        {
            indicatorText.text = "";
            indicatorText.gameObject.SetActive(false);
            return;
        }

        // Convert ordered IDs -> display names
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < inventoryOrder.Count; i++)
        {
            string id = inventoryOrder[i];
            if (inventory.TryGetValue(id, out string display))
            {
                if (i > 0) sb.Append('\n');
                sb.Append(display);
            }
        }

        indicatorText.text = sb.ToString();
        indicatorText.gameObject.SetActive(true);

        if (triggerAnimation)
            ShowIndicator();
    }

    // ===== UI Animation =====

    private void ShowIndicator()
    {
        if (indicatorText == null) return;

        if (indicatorRoutine != null)
            StopCoroutine(indicatorRoutine);

        indicatorRoutine = StartCoroutine(AnimateIndicator());
    }

    private IEnumerator AnimateIndicator()
    {
        RectTransform rect = indicatorText.rectTransform;

        // Snap to centre
        rect.anchorMin = rect.anchorMax = centreAnchor;
        rect.anchoredPosition = Vector2.zero;

        // Stay
        yield return new WaitForSeconds(stayTime);

        // Animate to top-right
        Vector2 startAnchor = centreAnchor;
        Vector2 endAnchor = topRightAnchor;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / moveDuration);

            rect.anchorMin = rect.anchorMax = Vector2.Lerp(startAnchor, endAnchor, t);
            rect.anchoredPosition = Vector2.zero;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchorMin = rect.anchorMax = endAnchor;
        rect.anchoredPosition = Vector2.zero;
    }
}
