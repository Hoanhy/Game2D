using UnityEngine;
using System.Collections;
using UnityEditor.Profiling.Memory.Experimental;

public class Weapon : MonoBehaviour
{
    private float rotateOffset = 180f;
    public GameObject sword;
    //private SpriteRenderer spriteRenderer;
    void Start()
    {
  
        //spriteRenderer.enabled = false;
    }


    void Update()
    {
        RotateSword();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    StartCoroutine(Attack());
        //}
    }
    void RotateSword()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }

        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);
    }

//    IEnumerator Attack()
//    {
//        sword.SetActive(true);
//        spriteRenderer.enabled = true;
//        yield return new WaitForSeconds(0.3f);
//        sword.SetActive(false);
//        spriteRenderer.enabled = false;
//    }
}
