using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // 导入DOTween命名空间
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 用于存储所有要用到的UI控件，用历史替换原则 父类装子类
    /// </summary>
    protected Dictionary<string, UIBehaviour> controlDic = new Dictionary<string, UIBehaviour>();

    #region 动效相关属性
    /// <summary>
    /// 是否启用显示动效
    /// </summary>
    // [Header("动效设置")]
    private bool enableShowAnimation = true;
    /// <summary>
    /// 如果不想使用UIMgr管理，就使用这个属性来指定显示动效类型
    /// </summary>
    private E_ShowAnimType showAnimType = E_ShowAnimType.None;

    /// <summary>
    /// 是否启用隐藏动效
    /// </summary>
    private bool enableHideAnimation = true;
    /// <summary>
    /// 如果不想使用UIMgr管理，就使用这个属性来指定隐藏动效类型
    /// </summary>
    private E_HideAnimType hideAnimType = E_HideAnimType.None;


    #region 动效参数
    /// <summary>
    /// 动效持续时间（秒）
    /// </summary>
    private float animationDuration = 0.3f;

    /// <summary>
    /// 缓动函数
    /// </summary>
    private Ease animationEase = Ease.OutQuad;

    /// <summary>
    /// 原始位置（用于恢复）
    /// </summary>
    private Vector3 originalPosition;

    /// <summary>
    /// 原始缩放（用于恢复）
    /// </summary>
    private Vector3 originalScale = Vector3.one;

    /// <summary>
    /// 原始透明度（用于恢复）
    /// </summary>
    private float originalAlpha = 1f;
    #endregion

    #endregion

    protected virtual void Awake()
    {
        if (!enableShowAnimation && !enableHideAnimation)
        {
            return;
        }
        // 保存原始状态
        originalPosition = transform.position;
        originalScale = transform.localScale;

        // 获取CanvasGroup组件，如果没有则添加
        CanvasGroup canvasGroup;
        if (TryGetComponent(out canvasGroup))
        {
            originalAlpha = canvasGroup.alpha;
        }
        else
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            originalAlpha = canvasGroup.alpha;
        }
    }

    protected virtual void Start()
    {

    }

    /// <summary>
    /// 面板显示时会调用的逻辑
    /// </summary>
    public virtual void ShowMe(UnityAction callback = null)
    {
        //执行默认的显示面板想要做的事情
        callback?.Invoke();
    }

    /// <summary>
    /// 面板隐藏时会调用的逻辑
    /// </summary>
    public virtual void HideMe(UnityAction callback = null)
    {
        //执行默认的隐藏面板想要做的事情
        callback?.Invoke();
    }

    #region 指定当前面板动效类型的方法
    /// <summary>
    /// 设置面板的动效，不通过UIMgr管理时使用
    /// 这里动效显示和隐藏是配套的，所以只需要设置显示动效类型，可以同步把隐藏动效类型设置好
    /// </summary>
    /// <param name="type"></param>
    public void SetAnimType(E_ShowAnimType type)
    {
        // 如果不启用显示动效，就直接返回
        if (!enableShowAnimation)
        {
            return;
        }
        // 如果动效类型没有变化，就直接返回
        if (showAnimType == type)
        {
            return;
        }
        showAnimType = type;
        // 根据显示动效类型设置对应的隐藏动效类型
        switch (type)
        {
            case E_ShowAnimType.SlideInFromBottom:
                hideAnimType = E_HideAnimType.SlideOutToBottom;
                break;
            case E_ShowAnimType.SlideInFromLeft:
                hideAnimType = E_HideAnimType.SlideOutToLeft;
                break;
            case E_ShowAnimType.SlideInFromRight:
                hideAnimType = E_HideAnimType.SlideOutToRight;
                break;
            case E_ShowAnimType.SlideInFromTop:
                hideAnimType = E_HideAnimType.SlideOutToTop;
                break;
            case E_ShowAnimType.ScaleIn:
                hideAnimType = E_HideAnimType.ScaleOut;
                break;
            case E_ShowAnimType.FadeIn:
                hideAnimType = E_HideAnimType.FadeOut;
                break;
            case E_ShowAnimType.None:
                hideAnimType = E_HideAnimType.None;
                break;
            case E_ShowAnimType.BounceIn:
                hideAnimType = E_HideAnimType.BounceOut;
                break;

        }
    }
    #endregion

    #region 动效方法

    /// <summary>
    /// 如果玩家没有传入动效类型，就调用这个默认显示动效
    /// 默认是玩家第一次打开面板时使用的动效，在内部会记录下来，下次会使用这个动效
    /// </summary>
    /// <param name="callback">动画完成回调</param>
    public void ShowByDefault(UnityAction callback = null)
    {
        if (!enableShowAnimation)
        {
            callback?.Invoke();
            return;
        }

        switch (showAnimType)
        {
            case E_ShowAnimType.FadeIn:
                ShowWithFade(callback);
                break;
            case E_ShowAnimType.ScaleIn:
                ShowWithScale(0.5f, callback);
                break;
            case E_ShowAnimType.SlideInFromTop:
                ShowFromTop(200f, callback);
                break;
            case E_ShowAnimType.SlideInFromBottom:
                ShowFromBottom(200f, callback);
                break;
            case E_ShowAnimType.SlideInFromLeft:
                ShowFromLeft(200f, callback);
                break;
            case E_ShowAnimType.SlideInFromRight:
                ShowFromRight(200f, callback);
                break;
            case E_ShowAnimType.None:
                ShowMe(callback);
                break;
            case E_ShowAnimType.BounceIn:
                ShowWithBounce(callback);
                break;
        }
    }

    /// <summary>
    /// 从下到上弹出显示
    /// </summary>
    /// <param name="offsetY">初始Y轴偏移量</param>
    /// <param name="callback">动画完成回调</param>
    public void ShowFromBottom(float offsetY = 200f, UnityAction callback = null)
    {
        if (!enableShowAnimation)
        {
            callback?.Invoke();
            return;
        }
        this.SetAnimType(E_ShowAnimType.SlideInFromBottom);

        // 重置状态
        transform.position = originalPosition + new Vector3(0, -offsetY, 0);
        SetAlpha(0);

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition, animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), originalAlpha, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 从小到大缩放显示
    /// </summary>
    /// <param name="scaleFactor">初始缩放因子</param>
    /// <param name="callback">动画完成回调</param>
    public void ShowWithScale(float scaleFactor = 0.5f, UnityAction callback = null)
    {
        if (!enableShowAnimation)
        {
            callback?.Invoke();
            return;
        }
        this.SetAnimType(E_ShowAnimType.ScaleIn);

        // 重置状态
        // 为了避免玩家上次使用了其他动效，导致动效不一致，导致位置偏移，这里重置位置和缩放
        ResetToInitialState();
        transform.localScale = originalScale * scaleFactor;
        SetAlpha(0);

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(originalScale, animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), originalAlpha, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 淡入显示
    /// </summary>
    /// <param name="callback">动画完成回调</param>
    public void ShowWithFade(UnityAction callback = null)
    {
        if (!enableShowAnimation)
        {
            callback?.Invoke();
            return;
        }
        this.SetAnimType(E_ShowAnimType.FadeIn);
        // 为了避免玩家上次使用了其他动效，导致动效不一致，导致位置偏移，这里重置位置和缩放
        ResetToInitialState();
        // 重置状态
        SetAlpha(0);

        // 执行动画
        DOTween.To(() => GetAlpha(), x => SetAlpha(x), originalAlpha, animationDuration)
            .SetEase(animationEase)
            .OnComplete(() => callback?.Invoke());
    }

    /// <summary>
    /// 从左到右滑入显示
    /// </summary>
    /// <param name="offsetX">初始X轴偏移量</param>
    /// <param name="callback">动画完成回调</param>
    public void ShowFromLeft(float offsetX = 200f, UnityAction callback = null)
    {
        if (!enableShowAnimation)
        {
            callback?.Invoke();
            return;
        }
        this.SetAnimType(E_ShowAnimType.SlideInFromLeft);
        // 重置状态
        transform.position = originalPosition + new Vector3(-offsetX, 0, 0);
        SetAlpha(0);

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition, animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), originalAlpha, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 从上到下滑入显示
    /// </summary>
    /// <param name="offsetY">初始Y轴偏移量</param>
    /// <param name="callback">动画完成回调</param>
    public void ShowFromTop(float offsetY = 200f, UnityAction callback = null)
    {
        if (!enableShowAnimation)
        {
            callback?.Invoke();
            return;
        }
        this.SetAnimType(E_ShowAnimType.SlideInFromTop);
        // 重置状态
        transform.position = originalPosition + new Vector3(0, offsetY, 0);
        SetAlpha(0);

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition, animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), originalAlpha, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 从右到左滑入显示
    /// </summary>
    /// <param name="offsetX">初始X轴偏移量</param>
    /// <param name="callback">动画完成回调</param>
    public void ShowFromRight(float offsetX = 200f, UnityAction callback = null)
    {
        if (!enableShowAnimation)
        {
            callback?.Invoke();
            return;
        }
        this.SetAnimType(E_ShowAnimType.SlideInFromRight);
        // 重置状态
        transform.position = originalPosition + new Vector3(offsetX, 0, 0);
        SetAlpha(0);

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition, animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), originalAlpha, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 带弹跳效果的显示
    /// </summary>
    /// <param name="callback">动画完成回调</param>
    public void ShowWithBounce(UnityAction callback = null)
    {
        if (!enableShowAnimation)
        {
            callback?.Invoke();
            return;
        }
        this.SetAnimType(E_ShowAnimType.BounceIn);
        // 重置状态
        transform.localScale = Vector3.zero;
        SetAlpha(0);

        // 执行动画 - 更快的弹跳效果
        Sequence sequence = DOTween.Sequence();
        // 缩短动画时长，增加缩放比例的变化速度
        float quickDuration = animationDuration * 0.7f; // 整体动画时长缩短
        sequence.Append(transform.DOScale(1.3f, quickDuration * 0.4f).SetEase(Ease.OutBack)); // 更快地放大到更大的比例
        sequence.Append(transform.DOScale(originalScale, quickDuration * 0.6f).SetEase(Ease.InBack)); // 稍微慢一点收缩到原始大小，避免抖动
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), originalAlpha, quickDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    public void HideByDefault(UnityAction callback = null)
    {
        if (!enableHideAnimation)
        {
            callback?.Invoke();
            return;
        }

        switch (hideAnimType)
        {
            case E_HideAnimType.FadeOut:
                HideWithFade(callback);
                break;
            case E_HideAnimType.ScaleOut:
                HideWithScale(0.5f, callback);
                break;
            case E_HideAnimType.SlideOutToTop:
                HideToTop(200f, callback);
                break;
            case E_HideAnimType.SlideOutToBottom:
                HideToBottom(200f, callback);
                break;
            case E_HideAnimType.SlideOutToLeft:
                HideToLeft(200f, callback);
                break;
            case E_HideAnimType.SlideOutToRight:
                HideToRight(200f, callback);
                break;
            case E_HideAnimType.None:
                HideMe(callback);
                break;
            case E_HideAnimType.BounceOut:
                HideWithBounce(callback);
                break;
        }
    }

    /// <summary>
    /// 向下滑出隐藏
    /// </summary>
    /// <param name="offsetY">Y轴偏移量</param>
    /// <param name="callback">动画完成回调</param>
    public void HideToBottom(float offsetY = 200f, UnityAction callback = null)
    {
        if (!enableHideAnimation)
        {
            callback?.Invoke();
            return;
        }

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition + new Vector3(0, -offsetY, 0), animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), 0, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 缩小隐藏
    /// </summary>
    /// <param name="scaleFactor">最终缩放因子</param>
    /// <param name="callback">动画完成回调</param>
    public void HideWithScale(float scaleFactor = 0.5f, UnityAction callback = null)
    {
        if (!enableHideAnimation)
        {
            callback?.Invoke();
            return;
        }

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(originalScale * scaleFactor, animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), 0, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 淡出隐藏
    /// </summary>
    /// <param name="callback">动画完成回调</param>
    public void HideWithFade(UnityAction callback = null)
    {
        if (!enableHideAnimation)
        {
            callback?.Invoke();
            return;
        }

        // 执行动画
        DOTween.To(() => GetAlpha(), x => SetAlpha(x), 0, animationDuration)
            .SetEase(animationEase)
            .OnComplete(() => callback?.Invoke());
    }

    /// <summary>
    /// 向左滑出隐藏
    /// </summary>
    /// <param name="offsetX">X轴偏移量</param>
    /// <param name="callback">动画完成回调</param>
    public void HideToLeft(float offsetX = 200f, UnityAction callback = null)
    {
        if (!enableHideAnimation)
        {
            callback?.Invoke();
            return;
        }

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition + new Vector3(-offsetX, 0, 0), animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), 0, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 向上滑出隐藏
    /// </summary>
    /// <param name="offsetY">Y轴偏移量</param>
    /// <param name="callback">动画完成回调</param>
    public void HideToTop(float offsetY = 200f, UnityAction callback = null)
    {
        if (!enableHideAnimation)
        {
            callback?.Invoke();
            return;
        }

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition + new Vector3(0, offsetY, 0), animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), 0, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 向右滑出隐藏
    /// </summary>
    /// <param name="offsetX">X轴偏移量</param>
    /// <param name="callback">动画完成回调</param>
    public void HideToRight(float offsetX = 200f, UnityAction callback = null)
    {
        if (!enableHideAnimation)
        {
            callback?.Invoke();
            return;
        }

        // 执行动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originalPosition + new Vector3(offsetX, 0, 0), animationDuration).SetEase(animationEase));
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), 0, animationDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    /// <summary>
    /// 弹跳隐藏
    /// </summary>
    /// <param name="callback">动画完成回调</param>
    public void HideWithBounce(UnityAction callback = null)
    {
        if (!enableHideAnimation)
        {
            callback?.Invoke();
            return;
        }

        // 执行动画 - 与显示对应的弹跳隐藏效果
        Sequence sequence = DOTween.Sequence();
        // 先稍微放大，然后快速缩小到零，创造弹跳效果
        float quickDuration = animationDuration * 0.8f; // 整体动画时长
        sequence.Append(transform.DOScale(1.1f, quickDuration * 0.3f).SetEase(Ease.OutBack)); // 先稍微放大
        sequence.Append(transform.DOScale(0, quickDuration * 0.7f).SetEase(Ease.InBack)); // 然后快速缩小到零
        sequence.Join(DOTween.To(() => GetAlpha(), x => SetAlpha(x), 0, quickDuration).SetEase(animationEase));

        if (callback != null)
        {
            sequence.OnComplete(() => callback());
        }
    }

    #endregion

    #region 辅助方法

    /// <summary>
    /// 获取当前透明度
    /// </summary>
    /// <returns>透明度值</returns>
    private float GetAlpha()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            return canvasGroup.alpha;
        }
        return 1f;
    }

    /// <summary>
    /// 设置透明度
    /// </summary>
    /// <param name="alpha">透明度值</param>
    private void SetAlpha(float alpha)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }

    /// <summary>
    /// 重置到初始状态
    /// </summary>
    protected void ResetToInitialState()
    {
        transform.position = originalPosition;
        transform.localScale = originalScale;
        SetAlpha(originalAlpha);
    }

    #endregion
}