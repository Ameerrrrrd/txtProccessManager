using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;

namespace KP_KAZLOVSKIY
{
    public class EmailDomainChartForm : Form
    {
        private Chart emailChart;
        private const string ConnectionString = "Server=localhost;Database=lol;Port=3306;Uid=root;Pwd=root";

        public EmailDomainChartForm()
        {
            this.Text = "Email Domain Chart";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(800, 600);
            this.Icon = Properties.Resources.cat;

            emailChart = new Chart
            {
                Dock = DockStyle.Fill
            };

            ChartArea chartArea = new ChartArea("MainArea");
            emailChart.ChartAreas.Add(chartArea);
            Series series = new Series("Domains")
            {
                ChartType = SeriesChartType.Column,
                XValueType = ChartValueType.String
            };
            emailChart.Series.Add(series);
            this.Controls.Add(emailChart);

            LoadAndRenderChart();
        }

        private void LoadAndRenderChart()
        {
            Dictionary<string, int> domainCounts = GetEmailDomainStats();

            var series = emailChart.Series["Domains"];
            series.Points.Clear();

            foreach (var kv in domainCounts.OrderByDescending(k => k.Value))
            {
                series.Points.AddXY(kv.Key, kv.Value);
            }
        }

        private Dictionary<string, int> GetEmailDomainStats()
        {
            var stats = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT email FROM users WHERE email IS NOT NULL AND email != ''";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string email = reader.GetString("email");
                        var parts = email.Split('@');
                        if (parts.Length == 2)
                        {
                            string domain = parts[1].Trim().ToLower();
                            if (!string.IsNullOrEmpty(domain))
                            {
                                if (stats.ContainsKey(domain))
                                    stats[domain]++;
                                else
                                    stats[domain] = 1;
                            }
                        }
                    }
                }
            }

            return stats;
        }
    }
}
