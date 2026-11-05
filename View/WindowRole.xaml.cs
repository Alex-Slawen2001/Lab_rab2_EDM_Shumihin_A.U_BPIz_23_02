// ==========================================
// Файл: View/WindowRole.xaml.cs
// Назначение: Code-behind окна должностей.
//             Вся бизнес-логика вынесена в RoleViewModel.
//             Здесь только инициализация и установка DataContext.
// ==========================================

using System.Windows;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.ViewModel;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.View
{
    /// <summary>
    /// Логика взаимодействия для WindowRole.xaml
    /// </summary>
    public partial class WindowRole : Window
    {
        public WindowRole()
        {
            InitializeComponent();

            // Устанавливаем DataContext — теперь все Binding в XAML
            // ссылаются на свойства RoleViewModel
            DataContext = new RoleViewModel();
        }
    }
}
