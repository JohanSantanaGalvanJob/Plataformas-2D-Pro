using UnityEngine;

public class CameraController : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    GameObject player;

    public float cameraPosX;
    public float cameraPosXMin;
    public float cameraPosXMax;

    public float cameraPosY;
    public float cameraPosYMin;
    public float cameraPosYMax;

    void Start() {
        AudioManager.instance.PlayMusic("MainTheme");
    }

    // Update is called once per frame
    void Update() {
        cameraPosX = Mathf.Clamp(player.transform.position.x, cameraPosXMin, cameraPosXMax);
        cameraPosY = Mathf.Clamp(player.transform.position.y,cameraPosYMin,cameraPosYMax);
        transform.position = new Vector3(cameraPosX, cameraPosY, transform.position.z);

    }
}
