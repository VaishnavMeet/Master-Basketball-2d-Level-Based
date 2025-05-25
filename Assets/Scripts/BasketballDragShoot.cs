using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Animations;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D))]
public class BasketballDragShoot : MonoBehaviour
{
    public System.Action OnBallShot;
    public System.Action OnGoalScored;

    private Vector2 startPoint;
    private Rigidbody2D rb;

    [Header("Force & Drag Settings")]
    public float forceMultiplier = 4f;
    public float maxDragDistance = 3f;

    private bool isDragging = false;
    private bool isShot = false;

    [Header("Trajectory Dots")]
    public GameObject dotPrefab;
    public int dotCount = 10;
    public float dotSpacing = 0.1f;
    public float dotScale = 0.1f;
    private List<GameObject> dotList = new List<GameObject>();

    [Header("History Dots")]
    public GameObject dotHistoryPrefab;
    private List<GameObject> dotHistoryList = new List<GameObject>();

    Animator animator;
    string goalBoolName = "IsGoal";
    bool isEnter=false;
    int starCounts = 0;
    AudioSource audioSource;

    [Header("Level Reference")]
    public BasketballLevelSO levelData;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Net2"))
        {
            isEnter = true;
        }
        if (collision.CompareTag("star"))
        {
            collision.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            audioSource = collision.gameObject.GetComponent<AudioSource>();
            audioSource.Play();
            starCounts++;
            StartCoroutine(FadeAndDestroy(collision.gameObject));
        }

        if (collision.CompareTag("Net1") && isEnter)
        {
            animator = collision.gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
                animator.SetBool(goalBoolName, true);
                StartCoroutine(FadeAndDestroy(gameObject));
                OnGoalScored?.Invoke();

                if (levelData != null)
                {
                    levelData.isCompeleted = true;
                    levelData.stars = Mathf.Max(levelData.stars, starCounts); // Avoid overwriting higher stars
                }

            }
        }
        else if (collision.CompareTag("wall") && isShot)
        {
            StartCoroutine(FadeAndDestroy(gameObject));
        }
    }

    private IEnumerator FadeAndDestroy(GameObject obj)
    {
        if (animator != null)
        {
            yield return new WaitForSeconds(0.18f);
            animator.SetBool(goalBoolName, false);
        }
      
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Destroy(obj);
            yield break;
        }

        float duration = 1f;
        float elapsed = 0f;
        Color originalColor = sr.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Destroy(obj);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure no velocity or angular motion
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Stop gravity
        rb.gravityScale = 0;

        // Optional: Ensure it's dynamic and simulated
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.simulated = true;

        CreateDots();
        HideDots();
    }


    void Update()
    {
        if (isShot) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            BeginDrag(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButton(0))
            ContinueDrag(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonUp(0))
            ReleaseBall(Camera.main.ScreenToWorldPoint(Input.mousePosition));
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
                BeginDrag(touchPos);
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                ContinueDrag(touchPos);
            else if (touch.phase == TouchPhase.Ended)
                ReleaseBall(touchPos);
        }
#endif
    }

    void BeginDrag(Vector2 pos)
    {
        if (Vector2.Distance(pos, rb.position) <= 1f)
        {
            isDragging = true;
            startPoint = pos;
            ShowDots();
        }
    }

    void ContinueDrag(Vector2 pos)
    {
        if (!isDragging) return;

        Vector2 dragVector = pos - rb.position;
        dragVector = Vector2.ClampMagnitude(dragVector, maxDragDistance);

        Vector2 force = -dragVector * forceMultiplier;
        Vector2 velocity = force / rb.mass;

        DisplayTrajectory(rb.position, velocity);
    }

    void ReleaseBall(Vector2 pos)
    {
        if (!isDragging) return;

        isDragging = false;
        HideDots();

        Vector2 dragVector = pos - rb.position;
        dragVector = Vector2.ClampMagnitude(dragVector, maxDragDistance);
        Vector2 shootDirection = rb.position - pos;

        rb.isKinematic = false;
        rb.gravityScale = 1f; //  Now allow ball to fall
        rb.AddForce(shootDirection * forceMultiplier, ForceMode2D.Impulse);
        isShot = true;
        OnBallShot?.Invoke();

        //  Show previous shot using history dots
        Vector2 velocity = shootDirection * forceMultiplier / rb.mass;
        DisplayHistoryTrajectory(rb.position, velocity);
    }

    void CreateDots()
    {
        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab);
            dot.transform.localScale = Vector3.one * dotScale;
            dot.SetActive(false);
            dotList.Add(dot);

            GameObject histDot = Instantiate(dotHistoryPrefab);
            histDot.transform.localScale = Vector3.one * dotScale;
            histDot.SetActive(false);
            dotHistoryList.Add(histDot);
        }
    }

    void ShowDots()
    {
        foreach (var dot in dotList)
            dot.SetActive(true);
    }

    void HideDots()
    {
        foreach (var dot in dotList)
            dot.SetActive(false);
    }

    void DisplayTrajectory(Vector2 startPos, Vector2 velocity)
    {
        float timeStep = dotSpacing;
        for (int i = 0; i < dotCount; i++)
        {
            float t = timeStep * i;
            Vector2 pos = startPos + velocity * t + 0.5f * Physics2D.gravity * t * t;
            dotList[i].transform.position = pos;
        }
    }

    void DisplayHistoryTrajectory(Vector2 startPos, Vector2 velocity)
    {
        float timeStep = dotSpacing;
        for (int i = 0; i < dotCount; i++)
        {
            float t = timeStep * i;
            Vector2 pos = startPos + velocity * t + 0.5f * Physics2D.gravity * t * t;
            dotHistoryList[i].transform.position = pos;
            dotHistoryList[i].SetActive(true);
        }
    }
}
