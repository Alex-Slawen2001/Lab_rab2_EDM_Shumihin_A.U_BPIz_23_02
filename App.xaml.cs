// ==========================================
// Файл: App.xaml.cs
// Назначение: Code-behind точки входа.
//             Устанавливает русскую культуру, чтобы DatePicker и списки
//             показывали даты в формате дд.ММ.гггг.
// ==========================================

using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfo ru = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture   = ru;
            Thread.CurrentThread.CurrentUICulture = ru;
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(ru.IetfLanguageTag)));

            base.OnStartup(e);
        }
    }
}
