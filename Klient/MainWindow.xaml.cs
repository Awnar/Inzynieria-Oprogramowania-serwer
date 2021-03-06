﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace Klient
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Job> Jobs;

        public MainWindow()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            try
            {
                lista.Items.Clear();
                var tmp = SynchronousTCPClient.JobList();
                var answer = tmp.Split(new string[] { "\n\r" }, StringSplitOptions.RemoveEmptyEntries);
                Jobs = new List<Job>(answer.Length / 6);
                for (int i = 0; i < answer.Length; i += 6)
                    Jobs.Add(Job.Parse(answer[i] + answer[i + 1] + answer[i + 2] + answer[i + 4] + answer[i + 5]));

                foreach (var item in Jobs)
                    lista.Items.Add(item.Name);
            }
            catch (Exception)
            {
                if (MessageBox.Show("Błąd sesji, proszę spróbować zalogować się jeszcze raz.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    new Login().Show();
                    this.Close();
                }
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            name.Text = des.Text = CreateTime.Text = UpdateTime.Text = "";
            Refresh();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (name.Text.Length > 0 && des.Text.Length > 0)
                {
                    var answer = SynchronousTCPClient.AddJob(name.Text, des.Text);
                    if (answer == null)
                        Refresh();
                    else
                        MessageBox.Show(answer, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Uzupełnij nazwę i opis", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                if (MessageBox.Show("Błąd sesji, proszę spróbować zalogować się jeszcze raz.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    new Login().Show();
                    this.Close();
                }
            }
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tmp = lista.SelectedIndex;
            if (tmp >= 0)
            {
                name.Text = Jobs[tmp].Name;
                des.Text = Jobs[tmp].Description;
                CreateTime.Text = Jobs[tmp].CreateTime;
                UpdateTime.Text = Jobs[tmp].UpdateTime;
            }
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tmp = lista.SelectedIndex;
                if (tmp >= 0)
                {
                    if (SynchronousTCPClient.DelJob(Jobs[tmp].Id))
                        Refresh();
                    else
                        MessageBox.Show("Operacja zakończona niepowodzeniem", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Zaznacz pozycje", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                if (MessageBox.Show("Błąd sesji, proszę spróbować zalogować się jeszcze raz.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    new Login().Show();
                    this.Close();
                }
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tmp = lista.SelectedIndex;
                if (tmp >= 0)
                {
                    if (name.Text.Length > 0 && des.Text.Length > 0)
                    {
                        if (SynchronousTCPClient.UpdateJob(Jobs[tmp].Id, name.Text, des.Text))
                            Refresh();
                        else
                            MessageBox.Show("Operacja zakończona niepowodzeniem", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("Uzupełnij nazwę i opis", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Zaznacz pozycje", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                if (MessageBox.Show("Błąd sesji, proszę spróbować zalogować się jeszcze raz.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    new Login().Show();
                    this.Close();
                }
            }
        }
    }
}