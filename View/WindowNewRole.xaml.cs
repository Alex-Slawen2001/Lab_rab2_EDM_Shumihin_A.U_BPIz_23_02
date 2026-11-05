// ==========================================
// Файл: View/WindowNewRole.xaml.cs
// Назначение: Code-behind диалога добавления/редактирования должности.
//             BtnSave_Click   → DialogResult = true  (RoleViewModel сохраняет данные)
//             BtnCancel_Click → DialogResult = false (изменения отменяются)
// ==========================================

using System.Windows;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.View
{
    /// <summary>
    /// Логика взаимодействия для WindowNewRole.xaml
    /// </summary>
    public partial class WindowNewRole : Window
    {
        public WindowNewRole()
        {
            InitializeComponent();
        }

        // Кнопка "Сохранить" — подтверждает диалог
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        // Кнопка "Отмена" — отменяет диалог
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
