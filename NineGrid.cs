using System.ComponentModel;

namespace EightPuzzle
{
    /// <summary>
    /// Node类型，对应单个格子的类型，用于操作每个格子
    /// </summary>
    public class Node:INotifyPropertyChanged
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
                DisplayValue = _value==0?" ":_value.ToString();
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
            _value = o.Value;
            X = o.X;
            Y = o.Y;
            _displayValue = o.DisplayValue;
            _index = o.Index;
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
            X = index/Constants.GridRange;
            Y = index%Constants.GridRange;
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
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    /// <summary>
    /// NineGrid九宫格类，用于管理九宫格
    /// </summary>
    class NineGrid
    {
        public Node[,] map = new Node[Constants.GridRange,Constants.GridRange];//九宫格二维数组
        public int inverseNumber = 0;//逆序数
        
        /// <summary>
        /// 无参构造函数，初始化九宫格
        /// </summary>
        public NineGrid()
        {
            for(int i = 0; i < Constants.GridRange; i++)
            {
                for (int j = 0; j < Constants.GridRange; j++)
                {
                    map[i,j] = new Node();
                }
            }
        }
        /// <summary>
        /// 计算逆序数
        /// </summary>
        /// <param name="list">数值排列表</param>
        private void SetInverseNumber(List<int> list)
        {
            int count = 0;
            for(int i=0;i<list.Count-1;i++)
            {
                int temp = list[i];
                for(int j = i+1;j<list.Count;j++)
                {
                    if(temp > list[j])
                    {
                        count++;
                    }
                }
            }
            inverseNumber = count;
        }
        /// <summary>
        /// 设置每格相关数据
        /// </summary>
        /// <param name="list">数值排列表</param>
        public void SetValue(List<int> list)
        {
            int k = 0;
            for(int i = 0;i<Constants.GridRange;i++)
            {
                for(int j = 0;j<Constants.GridRange;j++)
                {
                    map[i,j].Value = list[k];
                    if (list[k] == 0)
                    {
                        map[i, j].DisplayValue = " ";
                    }
                    else
                    {
                        map[i, j].DisplayValue = list[k].ToString();
                    }
                    map[i,j].Index = k;
                    map[i, j].X = k / Constants.GridRange;
                    map[i, j].Y = k % Constants.GridRange;
                    k++;
                }
            }
            SetInverseNumber(list);
        }
        /// <summary>
        /// 设置九宫格Node值
        /// </summary>
        /// <param name="list">Node排列表</param>
        public void SetValue(List<Node> list)
        {
            int k = 0;
            for (int i = 0; i < Constants.GridRange; i++)
            {
                for (int j = 0; j < Constants.GridRange; j++)
                {
                    map[i,j] = new Node(list[k++]);
                }
            }
        }
    }
}
