using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="CreatManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVars()
    {
        //返回脚本引用
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
    public List<Type_Prefab> typePrefabs = new List<Type_Prefab>();

}