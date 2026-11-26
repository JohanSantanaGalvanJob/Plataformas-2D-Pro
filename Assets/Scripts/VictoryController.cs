using TMPro;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI txtRank;
    public GameObject Image;

    private void Start() {
        txtRank.text = GameResults.rank;
        if (GameResults.rank.Equals("A")) {
            GameObject activationBlockObject = GameObject.FindGameObjectWithTag("Prize");
            Image.SetActive(true);
        }
    }
}
