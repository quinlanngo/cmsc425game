using UnityEngine;

public class LockCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        // esc to unlock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


}
