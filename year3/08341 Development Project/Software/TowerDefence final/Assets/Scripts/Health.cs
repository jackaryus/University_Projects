using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
    TextMesh tm;

	void Start() 
    {
        tm = GetComponent<TextMesh>();
	}
	
	void Update() 
    {
        transform.forward = Camera.main.transform.forward;
	}

    public int currentHealth()
    {
        return tm.text.Length;
    }

    public void DecreaseHealth()
    {
        if (currentHealth() > 1)
        {
            tm.text = tm.text.Remove(tm.text.Length - 1);
        }
        else if (transform.parent.gameObject.name == "Castle")
        {
            Destroy(transform.parent.gameObject);
            Application.LoadLevel(0);
        }
        else
            Destroy(transform.parent.gameObject);
    }
}
