using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float restoreDefautMatTime = 0.2f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Material defaultMat;
    

    private void Awake()
    {
        defaultMat = spriteRenderer.material;
    }

    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(restoreDefautMatTime);
        spriteRenderer.material = defaultMat;
    }
}
