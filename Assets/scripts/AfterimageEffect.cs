using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AfterimageEffect : MonoBehaviour
{
    [Header("残影生成")]
    public int count = 5;                 
    public float interval = 0.03f;        
    public float lifetime = 0.3f;         

    [Header("追踪设置")]
    public float followSpeed = 15f;       

    [Header("外观")]
    public Color color = new Color(1, 1, 1, 0.3f);

    private SpriteRenderer playerSr;
    private List<AfterimageData> activeList = new List<AfterimageData>();

    void Start()
    {
        playerSr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            AfterimageData data = activeList[i];
            data.remainingLifetime -= Time.deltaTime;

            
            if (data.remainingLifetime <= 0f)
            {
                Destroy(data.gameObject);
                activeList.RemoveAt(i);
                continue;
            }

            
            if (followSpeed > 0f)
            {
                data.gameObject.transform.position = Vector3.MoveTowards(
                    data.gameObject.transform.position,
                    transform.position,
                    followSpeed * Time.deltaTime
                );
            }

            float alphaRatio = data.remainingLifetime / lifetime;
            Color c = data.renderer.color;
            c.a = color.a * alphaRatio;
            data.renderer.color = c;
        }
    }

    public void SpawnAfterimage()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        if (playerSr.sprite == null) yield break;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = new GameObject("Afterimage");
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.transform.localScale = transform.localScale;

            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = playerSr.sprite;
            sr.flipX = playerSr.flipX;
            sr.color = color;
            sr.sortingOrder = playerSr.sortingOrder - 1;

            
            activeList.Add(new AfterimageData
            {
                gameObject = obj,
                renderer = sr,
                remainingLifetime = lifetime
            });

            yield return new WaitForSeconds(interval);
        }
    }

    
    private class AfterimageData
    {
        public GameObject gameObject;
        public SpriteRenderer renderer;
        public float remainingLifetime;
    }
}