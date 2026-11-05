// ==========================================
// Файл: View/WindowEmployee.xaml.cs
// Назначение: Code-behind окна сотрудников.
//             Вся бизнес-логика вынесена в PersonViewModel.
//             Здесь только инициализация и установка DataContext.
// ==========================================

using System.Windows;
using Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.ViewModel;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.View
{
    /// <summary>
    /// Логика взаимодействия для WindowEmployee.xaml
    /// </summary>
    public partial class WindowEmployee : Window
    {
        public WindowEmployee()
        {
            InitializeComponent();

            // Устанавливаем DataContext — теперь все Binding в XAML
            // ссылаются на свойства PersonViewModel
            DataContext = new PersonViewModel();
        }
    }
}
