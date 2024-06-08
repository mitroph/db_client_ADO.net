using System;
using System.Windows.Forms;

namespace DB
{
    /// <summary>
    /// Базовий об'єкт для користувацького типу стовпця DataGridView. 
    /// Цей клас встановлює основні значення та шаблони комірок, що контролюють типову поведінку для комірок цього типу.
    /// </summary>
    public class MaskedTextBoxColumn : DataGridViewColumn
    {
        // Поля для збереження налаштувань MaskedTextBox
        private string mask;
        private char promptChar;
        private bool includePrompt;
        private bool includeLiterals;
        private Type validatingType;

        /// <summary> Ініціалізує новий екземпляр цього класу,
        /// до його базового конструктора передається екземпляр класу MaskedTextBoxCell для використання як основний шаблон.
        /// </summary>
        public MaskedTextBoxColumn()
            : base(new MaskedTextBoxCell())
        {
        }

        /// <summary>
        /// Процедура перетворення з логічного значення (boolean) на DataGridViewTriState.
        /// </summary>
        /// <param name="value">true або false</param>
        /// <returns>DataGridViewTriState.True або DataGridViewTriState.False </returns>
        private static DataGridViewTriState TriBool(bool value)
        {
            return value ? DataGridViewTriState.True
                         : DataGridViewTriState.False;
        }

        /// <summary> Шаблон комірки, який буде використовуватися для цього стовпця за замовчуванням при створені нових комірок. </summary>
        public override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate;

            set
            {
                // Підтримуються лише типи комірок, що походять від MaskedTextBoxCell.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(MaskedTextBoxCell)))
                {
                    string errorMessage = "Тип комірки не базується на MaskedTextBoxCell.";
                    throw new InvalidCastException(errorMessage);
                }

                base.CellTemplate = value;
            }
        }

        /// <summary>
        /// Вказує на властивість Mask, яка використовується в MaskedTextBox для введення нових даних у комірки цього типу.
        ///
        /// Додаткову інформацію дивіться в документації з елемента керування MaskedTextBox.
        /// </summary>
        public virtual string Mask
        {
            get => mask;
            set
            {
                MaskedTextBoxCell mtbCell;
                DataGridViewCell dgvCell;
                int rowCount;

                if (this.mask != value)
                {
                    this.mask = value;

                    // Спочатку оновіть значення в комірці шаблону.
                    mtbCell = (MaskedTextBoxCell)this.CellTemplate;
                    mtbCell.Mask = value;

                    // Тепер встановіть його на всі комірки в інших рядках.
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvCell is MaskedTextBoxCell)
                            {
                                mtbCell = (MaskedTextBoxCell)dgvCell;
                                mtbCell.Mask = value;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// За замовчуванням MaskedTextBox використовує символ підкреслення (_) для запиту на введення необхідних символів.
        /// Це власне дозволяє вибрати інший.
        ///
        /// Додаткову інформацію дивіться в документації з елемента керування MaskedTextBox.
        /// </summary>
        public virtual char PromptChar
        {
            get => promptChar;
            set
            {
                MaskedTextBoxCell mtbCell;
                DataGridViewCell dgvCell;
                int rowCount;

                if (this.promptChar != value)
                {
                    this.promptChar = value;
                    // Спочатку оновіть значення в комірці шаблону.
                    mtbCell = (MaskedTextBoxCell)this.CellTemplate;
                    mtbCell.PromptChar = value;

                    // Тепер встановіть його на всі комірки в інших рядках.
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvCell is MaskedTextBoxCell)
                            {
                                mtbCell = (MaskedTextBoxCell)dgvCell;
                                mtbCell.PromptChar = value;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Вказує, чи будь-які незаповнені символи в масці повинні бути включені як символи підказки, коли хтось запитує текст MaskedTextBox для певної комірки програмним шляхом.
        ///
        /// Додаткову інформацію дивіться в документації з елемента керування MaskedTextBox.
        /// </summary>
        public virtual bool IncludePrompt
        {
            get
            {
                return this.includePrompt;
            }
            set
            {
                MaskedTextBoxCell mtbc;
                DataGridViewCell dgvc;
                int rowCount;

                if (this.includePrompt != value)
                {
                    this.includePrompt = value;

                    // Спочатку оновіть значення в комірці шаблону.
                    mtbc = (MaskedTextBoxCell)this.CellTemplate;
                    mtbc.IncludePrompt = TriBool(value);

                    // Тепер встановіть його на всі комірки в інших рядках.
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvc is MaskedTextBoxCell)
                            {
                                mtbc = (MaskedTextBoxCell)dgvc;
                                mtbc.IncludePrompt = TriBool(value);
                            }
                        }
                    }
                }
            }
        }

        
        public virtual bool IncludeLiterals
        {
            get
            {
                return this.includeLiterals;
            }
            set
            {
                MaskedTextBoxCell mtbCell;
                DataGridViewCell dgvCell;
                int rowCount;

                if (this.includeLiterals != value)
                {
                    this.includeLiterals = value;

                    // Спочатку оновіть значення в комірці шаблону.
                    mtbCell = (MaskedTextBoxCell)this.CellTemplate;
                    mtbCell.IncludeLiterals = TriBool(value);

                    // Тепер встановіть його на всі комірки в інших рядках.
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {

                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvCell is MaskedTextBoxCell)
                            {
                                mtbCell = (MaskedTextBoxCell)dgvCell;
                                mtbCell.IncludeLiterals = TriBool(value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Визначає тип на основі даних, що були введені в MaskedTextBox.
        /// Елемент керування MaskedTextBox спробує створити цей тип і призначити значення вмісту текстового поля.
        /// Виникне помилка, якщо не вдасться призначити цей тип.
        /// Додаткову інформацію дивіться в документації з елемента керування MaskedTextBox.
        /// </summary>
        public virtual Type ValidatingType
        {
            get => validatingType; // return this.validatingType;
            
            set
            {
                MaskedTextBoxCell mtbCell;
                DataGridViewCell dgvCell;
                int rowCount;

                if (this.validatingType != value)
                {
                    this.validatingType = value;

                    // Спочатку оновіть значення в комірці шаблону.
                    mtbCell = (MaskedTextBoxCell)this.CellTemplate;
                    mtbCell.ValidatingType = value;

                    // Тепер встановіть його на всі комірки в інших рядках.
                    if (this.DataGridView != null && this.DataGridView.Rows != null)
                    {
                        rowCount = this.DataGridView.Rows.Count;
                        for (int x = 0; x < rowCount; x++)
                        {
                            dgvCell = this.DataGridView.Rows.SharedRow(x).Cells[x];
                            if (dgvCell is MaskedTextBoxCell)
                            {
                                mtbCell = (MaskedTextBoxCell)dgvCell;
                                mtbCell.ValidatingType = value;
                            }
                        }
                    }
                }
            }
        }
    }
}