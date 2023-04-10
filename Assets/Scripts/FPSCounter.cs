using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _text;
    private float _lastFrame;

    // Start is called before the first frame update
    void Start()
    {
        _lastFrame = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        float delay = Time.realtimeSinceStartup - _lastFrame;
        _lastFrame = Time.realtimeSinceStartup;

        _text.text = Mathf.RoundToInt(1 / delay).ToString();
    }
}
