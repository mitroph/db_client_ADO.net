using System;
using System.Windows.Forms;

namespace DB
{
    /// <summary>
    /// Клас для комірок DataGridView, які використовують MaskedTextBox для редагування числових значень
    /// </summary>
    internal class MaskedTextBoxCell : DataGridViewTextBoxCell
    {
        // Поля для збереження налаштувань MaskedTextBox
        private string mask;
        private char promptChar;
        private DataGridViewTriState includePrompt;
        private DataGridViewTriState includeLiterals;
        private Type validatingType;

        // Ініціалізація за замовчуванням
        public MaskedTextBoxCell()
            : base()
        {
            // Налаштування значень за замовчуванням
            mask = ">L<LLLLLLLLLLLLLLLLLLLL";
            promptChar = ' ';
            includePrompt = DataGridViewTriState.NotSet;
            includeLiterals = DataGridViewTriState.NotSet;
            validatingType = typeof(string);
        }

        /// <summary>
        /// Ініціалізація елемента управління для редагування комірки
        /// </summary>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            MaskedTextBoxEditingControl mtbEditingCtrl = DataGridView.EditingControl as MaskedTextBoxEditingControl;

            // Отримання відповідностей з колонкою MaskedTextBoxColumn
            MaskedTextBoxColumn mtbColumn = OwningColumn as MaskedTextBoxColumn;

            // Встановлення властивостей MaskedTextBox
            if (!string.IsNullOrEmpty(mask))
                mtbEditingCtrl.Mask = mask;
            else
                mtbEditingCtrl.Mask = mtbColumn?.Mask ?? "";

            mtbEditingCtrl.PromptChar = promptChar;

             // Встановлення ValidatingType
            mtbEditingCtrl.ValidatingType = validatingType;

            try { mtbEditingCtrl.Text = Convert.ToString(Value); }
            catch { mtbEditingCtrl.Text = ""; }
        }

        // Повернення типу елемента для редагування
        public override Type EditType => typeof(MaskedTextBoxEditingControl);


        // Властивості для установки/отримання налаштувань MaskedTextBox

        /// <summary>
        /// Отримує або встановлює значення, що вказує, чи включати символи-підказки
        /// у значення властивості Text.
        /// </summary>
        public virtual DataGridViewTriState IncludePrompt
        {
            get => includePrompt;
            set { includePrompt = value; }
        }

        /// <summary>
        /// Отримує або встановлює значення типу DataGridViewTriState, яке вказує,
        /// чи включати літерал-символи у вихідне значення властивості Text.
        /// </summary>
        public virtual DataGridViewTriState IncludeLiterals
        {
            get => includeLiterals;
            set { includeLiterals = value; }
        }

        /// <summary>
        /// Отримує або встановлює тип об'єкта для перевірки введення.
        /// </summary>
        public virtual Type ValidatingType
        {
            get => validatingType;
            set { validatingType = value; }
        }

        /// <summary>
        /// Рядок з маскою введення для MaskedTextBox
        /// </summary>
        public virtual string Mask
        {
            get => mask;
            set { mask = value; }
        }

        /// <summary>
        /// Символ-підказка для нового введення
        /// </summary>
        public virtual char PromptChar
        {
            get => promptChar; 
            set { promptChar = value; }
        }
 
        // Конвертує значення DataGridViewTriState в boolean
        protected static bool BoolFromTri(DataGridViewTriState tri)
        {
            return (tri == DataGridViewTriState.True) ? true : false;
        }
    }
}