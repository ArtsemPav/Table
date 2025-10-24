using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject rightPopup;
    [SerializeField] private GameObject wrongPopup;
    [SerializeField] private GameObject errorPopup;
    void Start()
    {
        rightPopup.SetActive(false);
        wrongPopup.SetActive(false);
        errorPopup.SetActive(false);
    }

    public void ShowRightPopup()

}
