using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DmgEffect : MonoBehaviour
{
    [SerializeField] Image _dmgImage;
    private float _a;
    private float _r;
    private float _g;
    private float _b;

    private float _transparencyValue;
    // Start is called before the first frame update
    void Start()
    {
        _a = _dmgImage.color.a;
        _r = _dmgImage.color.r;
        _g = _dmgImage.color.g;
        _b = _dmgImage.color.b;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeColor(float _newTransparency){
        _a = _newTransparency;
        Color _newColor = new Color(_r,_g,_b,_a);
        _dmgImage.color = _newColor;
    }
}
