using Avalonia;
using Avalonia.Controls;

namespace FluentAvalonia.UI.Controls;

/// <summary>
/// 骨架屏占位控件（CodeHub 版新增控件）。
///
/// 由 Tio 的 Skeleton 移植而来，对外接口保持完全一致
/// （<see cref="IsActive"/> / <see cref="IsLoading"/>），
/// 因此消费方只需替换命名空间前缀，属性用法无需改动。
///
/// 外观由 Styling/ControlThemes/FAControls/SkeletonStyles.axaml 提供：
/// IsLoading 为真时用骨架色遮住内容；IsActive 为真时叠加脉冲动画。
/// </summary>
public class FASkeleton : ContentControl
{
    /// <summary>
    /// 是否播放加载动画（脉冲）。
    /// </summary>
    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<FASkeleton, bool>(nameof(IsActive));

    /// <summary>
    /// 是否处于加载中：为真时用骨架遮罩盖住内容，为假时显示真实内容。
    /// </summary>
    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<FASkeleton, bool>(nameof(IsLoading));

    /// <inheritdoc cref="IsActiveProperty"/>
    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    /// <inheritdoc cref="IsLoadingProperty"/>
    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }
}
