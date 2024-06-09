using UnityEditor;
using UnityEngine;

public class BaseCampBtn : MonoBehaviour
{
    public void OnReturnToBaseCampButtonClicked()
    {
        Time.timeScale = 1f;
        UIManager.instance.SetActiveUI(gameObject, false);
        SceneLoader.Instance.LoadScene("BaseCamp");
    }
}