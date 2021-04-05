using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        Map map = target as Map;
        //Inspector에서 값 변경 일어나면 true 로 반환해줌.
        if (DrawDefaultInspector())
        {
            map.GeneratorMap();
        }
        //수동으로 맵 생성 스크립트에서 값 변경 됐을 경우에 사용
        if (GUILayout.Button("Gereted Map"))
        {
            map.GeneratorMap();
        }
    }
}