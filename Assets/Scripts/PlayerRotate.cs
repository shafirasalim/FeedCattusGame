using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//  butuh komponen yang dibutuhkan (playerinput), tapi sudah di playerMove
public class PlayerRotate : MonoBehaviour
{
    private void OnLook(InputValue value)
    {
        Vector2 posisiMouse = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        LookAt(posisiMouse);
    }
    private float AngleBetween2Point(Vector3 object1, Vector3 object2)
    {
        return Mathf.Atan2(object2.y - object1.y, object2.x - object1.x) * Mathf.Rad2Deg;
    }
    private void LookAt(Vector3 tujuan)
    {

        float angle = AngleBetween2Point(transform.position, tujuan);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}