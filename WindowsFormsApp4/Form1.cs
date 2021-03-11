using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static System.Convert;

namespace WindowsFormsApp4
{
    public partial class Tab : Form
    {
        internal static List<StoredProduct> Products = new List<StoredProduct>();
        sbyte edit = -1;
        public Tab()
        {
            InitializeComponent();
            FileOpen.Filter = "Списки(*.l)|*.l|Текстовые файлы(*.txt)|*.txt|Все файлы(*.)|*.*";
            SaveFile.Filter = FileOpen.Filter;
        }

        private void AddProductButton_Click(object sender, EventArgs e)
        {
            try
            {
                Products.Add(new StoredProduct(NameTextBox.Text, ToDouble(PriceTextBox.Text), ToInt64(SizeTextBox.Text), Convert.ToString(DateTimePickerOfProduct.Value)));
                ProductsGridView.Rows.Add(Products[Product.ID - 1].Id, Products[Product.ID - 1].Size, Products[Product.ID - 1].Date, Products[Product.ID - 1].Name, Products[Product.ID - 1].Price);
            }
            catch (Exception)
            {
                MessageBox.Show("Не все данные введены корректно.");
            }
            SizeTextBox.Clear();
            NameTextBox.Clear();
            PriceTextBox.Clear();
            EnabledButtons();
        }

        private void DeleteProductButton_Click(object sender, EventArgs e)
        {
            if (Products.Count > 1)
            {
                ProductsGridView.Rows.RemoveAt(ProductsGridView.CurrentCell.RowIndex);
                Products[ProductsGridView.CurrentCell.RowIndex] = null;
                Products.Remove(Products[ProductsGridView.CurrentCell.RowIndex]);
            }
            else
            {
                Products.Clear();
                ProductsGridView.Rows.RemoveAt(0);
                Product.ID = 0;
            }
            EnabledButtons();
        }

        private void EditProductButton_Clickk(object sender, EventArgs e)
        {
            if (edit == -1)
            {
                AddProductButton.Hide();
                DeleteProductButton.Hide();
            }
            else
            {
                AddProductButton.Show();
                DeleteProductButton.Show();
            }

            if (edit == 1)
            {
                try
                {
                    Products[ProductsGridView.CurrentCell.RowIndex].Size = ToInt64(SizeTextBox.Text);
                    Products[ProductsGridView.CurrentCell.RowIndex].Name = NameTextBox.Text;
                    Products[ProductsGridView.CurrentCell.RowIndex].Price = ToDouble(PriceTextBox.Text);
                    Products[ProductsGridView.CurrentCell.RowIndex].Date = Convert.ToString(DateTimePickerOfProduct.Value);
                    ProductsGridView.CurrentRow.SetValues(Products[ProductsGridView.CurrentCell.RowIndex].Id, Products[ProductsGridView.CurrentCell.RowIndex].Size, Products[ProductsGridView.CurrentCell.RowIndex].Date, Products[ProductsGridView.CurrentCell.RowIndex].Name, Products[ProductsGridView.CurrentCell.RowIndex].Price);
                    SizeTextBox.Clear();
                    NameTextBox.Clear();
                    PriceTextBox.Clear();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка");
                }
            }
            edit *= -1;
        }

        private void SaveList_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == SaveFile.ShowDialog())
            {
                string path = SaveFile.FileName;
                foreach (StoredProduct p in Products)
                    File.AppendAllText(path, p.Id + " " + p.Size + " " + p.Date + " " + p.Name + " " + p.Price + "\n");
                if (path != "")
                    MessageBox.Show("Список сохранён");
            }
        }

        private void LoadList_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == FileOpen.ShowDialog())
            {
                string[] data = File.ReadAllLines(FileOpen.FileName);
                ProductsGridView.Rows.Clear();
                Products.Clear();
                Product.ID = 0;
                try
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        string[] line = data[i].Split(' ');
                        double size = 0;
                        string name = "";
                        double price = 0.00;
                        string date = "";
                        int j = 0;
                        foreach (string s in line)
                        {
                            switch (j)
                            {
                                case 1:
                                    size = ToDouble(s);
                                    break;
                                case 2:
                                    date = s;
                                    break;
                                case 3:
                                    date += " " + s;
                                    break;
                                case 4:
                                    name = s;
                                    break;
                                case 5:
                                    price = ToDouble(s);
                                    break;
                            }
                            j++;
                        }
                        Products.Add(new StoredProduct(name, price, size, date));
                        ProductsGridView.Rows.Add(Products[Product.ID - 1].Id, size, date, name, price);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Невозможно прочесть файл!");
                }    
            }
            EnabledButtons();
        }

        void ClearList_Button(object sender, EventArgs e)
        {
            Products.Clear();
            ProductsGridView.Rows.Clear();
            Product.ID = 0;
            EnabledButtons();
        }
        public void EnabledButtons()
        {
            if (Products.Count > 0)
            {
                EditProductButton.Enabled = true;
                DeleteProductButton.Enabled = true;
                ClearListButton.Enabled = true;
                GetProductsButton.Enabled = true;
            }
            else
            {
                EditProductButton.Enabled = false;
                DeleteProductButton.Enabled = false;
                ClearListButton.Enabled = false;
                GetProductsButton.Enabled = false;
            }
        }

        void GetProductsButton_Click(object sender, EventArgs e)
        {
            GetProductsForm form2 = new GetProductsForm();
            form2.Show();
        }
    }
    public abstract class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public static int ID = 0;
        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }

    internal class StoredProduct : Product
    {
        internal double Size { get; set; }
        internal string Date { get; set; }
        internal int Id { get; set; }
        internal StoredProduct(string name, double price, double size, string date) : base(name, price)
        {
            Size = size;
            Date = date;
            Id = ++ID;
        }
    }
}
