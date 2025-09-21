using UnityEngine;

public class WallBox : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void WallOpen(bool isOpen)
    {
        anim.SetBool("IsOpen", isOpen);
    }
}
