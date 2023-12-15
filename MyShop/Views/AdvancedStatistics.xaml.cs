﻿using MyShop.BUS;
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

namespace MyShop.Views
{
    /// <summary>
    /// Interaction logic for AdvancedStatistics.xaml
    /// </summary>
    public partial class AdvancedStatistics : Page
    {
        public AdvancedStatistics(Statistics srcPage)
        {
            InitializeComponent();

            _statisticsPage = srcPage;

            statisticsDatePicker.SelectedDate = selectedDate;

            statisticsCombobox.ItemsSource = statisticsFigureValues;
            statisticsCombobox.SelectedIndex = statisticsFigureIndex;

            timeCombobox.ItemsSource = figureValues;
            timeCombobox.SelectedIndex = figureIndex;

            phones = PhoneBUS.Instance.getBestSellingPhonesInWeek(selectedDate);

            for (int i = 0; i < phones.Count(); i++)
            {
                System.Diagnostics.Debug.WriteLine(phones[i].PhoneName);
            }

            PhoneDataGrid.ItemsSource = phones;

            DataContext = this;
        }

        public void getSpecificStatistic(ParticularStatistics srcSpecificStatistics)
        {
            _specificPage = srcSpecificStatistics;
        }
        public DateTime selectedDate { get; set; } = DateTime.Now;
        public int figureIndex { get; set; } = 0;
        public List<string> figureValues = new List<string>() { "In Week", "In Month", "In Year" };
        public List<string> statisticsFigureValues = new List<string>() { "General", "Specific", "Advanced" };
        public int statisticsFigureIndex { get; set; } = 2;
        public List<BestSellingPhone> phones;
        private Statistics _statisticsPage;
        private ParticularStatistics _specificPage;

        private void statisticsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (statisticsFigureIndex)
            {
                case 0:
                    NavigationService.Navigate(_statisticsPage);
                    statisticsFigureIndex = 2;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                case 1:
                    NavigationService.Navigate(_specificPage);
                    statisticsFigureIndex = 2;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        public void configureBestSellingPhonesDataGrid()
        {
            switch (figureIndex)
            {
                case 0:
                    phones = PhoneBUS.Instance.getBestSellingPhonesInWeek(selectedDate);
                    PhoneDataGrid.ItemsSource = phones;
                    break;
                case 1:
                    phones = PhoneBUS.Instance.getBestSellingPhonesInMonth(selectedDate);
                    PhoneDataGrid.ItemsSource = phones;
                    break;
                case 2:
                    phones = PhoneBUS.Instance.getBestSellingPhonesInYear(selectedDate);
                    PhoneDataGrid.ItemsSource = phones;
                    break;
                default:
                    phones = PhoneBUS.Instance.getBestSellingPhonesInWeek(selectedDate);
                    PhoneDataGrid.ItemsSource = phones;
                    break;
            }
        }

        private void timeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            configureBestSellingPhonesDataGrid();
        }

        private void statisticsDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            configureBestSellingPhonesDataGrid();
        }
    }
}
