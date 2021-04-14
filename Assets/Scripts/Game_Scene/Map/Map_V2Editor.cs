using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Map_V2))]
public class Map_V2Editor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        Map_V2 map = target as Map_V2;

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
#endif