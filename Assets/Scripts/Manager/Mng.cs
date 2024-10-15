using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 프로젝트 내의 Manager(Singleton)들을 관리하는 클래스
/// </summary>
static public class Mng
{
    static public StageManager stage
    {
        get { return StageManager.Instance; }
    }
    
    static public CanvasManager canvas
    {
        get { return CanvasManager.Instance; }
    }
}
