using System.Collections;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    [SerializeField] private float _bounceHeight = 0.3f;
    [SerializeField] private float _bounceDuration = 0.4f;
    [SerializeField] private int _bounceCount = 2;

    public void StartBounce()
    {
        StartCoroutine(BounceHandlerCoroutine());
    }

    private IEnumerator BounceHandlerCoroutine()
    {
        Vector3 startPosition = transform.position;
        float localHeight = _bounceHeight;
        float localDuration = _bounceDuration;

        for (int i = 0; i < _bounceCount; i++)
        {
            yield return BounceCoroutine(startPosition, localHeight, localDuration * 0.5f);
            localHeight *= 0.5f;
            localDuration *= 0.8f;
        }

        transform.position = startPosition;
    }

    private IEnumerator BounceCoroutine(Vector3 startPosition, float height, float duration)
    {
        Vector3 peakPosition = startPosition + Vector3.up * height;
        float elapsedTime = 0f;

        // Move upwards
        while (elapsedTime < _bounceDuration)
        {
            transform.position = Vector3.Lerp(startPosition, peakPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        // Move downwards
        while (elapsedTime < _bounceDuration)
        {
            transform.position = Vector3.Lerp(peakPosition, startPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}