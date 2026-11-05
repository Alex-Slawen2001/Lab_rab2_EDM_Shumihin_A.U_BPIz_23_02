// ==========================================
// Файл: Model/Role.cs
// Назначение: Сущность "Должность" модели EDM.
//             Свойства Id и NameRole описывают должность,
//             коллекция Persons — связь с сотрудниками (навигационное свойство).
//             Реализует INotifyPropertyChanged; ShallowCopy() — для редактирования.
// ==========================================

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.Model
{
    /// <summary>Должность сотрудника</summary>
    public class Role : INotifyPropertyChanged
    {
        /// <summary>код должности (первичный ключ)</summary>
        public int Id { get; set; }

        private string nameRole;

        /// <summary>наименование должности</summary>
        public string NameRole
        {
            get { return nameRole; }
            set
            {
                nameRole = value;
                OnPropertyChanged("NameRole");
            }
        }

        /// <summary>сотрудники, занимающие должность (связь с классом Person)</summary>
        public virtual ICollection<Person> Persons { get; set; } = new List<Person>();

        public Role() { }

        /// <summary>Поверхностное копирование — используется при редактировании</summary>
        public Role ShallowCopy()
        {
            return (Role)this.MemberwiseClone();
        }

        // ---- INotifyPropertyChanged ----
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
