using LiveCharts.Wpf;
using LiveCharts;
using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace MyShop.Views
{
    /// <summary>
    /// Interaction logic for ParticularStatistics.xaml
    /// </summary>
    public partial class ParticularStatistics : Page
    {
        public ParticularStatistics(Statistics srcPage)
        {
            InitializeComponent();

            _statisticsPage = srcPage;
            _advancedPage = new AdvancedStatistics(srcPage);


            categories = CategoryBUS.Instance.getAllCategories();
            categoriesCombobox.ItemsSource = categories;

            if (categories.Count() > 0)
                phones = PhoneBUS.Instance.getPhonesByCategory(categories[categoriesFigureIndex].ID);
            else
                phones = new List<Phone> { };

            productCombobox.ItemsSource = phones;

            statisticsCombobox.ItemsSource = statisticsFigureValues;
            statisticsCombobox.SelectedIndex = statisticsFigureIndex;

            bargraphCombobox.ItemsSource = figureValues;
            bargraphCombobox.SelectedIndex = bargraphFigureIndex;

            statisticsDatePicker.SelectedDate = selectedDate;

            chartTabControl.SelectedIndex = tabSelectedIndex;

            DataContext = this;
        }

        public void getAdvancedStatistic(AdvancedStatistics srcAdvancedStatistics)
        {
            _advancedPage = srcAdvancedStatistics;
        }
        public int statisticsFigureIndex { get; set; } = 1;
        public int bargraphFigureIndex { get; set; } = 0;
        public int tabSelectedIndex { get; set; } = 0;
        public int categoriesFigureIndex { get; set; } = 0;
        public int productFigureIndex { get; set; } = 0;
        public DateTime selectedDate { get; set; } = DateTime.Now;
        public List<string> figureValues = new List<string>() { "Daily", "Weekly", "Monthly", "Yearly" };
        public List<string> statisticsFigureValues = new List<string>() { "General", "Specific", "Advanced" };
        public BindingList<Category> categories;
        public List<Phone> phones;
        private Statistics _statisticsPage;
        private AdvancedStatistics _advancedPage;

        Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        public void configureBarGraphs()
        {
            switch (bargraphFigureIndex)
            {
                case 0:
                    if (phones.Count() > 0 && categories.Count() > 0)
                    {
                        var productResult = StatisticsBUS.Instance.getDailyQuantityOfSpecificProduct(phones[productFigureIndex].ID, categories[categoriesFigureIndex].ID, selectedDate);

                        var quantity = new ChartValues<int>();
                        var dates = new List<string>();

                        foreach (var item in productResult)
                        {
                            quantity.Add((int)item.Item2);
                            dates.Add(item.Item1.ToString());
                        }

                        var quantityCollection = new SeriesCollection()
                    {
                    new RowSeries
                    {
                        Title = "Quantity: ",
                        Values = quantity,
                    }
                    };


                        productBarGraph.AxisY.Clear();
                        productBarGraph.AxisY.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Date",
                            Labels = dates
                        });

                        productBarGraph.AxisX.Clear();
                        productBarGraph.AxisX.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Quantity",
                            LabelFormatter = x => x.ToString("N0")

                        });

                        productBarGraph.Series = quantityCollection;
                    }
                    break;
                case 1:
                    if (phones.Count() > 0 && categories.Count() > 0)
                    {
                        var weeklyProductResult = StatisticsBUS.Instance.getMonthlyQuantityOfSpecificProduct(phones[productFigureIndex].ID, categories[categoriesFigureIndex].ID, selectedDate);

                        var weeklyQuantity = new ChartValues<int>();
                        var weeks = new List<string>();

                        foreach (var item in weeklyProductResult)
                        {
                            weeklyQuantity.Add((int)item.Item2);
                            weeks.Add(item.Item1.ToString());
                        }

                        var weeklyQuantityCollection = new SeriesCollection()
                        {
                            new ColumnSeries
                            {
                                Title = "Quantity: ",
                                Values = weeklyQuantity,
                            }
                        };

                        productBarGraph.AxisX.Clear();
                        productBarGraph.AxisX.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Week",
                            Labels = weeks
                        });

                        productBarGraph.AxisY.Clear();
                        productBarGraph.AxisY.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Quantity",
                            LabelFormatter = x => x.ToString("N0")

                        });

                        productBarGraph.Series = weeklyQuantityCollection;
                    }
                    break;
                case 2:
                    if (phones.Count() > 0 && categories.Count() > 0)
                    {
                        var monthlyProductResult = StatisticsBUS.Instance.getMonthlyQuantityOfSpecificProduct(phones[productFigureIndex].ID, categories[categoriesFigureIndex].ID, selectedDate);

                        var monthlyQuantity = new ChartValues<int>();
                        var months = new List<string>();

                        foreach (var item in monthlyProductResult)
                        {
                            monthlyQuantity.Add((int)item.Item2);
                            months.Add(item.Item1.ToString());
                        }

                        var monthlyQuantityCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Quantity: ",
                        Values = monthlyQuantity,
                    }
                    };


                        productBarGraph.AxisX.Clear();
                        productBarGraph.AxisX.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Month",
                            Labels = months
                        });

                        productBarGraph.AxisY.Clear();
                        productBarGraph.AxisY.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Quantity",
                            LabelFormatter = x => x.ToString("N0")

                        });

                        productBarGraph.Series = monthlyQuantityCollection;
                    }
                    break;
                case 3:
                    if (phones.Count() > 0 && categories.Count() > 0)
                    {
                        var yearlyProductResult = StatisticsBUS.Instance.getYearlyQuantityOfSpecificProduct(phones[productFigureIndex].ID, categories[categoriesFigureIndex].ID);

                        var yearlyQuantity = new ChartValues<int>();
                        var years = new List<string>();

                        foreach (var item in yearlyProductResult)
                        {
                            yearlyQuantity.Add((int)item.Item2);
                            years.Add(item.Item1.ToString());
                        }

                        var yearlyQuantityCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Quantity: ",
                        Values = yearlyQuantity,
                    }
                    };
                        productBarGraph.AxisX.Clear();
                        productBarGraph.AxisX.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Month",
                            Labels = years
                        });

                        productBarGraph.AxisY.Clear();
                        productBarGraph.AxisY.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Quantity",
                            LabelFormatter = x => x.ToString("N0")

                        });

                        productBarGraph.Series = yearlyQuantityCollection;
                    }
                    break;
            }
        }

        public void configurePieChart()
        {
            if (phones.Count() > 0 && categories.Count() > 0)
            {
                var phoneResult = StatisticsBUS.Instance.getPhoneQuantityInCategory(categories[categoriesFigureIndex].ID);

                var phoneQuantityCollection = new SeriesCollection();

                foreach (var item in phoneResult)
                {
                    phoneQuantityCollection.Add(new PieSeries
                    {
                        Title = item.Item1.ToString(),
                        Values = new ChartValues<double> { Convert.ToDouble((int)item.Item2) },
                        DataLabels = true,
                        //LabelPoint = labelPoint,
                    });
                }

                productPieChart.Series = phoneQuantityCollection;
            }
        }

        private void categoriesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            phones = PhoneBUS.Instance.getPhonesByCategory(categories[categoriesFigureIndex].ID);
            productCombobox.ItemsSource = phones;

            configurePieChart();

            if (phones.Count > 0)
            {
                productFigureIndex = 0;
                productCombobox.SelectedIndex = productFigureIndex;
            }
        }

        private void productCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productFigureIndex != -1)
            {
                configureBarGraphs();
            }
        }

        private void bargraphCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoriesFigureIndex != -1 && productFigureIndex != -1)
            {
                configureBarGraphs();
            }
        }

        private void statisticsDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            configureBarGraphs();
        }

        private void statisticsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (statisticsFigureIndex)
            {
                case 0:
                    NavigationService.Navigate(_statisticsPage);
                    statisticsFigureIndex = 1;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                case 1:
                    break;
                case 2:
                    NavigationService.Navigate(_advancedPage);
                    statisticsFigureIndex = 1;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                default:
                    break;
            }
        }

        private void chartTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabSelectedIndex)
            {
                case 0:
                    configureBarGraphs();
                    break;
                case 1:
                    configurePieChart();
                    break;
            }
        }
    }
}
