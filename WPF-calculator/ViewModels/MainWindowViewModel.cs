using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_calculator.Models;

namespace WPF_calculator.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        //поле ввода текста
        private string calcText;
        public string CalcText
        {
            get => calcText;
            set
            {
                calcText = value;
                OnPropertyChanged();
            }
        }
        //поле вывода результата
        private string result;
        public string Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }
        //запись текста
        public ICommand AddCommand { get; }
        private void OnAddCommandExecute(object p)
        {

            CalcText += p.ToString();
        }
        private bool CanAddCommandExecuted(object p)
        {
            return true;
        }
        //очистка данных
        public ICommand ResetCommand { get; }
        private void OnResetCommandExecute(object p)
        {
            CalcText = "0";
            Result = string.Empty;
        }
        private bool CanResetCommandExecuted(object p)
        {
            return true;
        }
        //простые арифметические действия + - * / : установлены на знак =
        public ICommand ArifCommand { get; }
        private void OnArifCommandExecute(object p)
        {
            Result = CalcText + "=" + new DataTable().Compute(CalcText, null).ToString();

            CalcText = "0";
        }
        private bool CanArifCommandExecuted(object p)
        {
            var a = CalcText != null ? Regex.IsMatch(CalcText, @"^\d*\.?\-?\d+(\s*[+*/-]\-?\s*\d*\.?\d+)+$") : false;
            return a;
            //return true;
        }
        //удаление из записи последнего символа
        public ICommand RemoveCommand { get; }
        private void OnRemoveCommandExecute(object p)
        {
            if (CalcText.Length != 0)
            {
                CalcText = CalcText.Remove(CalcText.Length - 1);
            }
            else
                CalcText = CalcText.Insert(CalcText.Length, "0");

        }
        private bool CanRemoveCommandExecuted(object p)
        {
            if (CalcText != null)
                return true;
            else
                return false;
        }
        //карень кв.из числа
        public ICommand SqrtCommand { get; }
        private void OnSqrtCommandExecute(object p)
        {
            var r = Convert.ToDouble(CalcText);
            var r1 = CalcArif.SqrtCalc(r);
            Result = $"√({CalcText}) = {r1}";
            CalcText ="0";
        }
        private bool CanSqrtCommandExecuted(object p)
        {
            if (CalcText != null)
                return true;
            else
                return false;
        }
        //1/на число
        public ICommand InverseCommand { get; }
        private void OnInverseCommandExecute(object p)
        {
            Result = $"1/({CalcText}) = {1 / Convert.ToDouble(CalcText)}";
            CalcText = "0";            
        }
        private bool CanInverseCommandExecuted(object p)
        {
            if (CalcText != null)
                return true;
            else
                return false;
        }
        // число в степени, ввод данных через точку
        public ICommand PowerCommand { get; }
        private void OnPowerCommandExecute(object p)
        {
            var reg = new Regex(@"\.{1}");
            if (reg.IsMatch(CalcText))
            {
                var pow = CalcText.Split('.');
                var on = Convert.ToDouble(pow[0]);
                var ty = Convert.ToDouble(pow[1]);
                var res = CalcArif.PowerCalc(on, ty);
                Result = $"{on}^{ty} = {res}";
            }
            else
            {
                Result = "Введите два числа через точку";
            }
            CalcText = "0";
        }
        private bool CanPowerCommandExecuted(object p)
        {
            if (CalcText != null)
                return true;
            else
                return false;
        }
        // процент от числа, ввод данных через точку
        public ICommand PercentagesCommand { get; }
        private void OnPercentagesCommandExecute(object p)
        {
            var reg = new Regex(@"\.{1}");
            if (reg.IsMatch(CalcText))
            {
                var pow = CalcText.Split('.');
                var on = Convert.ToDouble(pow[0]);
                var ty = Convert.ToDouble(pow[1]);
                Result = $"{on} % от {ty} = {on / 100 * ty}";
            }
            else
                Result = "Введите два числа через точку";

            CalcText = "0";
        }

        private bool CanPercentagesCommandExecuted(object p)
        {
            if (CalcText != null)
                return true;
            else
                return false;
        }
        // команда устанавливает "-" перед первым числом
        public ICommand SignCommand { get; }
        private void OnSignCommandExecute(object p)
        {
            if (CalcText[0] == '-')
                CalcText = CalcText.Remove(0, 1);
            else
                CalcText = "-" + CalcText;
        }
        private bool CanSignCommandExecuted(object p)
        {
            if (CalcText != null)
                return true;
            else
                return false;
        }
        public MainWindowViewModel()
        {
            AddCommand = new RelayCommand(OnAddCommandExecute, CanAddCommandExecuted);
            ResetCommand = new RelayCommand(OnResetCommandExecute, CanResetCommandExecuted);
            ArifCommand = new RelayCommand(OnArifCommandExecute, CanArifCommandExecuted);
            RemoveCommand = new RelayCommand(OnRemoveCommandExecute, CanRemoveCommandExecuted);
            SqrtCommand = new RelayCommand(OnSqrtCommandExecute, CanSqrtCommandExecuted);
            InverseCommand = new RelayCommand(OnInverseCommandExecute, CanInverseCommandExecuted);
            PowerCommand = new RelayCommand(OnPowerCommandExecute, CanPowerCommandExecuted);
            PercentagesCommand = new RelayCommand(OnPercentagesCommandExecute, CanPercentagesCommandExecuted);
            SignCommand = new RelayCommand(OnSignCommandExecute, CanSignCommandExecuted);
        }


    }
}
