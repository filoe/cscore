using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace CSCoreDemo.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private static MainViewModel _main;

        public static MainViewModel Main
        {
            get { return _main ?? (_main = new MainViewModel()); }
            set { _main = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetProperty<T>(T value, ref T field, Expression<Func<object>> property)
        {
            return SetProperty(value, ref field, GetPropertyName(property));
        }

        public bool SetProperty<T>(T value, ref T field, string property)
        {
            if (field == null || !field.Equals(value))
            {
                field = value;
                OnPropertyChanged(property);
                return true;
            }
            return false;
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnPropertyChanged(Expression<Func<object>> property)
        {
            OnPropertyChanged(GetPropertyName(property));
        }

        public string GetPropertyName(Expression<Func<object>> property)
        {
            var lambda = property as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }
            var constantExpression = memberExpression.Expression as ConstantExpression;
            var propertyInfo = memberExpression.Member as PropertyInfo;

            return propertyInfo.Name;
        }
    }
}