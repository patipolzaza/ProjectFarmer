using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectDataContainer
{
    private static int lastIndex = 0;
    private static Dictionary<int, ObjectSaveData> objectSaveDatas = new Dictionary<int, ObjectSaveData>();

    public static Dictionary<int, ObjectSaveData> GetDatas => objectSaveDatas;

    public static void UpdateData(int index, ObjectSaveData newData)
    {
        objectSaveDatas[index] = newData;
    }

    public static int AddData(ObjectSaveData data)
    {
        objectSaveDatas.Add(lastIndex++, data);
        return lastIndex;
    }

    public static void Remove(int index)
    {
        objectSaveDatas.Remove(index);
    }

    public static void ClearDatas()
    {
        objectSaveDatas.Clear();
    }

    public static void SaveDatas()
    {
        SaveManager.Save("saveDatas", objectSaveDatas);
    }
}
