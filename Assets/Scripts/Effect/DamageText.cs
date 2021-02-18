using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DamageText : MonoBehaviour
{
    [SerializeField]
    TextMesh damage;
    float lifeTime = 1;
    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            transform.LookAt(Camera.main.transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ViewDamege(int i,Vector3 pos)
    {
        damage.text = "" + i;
        transform.position = pos * 1.02f;
        lifeTime = 1;
    }
}
