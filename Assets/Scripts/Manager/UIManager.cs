using Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject cScreen, cPopup, cNotify, cOverlap;

    private Dictionary<string, BaseScreen> screens = new Dictionary<string, BaseScreen>();
    private Dictionary<string, BasePopup> popups = new Dictionary<string, BasePopup>();
    private Dictionary<string, BaseNotify> notifies = new Dictionary<string, BaseNotify>();
    private Dictionary<string, BaseOverlap> overlaps = new Dictionary<string, BaseOverlap>();

    public Dictionary<string, BaseScreen> Screens => screens;
    public Dictionary<string, BasePopup> Popups => popups;
    public Dictionary<string, BaseNotify> Notifies => notifies;
    public Dictionary<string, BaseOverlap> Overlaps => overlaps;

    private BaseScreen curScreen;
    private BasePopup curPopup;
    private BaseNotify curNotify;
    private BaseOverlap curOverlap;

    public BaseScreen CurScreen => curScreen;
    public BasePopup CurPopup => curPopup;
    public BaseNotify CurNotify => curNotify;
    public BaseOverlap CurOverlap => curOverlap;

    private const string SCREEN_RESOURCES_PATH = "Prefabs/UI/Screen/";
    private const string POPUP_RESOURCES_PATH = "Prefabs/UI/Popup/";
    private const string NOTIFY_RESOURCES_PATH = "Prefabs/UI/Notify/";
    private const string OVERLAP_RESOURCES_PATH = "Prefabs/UI/Overlap/";

    private List<string> rmScreens = new List<string>();
    private List<string> rmPopups = new List<string>();
    private List<string> rmNotifies = new List<string>();
    private List<string> rmOverlaps = new List<string>();

    #region Screen

    private BaseScreen GetNewScreen<T>() where T : BaseScreen
    {
        string nameScreen = typeof(T).Name;
        GameObject pfScreen = GetUIPrefab(UIType.Screen, nameScreen);
        if (pfScreen == null || !pfScreen.GetComponent<BaseScreen>())
        {
            throw new MissingReferenceException("Can not found" + nameScreen + "screen. !!!");
        }
        GameObject ob = Instantiate(pfScreen) as GameObject;
        ob.transform.SetParent(this.cScreen.transform);
        ob.transform.localScale = Vector3.one;
        ob.transform.localPosition = Vector3.zero;
#if UNITY_EDITOR
        ob.name = "SCREEN_" + nameScreen;
#endif
        BaseScreen screenScr = ob.GetComponent<BaseScreen>();
        screenScr.Init();
        return screenScr;
    }

    public void HideAllScreens()
    {
        BaseScreen screenScr = null;

        foreach (KeyValuePair<string, BaseScreen> item in screens)
        {
            screenScr = item.Value;
            if (screenScr == null || screenScr.IsHide)
                continue;
            screenScr.Hide();

            if (screens.Count <= 0)
                break;
        }
    }

    public T GetExistScreen<T>() where T : BaseScreen
    {
        string screenName = typeof(T).Name;
        if (screens.ContainsKey(screenName))
        {
            return screens[screenName] as T;
        }
        return null;
    }

    private void RemoveScreen(string v)
    {
        for (int i = 0; i < rmScreens.Count; i++)
        {
            if (rmScreens[i].Equals(v))
            {
                if (screens.ContainsKey(v))
                {
                    Destroy(screens[v].gameObject);
                    screens.Remove(v);

                    Resources.UnloadUnusedAssets();
                    System.GC.Collect();
                }
                break;
            }
        }
    }

    public void ShowScreen<T>(object data = null, bool forceShowData = false) where T : BaseScreen
    {
        string screenName = typeof(T).Name;
        BaseScreen result = null;
        if (curScreen != null)
        {
            var curName = curScreen.GetType().Name;
            if (curName.Equals(screenName))
            {
                result = curScreen;
            }
            else
            {
                screens[curName].Hide();
                RemoveScreen(curName);
            }
        }

        if (result == null)
        {
            if (!screens.ContainsKey(screenName))
            {
                BaseScreen screenScr = GetNewScreen<T>();
                if (screenScr != null)
                {
                    screens.Add(screenName, screenScr);
                }
            }

            if (screens.ContainsKey(screenName))
            {
                result = screens[screenName];
            }
        }

        bool isShow = false;
        if (result != null)
        {
            if (forceShowData)
            {
                isShow = true;
            }
            else
            {
                if (result.IsHide)
                {
                    isShow = true;
                }
            }
        }

        if (isShow)
        {
            curScreen = result;
            result.transform.SetAsLastSibling();
            result.Show(data);
        }
    }

    #endregion

    #region Popup

    private BasePopup GetNewPopup<T>() where T : BasePopup
    {
        string namePopup = typeof(T).Name;
        GameObject pfPopup = GetUIPrefab(UIType.Popup, namePopup);
        if (pfPopup == null || !pfPopup.GetComponent<BasePopup>())
        {
            throw new MissingReferenceException("Can not found" + namePopup + "popup. !!!");
        }
        GameObject ob = Instantiate(pfPopup) as GameObject;
        ob.transform.SetParent(this.cPopup.transform);
        ob.transform.localScale = Vector3.one;
        ob.transform.localPosition = Vector3.zero;
#if UNITY_EDITOR
        ob.name = "POPUP_" + namePopup;
#endif
        BasePopup popupScr = ob.GetComponent<BasePopup>();
        popupScr.Init();
        return popupScr;
    }

    public void HideAllPopups()
    {
        BasePopup popupScr = null;

        foreach (KeyValuePair<string, BasePopup> item in popups)
        {
            popupScr = item.Value;
            if (popupScr == null || popupScr.IsHide)
                continue;
            popupScr.Hide();

            if (popups.Count <= 0)
                break;
        }
    }

    public T GetExistPopup<T>() where T : BasePopup
    {
        string popupName = typeof(T).Name;
        if (popups.ContainsKey(popupName))
        {
            return popups[popupName] as T;
        }
        return null;
    }

    private void RemovePopup(string v)
    {
        for (int i = 0; i < rmPopups.Count; i++)
        {
            if (rmPopups[i].Equals(v))
            {
                if (popups.ContainsKey(v))
                {
                    Destroy(popups[v].gameObject);
                    popups.Remove(v);

                    Resources.UnloadUnusedAssets();
                    System.GC.Collect();
                }
                break;
            }
        }
    }

    public void ShowPopup<T>(object data = null, bool forceShowData = false) where T : BasePopup
    {
        string popupName = typeof(T).Name;
        BasePopup result = null;

        if (curPopup != null)
        {
            var curName = curPopup.GetType().Name;
            if (curName.Equals(popupName))
            {
                result = curPopup;
            }
            else
            {
                popups[curName].Hide();
                RemovePopup(curName);
            }
        }

        if (result == null)
        {
            if (!popups.ContainsKey(popupName))
            {
                BasePopup popupScr = GetNewPopup<T>();
                if (popupScr != null)
                {
                    popups.Add(popupName, popupScr);
                }
            }

            if (popups.ContainsKey(popupName))
            {
                result = popups[popupName];
            }
        }


        if (result != null && (forceShowData || result.IsHide))
        {
            curPopup = result;
            result.transform.SetAsLastSibling();
            (result as T).Show(data);
        }
    }

    #endregion

    #region Notify

    private BaseNotify GetNewNotify<T>() where T : BaseNotify
    {
        string nameNotify = typeof(T).Name;
        GameObject pfNotify = GetUIPrefab(UIType.Notify, nameNotify);
        if (pfNotify == null || !pfNotify.GetComponent<BaseNotify>())
        {
            throw new MissingReferenceException("Can not found" + nameNotify + "notify. !!!");
        }
        GameObject ob = Instantiate(pfNotify) as GameObject;
        ob.transform.SetParent(this.cNotify.transform);
        ob.transform.localScale = Vector3.one;
        ob.transform.localPosition = Vector3.zero;
#if UNITY_EDITOR
        ob.name = "NOTIFY_" + nameNotify;
#endif
        BaseNotify notifyScr = ob.GetComponent<BaseNotify>();
        notifyScr.Init();
        return notifyScr;
    }

    public void HideAllNotifies()
    {
        BaseNotify notifyScr = null;

        foreach (KeyValuePair<string, BaseNotify> item in notifies)
        {
            notifyScr = item.Value;
            if (notifyScr == null || notifyScr.IsHide)
                continue;
            notifyScr.Hide();

            if (notifies.Count <= 0)
                break;
        }
    }

    public T GetExistNotify<T>() where T : BaseNotify
    {
        string notifyName = typeof(T).Name;
        if (notifies.ContainsKey(notifyName))
        {
            return notifies[notifyName] as T;
        }
        return null;
    }

    private void RemoveNotify(string v)
    {
        for (int i = 0; i < rmNotifies.Count; i++)
        {
            if (rmNotifies[i].Equals(v))
            {
                if (notifies.ContainsKey(v))
                {
                    Destroy(notifies[v].gameObject);
                    notifies.Remove(v);

                    Resources.UnloadUnusedAssets();
                    System.GC.Collect();
                }
                break;
            }
        }
    }

    public void ShowNotify<T>(object data = null, bool forceShowData = false) where T : BaseNotify
    {
        string notifyName = typeof(T).Name;
        BaseNotify result = null;

        if (curNotify != null)
        {
            var curName = curPopup.GetType().Name;
            if (curName.Equals(notifyName))
            {
                result = curNotify;
            }
            else
            {
                notifies[curName].Hide();
                RemoveNotify(curName);
            }
        }

        if (result == null)
        {
            if (!notifies.ContainsKey(notifyName))
            {
                BaseNotify notifyScr = GetNewNotify<T>();
                if (notifyScr != null)
                {
                    notifies.Add(notifyName, notifyScr);
                }
            }

            if (notifies.ContainsKey(notifyName))
            {
                result = notifies[notifyName];
            }
        }

        bool isShow = false;
        if (result != null)
        {
            if (forceShowData)
            {
                isShow = true;
            }
            else
            {
                if (result.IsHide)
                {
                    isShow = true;
                }
            }
        }

        if (isShow)
        {
            curNotify = result;
            result.transform.SetAsLastSibling();
            result.Show(data);
        }
    }

    #endregion

    #region Overlap

    private BaseOverlap GetNewOverLap<T>() where T : BaseOverlap
    {
        string nameOverlap = typeof(T).Name;
        GameObject pfOverlap = GetUIPrefab(UIType.Overlap, nameOverlap);
        if (pfOverlap == null || !pfOverlap.GetComponent<BaseOverlap>())
        {
            throw new MissingReferenceException("Can not found" + nameOverlap + "overlap. !!!");
        }
        GameObject ob = Instantiate(pfOverlap) as GameObject;
        ob.transform.SetParent(this.cOverlap.transform);
        ob.transform.localScale = Vector3.one;
        ob.transform.localPosition = Vector3.zero;
#if UNITY_EDITOR
        ob.name = "OVERLAP_" + nameOverlap;
#endif
        BaseOverlap overlapScr = ob.GetComponent<BaseOverlap>();
        overlapScr.Init();
        return overlapScr;
    }

    public void HideAllOverlaps()
    {
        BaseOverlap overlapScr = null;

        foreach (KeyValuePair<string, BaseOverlap> item in overlaps)
        {
            overlapScr = item.Value;
            if (overlapScr == null || overlapScr.IsHide)
                continue;
            overlapScr.Hide();

            if (overlaps.Count <= 0)
                break;
        }
    }

    public T GetExistOverlap<T>() where T : BaseOverlap
    {
        string overlapName = typeof(T).Name;
        if (overlaps.ContainsKey(overlapName))
        {
            return overlaps[overlapName] as T;
        }
        return null;
    }

    private void RemoveOverlap(string v)
    {
        for (int i = 0; i < rmOverlaps.Count; i++)
        {
            if (rmOverlaps[i].Equals(v))
            {
                if (overlaps.ContainsKey(v))
                {
                    Destroy(overlaps[v].gameObject);
                    overlaps.Remove(v);

                    Resources.UnloadUnusedAssets();
                    System.GC.Collect();
                }
                break;
            }
        }
    }

    public void ShowOverlap<T>(object data = null, bool forceShowData = false) where T : BaseOverlap
    {
        string overlapName = typeof(T).Name;
        BaseOverlap result = null;

        if (curOverlap != null)
        {
            var curName = curOverlap.GetType().Name;
            if (curName.Equals(overlapName))
            {
                result = curOverlap;
            }
            else
            {
                overlaps[curName].Hide();
                RemoveOverlap(curName);
            }
        }

        if (result == null)
        {
            if (!overlaps.ContainsKey(overlapName))
            {
                BaseOverlap overlapScr = GetNewOverLap<T>();
                if (overlapScr != null)
                {
                    overlaps.Add(overlapName, overlapScr);
                }
            }

            if (overlaps.ContainsKey(overlapName))
            {
                result = overlaps[overlapName];
            }
        }

        bool isShow = false;
        if (result != null)
        {
            if (forceShowData)
            {
                isShow = true;
            }
            else
            {
                if (result.IsHide)
                {
                    isShow = true;
                }
            }
        }

        if (isShow)
        {
            curOverlap = result;
            result.transform.SetAsLastSibling();
            result.Show(data);
        }
    }

    #endregion

    private GameObject GetUIPrefab(UIType t, string uiName)
    {
        GameObject result = null;
        var defaultPath = "";
        if (result == null)
        {
            switch (t)
            {
                case UIType.Screen:
                    {
                        defaultPath = SCREEN_RESOURCES_PATH + uiName;
                    }
                    break;
                case UIType.Popup:
                    {
                        defaultPath = POPUP_RESOURCES_PATH + uiName;
                    }
                    break;
                case UIType.Notify:
                    {
                        defaultPath = NOTIFY_RESOURCES_PATH + uiName;
                    }
                    break;
                case UIType.Overlap:
                    {
                        defaultPath = OVERLAP_RESOURCES_PATH + uiName;
                    }
                    break;
            }

            result = Resources.Load(defaultPath) as GameObject;
        }
        return result;
    }

}