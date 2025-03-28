using System;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CurrencyConverter_Static
{
    public partial class MainWindow : Window
    {
        Root val = new Root();  // Ensure val is instantiated

        public class Root
        {
            public Rate rates { get; set; }
            public long timestamp;
            public string license;
        }
        public class Rate
        {
            public double INR { get; set; }
            public double JPY { get; set; }
            public double USD { get; set; }
            public double NZD { get; set; }
            public double EUR { get; set; }
            public double CAD { get; set; }
            public double ISK { get; set; }
            public double PHP { get; set; }
            public double DKK { get; set; }
            public double CZK { get; set; }
            public double AFN { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            // ClearControls method is used to clear all control values
            ClearControls();

            // Fetch exchange rates from API before binding the currency list
            GetValue();
        }

        private async void GetValue()
        {
            try
            {
                val = await GetData("https://openexchangerates.org/api/latest.json?app_id=183d2227d6724417ad0d85ecdfb882f7");

                if (val != null && val.rates != null)
                {
                    MessageBox.Show("TimeStamp: " + val.timestamp, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    BindCurrency();  // Call BindCurrency only after API data is fetched
                }
                else
                {
                    MessageBox.Show("Error: Unable to fetch currency rates.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching currency data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static async Task<Root> GetData(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<Root>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return new Root(); // Return an empty object if API call fails
        }

        #region Bind Currency From and To Combobox
        private void BindCurrency()
        {
            if (val == null || val.rates == null)
            {
                MessageBox.Show("Error: Currency data is not available.");
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Value");

            dt.Rows.Add("--SELECT--", 0);
            dt.Rows.Add("INR", val.rates.INR);
            dt.Rows.Add("USD", val.rates.USD);
            dt.Rows.Add("NZD", val.rates.NZD);
            dt.Rows.Add("JPY", val.rates.JPY);
            dt.Rows.Add("EUR", val.rates.EUR);
            dt.Rows.Add("CAD", val.rates.CAD);
            dt.Rows.Add("ISK", val.rates.ISK);
            dt.Rows.Add("PHP", val.rates.PHP);
            dt.Rows.Add("DKK", val.rates.DKK);
            dt.Rows.Add("CZK", val.rates.CZK);
            dt.Rows.Add("AFN", val.rates.AFN);

            if (cmbFromCurrency == null || cmbToCurrency == null)
            {
                MessageBox.Show("Error: ComboBox controls are not initialized.");
                return;
            }

            cmbFromCurrency.ItemsSource = dt.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Text";
            cmbFromCurrency.SelectedValuePath = "Value";
            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.ItemsSource = dt.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Value";
            cmbToCurrency.SelectedIndex = 0;
        }
        #endregion

        #region Button Click Event
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCurrency.Text))
            {
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCurrency.Focus();
                return;
            }
            if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }
            if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbToCurrency.Focus();
                return;
            }

            double convertedValue;
            double inputAmount = double.Parse(txtCurrency.Text);

            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                convertedValue = inputAmount;
            }
            else
            {
                double fromRate = double.Parse(cmbFromCurrency.SelectedValue.ToString());
                double toRate = double.Parse(cmbToCurrency.SelectedValue.ToString());
                convertedValue = (toRate * inputAmount) / fromRate;
            }

            lblCurrency.Content = $"{cmbToCurrency.Text} {convertedValue:N3}";
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }
        #endregion

        #region Extra Events
        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0) cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0) cmbToCurrency.SelectedIndex = 0;
            lblCurrency.Content = "";
            txtCurrency.Focus();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
    }
}
