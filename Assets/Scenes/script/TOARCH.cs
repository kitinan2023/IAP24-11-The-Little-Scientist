using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TOARCH : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()  {
    }
    public void OpenScene(){
        SceneManager.LoadScene("arch");
    
    }
}
