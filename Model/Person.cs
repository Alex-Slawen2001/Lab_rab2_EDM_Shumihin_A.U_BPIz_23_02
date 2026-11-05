// ==========================================
// Файл: Model/Person.cs
// Назначение: Сущность "Сотрудник" модели EDM.
//             RoleId — внешний ключ на таблицу Role,
//             Role   — навигационное свойство (связь с классом Role);
//             через него в списке выводится Role.NameRole.
//             Класс PersonDpo больше не нужен — удалён.
// ==========================================

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab_rab2_EDM_Shumihin_A_U_BPIz_23_02.Model
{
    public class Person : INotifyPropertyChanged
    {
        /// <summary>код сотрудника (первичный ключ)</summary>
        public int Id { get; set; }

        private int roleId;
        /// <summary>код должности (внешний ключ)</summary>
        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; OnPropertyChanged("RoleId"); }
        }

        private string firstName;
        /// <summary>имя сотрудника</summary>
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged("FirstName"); }
        }

        private string lastName;
        /// <summary>фамилия сотрудника</summary>
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged("LastName"); }
        }

        private DateTime birthday;
        /// <summary>дата рождения</summary>
        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; OnPropertyChanged("Birthday"); }
        }

        /// <summary>должность сотрудника (связь с классом Role)</summary>
        public virtual Role Role { get; set; }

        public Person() { }

        /// <summary>Поверхностное копирование — используется при редактировании</summary>
        public Person ShallowCopy()
        {
            return (Person)this.MemberwiseClone();
        }

        // ---- INotifyPropertyChanged ----
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
