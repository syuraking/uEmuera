﻿using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class GenericUtils
{
    public static void Info(object content)
    {
        UnityEngine.Debug.Log(content);
    }
    public static void Warn(object content)
    {
        UnityEngine.Debug.LogWarning(content);
    }
    public static void Error(object content)
    {
        UnityEngine.Debug.LogError(content);
    }
    /// <summary>
    /// 获得子对象
    /// </summary>
    public static Component FindChildByName(System.Type type, GameObject obj,
                                            string childname, bool includeInactive = false)
    {
        if(!obj)
            return null;
        var list = obj.GetComponentsInChildren(type, includeInactive);
        foreach(var v in list)
        {
            if(v.name.CompareTo(childname) == 0)
                return v;
        }
        return null;
    }
    /// <summary>
    /// 获得子对象
    /// </summary>
    public static T FindChildByName<T>(GameObject obj, string childname, bool includeInactive = false) where T : Component
    {
        if(!obj)
            return null;
        var list = obj.GetComponentsInChildren<T>(includeInactive);
        foreach(var v in list)
        {
            if(v.name.CompareTo(childname) == 0)
                return v;
        }
        return null;
    }
    /// <summary>
    /// 获得子对象
    /// </summary>
    public static GameObject FindChildByName(GameObject obj, string childname, bool includeInactive = false)
    {
        if(!obj)
            return null;
        var list = obj.GetComponentsInChildren<Transform>(includeInactive);
        foreach(var v in list)
        {
            if(v.name.CompareTo(childname) == 0)
                return v.gameObject;
        }
        return null;
    }
    public static UnityEngine.Color ToUnityColor(uEmuera.Drawing.Color color)
    {
        return new UnityEngine.Color(color.r, color.g, color.b, color.a);
    }
    public static string GetColorCode(UnityEngine.Color color)
    {
        return GetColorCode(new uEmuera.Drawing.Color(color.r, color.g, color.b, color.a));
    }
    public static string GetColorCode(uEmuera.Drawing.Color color)
    {
        return string.Format("{0:x8}", color.ToRGBA());
    }

    public class PointerClickListener : MonoBehaviour, IPointerClickHandler
    {
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            var _callbacks1 = callbacks1;
            foreach(var callback in _callbacks1)
                callback();
            var _callbacks2 = callbacks2;
            foreach(var callback in _callbacks2)
                callback(eventData);
        }
        void OnDestroy()
        {
            callbacks1.Clear();
            callbacks2.Clear();
        }
        public HashSet<Action> callbacks1 = new HashSet<Action>();
        public HashSet<Action<PointerEventData>> callbacks2 = new HashSet<Action<PointerEventData>>();
    }
    /// <summary>
    /// 设置OnClick回调
    /// </summary>
    /// <param name="obj">设置回调的目标UI</param>
    /// <param name="callback">回调函数</param>
    public static void SetListenerOnClick(GameObject obj, Action callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<PointerClickListener>();
        if(!l)
            l = obj.AddComponent<PointerClickListener>();
        l.callbacks1.Add(callback);
    }
    public static void SetListenerOnClick(GameObject obj, Action<PointerEventData> callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<PointerClickListener>();
        if(!l)
            l = obj.AddComponent<PointerClickListener>();
        l.callbacks2.Add(callback);
    }
    public static void RemoveListenerOnClick(GameObject obj, Action callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<PointerClickListener>();
        if(!l)
            return;
        l.callbacks1.Remove(callback);
    }
    public static void RemoveListenerOnClick(GameObject obj, Action<PointerEventData> callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<PointerClickListener>();
        if(!l)
            return;
        l.callbacks2.Remove(callback);
    }
    public static void RemoveListenerOnClick(GameObject obj)
    {
        if(!obj)
            return;
        var l = obj.GetComponent<PointerClickListener>();
        if(!l)
            return;
        l.callbacks1 = new HashSet<Action>();
        l.callbacks2 = new HashSet<Action<PointerEventData>>();
    }


    //监听类
    class BeginDragListener : MonoBehaviour, IBeginDragHandler
    {
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            foreach(var callback in callbacks)
                callback(eventData);
        }
        void OnDestroy()
        {
            callbacks.Clear();
        }
        public HashSet<Action<PointerEventData>> callbacks = new HashSet<Action<PointerEventData>>();
    }
    class DragListener : MonoBehaviour, IDragHandler
    {
        public virtual void OnDrag(PointerEventData eventData)
        {
            foreach(var callback in callbacks)
                callback(eventData);
        }
        void OnDestroy()
        {
            callbacks.Clear();
        }
        public HashSet<Action<PointerEventData>> callbacks = new HashSet<Action<PointerEventData>>();
    }
    class EndDragListener : MonoBehaviour, IEndDragHandler
    {
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            foreach(var callback in callbacks)
                callback(eventData);
        }
        void OnDestroy()
        {
            callbacks.Clear();
        }
        public HashSet<Action<PointerEventData>> callbacks = new HashSet<Action<PointerEventData>>();
    }

    /// <summary>
    /// 设置OnDrag回调
    /// </summary>
    /// <param name="obj">设置回调的目标UI</param>
    /// <param name="callback">回调函数</param>
    public static void SetListenerOnDrag(GameObject obj, Action<PointerEventData> callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<DragListener>();
        if(!l)
            l = obj.AddComponent<DragListener>();
        l.callbacks.Add(callback);
    }
    public static void RemoveListenerOnDrag(GameObject obj, Action<PointerEventData> callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<DragListener>();
        if(!l)
            return;
        l.callbacks.Remove(callback);
    }
    public static void RemoveListenerOnDrag(GameObject obj)
    {
        if(!obj)
            return;
        var l = obj.GetComponent<DragListener>();
        if(!l)
            return;
        l.callbacks.Clear();
    }
    /// <summary>
    /// 设置OnBeginDrag回调
    /// </summary>
    /// <param name="obj">设置回调的目标UI</param>
    /// <param name="callback">回调函数</param>
    public static void SetListenerOnBeginDrag(GameObject obj, Action<PointerEventData> callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<BeginDragListener>();
        if(!l)
            l = obj.AddComponent<BeginDragListener>();
        l.callbacks.Add(callback);
    }
    public static void RemoveListenerOnBeginDrag(GameObject obj, Action<PointerEventData> callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<BeginDragListener>();
        if(!l)
            return;
        l.callbacks.Remove(callback);
    }
    public static void RemoveListenerOnBeginDrag(GameObject obj)
    {
        if(!obj)
            return;
        var l = obj.GetComponent<BeginDragListener>();
        if(!l)
            return;
        l.callbacks.Clear();
    }
    /// <summary>
    /// 设置OnEndDrag回调
    /// </summary>
    /// <param name="obj">设置回调的目标UI</param>
    /// <param name="callback">回调函数</param>
    public static void SetListenerOnEndDrag(GameObject obj, Action<PointerEventData> callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<EndDragListener>();
        if(!l)
            l = obj.AddComponent<EndDragListener>();
        l.callbacks.Add(callback);
    }
    public static void RemoveListenerOnEndDrag(GameObject obj, Action<PointerEventData> callback)
    {
        if(!obj || callback == null)
            return;
        var l = obj.GetComponent<EndDragListener>();
        if(!l)
            return;
        l.callbacks.Remove(callback);
    }
    public static void RemoveListenerOnEndDrag(GameObject obj)
    {
        if(!obj)
            return;
        var l = obj.GetComponent<EndDragListener>();
        if(!l)
            return;
        l.callbacks.Clear();
    }

    class CoroutineHelper : MonoBehaviour
    {
        public static CoroutineHelper instance
        {
            get
            {
                if(!instance_)
                {
                    var obj = new GameObject();
                    obj.name = "CoroutineHelper";
                    instance_ = obj.AddComponent<CoroutineHelper>();
                }
                return instance_;
            }
        }
        static CoroutineHelper instance_ = null;

        public Coroutine DoCoroutine(System.Collections.IEnumerator e)
        {
            return StartCoroutine(e);
        }
    }
    /// <summary>
    /// 开启协程，方便在非MonoBehaviour对象中使用协程
    /// </summary>
    public static Coroutine StartCoroutine(System.Collections.IEnumerator e)
    {
        return CoroutineHelper.instance.DoCoroutine(e);
    }
    public static void StopAllCoroutines()
    {
        CoroutineHelper.instance.StopAllCoroutines();
    }
    public static void StopCoroutine(Coroutine co)
    {
        CoroutineHelper.instance.StopCoroutine(co);
    }

    public static void SetBackgroundColor(uEmuera.Drawing.Color color)
    {
        text_content.SetBackgroundColor(color);
    }
    public static void ClearText()
    {
        //text_content.Clear();
        text_content.RemoveLine(text_content.max_log_count);
    }
    public static void AddText(object console_line, bool roll_to_bottom)
    {
        text_content.AddLine(console_line, roll_to_bottom);
    }
    public static object GetText(int index)
    {
        return text_content.GetLine(index);
    }
    public static int GetTextCount()
    {
        return text_content.GetLineCount();
    }
    public static int GetTextMaxLineNo()
    {
        return text_content.GetMaxLineNo();
    }
    public static int GetTextMinLineNo()
    {
        return text_content.GetMinLineNo();
    }
    public static void RemoveTextCount(int count)
    {
        text_content.RemoveLine(count);
    }
    public static void ToBottom()
    {
        text_content.ToBottom();
    }
    public static void TextUpdate()
    {
        text_content.Update();
    }
    public static void SetLastButtonGeneration(int generation)
    {
        text_content.SetLastButtonGeneration(generation);
    }
    public static void ShowIsInProcess(bool value)
    {
        text_content.isprocess.SetActive(value);
    }
    static EmueraContent text_content
    {
        get
        {
            if(_text_content == null)
            {
                _text_content = EmueraContent.instance;
            }
            return _text_content;
        }
    }
    static EmueraContent _text_content = null;
}
