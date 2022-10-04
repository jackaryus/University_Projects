using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class StartGame : MonoBehaviour {
    public Text ipText;
    public Text placeholder;

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }

    public void LoadMultiplayerScene(int level)
    {
        try
        {
            IPAddress ip = IPAddress.Parse(ipText.text);
            Networking.addressip = ipText.text;
            Application.LoadLevel(level);
        }
        catch 
        {
            placeholder.text = "Enter valid ip...";
            placeholder.color = Color.red;
        }
        
    }
}
