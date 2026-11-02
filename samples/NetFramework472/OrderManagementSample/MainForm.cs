using System;
using System.Drawing;
using System.Windows.Forms;
using EBay.OrderManagement;

namespace OrderManagementSample
{
    /// <summary>Lists recent orders from the Fulfillment REST API and the GraphQL orders query side by side.</summary>
    public sealed class MainForm : Form
    {
        private readonly RadioButton _production = new RadioButton { Text = "Production", Checked = true, AutoSize = true };
        private readonly RadioButton _sandbox = new RadioButton { Text = "Sandbox", AutoSize = true };
        private readonly TextBox _token = new TextBox { Width = 320, UseSystemPasswordChar = true };
        private readonly TextBox _graphQLUrl = new TextBox { Width = 260 };
        private readonly NumericUpDown _days = new NumericUpDown { Minimum = 1, Maximum = 90, Value = 30, Width = 50 };
        private readonly Button _load = new Button { Text = "Load orders", AutoSize = true };
        private readonly Label _status = new Label { Dock = DockStyle.Bottom, Height = 40, Padding = new Padding(6) };
        private readonly DataGridView _orders = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        private readonly TextBox _details = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Both,
            Font = new Font(FontFamily.GenericMonospace, 9f)
        };

        private OrderManagementClient _client;

        public MainForm()
        {
            Text = "eBay Order Management SDK sample (.NET Framework 4.7.2)";
            Width = 1100;
            Height = 720;

            var options = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 70, Padding = new Padding(6), WrapContents = true };
            options.Controls.AddRange(new Control[]
            {
                _production, _sandbox,
                new Label { Text = "User access token:", AutoSize = true, Padding = new Padding(12, 6, 0, 0) }, _token,
                new Label { Text = "Days:", AutoSize = true, Padding = new Padding(6, 6, 0, 0) }, _days,
                _load,
                new Label { Text = "GraphQL URL (optional):", AutoSize = true, Padding = new Padding(0, 6, 0, 0) }, _graphQLUrl
            });

            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 360 };
            split.Panel1.Controls.Add(_orders);
            split.Panel2.Controls.Add(_details);

            Controls.Add(split);
            Controls.Add(options);
            Controls.Add(_status);

            _sandbox.CheckedChanged += (s, e) => _graphQLUrl.Text = _sandbox.Checked ? SdkSetup.SandboxGraphQLUrl : string.Empty;
            _load.Click += OnLoadClicked;
            _orders.SelectionChanged += OnOrderSelected;
            FormClosed += (s, e) => DisposeClient();
            _status.Text = "Paste a user access token (eBay developer portal > User Tokens) and click Load orders.";
        }

        private async void OnLoadClicked(object sender, EventArgs e)
        {
            _load.Enabled = false;
            _details.Text = string.Empty;
            try
            {
                DisposeClient();
                var environment = _sandbox.Checked ? EbayEnvironment.Sandbox : EbayEnvironment.Production;
                _client = new OrderManagementClient(SdkSetup.Create(environment, _token.Text.Trim(), _graphQLUrl.Text.Trim()));
                _status.Text = "Loading " + environment + " orders...";

                var result = await OrderOverview.LoadAsync(_client, (int)_days.Value);
                _orders.DataSource = result.Rows;
                _status.Text = result.Rows.Count + " orders."
                    + (result.RestError != null ? "  REST failed: " + result.RestError : string.Empty)
                    + (result.GraphQLError != null ? "  GraphQL failed: " + result.GraphQLError : string.Empty);
            }
            catch (Exception ex)
            {
                _status.Text = ex.GetType().Name + ": " + ex.Message;
            }
            finally
            {
                _load.Enabled = true;
            }
        }

        private async void OnOrderSelected(object sender, EventArgs e)
        {
            var row = _orders.CurrentRow != null ? _orders.CurrentRow.DataBoundItem as OrderRow : null;
            if (row == null || _client == null)
            {
                return;
            }
            try
            {
                _details.Text = "Loading " + row.OrderId + "...";
                _details.Text = await OrderOverview.DetailsJsonAsync(_client, row.OrderId);
            }
            catch (Exception ex)
            {
                _details.Text = ex.GetType().Name + ": " + ex.Message;
            }
        }

        private void DisposeClient()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
    }
}
