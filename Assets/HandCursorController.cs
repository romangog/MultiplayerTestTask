
using UnityEngine;
using UnityEngine.UI;

public class HandCursorController : MonoBehaviour
{
    [SerializeField] private KeyCode activationKey = KeyCode.H;

    [SerializeField] Transform handMain;
    [SerializeField] Transform handReal;
    [SerializeField] Transform handShadow;


    private bool IsOn = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            SwitchCursorMode();
        }

        if (IsOn)
        {
            handMain.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            handReal.Rotate(Vector3.up * 50, Space.Self);
            handShadow.localScale = new Vector3(0.8f, 0.9f, 1);
            handShadow.localPosition = Vector3.zero;
        }
        if (Input.GetMouseButtonUp(0))
        {
            handReal.Rotate(-Vector3.up * 50, Space.Self);
            handShadow.localScale = new Vector3(1, 1, 1);
            handShadow.localPosition = new Vector3(27.7f, -18.2f, 0);
        }
    }

    private void SwitchCursorMode()
    {
        IsOn = !IsOn;

        if (IsOn)
            handMain.gameObject.SetActive(true);
        else
            handMain.gameObject.SetActive(false);
    }
}
