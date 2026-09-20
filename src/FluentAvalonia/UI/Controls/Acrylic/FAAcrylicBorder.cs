using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

namespace FluentAvalonia.UI.Controls;

/// <summary>
/// Fluent 磨砂玻璃容器（应用内亚克力 / Mica 质感）。
///
/// 补的是 Avalonia 的一个真实缺口：Avalonia 只提供 <see cref="ExperimentalAcrylicBorder"/>
/// 这个"容器控件"，<b>没有可以当 Background 用的亚克力画刷</b>，
/// 所以无法给任意 Border / 面板刷上磨砂；FluentAvalonia 则完全没有这部分。
/// 本控件把它包成 Fluent 语义：主题感知的着色、圆角、可选开关。
///
/// 用法：
/// <code>
/// &lt;fa:FAAcrylicBorder CornerRadius="8"&gt;
///     &lt;StackPanel&gt;…侧栏内容…&lt;/StackPanel&gt;
/// &lt;/fa:FAAcrylicBorder&gt;
/// </code>
///
/// 说明：
/// <list type="bullet">
/// <item>底层材质用 <see cref="ExperimentalAcrylicMaterial"/>，其 BackgroundSource 为
/// <see cref="AcrylicBackgroundSource.Digger"/> —— 即"挖"开自己、对窗口内已绘制的内容做模糊。</item>
/// <item><see cref="IsAcrylicEnabled"/> 关掉时只显示 <see cref="FallbackColor"/> 半透明底
/// （低性能设备 / 用户关闭特效 / 平台不支持时使用）。</item>
/// <item>着色随 <see cref="ThemeVariant"/> 自动切换（浅色/深色取不同 Fluent 取值）。</item>
/// </list>
/// </summary>
public class FAAcrylicBorder : ContentControl
{
    /// <summary>是否启用真正的磨砂效果；关闭则退化为半透明纯色底。</summary>
    public static readonly StyledProperty<bool> IsAcrylicEnabledProperty =
        AvaloniaProperty.Register<FAAcrylicBorder, bool>(nameof(IsAcrylicEnabled), true);

    /// <summary>着色。默认值由主题给出（浅色/深色不同）。</summary>
    public static readonly StyledProperty<Color> TintColorProperty =
        AvaloniaProperty.Register<FAAcrylicBorder, Color>(nameof(TintColor));

    /// <summary>着色不透明度：越小越透（磨砂感越强）。</summary>
    public static readonly StyledProperty<double> TintOpacityProperty =
        AvaloniaProperty.Register<FAAcrylicBorder, double>(nameof(TintOpacity), 0.5);

    /// <summary>亮度层不透明度（材质整体的明暗补偿）。</summary>
    public static readonly StyledProperty<double> LuminosityOpacityProperty =
        AvaloniaProperty.Register<FAAcrylicBorder, double>(nameof(LuminosityOpacity), 0.8);

    /// <summary>平台不支持磨砂时的兜底色。必须是<b>不透明</b>色，否则会露出未初始化像素。</summary>
    public static readonly StyledProperty<Color> FallbackColorProperty =
        AvaloniaProperty.Register<FAAcrylicBorder, Color>(nameof(FallbackColor), Color.FromRgb(32, 32, 32));

    /// <summary>供模板绑定的材质对象（由上面几个属性推导，只读）。</summary>
    public static readonly DirectProperty<FAAcrylicBorder, ExperimentalAcrylicMaterial> MaterialProperty =
        AvaloniaProperty.RegisterDirect<FAAcrylicBorder, ExperimentalAcrylicMaterial>(
            nameof(Material), o => o.Material);

    private ExperimentalAcrylicMaterial _material;

    static FAAcrylicBorder()
    {
        IsAcrylicEnabledProperty.Changed.AddClassHandler<FAAcrylicBorder>((o, _) => o.UpdateMaterial());
        TintColorProperty.Changed.AddClassHandler<FAAcrylicBorder>((o, _) => o.UpdateMaterial());
        TintOpacityProperty.Changed.AddClassHandler<FAAcrylicBorder>((o, _) => o.UpdateMaterial());
        LuminosityOpacityProperty.Changed.AddClassHandler<FAAcrylicBorder>((o, _) => o.UpdateMaterial());
        FallbackColorProperty.Changed.AddClassHandler<FAAcrylicBorder>((o, _) => o.UpdateMaterial());
    }

    public FAAcrylicBorder()
    {
        _material = BuildMaterial();
        ActualThemeVariantChanged += (_, _) => UpdateMaterial();
    }

    /// <inheritdoc cref="IsAcrylicEnabledProperty"/>
    public bool IsAcrylicEnabled
    {
        get => GetValue(IsAcrylicEnabledProperty);
        set => SetValue(IsAcrylicEnabledProperty, value);
    }

    /// <inheritdoc cref="TintColorProperty"/>
    public Color TintColor
    {
        get => GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }

    /// <inheritdoc cref="TintOpacityProperty"/>
    public double TintOpacity
    {
        get => GetValue(TintOpacityProperty);
        set => SetValue(TintOpacityProperty, value);
    }

    /// <inheritdoc cref="LuminosityOpacityProperty"/>
    public double LuminosityOpacity
    {
        get => GetValue(LuminosityOpacityProperty);
        set => SetValue(LuminosityOpacityProperty, value);
    }

    /// <inheritdoc cref="FallbackColorProperty"/>
    public Color FallbackColor
    {
        get => GetValue(FallbackColorProperty);
        set => SetValue(FallbackColorProperty, value);
    }

    /// <inheritdoc cref="MaterialProperty"/>
    public ExperimentalAcrylicMaterial Material
    {
        get => _material;
        private set => SetAndRaise(MaterialProperty, ref _material, value);
    }

    private void UpdateMaterial() => Material = BuildMaterial();

    private ExperimentalAcrylicMaterial BuildMaterial()
    {
        return new ExperimentalAcrylicMaterial
        {
            // Digger = 对自己下方（窗口内）已绘制内容做模糊，即"应用内磨砂"；
            // 关闭磨砂时用 None —— 只保留着色、不做模糊，同样走这套材质，无需在模板里切换视觉树。
            BackgroundSource = IsAcrylicEnabled ? AcrylicBackgroundSource.Digger : AcrylicBackgroundSource.None,
            TintColor = TintColor,
            TintOpacity = TintOpacity,
            MaterialOpacity = LuminosityOpacity,
            FallbackColor = FallbackColor,
        };
    }
}
