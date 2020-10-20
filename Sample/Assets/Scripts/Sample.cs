using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private const float SCREEN_WIDTH = 540f;
    private const float SCREEN_HEIGHT = 960f;

    private List<string> messages = new List<string>();
    private Vector2 logPosition;

    private Vector3 scaleV3;
    private float width, height;

    private static EdgeInsets SafeAreaInsets
    {
        get
        {
            var safeArea = Screen.safeArea;
            return new EdgeInsets(safeArea.yMin, safeArea.xMin, Screen.height - safeArea.yMax, Screen.width - safeArea.xMax);
        }
    }

    private void Awake()
    {
        float scaleX = Screen.width / SCREEN_WIDTH;
        float scaleY = Screen.height / SCREEN_HEIGHT;
        float scale = scaleX < scaleY ? scaleX : scaleY;
        scaleV3 = new Vector3(scale, scale, 1.0f);
        
        width = Screen.width / scale;
        height = Screen.height / scale;
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.black;
        var btnStyle = GUI.skin.button;
        btnStyle.alignment = TextAnchor.MiddleCenter;
        btnStyle.fontSize = 20;

        var labelStyle = GUI.skin.label;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 20;
        labelStyle.clipping = TextClipping.Overflow;

        GUI.matrix = Matrix4x4.Scale(scaleV3);

        labelStyle.fontStyle = FontStyle.BoldAndItalic;
        GUI.Label(new Rect((width - 300) / 2, 60 + SafeAreaInsets.Top, 300, 60), "SS ATT Sample for Unity", labelStyle);

        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontStyle = FontStyle.Normal;

        if (GUI.Button(new Rect((width - 300) / 2, 130 + SafeAreaInsets.Top, 300, 60), "RequestAdvertisingIdentifier"))
        {
            AddMessage("Click RequestAdvertisingIdentifier");
            Application.RequestAdvertisingIdentifierAsync((advertisingId, trackingEnabled, errorMsg) =>
            {
                AddMessage($"RequestAdvertisingIdentifier completed. advertisingId:{advertisingId}, trackingEnabled:{trackingEnabled}, errorMsg:{errorMsg}");
            });
        }

        if (GUI.Button(new Rect((width - 300) / 2, 200 + SafeAreaInsets.Top, 300, 60), "IsAvailable"))
        {
            AddMessage($"IsAvailable:{Supership.ATT.TrackingManager.IsAvailable}");
        }

        if (GUI.Button(new Rect((width - 300) / 2, 270 + SafeAreaInsets.Top, 300, 60), "RequestTrackingAuthorization"))
        {
            AddMessage("Click RequestTrackingAuthorization button.");
            Supership.ATT.TrackingManager.RequestTrackingAuthorization(status =>
            {
                AddMessage($"RequestTrackingAuthorization completed. TrackingAuthorizationStatus:{status}");
            });
        }

        if (GUI.Button(new Rect((width - 300) / 2, 340 + SafeAreaInsets.Top, 300, 60), "TrackingAuthorizationStatus"))
        {
            var status = Supership.ATT.TrackingManager.TrackingAuthorizationStatus;
            AddMessage($"TrackingAuthorizationStatus:{status}");
        }

        GUILayout.BeginArea(new Rect(20, 410 + SafeAreaInsets.Top, width - 40, height - 350 - SafeAreaInsets.Bottom));

        logPosition = GUILayout.BeginScrollView(logPosition);
        var count = 0;

        for (var i = messages.Count - 1; i >= 0; i --)
        {
            GUILayout.Label(messages[i], labelStyle);
            count++;

            if (count >= 200) break;
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

        labelStyle.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, height - 60 - SafeAreaInsets.Bottom, width, 50),
           $"APP {Application.version} / SDK {Supership.ATT.TrackingManager.Version}", labelStyle);

    }

    void AddMessage(string str)
    {
        messages.Add(System.DateTime.Now.ToString("MM/dd HH:mm:ss ") + str);

        Debug.Log("[SSATT] " + str);
    }

    public struct EdgeInsets
    {
        public float Top { get; }

        public float Left { get; }

        public float Bottom { get; }

        public float Right { get; }

        public EdgeInsets(float top, float left, float bottom, float right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public override string ToString()
        {
            return
                string.Format("Top:{0}, Left:{1} Bottom:{2} Right:{3}", Top, Left, Bottom, Right);
        }
    }
}
