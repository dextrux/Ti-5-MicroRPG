using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform playerCam;

    void Start()
    {
        // Pegamos a câmera do jogador (MainCamera)
        playerCam = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Faz o canvas olhar diretamente para a câmera
        transform.LookAt(transform.position + playerCam.forward);
    }
}
