using System;
using System.Windows.Forms;

namespace DB
{
    /// <summary> Клас для відображення стовпця DataGridView з числовими значеннями з можливістю вибору через NumericUpDown </summary>
    public class NumericUpDownColumn : DataGridViewColumn
    {
        public NumericUpDownColumn()
            : base(new NumericUpDownCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate; // return base.CellTemplate;
            set
            {
                // Перевірка на відповідність типу комірки
                if (value != null && !value.GetType().IsAssignableFrom(typeof(NumericUpDownCell)))
                {
                    throw new InvalidCastException("Повинно бути NumericUpDownCell");
                }
                base.CellTemplate = value;
            }
        }
    }
    /// <summary> Клас комірки DataGridView, що використовує NumericUpDown для редагування числових значень </summary>
    public class NumericUpDownCell : DataGridViewTextBoxCell
    {
        private readonly decimal min = 0;
        private readonly decimal max = 100;

        public NumericUpDownCell()
            : base()
        {
            Style.Format = "F0"; // Формат відображення для значення
        }

        public NumericUpDownCell(decimal min, decimal max)
            : base()
        {
            this.min = min;
            this.max = max;
            Style.Format = "F0"; // Формат відображення для значення
        }
        /// <summary> Ініціалізація елемента управління для редагування комірки </summary>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            NumericUpDownEditingControl ctl = DataGridView.EditingControl as NumericUpDownEditingControl;
            ctl.Minimum = this.min;
            ctl.Maximum = this.max;
            try { ctl.Value = Convert.ToDecimal(this.Value); }
            catch { ctl.Value = 0; }
        }

        public override Type EditType => typeof(NumericUpDownEditingControl); // get { return typeof(NumericUpDownEditingControl); }

        public override Type ValueType => typeof(decimal); // get { return typeof(Decimal); }

        public override object DefaultNewRowValue => null; //get { return null; }
    }
    /// <summary> Елемент управління для редагування числових значень в комірці DataGridView </summary>
    public class NumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
    {
        private bool valueIsChanged = false;

        public NumericUpDownEditingControl()
            : base()
        {
            this.DecimalPlaces = 0;
        }

        public DataGridView EditingControlDataGridView { get; set; }

        public object EditingControlFormattedValue
        {
            get => this.Value.ToString("F0");
            set => this.Value = Decimal.Parse(value.ToString());
        }

        public int EditingControlRowIndex { get; set; }

        public bool EditingControlValueChanged
        {
            get => valueIsChanged;
            set => valueIsChanged = value;
        }

        public Cursor EditingPanelCursor => base.Cursor;

        public bool RepositionEditingControlOnValueChange => false;

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }
        /// <summary> Перевірка вводу клавіш </summary>
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return (keyData == Keys.Left || keyData == Keys.Right ||
                keyData == Keys.Up || keyData == Keys.Down ||
                keyData == Keys.Home || keyData == Keys.End ||
                keyData == Keys.PageDown || keyData == Keys.PageUp);
        }
        /// <summary> Отримання форматованого значення для відображення в DataGridView </summary>
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Value.ToString();
        }

        /// <summary> Підготовка обранної комірки до редагування </summary>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        /// <summary> Перевизначення події зміни значення </summary>
        protected override void OnValueChanged(EventArgs e)
        {
            valueIsChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(e);
        }
    }
}