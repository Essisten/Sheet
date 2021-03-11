using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static WindowsFormsApp4.Tab;

namespace WindowsFormsApp4
{
    public partial class GetProductsForm : Form
    {
        List<TakenProduct> Box = new List<TakenProduct>();
        public GetProductsForm()
        {
            InitializeComponent();
        }

        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Box.Count > 0 && dataGridView1.CurrentCell.RowIndex < Box.Count)
            {
                Box.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            }
            else if (dataGridView1.CurrentCell == null || dataGridView1.Rows.Count == 1)
                dataGridView1.Rows.Clear();
            else if (dataGridView1.CurrentCell.RowIndex >= Box.Count)
                MessageBox.Show("Чтобы удалить товар из корзины, выделите его в таблице и только затем нажмите на соответствующую кнопку.");
            else
                MessageBox.Show("Ошибка");
            Checker();
        }

        void FinishButton_Click(object sender, EventArgs e)
        {
            double sum = 0;
            foreach (TakenProduct product in Box)
            {
                sum += product.Price * product.Count;
            }
            MessageBox.Show($"Общая сумма вашего заказа составляет {Convert.ToString(sum)} рублей.");
        }

        void AddButton_Click(object sender, EventArgs e)
        {
            if (SelectProduct.SelectedItem != null && Counter.Value != 0)
            {
                Product p = Products[SelectProduct.SelectedIndex];
                Box.Add(new TakenProduct(p.Name, p.Price, (int)Counter.Value));
                dataGridView1.Rows.Add(Box[Box.Count - 1].Name, Box[Box.Count - 1].Price, Counter.Value);
            }
            else if (SelectProduct.SelectedItem == null)
                MessageBox.Show("Прежде чем добавлять продукт в корзину, выберите его среди представленных.");
            else if (Counter.Value == 0)
                MessageBox.Show("Зачем заказывать 0 товаров?");
            else
                MessageBox.Show("Ошибка");
            Checker();
        }

        void GetProductsForm_Load(object sender, EventArgs e)
        {
            SelectProduct.Items.Clear();
            foreach (StoredProduct p in Products)
            {
                SelectProduct.Items.Add(p.Name);
            }
        }

        void Checker()
        {
            if (dataGridView1.Rows.Count > 0 && Box.Count > 0)
            {
                DeleteButton.Enabled = true;
                FinishButton.Enabled = true;
            }
            else
            {
                DeleteButton.Enabled = false;
                FinishButton.Enabled = false;
            }
        }
    }
    internal class TakenProduct : Product
    {
        internal int Count { get; set; }
        internal TakenProduct(string name, double price, int count) : base(name, price)
        {
            Count = count;
        }
    }
}
