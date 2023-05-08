using System.Collections;
using UnityEngine;

public class SplashController : MonoBehaviour, IPoolable
{
    [SerializeField] PoolID _poolID;
    public PoolID PoolID => _poolID;
    SpriteRenderer _sprite;
    Transform _transform;
    private void Awake()
    {
        _sprite= GetComponent<SpriteRenderer>();
        _transform = transform;
    }
    void SetColor(Color newColor)
    {
        _sprite.color = newColor;
    }
    void DoFadeOut(float duration)
    {
        StartCoroutine(FadeOut(duration));
    }
    void SetRandomScale()
    {
        _transform.localScale = new Vector3(Random.Range(1.3f, 2.6f), Random.Range(1.3f, 2.6f), 1f);
    }
    public void DoSplash(Color newColor, float duration)
    {
        SetRandomScale();
        SetColor(newColor);
        DoFadeOut(duration);
    }
    IEnumerator FadeOut(float durationVal)
    {
        float elapsed = 0f;
        float duration = durationVal;
     
        _sprite.color = _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0.55f);
        while (elapsed < duration)
        {
            float alphaVal = Mathf.Lerp(_sprite.color.a, 0, Time.deltaTime);
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g,_sprite.color.b, alphaVal);
            elapsed += Time.deltaTime;
  
            yield return null;
        }
        
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0);
    }
}
