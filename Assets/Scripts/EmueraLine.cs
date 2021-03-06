﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinorShift.Emuera;

public class EmueraLine : MonoBehaviour
{
    public static void OnClick(UnityEngine.EventSystems.PointerEventData e)
    {
        var obj = e.rawPointerPress;
        if(obj == null)
            return;
        var text = obj.GetComponent<EmueraLine>();
        if(text == null)
        {
            EmueraThread.instance.Input("", false);
            return;
        }
        var unit_desc = text.unit_desc;
        if(!unit_desc.isbutton)
            return;
        if(unit_desc.generation < EmueraContent.instance.button_generation)
            EmueraThread.instance.Input("", false);
        else
            EmueraThread.instance.Input(unit_desc.code, true);
    }

    void Awake()
    {
        GenericUtils.SetListenerOnClick(gameObject, OnClick);
        monospaced_ = GetComponent<UnityEngine.UI.Monospaced>();
        click_handler_ = GetComponent<GenericUtils.PointerClickListener>();
    }

    UnityEngine.UI.Text text
    {
        get
        {
            if(text_ == null)
                text_ = GetComponent<UnityEngine.UI.Text>();
            return text_;
        }
    }
    UnityEngine.UI.Text text_ = null;
    UnityEngine.UI.Monospaced monospaced
    {
        get
        {
            if(monospaced_ == null)
                monospaced_ = GetComponent<UnityEngine.UI.Monospaced>();
            return monospaced_;
        }
    }
    UnityEngine.UI.Monospaced monospaced_ = null;
    UnityEngine.UI.ContentSizeFitter size_fitter
    {
        get
        {
            if(size_fitter_ == null)
            {
                size_fitter_ = GetComponent<UnityEngine.UI.ContentSizeFitter>();
                size_fitter_.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
            }
            return size_fitter_;
        }
    }
    UnityEngine.UI.ContentSizeFitter size_fitter_ = null;
    GenericUtils.PointerClickListener click_handler_ = null;

    /// <summary>
    /// 更新内容
    /// </summary>
    public void UpdateContent()
    {
        var ud = unit_desc;

        text.text = ud.content;
        //text.alignment = (TextAnchor)line_desc.align;
        //if((int)text.alignment > 0)
        //    ud.posx = 0;
        text.color = ud.color;
        text.supportRichText = ud.richedit;

        if(ud.isbutton && ud.generation >= EmueraContent.instance.button_generation)
        {
            click_handler_.enabled = true;
            text.raycastTarget = true;
#if UNITY_EDITOR
            code = ud.code;
            generation = ud.generation;
#endif
        }
        else
        {
            click_handler_.enabled = false;
            text.raycastTarget = false;
        }

        var font = FontUtils.default_font;
        var monospaced = FontUtils.default_monospaced;
        if(ud.fontname != null)
        {
            font = FontUtils.GetFont(ud.fontname);
            monospaced = FontUtils.GetMonospaced(ud.fontname);
        }
        if(text.font != font)
            text.font = font;

        monospaced_.enabled = monospaced;

        logic_y = line_desc.position_y;
        logic_height = line_desc.height;

        if(SizeFitter)
        {
            size_fitter.horizontalFit =
                UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
        }
        else
        {
            size_fitter.horizontalFit =
                UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained;
            text_.rectTransform.sizeDelta = new Vector2(Width, 0);
        }

        if(unit_desc.underline)
        {
            if(underline_ == null)
            {
                var obj = GameObject.Instantiate(EmueraContent.instance.template_block.gameObject);
                underline_ = obj.GetComponent<RectTransform>();
                underline_.transform.SetParent(this.transform);
                underline_.anchorMin = new Vector2(0, 1);
                underline_.anchorMax = new Vector2(0, 1);
                underline_.localScale = Vector3.one;
                //underline_.position = transform.position + new Vector3(0, 1 - font.fontSize);
                underline_.anchoredPosition = new Vector2(0, - font.fontSize - 1);
            }
            underline_.sizeDelta = new Vector2(Width, 1);
            underline_.GetComponent<UnityEngine.UI.Image>().color = unit_desc.color;
            underline_.gameObject.SetActive(true);
        }
        else if(underline_ != null)
            underline_.gameObject.SetActive(false);

        if(unit_desc.strickout)
        {
            if(strickout_ == null)
            {
                var obj = GameObject.Instantiate(EmueraContent.instance.template_block.gameObject);
                strickout_ = obj.GetComponent<RectTransform>();
                strickout_.transform.SetParent(this.transform);
                strickout_.anchorMin = new Vector2(0, 1);
                strickout_.anchorMax = new Vector2(0, 1);
                strickout_.localScale = Vector3.one;
                //strickout_.position = transform.position + new Vector3(0, - font.fontSize / 2.0f);
                strickout_.anchoredPosition = new Vector2(0, - font.fontSize / 2.0f);
            }
            strickout_.sizeDelta = new Vector2(Width, 1);
            strickout_.GetComponent<UnityEngine.UI.Image>().color = unit_desc.color;
            strickout_.gameObject.SetActive(true);
        }
        else if(strickout_ != null)
            strickout_.gameObject.SetActive(false);

        gameObject.name = string.Format("line:{0}:{1}", LineNo, UnitIdx);
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        line_desc = null;
        UnitIdx = -1;
        text.text = "";
        if(underline_ != null)
            underline_.gameObject.SetActive(false);
        if(strickout_ != null)
            strickout_.gameObject.SetActive(false);
    }

    public void SetPosition(float x, float y)
    {
        var rt = (RectTransform)transform;
        rt.anchoredPosition = new Vector2(x, y);
    }

    public EmueraContent.LineDesc line_desc = null;
    public int LineNo { get { return line_desc.LineNo; } }
    public int UnitIdx = -1;
    public bool SizeFitter = false;
    public EmueraContent.UnitDesc unit_desc
    {
        get
        {
            if(line_desc == null || UnitIdx >= line_desc.units.Count)
                return null;
            return line_desc.units[UnitIdx];
        }
    }
    public float Width = 0;
    public float logic_y = 0.0f;
    public float logic_height = 0.0f;
#if UNITY_EDITOR
    public string code;
    public int generation;
#endif
    RectTransform underline_ = null;
    RectTransform strickout_ = null;
}
