using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    // Config params
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprites;

    // Cached reference
    Level level;

    // state
    [SerializeField] int timesHit; // serialized for debugging purposes

    private void Start()
    {
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(tag == "Breakable")
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block sprite is missing from array!" + gameObject.name);
        }
    }

    private void DestroyBlock()
    {
        PlayDestroySFX();
        FindObjectOfType<GameSession>().AddToScore();
        Destroy(gameObject);
        TriggerSparklesVFX();
        level.BlockDestroyed();
    }

    private void PlayDestroySFX()
    {
        AudioSource.PlayClipAtPoint(breakSound, transform.position);
    }

    private void TriggerSparklesVFX()
    {
        GameObject spakles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(spakles, 1f);
    }
}
