using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotodiodeToggle : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    private bool _black;

    private void Awake()
    {
        _black = true;
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.material.color = _black ? Color.black : Color.white;
    }

    public void TogglePhotodiode()
    {
        _black = !_black;
    }

    public void FlashPhotodiode()
    {
        _black = !_black;
        StartCoroutine(DelayedFlashRevert());
    }

    private IEnumerator DelayedFlashRevert()
    {
        yield return new WaitForSeconds(0.020f);
        _black = !_black;
    }
}