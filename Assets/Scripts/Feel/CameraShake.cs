using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Shake Settings")]
    [SerializeField] private float traumaDecaySpeed = 1.5f;
    [SerializeField] private float maxAngle = 3f;
    [SerializeField] private float maxOffsetX = 0.3f;
    [SerializeField] private float maxOffsetY = 0.3f;

    private float trauma = 0f;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (trauma > 0)
        {
            trauma -= traumaDecaySpeed * Time.deltaTime;
            trauma = Mathf.Clamp01(trauma);
            ApplyShake();
        }
        else
        {
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
    }

    private void ApplyShake()
    {
        float shake = trauma * trauma;

        float offsetX = maxOffsetX * shake * Random.Range(-1f, 1f);
        float offsetY = maxOffsetY * shake * Random.Range(-1f, 1f);
        float angle = maxAngle * shake * Random.Range(-1f, 1f);

        transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);
        transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, angle);
    }

    public void AddTrauma(float amount)
    {
        trauma = Mathf.Clamp01(trauma + amount);
    }
}