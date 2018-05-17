#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class EditorLabel : MonoBehaviour
{
    private static GUIStyle style;


    public string text;

    private static GUIStyle Style
    {
        get
        {
            if (style == null)
            {
                style = new GUIStyle(EditorStyles.largeLabel);
                style.alignment = TextAnchor.MiddleCenter;
                style.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
                style.fontSize = 32;
            }

            return style;
        }
    }


    private void OnDrawGizmos()
    {
        RaycastHit hit;
        var r = new Ray(transform.position + Camera.current.transform.up * 8f, -Camera.current.transform.up);
        if (GetComponent<Collider>().Raycast(r, out hit, Mathf.Infinity))
        {
            var dist = (Camera.current.transform.position - hit.point).magnitude;

            var fontSize = Mathf.Lerp(64, 12, dist / 10f);

            Style.fontSize = (int) fontSize;

            var wPos = hit.point + Camera.current.transform.up * dist * 0.07f;


            var scPos = Camera.current.WorldToScreenPoint(wPos);
            if (scPos.z <= 0) return;


            var alpha = Mathf.Clamp(-Camera.current.transform.forward.y, 0f, 1f);
            alpha = 1f - (1f - alpha) * (1f - alpha);

            alpha = Mathf.Lerp(-0.2f, 1f, alpha);

            Handles.BeginGUI();


            scPos.y = Screen.height - scPos.y; // Flip Y


            var strSize = Style.CalcSize(new GUIContent(text));

            var rect = new Rect(0f, 0f, strSize.x + 6, strSize.y + 4);
            rect.center = scPos - Vector3.up * rect.height * 0.5f;
            GUI.color = new Color(0f, 0f, 0f, 0.8f * alpha);
            GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
            GUI.color = Color.white;
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            GUI.Label(rect, text, Style);
            GUI.color = Color.white;

            Handles.EndGUI();
        }
    }
}
#endif