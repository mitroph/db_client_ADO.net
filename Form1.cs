using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;
using MySql.Data.MySqlClient;
using DB;


namespace WindowsFormsApp1
{

    public partial class FormMain : Form
    {

        public static readonly string connectionString = "server=localhost;port=3306;database=mydb;uid=root;pwd=1112;";
        

        public static readonly MySqlConnection database = new MySqlConnection(connectionString);





        private MySqlDataAdapter dataAdapter = new MySqlDataAdapter();

        private DataGridViewComboBoxColumn idMasterclass;

        private DataGridViewComboBoxColumn idVisitor;

        private DataGridViewComboBoxColumn idSpecialization;

        public FormMain()
        {
            InitializeComponent();

           

            

            try
            {
                using (database)
                {
                    database.Open();
                    DataTable tables = database.GetSchema("Tables");

                    foreach (DataRow row in tables.Rows)
                    {
                        string tableName = (string)row["TABLE_NAME"];
                        this.Tables.Items.Add(tableName);

                    }
                }
                this.Tables.SelectedIndex = 5;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + "\n", "Произошла ошибка при выполнении SQL-запроса!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            


        }





        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand(this.textBox_SQL.Text, database);
            // MySqlCommand cmd = new MySqlCommand("SELECT * FROM khai.student;", database);
            DataSet dataSet = new DataSet();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
            try
            {
                dataAdapter.Fill(dataSet);
                this.dataGridViewDB.DataSource = dataSet.Tables[0];

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + "\n" + cmd.CommandText, "Произошла ошибка при выполнении SQL-запроса!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Безвыходным мы называем положение, выход из которого нам не нравится.", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {

            Settings.Default.Save();


        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = MessageBox.Show("Вы хотите закрыть программу?",
            "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;


        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
        public void GetData(string selectCommand, BindingSource bindingSource, DataGridView dataGridView)
        {
            try
            {
                // Створює новий адаптер даних на основі вказаного запиту.
                dataAdapter = new MySqlDataAdapter(selectCommand, connectionString);

                // Створює конструктор команд для генерації команд оновлення,
                // вставки та видалення SQL на основі selectCommand.
                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

                DataTable table = new DataTable     // Створює нову таблицю даних,
                {
                    Locale = System.Globalization.CultureInfo.InvariantCulture
                };
                dataAdapter.Fill(table);    // заповнює цю нову таблицю даних з БД
                bindingSource.DataSource = table;   // й прив’язує її до BindingSource.

                if (dataGridView != null)
                {
                    dataGridView.DataSource = bindingSource;// Підвантажує таблицю у dataGridView 
                                                            // Змінює розмір стовпців DataGridView відповідно до щойно завантаженого вмісту.
                    dataGridView.AutoResizeColumns(
              DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(selectCommand + "\n\n" + ex.Message, "Сталася помилка під час виконання SQL-запиту!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if ((new System.Text.RegularExpressions.Regex(@"^[A-Za-zA-Яа-яӀіЇїЄє]+$")).IsMatch(Search.Text))
            {
                Search.ForeColor = System.Drawing.Color.Black;
                int i = this.bindingSourceMySQL.Find("SURNAME", Search.Text);

                if (i == -1)
                {
                    DataView dv = new DataView((DataTable)bindingSourceMySQL.DataSource);
                    dv.RowFilter = String.Format("SURNAME LIKE '*(0)*'", Search.Text);

                    if (dv.Count != 0) i = this.bindingSourceMySQL.Find("SURNAME", dv[0]["SURNAME"]);
                    dv.Dispose();
                }
                this.bindingSourceMySQL.Position = i;
            }

            else
            {
                Search.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            dataAdapter.Update((DataTable)bindingSourceMySQL.DataSource);
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void Tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindingSourceMySQL.DataSource = null;
            dataGridViewDB.Columns.Clear();

            GetData("select * from mydb." + this.Tables.Text, bindingSourceMySQL, dataGridViewDB);
            switch (Tables.Text)
            {
                case "visitor":
                    AddVisitorDelegate();
                    break;
                case "participant":
                    AddParticipantDelegate();
                    break;
                case "review":
                    
                    break;
                default:
                    // Виконати дії за замовчуванням, якщо потрібно
                    break;
            }
        }
        private void AddVisitorDelegate()
        {
            GetData("select * from mydb.master_class;", bindingSourceGroupName, null);

            this.idMasterclass = new System.Windows.Forms.DataGridViewComboBoxColumn();

            this.dataGridViewDB.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {

                this.idMasterclass



            });




            this.idMasterclass.DataPropertyName = "id_Master-Class";

            this.idMasterclass.DataSource = this.bindingSourceGroupName;


            this.idMasterclass.DisplayMember = "Mastery";

            this.idMasterclass.HeaderText = "Назва майстеркласу";

            this.idMasterclass.ValueMember = "id_Master-Class";

            this.dataGridViewDB.Columns[0].Visible = false;

            this.dataGridViewDB.Columns[5].Visible = false;

            this.dataGridViewDB.RowHeadersVisible = false;
        }
        private void AddParticipantDelegate()
        {

            GetData("select * from mydb.visitor;", bindingSourceVisitorSurname, null);

            MaskedTextBoxColumn idVisitor = new MaskedTextBoxColumn();



            idVisitor.DataPropertyName = ((DataTable)bindingSourceMySQL.DataSource).Columns[2].ColumnName;

            idVisitor.HeaderText = "id Відвідувача";

         


            GetData("select * from mydb.master_class;", bindingSourceMasterclassName, null);

            this.idMasterclass = new System.Windows.Forms.DataGridViewComboBoxColumn();

            


            this.idMasterclass.DataPropertyName = "id_Master-Class";

            this.idMasterclass.DataSource = this.bindingSourceMasterclassName;


            this.idMasterclass.DisplayMember = "Mastery";

            this.idMasterclass.HeaderText = "Назва майстеркласу";

            this.idMasterclass.ValueMember = "id_Master-Class";



            GetData("select * from mydb.specialization;", bindingSourceSpecializationName, null);

            MaskedTextBoxColumn idSpecialization = new MaskedTextBoxColumn();



            idSpecialization.DataPropertyName = ((DataTable)bindingSourceMySQL.DataSource).Columns[2].ColumnName;

            idSpecialization.HeaderText = "id Спеціалізації";

            this.dataGridViewDB.Columns.AddRange(new DataGridViewColumn[] {

                idVisitor,
                this.idMasterclass,
                idSpecialization
                


            });

            this.dataGridViewDB.Columns[0].Visible = false;
            this.dataGridViewDB.Columns[1].Visible = false;
            this.dataGridViewDB.Columns[2].Visible = false;
            this.dataGridViewDB.Columns[3].Visible = false;
            this.dataGridViewDB.RowHeadersVisible = false;

        }

        private void bindingSourceGroupName_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
