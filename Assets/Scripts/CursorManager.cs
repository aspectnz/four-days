using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
  public Texture2D cursor;
  public Texture2D cursorActive;






  // Start is called before the first frame update
  void Start()
  {
    ChangeCursor(cursor);
    Cursor.lockState = CursorLockMode.Confined;
  }

  public void OnMouseEnter()
  {
    ChangeCursor(cursorActive);
  }

  public void OnMouseExit()
  {
    ChangeCursor(cursor);
  }


  private void ChangeCursor(Texture2D cursorType)
  {
    // Vector2 cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
    Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
  }


}
