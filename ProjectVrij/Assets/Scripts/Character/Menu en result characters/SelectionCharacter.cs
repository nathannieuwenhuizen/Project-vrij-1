using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCharacter : MonoBehaviour
{

    [SerializeField]
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetLayerWeight(1, 0);
    }
    private void OnEnable()
    {
        anim.SetBool("taunt", true);
    }
    private void OnDisable()
    {
        anim.SetBool("taunt", false);
    }
}
