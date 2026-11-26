
using UnityEngine;

public class PlayGame : MonoBehaviour {
    public void GoToGame() {
        SCManager.instance.LoadScene("GameScene");
    }

    public void GoToMenu() {
        SCManager.instance.LoadScene("MenuScene");
    }
}
