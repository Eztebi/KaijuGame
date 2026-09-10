using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class RadioactiveExplosion : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private GameObject kaiju;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private float hideVelocity;
    [SerializeField] private float timeToHide;
    [SerializeField] private CinemachineCamera camera;
    public void PlayExplotion()
    {
        if (explosionParticle != null)
        {
            ParticleSystem explosion = Instantiate(explosionParticle, kaiju.transform);
            explosion.Play();
        }

        StartCoroutine(HideItems());
    }

    private IEnumerator HideItems()
    {
        float elapsed = 0f;

        while (elapsed < timeToHide)
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.transform.position += Vector3.down * hideVelocity * Time.deltaTime;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }
        camera.Priority = 10;
    }
    public void GrowKaiju()
    {
        kaiju.transform.localScale *= 2;
    }
}
