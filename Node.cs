using System.ComponentModel;

namespace EightPuzzle
{
    /// <summary>
    /// Node类型，对应单个格子的类型，用于操作每个格子
    /// </summary>
    public class Node : INotifyPropertyChanged
    {
        int _value = 0;//格子对应的值
        string _displayValue = string.Empty;//格子对应显示的string值，0为空格
        int _x = 0;//格子在二维数组中的x坐标
        int _y = 0;//格子在二维数组中的y坐标
        int _index = 0;//格子在UniformGrid中的下标
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                DisplayValue = _value == 0 ? " " : _value.ToString();
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(DisplayValue));
            }
        }
        public string DisplayValue
        {
            get { return _displayValue; }
            set
            {
                _displayValue = value;
                OnPropertyChanged(nameof(DisplayValue));
            }
        }
        public int X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }
        public int Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }
        /// <summary>
        /// 深拷贝构造函数
        /// </summary>
        /// <param name="o">拷贝对象</param>
        public Node(Node o)
        {
            Value = o.Value;
            X = o.X;
            Y = o.Y;
            DisplayValue = o.DisplayValue;
            Index = o.Index;
        }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public Node()
        {
            //空
        }
        /// <summary>
        /// 设置下标，坐标
        /// </summary>
        /// <param name="index">下标</param>
        public void SetValue(int index)
        {
            Index = index;
            X = index / Constants.GridRange;
            Y = index % Constants.GridRange;
        }
        public void UpdateLocation(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }
        /// <summary>
        /// 数据绑定PropertyChanged事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// 数据绑定OnPropertyChanged，数据改变时调用
        /// </summary>
        /// <param name="propertyName">变量名</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
