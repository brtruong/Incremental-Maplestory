using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    public static Logger Instance {get; private set;}

    [SerializeField] private bool _ShowLogs,_UseScreenLogger , _IsScreenLogger;
    [SerializeField] private Text _TextLog;

    private int _LineCount;
    private static int _MaxLineCount = 28;

    private void Awake ()
    {
        if (Instance && _IsScreenLogger) Destroy(this);
        if (!Instance && _IsScreenLogger) Instance = this;
        _LineCount = 0;
    }

    public void Log (GameObject obj, string text)
    {
        if (!_ShowLogs) return;

        Debug.Log(string.Concat(obj.name, ": ", text));
        if (_UseScreenLogger) ScreenLog (obj.name, text);
    }

    public static void ScreenLog (string caller, string text)
    {
        if (!Instance) return;

        if (Instance._LineCount >= _MaxLineCount)
        {
            Instance._TextLog.text = string.Empty;
            Instance._LineCount = 0;
        }    
        Instance._TextLog.text += string.Concat("\n", caller, ": ", text);
        Instance._LineCount++;
    }
}
