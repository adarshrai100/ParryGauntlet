using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ParrySystem : MonoBehaviour
{
    [Header("Parry Settings")]
    [SerializeField] private float parryWindow = 0.2f;
    [SerializeField] private int inputBufferFrames = 10;

    [Header("Events")]
    public UnityEvent onParrySuccess;
    public UnityEvent onParryFail;
    public UnityEvent onParryInput;

    private bool isParryWindowOpen = false;
    private int bufferFramesRemaining = 0;
    private bool parryInputBuffered = false;

    private void Update()
    {
        HandleParryInput();
        TickInputBuffer();
    }

    private void HandleParryInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onParryInput.Invoke();

            if (isParryWindowOpen)
            {
                TriggerParrySuccess();
            }
            else
            {
                bufferFramesRemaining = inputBufferFrames;
                parryInputBuffered = true;
            }
        }
    }

    private void TickInputBuffer()
    {
        if (!parryInputBuffered) return;

        bufferFramesRemaining--;

        if (bufferFramesRemaining <= 0)
        {
            parryInputBuffered = false;
            bufferFramesRemaining = 0;
        }
    }

    public void OpenParryWindow()
    {
        StartCoroutine(ParryWindowRoutine());
    }

    private IEnumerator ParryWindowRoutine()
    {
        isParryWindowOpen = true;

        if (parryInputBuffered)
        {
            parryInputBuffered = false;
            TriggerParrySuccess();
            isParryWindowOpen = false;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < parryWindow)
        {
            elapsed += Time.deltaTime;

            if (parryInputBuffered)
            {
                parryInputBuffered = false;
                TriggerParrySuccess();
                isParryWindowOpen = false;
                yield break;
            }

            yield return null;
        }

        isParryWindowOpen = false;
        TriggerParryFail();
    }

    private void TriggerParrySuccess()
    {
        onParrySuccess.Invoke();
        Debug.Log("PARRY SUCCESS");
    }

    private void TriggerParryFail()
    {
        onParryFail.Invoke();
        Debug.Log("PARRY FAIL");
    }

    public bool IsParryWindowOpen() => isParryWindowOpen;
}