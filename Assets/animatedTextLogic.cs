using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class animatedTextLogic : MonoBehaviour
{
    public string text;
    public enum AnimatedTextAnimation
    {
        StraightUp,RandomUp
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(string text, AnimatedTextAnimation textAnimation)
    {
        GetComponent<TextMeshProUGUI>().text = text;

        if (textAnimation == AnimatedTextAnimation.StraightUp)
            UpAnimation();
        else
            RandomUpAnimation();

    }

    public void UpAnimation()
    {
        Vector3 endpoint = transform.up * 50;
        GetComponent<TextMeshProUGUI>().DOFade(150, 1).OnComplete(DestroySelf);
        this.transform.DOBlendableLocalMoveBy(endpoint, 1);
    }

    public void RandomUpAnimation()
    {
        Vector3 endpoint = transform.up * 30;
        Vector3 endpoint2 = transform.right * Random.Range(-50,50);
        GetComponent<TextMeshProUGUI>().DOFade(100, 1).OnComplete(DestroySelf);
        this.transform.DOBlendableLocalMoveBy(endpoint, 1);
        this.transform.DOBlendableLocalMoveBy(endpoint2, 1);
    }


    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        
    }
}
