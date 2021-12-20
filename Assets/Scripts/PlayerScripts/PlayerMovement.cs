using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public GameObject invisibleTimer;
    public float moveSpeed=10,mobilemoveSpeed=150;
    private Rigidbody2D rb;
    public GameObject bullet;
    private float ScreenWidth;
    private bool canWalk,isWalk=false;
    private bool canShoot;
    Animator animator;

    [HideInInspector] public bool armor;
    public float invinsible;
    bool alphaState;
    [SerializeField] float alpha = 1;
    void Start()
    {
        animator=GetComponent<Animator>();
        canWalk=true;canShoot=true;
        ScreenWidth=Screen.width;
        rb=GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(invinsible > 0)
        {
            invinsible -= Time.deltaTime;
            invisibleTimer.SetActive(true);
            invisibleTimer.GetComponent<TextMeshProUGUI>().text = (invinsible % 10).ToString();
            if (GetComponent<SpriteRenderer>().color.a > .2f && !alphaState)
            {
                alpha -= Time.deltaTime;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
                if (alpha < .2)
                    alphaState = true;
            }
            else if (alphaState)
            {
                alpha += Time.deltaTime;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
                if (alpha > 1)
                    alphaState = false;
            }
        }
        else
        {
            invisibleTimer.GetComponent<TextMeshProUGUI>().text = "0";
            invisibleTimer.SetActive(false);
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
            
    }

    void FixedUpdate()
    {
        int i=0;
        while(i<Input.touchCount)
        {
            if(Input.GetTouch(i).position.x>ScreenWidth/2)
            {
                //move right and shoot
                /*if(canWalk)
                {
                    MoveCharachter(1,mobilemoveSpeed);
                }*/
                //canWalk=false;
                if(canShoot)
                StartCoroutine(Shoot_());

            }/*
            if(Input.GetTouch(i).position.x<ScreenWidth/2)
            {
                //move left
                if(canWalk)
                {
                    MoveCharachter(-1,mobilemoveSpeed);
                }
            }*/
            ++i;
        }
        if(Input.touchCount==0)
        {
            animator.SetBool("isWalk",false);
            isWalk=false;
        }
        /*if(Input.touchCount>=2)
        {
            canWalk=false;
            if(canShoot)
                StartCoroutine(Shoot_());
        }*/


        if(Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1))
        { 

            canWalk=false;
            if(canShoot)
                StartCoroutine(Shoot_());
            //StartCoroutine(Shoot());
        }

        if(canWalk)
        {
            MoveCharachter(Input.GetAxis("Horizontal"),moveSpeed);
        }
        if(!Input.anyKey)
        {
            animator.SetBool("isWalk",false);
            isWalk=false;
        }
    }
    public void MoveCharachter(float input,float speed)
    {
        isWalk=true;
        animator.SetBool("isWalk",true);
        transform.position+=new Vector3(input*speed*Time.deltaTime,0,0);
    }
    IEnumerator Shoot_()
    {
        AudioManager.instance.AudioPlay("Fire");
        animator.SetBool("isShoot",true);
        canWalk=false;
        canShoot=false;
        Vector2 temp=transform.position;
        temp.y+=1;
        //mekanik instantiate vs 
        Instantiate(bullet,temp,Quaternion.identity);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bullet,temp,Quaternion.identity);
        yield return new WaitForSeconds(0.05f);
        Instantiate(bullet,temp,Quaternion.identity);
        canShoot=true;
        canWalk=true;
        animator.SetBool("isShoot",false); 
    }
}
