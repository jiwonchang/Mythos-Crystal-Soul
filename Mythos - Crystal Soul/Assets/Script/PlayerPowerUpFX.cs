using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerPowerUpFX : MonoBehaviour
{
    [SerializeField] GameObject emptyGameObject;
    //float[] shrinkRates = { 0.01f, 0.03f, 0.05f, 0.07f };
    //float[] minorShrinkRates = { 0.00625f, 0.01125f, 0.02025f, 0.03645f };
    //float[] minorGrowRates = { 0.00625f, 0.00875f, 0.01225f, 0.01715f, 0.02401f, 0.033614f, 0.0470596f, 0.06588344f }; // 1.4x ****
    //float[] minorGrowRates = { 0.00625f, 0.01225f, 0.02401f, 0.0470596f, 0.06588344f }; // 1.4x ****
    //float[] minorGrowRates = { 0.00625f, 0.01225f, 0.02401f, 0.0470596f }; // 1.4x ****
    float[] minorGrowRates = { 0.00625f, 0.01125f, 0.02025f, 0.03645f }; // 1.4x ****
    //float[] minorGrowRates = { 0.00625f, 0.009375f, 0.0140625f, 0.02109375f }; // 1.4x ****

    float[] superShrinkRates = { 0.00625f, 0.01125f, 0.02025f, 0.03645f };
    float[] superGrowRates = { 0.00625f, 0.01125f, 0.02025f, 0.03645f, 0.06561f, 0.118098f, 0.2125764f, 0.38263752f }; //*** 1.8x

    float[] mediumShrinkRates = { 0.00625f, 0.01125f, 0.02025f, 0.03645f };
    //float[] growRates = { 0.00625f, 0.01f, 0.016f, 0.0256f, 0.04096f, 0.065536f, 0.1048576f, 0.16777216f }; // 1.6x ****
    //float[] growRates = { 0.00625f, 0.010625f, 0.0180625f, 0.03070625f, 0.05220062f, 0.07830094f, 0.13311159f, 0.22628971f }; // 1.7x ****
    float[] mediumGrowRates = { 0.00625f, 0.01125f, 0.02025f, 0.03645f, 0.06561f, 0.118098f }; //*** 1.8x

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WaitForPUPKeyDown();
    }

    private void WaitForPUPKeyDown()
    {
        //CrossPlatformInputManager.GetButtonDown();
        if (Input.GetKeyDown(KeyCode.G)) {
            StartCoroutine(PlaySuperPowerUpFX());
        }
        else if (Input.GetKeyDown(KeyCode.R)) {
            StartCoroutine(PlayMinorPowerUpFX());
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(PlayMediumPowerUpFX());
        }
    }

    IEnumerator PlayMinorPowerUpFX()
    {
        /*
        for (int i = 0; i < superShrinkRates.Length; i++)
        {
            GameObject PUPFXShrink = Instantiate(emptyGameObject, transform.position, transform.rotation, transform) as GameObject;
            //PUPFX1.transform.parent = transform;
            PUPFXShrink.AddComponent<SpriteRenderer>();
            SpriteRenderer PUPFX1Renderer = PUPFXShrink.GetComponent<SpriteRenderer>();
            PUPFX1Renderer.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            PUPFX1Renderer.sortingLayerName = "Testing";
            Color tmp = PUPFX1Renderer.color;
            //tmp.a = .4f;
            //tmp.a = .05f + (i * 0.05f);
            tmp.a = .05f + (i * 0.08f);
            PUPFX1Renderer.color = tmp;
            //PUPFXShrink.transform.localScale = new Vector2(1.1f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.x), 1.1f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.y));
            //PUPFXShrink.transform.localScale = new Vector2(1.3f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.x), 1.3f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.y));
            PUPFXShrink.transform.localScale = new Vector2((1.3f - i * 0.05f) * Mathf.Abs(PUPFXShrink.transform.parent.localScale.x), (1.3f - i * 0.05f) * Mathf.Abs(PUPFXShrink.transform.parent.localScale.y));
            StartCoroutine(ShrinkAvatars(PUPFXShrink, minorShrinkRates, i));
            //Destroy(PUPFX1, 0.2f);
            //yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.3f);
        */
        yield return new WaitForSeconds(0f);
        for (int i = 0; i < minorGrowRates.Length; i++)
        {
            GameObject PUPFX1 = Instantiate(emptyGameObject, transform.position, transform.rotation, transform) as GameObject;
            //PUPFX1.transform.parent = transform;
            PUPFX1.AddComponent<SpriteRenderer>();
            SpriteRenderer PUPFX1Renderer = PUPFX1.GetComponent<SpriteRenderer>();
            PUPFX1Renderer.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            PUPFX1Renderer.sortingLayerName = "Player Effects";
            Color tmp = PUPFX1Renderer.color;
            //tmp.a = .5f;
            tmp.a = .4f;
            PUPFX1Renderer.color = tmp;
            //PUPFX1.transform.localScale = new Vector2(0.5f * Mathf.Abs(PUPFX1.transform.parent.localScale.x), 0.5f * Mathf.Abs(PUPFX1.transform.parent.localScale.y));
            PUPFX1.transform.localScale = new Vector2(1f * Mathf.Abs(PUPFX1.transform.parent.localScale.x), 1f * Mathf.Abs(PUPFX1.transform.parent.localScale.y));
            StartCoroutine(MinorGrowAvatars(PUPFX1, minorGrowRates, i));
            //Destroy(PUPFX1, 0.2f);
            //yield return new WaitForSeconds(0.02f);
        }
    }




    IEnumerator PlayMediumPowerUpFX()
    {
        for (int i = 0; i < mediumShrinkRates.Length; i++)
        {
            GameObject PUPFXShrink = Instantiate(emptyGameObject, transform.position, transform.rotation, transform) as GameObject;
            //PUPFX1.transform.parent = transform;
            PUPFXShrink.AddComponent<SpriteRenderer>();
            SpriteRenderer PUPFX1Renderer = PUPFXShrink.GetComponent<SpriteRenderer>();
            PUPFX1Renderer.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            PUPFX1Renderer.sortingLayerName = "Player Effects";
            Color tmp = PUPFX1Renderer.color;
            //tmp.a = .4f;
            //tmp.a = .05f + (i * 0.05f);
            tmp.a = .05f + (i * 0.08f);
            PUPFX1Renderer.color = tmp;
            //PUPFXShrink.transform.localScale = new Vector2(1.1f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.x), 1.1f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.y));
            //PUPFXShrink.transform.localScale = new Vector2(1.3f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.x), 1.3f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.y));
            PUPFXShrink.transform.localScale = new Vector2((1.3f - i * 0.05f) * Mathf.Abs(PUPFXShrink.transform.parent.localScale.x), (1.3f - i * 0.05f) * Mathf.Abs(PUPFXShrink.transform.parent.localScale.y));
            StartCoroutine(ShrinkAvatars(PUPFXShrink, mediumShrinkRates, i));
            //Destroy(PUPFX1, 0.2f);
            //yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < mediumGrowRates.Length; i++)
        {
            GameObject PUPFX1 = Instantiate(emptyGameObject, transform.position, transform.rotation, transform) as GameObject;
            //PUPFX1.transform.parent = transform;
            PUPFX1.AddComponent<SpriteRenderer>();
            SpriteRenderer PUPFX1Renderer = PUPFX1.GetComponent<SpriteRenderer>();
            PUPFX1Renderer.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            PUPFX1Renderer.sortingLayerName = "Player Effects";
            Color tmp = PUPFX1Renderer.color;
            //tmp.a = .5f;
            tmp.a = .4f;
            PUPFX1Renderer.color = tmp;
            //PUPFX1.transform.localScale = new Vector2(0.5f * Mathf.Abs(PUPFX1.transform.parent.localScale.x), 0.5f * Mathf.Abs(PUPFX1.transform.parent.localScale.y));
            PUPFX1.transform.localScale = new Vector2(1f * Mathf.Abs(PUPFX1.transform.parent.localScale.x), 1f * Mathf.Abs(PUPFX1.transform.parent.localScale.y));
            StartCoroutine(SuperGrowAvatars(PUPFX1, mediumGrowRates, i));
            //Destroy(PUPFX1, 0.2f);
            //yield return new WaitForSeconds(0.02f);
        }
    }




    IEnumerator PlaySuperPowerUpFX()
    {
        for (int i = 0; i < superShrinkRates.Length; i++)
        {
            GameObject PUPFXShrink = Instantiate(emptyGameObject, transform.position, transform.rotation, transform) as GameObject;
            //PUPFX1.transform.parent = transform;
            PUPFXShrink.AddComponent<SpriteRenderer>();
            SpriteRenderer PUPFX1Renderer = PUPFXShrink.GetComponent<SpriteRenderer>();
            PUPFX1Renderer.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            PUPFX1Renderer.sortingLayerName = "Player Effects";
            Color tmp = PUPFX1Renderer.color;
            //tmp.a = .4f;
            //tmp.a = .05f + (i * 0.05f);
            tmp.a = .05f + (i * 0.08f);
            PUPFX1Renderer.color = tmp;
            //PUPFXShrink.transform.localScale = new Vector2(1.1f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.x), 1.1f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.y));
            //PUPFXShrink.transform.localScale = new Vector2(1.3f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.x), 1.3f * Mathf.Abs(PUPFXShrink.transform.parent.localScale.y));
            PUPFXShrink.transform.localScale = new Vector2((1.3f - i * 0.05f) * Mathf.Abs(PUPFXShrink.transform.parent.localScale.x), (1.3f - i * 0.05f) * Mathf.Abs(PUPFXShrink.transform.parent.localScale.y));
            StartCoroutine(ShrinkAvatars(PUPFXShrink, superShrinkRates, i));
            //Destroy(PUPFX1, 0.2f);
            //yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < superGrowRates.Length; i++)
        {
            GameObject PUPFX1 = Instantiate(emptyGameObject, transform.position, transform.rotation, transform) as GameObject;
            //PUPFX1.transform.parent = transform;
            PUPFX1.AddComponent<SpriteRenderer>();
            SpriteRenderer PUPFX1Renderer = PUPFX1.GetComponent<SpriteRenderer>();
            PUPFX1Renderer.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            PUPFX1Renderer.sortingLayerName = "Player Effects";
            Color tmp = PUPFX1Renderer.color;
            //tmp.a = .5f;
            tmp.a = .4f;
            PUPFX1Renderer.color = tmp;
            //PUPFX1.transform.localScale = new Vector2(0.5f * Mathf.Abs(PUPFX1.transform.parent.localScale.x), 0.5f * Mathf.Abs(PUPFX1.transform.parent.localScale.y));
            PUPFX1.transform.localScale = new Vector2(1f * Mathf.Abs(PUPFX1.transform.parent.localScale.x), 1f * Mathf.Abs(PUPFX1.transform.parent.localScale.y));
            StartCoroutine(SuperGrowAvatars(PUPFX1, superGrowRates, i));
            //Destroy(PUPFX1, 0.2f);
            //yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator ShrinkAvatars(GameObject avatar, float[] shrinkArray, int index)
    {
        SpriteRenderer avatarRenderer = avatar.GetComponent<SpriteRenderer>();
        while (avatarRenderer.color.a > 0)
        {
            avatar.transform.localScale = new Vector2(avatar.transform.localScale.x - shrinkArray[index], avatar.transform.localScale.y - shrinkArray[index]);
            Color tmp = avatarRenderer.color;
            //tmp.a -= .0025f;
            //tmp.a -= 0.0015f + (shrinkArray[index] * 0.05f);
            tmp.a -= 0.0025f + (shrinkArray[index] * 0.05f);
            avatarRenderer.color = tmp;
            yield return new WaitForSeconds(0.0f);
        }
        Destroy(avatar);
    }

    IEnumerator SuperGrowAvatars(GameObject avatar, float[] growArray, int index)
    {
        SpriteRenderer avatarRenderer = avatar.GetComponent<SpriteRenderer>();
        while (avatarRenderer.color.a > 0)
        {
            avatar.transform.localScale = new Vector2(avatar.transform.localScale.x + growArray[index], avatar.transform.localScale.y + growArray[index]);
            Color tmp = avatarRenderer.color;

            //if (index < (growArray.Length / 2f))
            if (index < (growArray.Length / 3f))
            {
                tmp.a -= 0.005f; 
            }
            else
            {
                //tmp.a -= 0.005f + (index * 0.0004f);
                //tmp.a -= 0.005f + (growRates[index] * 0.02f); // pretty good with 1.6x or 1.7x
                tmp.a -= 0.005f + (growArray[index] * 0.025f);
            }
            avatarRenderer.color = tmp;
            yield return new WaitForSeconds(0.0f);
        }
        Destroy(avatar);
    }

    IEnumerator MinorGrowAvatars(GameObject avatar, float[] growArray, int index)
    {
        SpriteRenderer avatarRenderer = avatar.GetComponent<SpriteRenderer>();
        while (avatarRenderer.color.a > 0)
        {
            avatar.transform.localScale = new Vector2(avatar.transform.localScale.x + growArray[index], avatar.transform.localScale.y + growArray[index]);
            Color tmp = avatarRenderer.color;

            //if (index < (growRates.Length / 2f))
            if (index < (growArray.Length / 3f))
            {
                tmp.a -= 0.005f;
            }
            if (true)
            {
                //tmp.a -= 0.005f + (index * 0.0004f);
                //tmp.a -= 0.005f + (growRates[index] * 0.02f); // pretty good with 1.6x or 1.7x
                tmp.a -= 0.005f + (growArray[index] * 0.025f);
            }
            avatarRenderer.color = tmp;
            yield return new WaitForSeconds(0.0f);
        }
        Destroy(avatar);
    }
}
