using UnityEngine;
using TMPro;
using System.Collections;

public class PuzzleInteraction : MonoBehaviour {
    public GameObject Camera;
    public GameObject sourceObject;
    public GameObject targetObject;
    public TextMeshProUGUI indicatorText;
    public float Distance = 1.5f;
    bool gotItem = false;

    public float stayTime = 2f;
    public float moveDuration = 1f;
    Coroutine indicatorRoutine;

    void Start() {
        
    }

    bool checkRaycast(GameObject target) {
        Ray ray = new Ray(Camera.transform.position,
                Camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Distance)) {
            if (hit.collider.gameObject == target) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    void ShowIndicator(string message) {
        if (indicatorRoutine != null) { StopCoroutine(indicatorRoutine); }
        indicatorRoutine = StartCoroutine(AnimateIndicator(message));
    }
    
    IEnumerator AnimateIndicator(string message) {
        RectTransform rect = indicatorText.rectTransform;

        indicatorText.text = message;
        indicatorText.gameObject.SetActive(true);

        // ---- SCREEN PERCENT POSITIONS ----
        Vector2 centerAnchor = new Vector2(0.5f, 0.5f);   // 50% / 50%
        Vector2 topRightAnchor = new Vector2(0.95f, 0.95f); // 95% / 95%

        float stayTime = 2f;
        float moveDuration = 0.75f;

        // 1️. Snap to center
        rect.anchorMin = rect.anchorMax = centerAnchor;
        rect.anchoredPosition = Vector2.zero;

        // 2. Stay centered
        yield return new WaitForSeconds(stayTime);

        // 3️. Animate anchor movement to top-right
        Vector2 startAnchor = centerAnchor;
        Vector2 endAnchor = topRightAnchor;

        float elapsed = 0f;

        while (elapsed < moveDuration) {
            float t = elapsed / moveDuration;
            t = Mathf.SmoothStep(0f, 1f, t); // nice easing

            Vector2 currentAnchor = Vector2.Lerp(startAnchor, endAnchor, t);
            rect.anchorMin = rect.anchorMax = currentAnchor;
            rect.anchoredPosition = Vector2.zero;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Final snap
        rect.anchorMin = rect.anchorMax = endAnchor;
        rect.anchoredPosition = Vector2.zero;
    }

    void Update() {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.R)) {
            gotItem = false;
            targetObject.SetActive(true);
        }

        if (!gotItem) {
            if (checkRaycast(sourceObject)) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    gotItem = true;
                    ShowIndicator("Item: Key");
                }
            }        
        } else {
            if (checkRaycast(targetObject)) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    targetObject.SetActive(false);
                    indicatorText.gameObject.SetActive(false);
                }
            }
        }
    }
}
