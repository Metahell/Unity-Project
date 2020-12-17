using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem Rockshard;
    [SerializeField]
    private MeshRenderer self;
    [SerializeField]
    private Color StartColor;
    private Color EndColor;
    // Start is called before the first frame update
    void Start()
    {
        EndColor = new Color(StartColor.r, StartColor.g, StartColor.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<HealthOrb>().Damage(10);
        }
        Rockshard.Play();
        StartCoroutine(DestroyRock());
    }
    private IEnumerator DestroyRock()
    {
        StartCoroutine(Fade());
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
    private IEnumerator Fade()
    {
        float t;
        float nbFrame = 1 / Time.deltaTime;
        for (int i = 1; i < nbFrame + 1; i++)
        {
            t = i / nbFrame;
            self.material.color = Color.Lerp(StartColor, EndColor, t);
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
